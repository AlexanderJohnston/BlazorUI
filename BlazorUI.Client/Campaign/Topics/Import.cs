using System;
using Totem.Timeline;

namespace BlazorUI.Client.Campaign.Topics
{
  /// <summary>
  /// The timeline of importing data from the CAM program
  /// </summary>
  public class Import : Topic
  {
    bool _importing;
    bool _scheduled;

    void Given(ImportStarted e) =>
      _importing = true;

    void Given(ImportFinished e) =>
      _importing = false;

    void GivenScheduled(StartScheduledImport e) =>
      _scheduled = true;

    void Given(StartScheduledImport e) =>
      _scheduled = false;

    //
    // When
    //

    void When(StartImport e)
    {
      if(_importing)
      {
        Then(new ImportAlreadyStarted());
      }
      else
      {
        Then(new ImportStarted());
      }
    }

    void When(StartScheduledImport e)
    {
      if(_importing)
      {
        Then(new ImportAlreadyStarted());
      }
      else
      {
        Then(new ImportStarted());
      }
    }

    void When(ManifestDownloadFailed e) =>
      Then(new ImportFinished());

    void When(CampaignsImported e) =>
      Then(new ImportFinished());

    void When(ImportFinished e)
    {
      if(!_scheduled)
      {
        ThenSchedule.NextTimeOfDay(
          new StartScheduledImport(),
          TimeSpan.FromHours(4),
          TimeSpan.FromHours(22));
      }
    }
  }
}