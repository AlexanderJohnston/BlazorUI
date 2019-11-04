using System;
using System.Collections.Generic;
using System.Text;

namespace MLSection
{
    public class Vehicle
    {
        public string id { get; set; }
        public string vin { get; set; }
        public int year { get; set; }
        public string make { get; set; }
        public string model { get; set; }
        public List<string> vehiclePhotos { get; set; }
    }
}
