using System;
using System.Collections.Generic;
using System.Text;
using Totem.Timeline;

namespace DealerOn.Cam.Queries
{
    public class EchoQuery : Query
    {
        public int Count;

        void Given(Echoed e)
        {
            Count++;
        }
    }
}
