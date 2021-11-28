using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;
using Itinero;
using Itinero.Optimization.Strategies;
using Itinero.Optimization.Solvers;
using Itinero.Optimization.Solvers.Tours;
using Itinero.Optimization.Strategies.Random;

namespace Malloc.Data
{
    internal static class TSPTWFitness
    {
        /// <summary>
        /// Calculates a fitness value for the given tour.
        /// </summary>
        /// <param name="tour">The tour.</param>
        /// <param name="problem">The problem.</param>
        /// <param name="waitingTimePenaltyFactor">A penalty applied to time window violations as a factor. A very high number compared to travel times.</param>
        /// <param name="timeWindowViolationPenaltyFactor">A penalty applied to waiting times as a factor.</param>
        /// <returns>A fitness value that reflects violations of timewindows by huge penalties.</returns>
        public static float Fitness(this Tour tour, TSPTWProblem problem, float waitingTimePenaltyFactor = 0.9f,
            float timeWindowViolationPenaltyFactor = 600)
        {
            var violations = 0.0f;
            var waitingTime = 0.0f; // waits are not really a violation but a waste.
            var time = 0.0f;
            var travelTime = 0.0f;
            var previous = Tour.NOT_SET;
            using (var enumerator = tour.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    var current = enumerator.Current;
                    if (previous != Tour.NOT_SET)
                    {
                        // keep track of time.
                        var weight = problem.Weight(previous, current);
                        time += weight;
                        travelTime += weight;
                    }

                    var window = problem.Windows[current];
                    if (!window.IsEmpty)
                    {
                        if (window.Max < time)
                        {
                            // ok, unfeasible.
                            violations += time - window.Max;
                        }

                        if (window.Min > time)
                        {
                            // wait here!
                            waitingTime += (window.Min - time);
                            time = window.Min;
                        }
                    }

                    previous = current;
                }
            }

            if (tour.First == tour.Last &&
                previous != Tour.NOT_SET)
            {
                var weight = problem.Weight(previous, tour.First);
                travelTime += weight;
            }

            if (violations > float.Epsilon &&
                violations < 1)
            { // make the violations at least 1 if there are any, to make sure the penalty
              // stays unacceptable even for tiny violations.
                violations = 1;
            }

            return (violations * timeWindowViolationPenaltyFactor) + travelTime + (waitingTime * waitingTimePenaltyFactor);
        }
    }

    internal static class Local2OptOperation
    {
        /// <summary>
        /// Runs a local 2-opt search, 
        /// </summary>
        /// <param name="tour">The tour.</param>
        /// <param name="weightFunc">The function to get weights.</param>
        /// <param name="windows">The time windows.</param>
        /// <returns>True if the operation succeeded and an improvement was found.</returns>
        /// <remarks>* 2-opt: Removes two edges and reconnects the two resulting paths in a different way to obtain a new tour.</remarks>
        public static bool Do2Opt(this Tour tour, Func<int, int, float> weightFunc, TimeWindow[] windows)
        {
            var customers = new List<int>(windows.Length + 1);
            customers.AddRange(tour);
            if (tour.Last == tour.First)
            { // add last customer at the end if it's the same as the first one.
                customers.Add(tour.Last.Value);
            }

            var weight12 = 0.0f;
            if (windows[customers[0]].Min > weight12)
            { // wait here!
                weight12 = windows[customers[0]].Min;
            }
            for (var edge1 = 0; edge1 < customers.Count - 3; edge1++)
            { // iterate over all from-edges.
                var edge11 = customers[edge1];
                var edge12 = customers[edge1 + 1];

                var weight11 = weight12;
                weight12 += weightFunc(edge11, edge12);
                if (windows[edge12].Min > weight12)
                { // wait here!
                    weight12 = windows[edge12].Min;
                }

                float betweenForward = 0;
                for (var edge2 = edge1 + 2; edge2 < customers.Count - 1; edge2++)
                {
                    // iterate over all to-edges.
                    var edge20 = customers[edge2 - 1];
                    var edge21 = customers[edge2];
                    var edge22 = customers[edge2 + 1];

                    // calculate existing value of the part 11->21->(reverse)->12->22.
                    // @ 22: no need to take minimum of window into account, is valid now, will stay valid on reduction of arrival-time.
                    // completely re-calculate between-backward (because window min may be violated) and determine feasible at the same time.
                    var feasible = true;
                    var currentWeight = weight11 + weightFunc(edge11, edge21);
                    var edge21Windows = windows[edge21];
                    if (!edge21Windows.IsEmpty &&
                        edge21Windows.Min > currentWeight)
                    { // wait here!
                        currentWeight = edge21Windows.Min;
                    }

                    var previous = edge21;
                    for (var i = edge2 - 1; i > edge1; i--)
                    {
                        var current = customers[i];
                        var currentWindow = windows[current];
                        currentWeight += weightFunc(previous, current);
                        if (!currentWindow.IsEmpty)
                        {
                            if (currentWindow.Min > currentWeight)
                            { // wait here!
                                currentWeight = currentWindow.Min;
                            }

                            if (currentWindow.Max < currentWeight)
                            { // unfeasible.
                                feasible = false;
                                break;
                            }
                        }

                        previous = current;
                    }

                    var potential = currentWeight + weightFunc(edge12, edge22);

                    if (!feasible) continue;

                    // new reverse is feasible.
                    // calculate existing value of the part 11->12->...->21->22.
                    // @ 22: no need to take minimum of window into account, is valid now, will stay valid on reduction of arrival-time.
                    betweenForward += weightFunc(edge20, edge21);
                    if (betweenForward + weight12 < windows[edge21].Min)
                    {
                        // take into account minimum-window constraint.
                        betweenForward = windows[edge21].Min - weight12;
                    }

                    var existing = weight12 + betweenForward + weightFunc(edge21, edge22);
                    if (!(existing > potential)) continue;

                    // we found an improvement.
                    tour.ReplaceEdgeFrom(edge11, edge21);
                    tour.ReplaceEdgeFrom(edge12, edge22);
                    for (var i = edge1 + 1; i < edge2; i++)
                    {
                        tour.ReplaceEdgeFrom(customers[i + 1], customers[i]);
                    }

                    return true;
                }
            }
            return false;
        }
    }

    class Local2OptOperator : Operator<Candidate<TSPTWProblem, Tour>>
    {
        public override string Name => "2OPT";

        public override bool Apply(Candidate<TSPTWProblem, Tour> candidate)
        {
            if (!candidate.Solution.Do2Opt(candidate.Problem.Weight, candidate.Problem.Windows)) return false;

            candidate.Fitness = candidate.Solution.Fitness(candidate.Problem);
            return true;
        }

        private static readonly ThreadLocal<Local2OptOperator> DefaultLazy = new ThreadLocal<Local2OptOperator>(() => new Local2OptOperator());
        public static Local2OptOperator Default => DefaultLazy.Value;
    }
   

    public class RandomSolver : Strategy<TSPTWProblem, Candidate<TSPTWProblem, Tour>>
    {
        public override string Name => "RAN";

        public override Candidate<TSPTWProblem, Tour> Search(TSPTWProblem problem)
        {
            // initialize.
            var visits = new List<int>();
            foreach (var visit in problem.Visits)
            {
                if (visit != problem.First &&
                    visit != problem.Last)
                {
                    visits.Add(visit);
                }
            }

            visits.Shuffle();
            visits.Insert(0, problem.First);
            if (problem.Last.HasValue && problem.First != problem.Last)
            {
                visits.Add(problem.Last.Value);
            }
            var tour = new Tour(visits, problem.Last);

            return new Candidate<TSPTWProblem, Tour>()
            {
                Problem = problem,
                Solution = tour,
                Fitness = tour.Fitness(problem)
            };
        }

        private static readonly ThreadLocal<RandomSolver> DefaultLazy = new ThreadLocal<RandomSolver>(() => new RandomSolver());
        public static RandomSolver Default => DefaultLazy.Value;
    }


    public  struct TimeWindow
    {
        /// <summary>
        /// The minimum time in seconds.
        /// </summary>
        public float Min { get; set; }

        /// <summary>
        /// The maximum time in seconds.
        /// </summary>
        public float Max { get; set; }

        /// <summary>
        /// Returns true if this window is valid at the given seconds.
        /// </summary>
        /// <param name="seconds">The time.</param>
        /// <returns></returns>
        public bool IsValidAt(float seconds)
        {
            return this.Min <= seconds && this.Max >= seconds;
        }

        /// <summary>
        /// Returns the minimum difference.
        /// </summary>
        /// <param name="seconds">The time.</param>
        /// <returns></returns>
        public float MinDiff(float seconds)
        {
            if (this.Min <= seconds && this.Max >= seconds)
            {
                // the time is within the window, no difference.
                return 0;
            }

            if (seconds < this.Min)
            {
                // time window too late.
                return this.Min - seconds;
            }

            return seconds - this.Max;
        }

        /// <summary>
        /// Returns true if this timewindows is considered empty or 'to be ignored'.
        /// </summary>
        public bool IsEmpty => (this.Min == 0 &&
                               this.Max == 0) ||
                                (this.Min == float.MaxValue &&
                                this.Max == float.MinValue);

        /// <summary>
        /// Returns the fully qualified type name of this instance.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"[{this.Min}, {this.Max}]";
        }
    }

    public class NearestNeighbourArray : IEnumerable<int>
    {
        private readonly int[] _nn; // Contains the nearest neigbour per visit.

        /// <summary>
        /// Creates a new nearest neigbour array using bidirectional weights for all visits.
        /// </summary>
        /// <param name="weights">The weights to use.</param>
        public NearestNeighbourArray(float[][] weights)
            : this(weights, weights.Length - 1)
        {

        }

        /// <summary>
        /// Creates a new nearest neigbour array using bidirectional weights keeping <paramref name="n"/> per visit.
        /// </summary>
        /// <param name="weights">The weights to use.</param>
        /// <param name="n">The number of nearest neigbours to keep.</param>
        public NearestNeighbourArray(float[][] weights, int n)
            : this((v1, v2) => weights[v1][v2] + weights[v2][v1], weights.Length, n)
        {

        }

        /// <summary>
        /// Creates a new nearest neigbour array.
        /// </summary>
        /// <param name="weightFunc">The weight function.</param>
        /// <param name="count">The # of visits.</param>
        /// <param name="n">The number of neigbours to keep per visit.</param>
        public NearestNeighbourArray(Func<int, int, float> weightFunc, int count, int n)
        {
            N = n;
            _nn = new int[n * count];

            for (var v = 0; v < count; v++)
            {
                var neighbours = new SortedDictionary<float, List<int>>();
                for (var current = 0; current < count; current++)
                {
                    if (current == v) continue;
                    var weight = weightFunc(v, current);
                    if (!neighbours.TryGetValue(weight, out var visits))
                    {
                        visits = new List<int>();
                        neighbours.Add(weight, visits);
                    }
                    visits.Add(current);
                }

                var neigbourCount = 0;
                foreach (var pair in neighbours)
                {
                    foreach (var current in pair.Value)
                    {
                        if (neigbourCount >= n)
                        {
                            break;
                        }

                        _nn[v * n + neigbourCount] = current;
                        neigbourCount++;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the # of nearest neigbours per visit.
        /// </summary>
        /// <returns></returns>
        public int N { get; }

        /// <summary>
        /// Gets the nearest neigbours for the given visit.
        /// </summary>
        /// <param name="v">The visit.</param>
        public int[] this[int v]
        {
            get
            {
                var nn = new int[N];
                _nn.CopyTo(nn, 0, v * N, N);
                return nn;
            }
        }

        /// <summary>
        /// Copies the nearest neigbours of the given visit to the given array.
        /// </summary>
        /// <param name="v">The visit.</param>
        /// <param name="nn">The array to copy to.</param>
        /// <param name="index">The index to start copying at.</param>
        public void CopyTo(int v, int[] nn, int index = 0)
        {
            _nn.CopyTo(nn, index, v * N, N);
        }

        /// <summary>
        /// Copies the nearest neigbours of the given visit to the given array.
        /// </summary>
        /// <param name="v">The visit.</param>
        /// <param name="nn">The array to copy to.</param>
        /// <param name="index">The index to start copying at.</param>
        /// <param name="count">The # of nn to copy.</param>
        public void CopyTo(int v, int[] nn, int index, int count)
        {
            _nn.CopyTo(nn, index, v * N, count);
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>The enumerator.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public IEnumerator<int> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
