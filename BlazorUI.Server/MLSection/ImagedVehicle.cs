using System;
using System.Collections.Generic;
using System.Text;

namespace MLSection
{
    public class ImagedVehicle
    {
        public List<byte[]> Images = new List<byte[]>();

        // Represents the "Classification" e.g. "Ford.Explorer.2019"
        public string Label;
    }
}
