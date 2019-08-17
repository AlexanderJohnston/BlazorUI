using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Totem;
using Totem.Runtime;

namespace BlazorUI.Client.totem_timeline
{
    public class Event : IBindable
    {
        protected Event()
        {
            When = Clock.Now;
        }

        public Event(string type, string url)
        {
            Traits.Type.Set(this, type);
            Traits.Endpoint.Set(this, url);
        }

        public Fields Fields => _fields ?? (_fields = new Fields(this));
        private Fields _fields;
        Fields IBindable.Fields => Fields;

        protected IClock Clock => Traits.Clock.Get(this);

        public DateTimeOffset When
        {
            get { return Traits.When.Get(this); }
            private set { Traits.When.Set(this, value); }
        }

        public Field Position
        {
            get { return Traits.Position; }
            set { Traits.Position.Set(this, value);  }
        }

        public Field Type
        {
            get { return Traits.Type; }
            set { Traits.Type.Set(this, value); }
        }

        public Field Cause
        {
            get { return Traits.Cause; }
            set { Traits.Cause.Set(this, value); }
        }

        public Field WhenOccurs
        {
            get { return Traits.WhenOccurs; }
            set { Traits.WhenOccurs.Set(this, value); }
        }

        public Field ETag
        {
            get { return Traits.ETag; }
            set { Traits.ETag.Set(this, value); }
        }

        public Field Endpoint
        {
            get { return Traits.Endpoint; }
            set { Traits.Endpoint.Set(this, value); }
        }
        public static DateTimeOffset? GetWhenOccurs(Event e) =>
            Traits.WhenOccurs.Get(e);

        public static bool IsScheduled(Event e) =>
          GetWhenOccurs(e) != null;

        public new static class Traits
        {
            public static readonly Field<string> Endpoint = Field.Declare(() => Endpoint);
            public static readonly Field<string> ETag = Field.Declare(() => ETag);
            public static readonly Field<string> Cause = Field.Declare(() => Cause);
            public static readonly Field<string> Type = Field.Declare(() => Type);
            public static readonly Field<int> Position = Field.Declare(() => Position);
            public static readonly Field<DateTimeOffset> When = Field.Declare(() => When);
            public static readonly Field<DateTimeOffset?> WhenOccurs = Field.Declare(() => WhenOccurs);
            public static readonly Field<Id> EventId = Field.Declare(() => EventId);
            public static readonly Field<Id> CommandId = Field.Declare(() => CommandId);
            public static readonly Field<Id> UserId = Field.Declare(() => UserId);
            public static readonly Field<IClock> Clock = Field.Declare(() => Clock, new PlatformClock());
            class PlatformClock : IClock
            {
                public DateTimeOffset Now => DateTimeOffset.Now;
            }
        }

        

    }
}
