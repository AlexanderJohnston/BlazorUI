using System;
using System.Collections.Generic;
using System.Text;
using Totem.Timeline;

namespace DealerOn.Cam.Queries
{
    public class EchoQuery : Query
    {
        public int Count;
        public TimeSpan? TimeUntilCurrentEcho;
        public DateTime? TimeOfLastEcho;

        void Given(Echo e)
        {
            TimeUntilCurrentEcho = TimeOfLastEcho.HasValue ? (DateTime.Now - TimeOfLastEcho) : TimeSpan.FromSeconds(0);
            TimeOfLastEcho = DateTime.Now;
        }
    }
}
