/**
 * XRoute v1.0.0 - TSP Resolver for PostData
 * 
 * Copyright 2021 ANOVEI. All Rights Reserved
 * Author: Karol Szmajda (biuro@anovei.pl)
 */

using Itinero;
using Itinero.Algorithms.Matrices;
using Itinero.IO.Osm;
using Itinero.Profiles;
using Vehicle = Itinero.Osm.Vehicles.Vehicle;

namespace Malloc.Data
{
    public class XRouteService
    {
        public static readonly Profile DefaultProfile = Vehicle.Car.Fastest();
        private const string DatabaseFilename = "my.db";
        private const string OSMFilename = "pomorskie-latest.osm.pbf";
        private readonly RouterDb RouterDb;

        public readonly IHttpClientFactory _clientFactory;

        public readonly Router Router;

        public XRouteService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;

            RouterDb = new RouterDb();

            if (File.Exists(DatabaseFilename))
            {
                using (var stream = new FileInfo(DatabaseFilename).OpenRead())
                {
                    RouterDb = RouterDb.Deserialize(stream);
                }
            }
            else
            {
                if (!File.Exists(OSMFilename))
                {
                    throw new Exception("Brakuje pliku z bazą danych, pobierz ją na stronie http://download.geofabrik.de/");
                }

                using (var stream = new FileInfo(OSMFilename).OpenRead())
                {
                    RouterDb.LoadOsmData(stream, Vehicle.Car);
                }

                RouterDb.AddContracted(DefaultProfile);

                using (var stream = new FileInfo(DatabaseFilename).OpenWrite())
                {
                    RouterDb.Serialize(stream);
                }
            }

            Router = new Router(RouterDb);
        }


        public bool Solve(List<RouterPoint> Points, List<TimeWindow> Times, int StartIndex, int EndIndex, out List<RouterPoint> NewPoints)
        {
            NewPoints = new List<RouterPoint>(Points.Count);

            if (Points.Count <= 2)
            {
                return false;
            }

            var Invalids = new HashSet<int>();

            var DistanceMatrix = Router.CalculateWeight(DefaultProfile, Points.ToArray(), Invalids);

            foreach (var InvalidIndex in Invalids)
            {
                Points.RemoveAt(InvalidIndex);
                EndIndex--;
            }

            if (Points.Count > 2)
            {

               /* var XRoute = new XRouteResolver(Points.ToArray(), StartIndex, EndIndex, DistanceMatrix);
                XRoute.Solve();
                foreach (var Index in XRoute.Connections)
                {
                    NewPoints.Add(Points[Index]);
                }*/
                
                float m = Times.Min(x => x.Min);
                Times = Times.Select(x => new TimeWindow() { Min = x.Min - m, Max = x.Max - m }).ToList();
                var asddd = new TSPTWProblem(StartIndex, StartIndex, DistanceMatrix, Times.ToArray());
                RandomSolver aadd = new RandomSolver();
           
                Local2OptOperator asddda = new Local2OptOperator();
              
                var asdddasdasd = aadd.Search(asddd);
                asddda.Apply(asdddasdasd);
         
                var arr = asdddasdasd.Solution.ToArray();


                foreach (var Index in arr)
                {
                    NewPoints.Add(Points[Index]);
                }

                return (NewPoints.Count > 2);
            }

            return false;
        }

    }
}
