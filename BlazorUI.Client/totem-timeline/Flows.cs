using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reactive;
using System.Linq;
using System.Threading.Tasks;
using Totem.Timeline;

namespace BlazorUI.Client.totem_timeline
{
    public class Flows
    {
        public Dictionary<dynamic, dynamic> EventTypes { get; set; }
        
        public IObserver<dynamic> Events { get; set; }

        private int _position = 0;

        public void AppendEvent(string cause, string type, Event data)
        {
            _position++;
            data.Fields.Set(data.Position, _position);
            data.Fields.Set(data.Cause, cause);
            data.Fields.Set(data.Type, type);
            Events.OnNext(data);
        }

        public void ScheduleEvent(DateTime whenOccurs, string cause, string type, Event data)
        {
            _position++;
            data.Fields.Set(data.Position, _position);
            data.Fields.Set(data.Cause, cause);
            data.Fields.Set(data.Type, type);
            data.Fields.Set(data.WhenOccurs, whenOccurs);
            Events.OnNext(data);
        }

        public void AppendScheduledEvent(Event data)
        {
            _position++;
            data.Fields.Set(data.Position, _position);
            data.Fields.Set(data.WhenOccurs, null);
            Events.OnNext(data);
        }


    }
}
