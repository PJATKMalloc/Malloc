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
using System.Globalization;
using System.Text;
using System.Web;

namespace Malloc.Data
{
    public class XRouteJS
    {
        public XRouteService Service;
        public List<RouterPoint> Points { get; set; }
        public List<string> Addresses { get; set; }
        public Dictionary<string, Coordinate> AddressToPoint { get; set; }
        public delegate void OnAddPointEvent(List<RouterPoint> Points);
        public event OnAddPointEvent? OnAddPoint;

     
        public XRouteJS(XRouteService Service)
        {
            this.Service = Service;
            this.Points = new();
            this.Addresses = new(); 
            this.AddressToPoint = new();
        }

        [JSInvokableAttribute("AddAddress")]
        public async Task AddAddress(string Address)
        {
            if (AddressToPoint.ContainsKey(Address))
            {
                AddPoint(AddressToPoint[Address].Latitude, AddressToPoint[Address].Longitude);
                return;
            }

      
            HttpClient client =  Service._clientFactory.CreateClient("komoto") ;
            
       
            var request = new HttpRequestMessage(HttpMethod.Get,
               $"/?addressdetails=1&q={HttpUtility.UrlEncode(Address, Encoding.UTF8)}&format=json&limit=1");
            ///api/?q={HttpUtility.UrlEncode(Address, Encoding.UTF8)}&limit=1
            //var request = new HttpRequestMessage(HttpMethod.Get, "/api/?q=Micha%C5%82a+Drzyma%C5%82y+9%2c+81-771+Sopot&limit=1");
            //request.Headers.Add("Accept", "application/json");



            var xdd = await client.SendAsync(request);
           if(xdd.IsSuccessStatusCode)
            {
                var adasd = await xdd.Content.ReadAsStringAsync();
                OSMJSON[]? aa = JsonConvert.DeserializeObject<OSMJSON[]>(adasd);

               if (aa != null)
                {
                    OSMJSON aaadd = aa[0];
                    Coordinate cords = new Coordinate(float.Parse(aaadd.lat, CultureInfo.InvariantCulture.NumberFormat), float.Parse(aaadd.lon, CultureInfo.InvariantCulture.NumberFormat));
                    AddressToPoint.Add(Address, cords);
                    AddPoint(cords.Latitude, cords.Longitude);
                }
                //var test = await xdd.Content.ReadFromJsonAsync<OSMJSON>();
         
               // if (test?.features.Count>0)
                //{
               

                //}
            }



            // dynamic test2 = test.features[0].geometry;

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
