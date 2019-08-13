using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorUI.Client.totem_timeline
{
    public class Timeline
    {

        public delegate void Append(dynamic cause, dynamic type, dynamic data);

        public delegate void Schedule(dynamic whenOccurs, dynamic cause, dynamic type, dynamic data);

        public delegate void Topic(dynamic declaration);

        public delegate void Query(dynamic first, dynamic second);

        public delegate void WebRequest(dynamic loadedEventType, dynamic url, dynamic options);

        public delegate void ConfigureQueryHub(dynamic hub);
    }
}
