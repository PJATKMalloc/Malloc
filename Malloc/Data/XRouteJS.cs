/**
 * XRoute v1.0.0 - TSP Resolver for PostData
 * 
 * Copyright 2021 ANOVEI. All Rights Reserved
 * Author: Karol Szmajda (biuro@anovei.pl)
 */

using Itinero;
using Itinero.LocalGeo;
using Itinero.Osm.Vehicles;
using Microsoft.JSInterop;

namespace Malloc.Data
{
    public class XRouteJS
    {
        public XRouteService Service;
        public List<RouterPoint> Points { get; set; }
        public delegate void OnAddPointEvent(List<RouterPoint> Points);
        public event OnAddPointEvent? OnAddPoint;

        public XRouteJS(XRouteService Service)
        {
            this.Service = Service;
            this.Points = new();
        }

        [JSInvokableAttribute("AddPoint")]
        public bool AddPoint(float Latitude, float Longitude)
        {
            if (Coordinate.Validate(Latitude, Longitude))
            {
                var Result = Service.Router.TryResolve(Vehicle.Car.Shortest(), new Coordinate(Latitude, Longitude), 300);
                if (!Result.IsError)
                {
                    Points.Add(Result.Value);
                    OnAddPoint?.Invoke(Points);

                    return true;
                }
            }

            return false;
        }

        [JSInvokableAttribute("RemovePoint")]
        public bool RemovePoint(float Latitude, float Longitude)
        {

            //todo...
            if (Coordinate.Validate(Latitude, Longitude))
            {
                var Result = Service.Router.TryResolve(Vehicle.Car.Shortest(), new Coordinate(Latitude, Longitude));
                if (!Result.IsError)
                {
                    Points.Add(Result.Value);
                    OnAddPoint?.Invoke(Points);

                    return true;
                }
            }

            return false;
        }
    }
}
