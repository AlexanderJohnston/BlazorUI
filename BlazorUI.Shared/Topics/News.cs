using BlazorUI.Shared.Events;
using BlazorUI.Shared.Queries;
using System;
using System.Collections.Generic;
using System.Text;
using Totem;
using Totem.Timeline;

namespace BlazorUI.Shared.Topics
{
    class News : Topic
    {
        public List<Id> Facts { get; set; }
        
        public News()
        {
            Facts = new List<Id>();
        }

        void When(LearnFact e)
        {
            var fields = Json.ParseOutFields(e.Knowledge);
            Then(new FactLearned(e.Happened, e.Knowledge, fields));
        }

    }
    public static class Json
    {
        public static Dictionary<string, string> ParseOutFields(string knowledge)
        {
            return new Dictionary<string, string>();
        }
    }
    
}
