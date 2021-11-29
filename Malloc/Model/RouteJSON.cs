using Itinero;

namespace Malloc.Model
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public record RouteJSON : LocationJSON
    {
        public string OpenTime { get; set; }
        public string CloseTime { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public RouterPoint Point { get; set; }

        public override string ToString()
        {
            return Street + " " + StreetNumber + ", " + PostalCode + " " + City;
        }
    }


}
