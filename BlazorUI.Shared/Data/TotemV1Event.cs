using System.Collections.Generic;

namespace BlazorUI.Shared.Data
{
    public partial class TotemV1Event
    {
        public TotemV1Event()
        {
            InverseCauseNavigation = new HashSet<TotemV1Event>();
        }

        public long? Position { get; set; }
        public long? Cause { get; set; }
        public string Type { get; set; }
        public string Json { get; set; }

        public virtual TotemV1Event CauseNavigation { get; set; }
        public virtual ICollection<TotemV1Event> InverseCauseNavigation { get; set; }
    }
}
