using System;
using System.Collections.Generic;
using System.Text;
using Totem;

namespace BlazorUI.Shared.Data
{
    public class Moment
    {
        public Id Position { get; set; }
        public int Cause { get; set; }
        public Id UserId { get; set; }
        public string Type { get; set; }
    }
}
