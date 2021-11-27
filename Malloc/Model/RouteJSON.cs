namespace Malloc.Model
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public record RouteJSON : LocationJSON
    {
        public string OpenTime { get; set; }
        public string CloseTime { get; set; }
    }


}
