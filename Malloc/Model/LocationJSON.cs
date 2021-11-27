namespace Malloc.Model
{
    public record LocationJSON
    {
        public string StreetNumber { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public bool IsNormalized { get; set; }
    }
}
