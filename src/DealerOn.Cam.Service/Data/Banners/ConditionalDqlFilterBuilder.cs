using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DealerOn.Cam.Data;

namespace DealerOn.Cam.Service.Data.Banners
{
  /// <summary>
  /// Builds the DQL filter representing a banner's condition
  /// </summary>
  internal class ConditionalDqlFilterBuilder
  {
    readonly StringBuilder _filter = new StringBuilder();
    readonly CampaignDbCall.Campaign _campaign;

    internal ConditionalDqlFilterBuilder(CampaignDbCall.Campaign campaign)
    {
      _campaign = campaign;
    }

    internal string Build()
    {
      WriteModelFilter();

      WriteYearFilter();

      return _filter.ToString();
    }

    void WriteModelFilter() =>
      WriteFilter(_campaign.Model, "All Models", model => $"model = '{model}'");

    void WriteYearFilter()
    {
      if(_filter.Length > 0)
      {
        _filter.Append(" and ");
      }

      WriteFilter(_campaign.ModelYear, "All Years", year => $"year = {year}");
    }

    void WriteFilter(string campaignValue, string allValue, Func<string, string> getValueFilter)
    {
      var values = GetValues(campaignValue ?? "", allValue).ToList();

      if(values.Count > 0)
      {
        _filter.Append("(");

        for(var i = 0; i < values.Count; i++)
        {
          if(i > 0)
          {
            _filter.Append(" or ");
          }

          _filter.Append(getValueFilter(values[i]));
        }

        _filter.Append(")");
      }
    }

    IEnumerable<string> GetValues(string campaignValue, string allValue)
    {
      if(!campaignValue.Equals(allValue, StringComparison.InvariantCultureIgnoreCase))
      {
        foreach(var value in
          from part in (campaignValue ?? "").Split(',')
          let value = part.Trim()
          where !String.IsNullOrEmpty(value)
          select value)
        {
          yield return value;
        }
      }
    }
  }
}