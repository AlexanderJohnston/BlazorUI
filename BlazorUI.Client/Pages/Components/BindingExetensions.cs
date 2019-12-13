using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Totem;
using Totem.Timeline;
using Totem.Timeline.Area;

namespace BlazorUI.Client.Pages.Components
{
    public static class BindingExetensions
    {
        public static T Bind<T>(this T query, Id instanceId) where T : Query
        {
            var queryType = query.GetType();
            var areaMap = AreaMap.From(new[] { queryType });
            AreaTypeName.TryFrom(queryType.Name, out var areaType);
            var flowType = areaMap.GetFlow(areaType);
            var subscriptionKey = FlowKey.From(flowType, instanceId);
            FlowContext.Bind(query, subscriptionKey);
            return query;
        }
    }
}
