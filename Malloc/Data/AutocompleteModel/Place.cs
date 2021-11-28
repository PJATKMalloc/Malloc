using System;
using System.Collections.Generic;

namespace Malloc.Data.AutocompleteModel
{
    public partial class Place
    {
        public long OsmId { get; set; }
        public char OsmType { get; set; }
        public string Class { get; set; }
        public string Type { get; set; }
        public Dictionary<string, string> Name { get; set; }
        public short? AdminLevel { get; set; }
        public Dictionary<string, string> Address { get; set; }
        public Dictionary<string, string> Extratags { get; set; }
    }
}
