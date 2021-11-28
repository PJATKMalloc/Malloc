/**
* XRoute v1.0.0 - TSP Resolver for PostData
* 
* Copyright 2021 ANOVEI. All Rights Reserved
* Author: Karol Szmajda (biuro@anovei.pl)
*/
namespace Malloc.Data
{


    public sealed class NearestNeighbourCache
    {
        private readonly int _count;
        private readonly Func<int, int, float> _weightFunc;

        /// <summary>
        /// Creates a new cache.
        /// </summary>
        /// <param name="count"></param>
        /// <param name="weightFunc"></param>
        public NearestNeighbourCache(int count, Func<int, int, float> weightFunc)
        {
            _count = count;
            _weightFunc = weightFunc;
        }

        private Dictionary<int, NearestNeighbourArray> _nearestNeighbours = new Dictionary<int, NearestNeighbourArray>(); // Holds the nearest neighbours.
        private Dictionary<int, NearestNeighbourArray> _forwardNearestNeighbours = new Dictionary<int, NearestNeighbourArray>(); // Holds the nearest neighbours in forward direction.
        private Dictionary<int, NearestNeighbourArray> _backwardNearestNeighbours = new Dictionary<int, NearestNeighbourArray>(); // Holds the nearest neighbours in backward direction.

        /// <summary>
        /// Gets the nearest neighbours for the given 'n'.
        /// </summary>
        /// <returns>The nearest neighbour array.</returns>
        public NearestNeighbourArray GetNNearestNeighbours(int n)
        {
            lock (_nearestNeighbours)
            {
                if (_nearestNeighbours.TryGetValue(n, out var nearestNeighbours)) return nearestNeighbours;

                // not found for n, create.
                nearestNeighbours = new NearestNeighbourArray((x, y) => _weightFunc(y, x), _count, n);
                _nearestNeighbours.Add(n, nearestNeighbours);
                return nearestNeighbours;
            }
        }

        /// <summary>
        /// Gets the nearest neighbours for the given 'n' in forward direction.
        /// </summary>
        /// <returns>The nearest neighbour array.</returns>
        public NearestNeighbourArray GetNNearestNeighboursForward(int n)
        {
            lock (_forwardNearestNeighbours)
            {
                if (_forwardNearestNeighbours.TryGetValue(n, out var nearestNeighbours)) return nearestNeighbours;

                // not found for n, create.
                nearestNeighbours = new NearestNeighbourArray((x, y) => _weightFunc(x, y), _count, n);
                _forwardNearestNeighbours.Add(n, nearestNeighbours);
                return nearestNeighbours;
            }
        }

        /// <summary>
        /// Gets the nearest neighbours for the given 'n' in backward direction.
        /// </summary>
        /// <returns>The nearest neighbour array.</returns>
        public NearestNeighbourArray GetNNearestNeighboursBackward(int n, int customer)
        {
            lock (_backwardNearestNeighbours)
            {
                if (_backwardNearestNeighbours.TryGetValue(n, out var nearestNeighbours)) return nearestNeighbours;

                // not found for n, create.
                nearestNeighbours = new NearestNeighbourArray((x, y) => _weightFunc(y, x), _count, n);
                _backwardNearestNeighbours.Add(n, nearestNeighbours);
                return nearestNeighbours;
            }
        }
    }
}
