using BlazorUI.Shared.Services.Metrics;
using Microsoft.Extensions.Logging;
using PostSharp.Patterns.Diagnostics;
using PostSharp.Patterns.Formatters;
using System;
using System.Collections.Generic;
using System.Text;
using Totem.Runtime;

using static PostSharp.Patterns.Diagnostics.FormattedMessageBuilder;
using static PostSharp.Patterns.Diagnostics.SemanticMessageBuilder;

namespace BlazorUI.Service.Metrics
{
    //[Log(AttributeExclude = true)]
    public class MetricsClient : Notion, IMetricsClient
    {
        public void TrackEvent(string method, Dictionary<string, double> metrics)
        {
            var sb = new StringBuilder();
            sb.Append($"[Tracked Metric] || ");
            sb.Append($"[Method]: {method}");
            foreach (var formatted in FormatToWrite(metrics))
                sb.Append($" {formatted} |");
            Traits.Log.Get(this).LogInformation(sb.ToString());

        }

        public IEnumerable<string> FormatToWrite(Dictionary<string, double> metrics)
        {
            foreach (var metric in metrics)
            {
                yield return $"{metric.Key}= {metric.Value}";
            }
        }
    }
}
