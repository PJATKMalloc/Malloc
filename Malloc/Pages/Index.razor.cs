
using Itinero;
using Itinero.Osm.Vehicles;
using Malloc.Data;
using Malloc.Model;
using Microsoft.JSInterop;
using Newtonsoft.Json;

namespace Malloc.Pages
{
    public partial class Index
    {
        public XRouteJS Data;
        private DotNetObjectReference<XRouteJS>? objRef;
        private Itinero.Route NewRoute;

        protected override void OnInitialized()
        {
            Data = new(XRouteService);
            Data.OnAddPoint += Data_OnAddPoint;
            Data.OnSolve += Data_OnSolve;
            objRef = DotNetObjectReference.Create(Data);

           // RouteJSON? testt= JsonConvert.DeserializeObject<RouteJSON>(File.ReadAllText("startPoint.json"));
            //Data?.AddAddress(testt);
            
        }

        private void Data_OnSolve()
        {
            var NewPoints = new List<RouterPoint>();

            if (XRouteService.Solve(Data.Points.Select(x=>x.Point).ToList(), Data.Times, 0, Data.Points.Count - 1, out NewPoints))
            {

                var Result = XRouteService.Router.TryCalculate(Vehicle.Car.Fastest(), NewPoints.ToArray());
                if (!Result.IsError)
                {
                    NewRoute = Result.Value;
                    JSRuntime.InvokeVoidAsync("leafletJsFunctions.setRoute", Result.Value.ToGeoJson());

                    InvokeAsync(() =>
                    {
                        StateHasChanged();
                    });
                }
            }
        }

        private void Data_OnAddPoint(RouteJSON Point)
        {
            JSRuntime.InvokeVoidAsync("leafletJsFunctions.addMarker", Point.Point.Latitude, Point.Point.Longitude, "adsasd");
 
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {

            if (firstRender)
            {
                await JSRuntime.InvokeAsync<object>("leafletJsFunctions.initialize", objRef);
            }

            base.OnAfterRender(firstRender);
        }



        public void Dispose()
        {
            if (Data != null)
            {
                //Data.OnAddPoint -= Data_OnAddPoint;
            }

            objRef?.Dispose();
        }

    }
}
