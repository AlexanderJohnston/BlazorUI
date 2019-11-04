using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace MLSection
{
    public class UnlearnedImage
    {
        [LoadColumn(0)]
        public byte[] Image;

        [LoadColumn(1)]
        public string Label;
    }
}
