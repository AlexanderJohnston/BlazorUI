using System.Collections.Generic;
using System.Linq;
using BlazorUI.Client.Campaign.Data;
using Totem;
using Totem.Timeline;

namespace BlazorUI.Client.Campaign.Topics
{
  /// <summary>
  /// The timeline of enabling and disabling pages of CAM dealers
  /// </summary>
  public class Enrollment : Topic
  {
   readonly Dictionary<Id, Data.Pages> _pagesByDealerId = new Dictionary<Id, Data.Pages>();

    void Given(ManifestDownloaded e)
    {
      foreach(var added in e.AddedDealers)
      {
        _pagesByDealerId.Add(added.Id, Data.Pages.None);
      }

      foreach(var removedId in e.RemovedDealerIds)
      {
        _pagesByDealerId.Remove(removedId);
      }
    }

    void Given(EnrollmentChanged e) =>
      _pagesByDealerId[e.DealerId] = e.Pages;

    //
    // When
    //

    void When(ChangeEnrollment e)
    {
      if(!_pagesByDealerId.TryGetValue(e.DealerId, out var pages))
      {
        Then(new DealerNotEnrolled(e.DealerId));
      }
      else if(pages == e.Pages)
      {
        Then(new EnrollmentUnchanged(e.DealerId));
      }
      else
      {
        Then(new EnrollmentChanged(e.DealerId, e.Pages));
      }
    }

    void When(ManifestDownloaded e)
    {
      var enrolledDealerIds = Many.Of(
        from dealerId in _pagesByDealerId.Keys
        where _pagesByDealerId[dealerId] != Data.Pages.None
        select dealerId);

      if(enrolledDealerIds.Count == 0)
      {
        Then(new DealerManifestDownloadsSkipped());
      }
      else
      {
        Then(new DealerManifestDownloadsStarted(enrolledDealerIds));
      }
    }
  }
}