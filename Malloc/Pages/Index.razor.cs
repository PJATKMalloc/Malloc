
using Itinero;
using Itinero.Osm.Vehicles;
using Malloc.Data;
using Microsoft.JSInterop;

namespace Malloc.Pages
{
    public partial class Index
    {
        public XRouteJS? Data;
        private DotNetObjectReference<XRouteJS>? objRef;
        private Itinero.Route NewRoute;


        protected override void OnInitialized()
        {
            Data = new(XRouteService);
            Data.OnAddPoint += Data_OnAddPoint;
            objRef = DotNetObjectReference.Create(Data);
        }

        private void Data_OnAddPoint(List<Itinero.RouterPoint> Points)
        {
           var NewPoints = new List<Itinero.RouterPoint>();

            if (XRouteService.Solve(Points, 0, Points.Count - 1, out NewPoints))
            {

                var Result = XRouteService.Router.TryCalculate(Vehicle.Car.Shortest(), NewPoints.ToArray());
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
                Data.OnAddPoint -= Data_OnAddPoint;
            }

            objRef?.Dispose();
        }

    }
}
