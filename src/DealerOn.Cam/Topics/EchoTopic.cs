using System;
using System.Collections.Generic;
using System.Text;
using Totem.Timeline;

namespace DealerOn.Cam.Topics
{
    public class EchoTopic : Topic
    {
        void When(Echo e) => Then(new EchoSuccess());
    }
}
