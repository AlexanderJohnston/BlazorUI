using System;
using Totem.Timeline;

namespace DealerOn.Cam.Queries
{
  /// <summary>
  /// A data structure describing the status of CAM
  /// </summary>
  public class StatusQuery : Query
  {
    public bool Importing;
    public DateTimeOffset? WhenImportedLast;
    public DateTimeOffset? WhenImportsNext;

    void GivenScheduled(StartScheduledImport e) =>
      WhenImportsNext = Event.GetWhenOccurs(e);

    void Given(StartScheduledImport e) =>
      WhenImportsNext = null;

    void Given(ImportStarted e)
    {
      Importing = true;
      WhenImportedLast = e.When;
    }

    void Given(ImportFinished e) =>
      Importing = false;
  }
}