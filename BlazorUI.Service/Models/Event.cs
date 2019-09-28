using System;
using System.Collections.Generic;

namespace BlazorUI.Service.Models
{
    public partial class Event
    {
        public Event()
        {
            InverseCauseNavigation = new HashSet<Event>();
        }

        public long Position { get; set; }
        public long? Cause { get; set; }
        public string Type { get; set; }
        public string Json { get; set; }

        public virtual Event CauseNavigation { get; set; }
        public virtual ICollection<Event> InverseCauseNavigation { get; set; }
    }
}
