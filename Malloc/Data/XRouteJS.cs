/**
 * XRoute v1.0.0 - TSP Resolver for PostData
 * 
 * Copyright 2021 ANOVEI. All Rights Reserved
 * Author: Karol Szmajda (biuro@anovei.pl)
 */

using Itinero;
using Itinero.LocalGeo;
using Itinero.Osm.Vehicles;
using Malloc.Model;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using System.Globalization;
using System.Text;
using System.Web;

namespace Malloc.Data
{
    public class XRouteJS
    {
        public XRouteService Service;
        public List<RouteJSON> Points { get; set; }
        public List<string> Addresses { get; set; }
        public List<TimeWindow> Times { get; set; }
        public ConcurrentDictionary<string, RouteJSON> AddressToPoint { get; set; }

        public delegate void OnAddPointEvent(RouteJSON Point);
        public event OnAddPointEvent? OnAddPoint;

        public delegate void OnRenderEvent();
        public event OnRenderEvent? OnRender;

        public delegate void OnSolveEvent();
        public event OnSolveEvent? OnSolve;


        public XRouteJS(XRouteService Service)
        {
            this.Service = Service;
            this.Points = new();
            this.Addresses = new();
            this.AddressToPoint = new();
            this.Times = new();
        }

        public async Task<Coordinate?> SearchAddress(string Address)
        {
            HttpClient client = Service._clientFactory.CreateClient("komoto");
            var request = new HttpRequestMessage(HttpMethod.Get,
               $"/?addressdetails=1&q={HttpUtility.UrlEncode(Address, Encoding.UTF8)}&format=json&limit=1");

            var xdd = await client.SendAsync(request);
            if (xdd.IsSuccessStatusCode)
            {
                var adasd = await xdd.Content.ReadAsStringAsync();
                OSMJSON[]? aa = JsonConvert.DeserializeObject<OSMJSON[]>(adasd);

                if (aa != null && aa.Length != 0)
                {
                    OSMJSON aaadd = aa[0];
                    return new Coordinate(float.Parse(aaadd.lat, CultureInfo.InvariantCulture.NumberFormat), float.Parse(aaadd.lon, CultureInfo.InvariantCulture.NumberFormat));
                }
            }

            return null;
        }

        [JSInvokableAttribute("AddAddress")]
        public async Task AddAddress(RouteJSON Point)
        {
            string Address = Point.ToString();
            RouteJSON? p = null;
            if (AddressToPoint.TryGetValue(Address, out p))
            {
                AddPoint(p);
                return;
            }

            var r = await SearchAddress(Address);
            if (r != null)
            {
                Point.Latitude = r.Value.Latitude;
                Point.Longitude = r.Value.Longitude;

                AddPoint(Point);
            }
        }

             [JSInvokableAttribute("Clear")]
        public void Clear()
        {
           Points.Clear();
           Times.Clear();
         
        }

        [JSInvokableAttribute("Solve")]
        public void Solve()
        {
            OnSolve?.Invoke();
        }


        public bool AddPoint(RouteJSON Point)
        {
            var Result = Service.Router.TryResolve(XRouteService.DefaultProfile, new Coordinate((float)Point.Latitude, (float)Point.Longitude), 300);
            if (!Result.IsError)
            {
                Point.Point = Result.Value;

                TimeSpan StartTime, CloseTime;
                int StartTimeSeconds = 0, CloseTimeSeconds = 0;
                if (TimeSpan.TryParse(Point.OpenTime, out StartTime))
                {
                    StartTimeSeconds = (int)StartTime.TotalSeconds;
                }

                if (TimeSpan.TryParse(Point.CloseTime, out CloseTime))
                {
                    CloseTimeSeconds = (int)CloseTime.TotalSeconds;
                }

                AddressToPoint.TryAdd(Point.ToString(), Point);
                Times.Add(new TimeWindow() { Min = StartTimeSeconds, Max = CloseTimeSeconds });
                Points.Add(Point);
                OnAddPoint?.Invoke(Point);

                return true;
            }

            return false;
        }


    }

    public struct TimeWindow
    {

        public float Min { get; set; }

        public float Max { get; set; }

    }
}
