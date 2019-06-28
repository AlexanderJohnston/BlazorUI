using System.Threading.Tasks;
using DealerOn.Cam.Data;
using DealerOn.Cam.Service.Data.Banners;

namespace DealerOn.Cam.Service.Data
{
  /// <summary>
  /// The database containing campaign data for CAM dealers
  /// </summary>
  public sealed class CampaignDb : ICampaignDb
  {
    readonly IDealerOnDb _dealerOnDb;
    readonly IBannerTable _homeTable;
    readonly IBannerTable _conditionalTable;

    public CampaignDb(IDealerOnDb dealerOnDb, IBannerTable homeTable, IBannerTable conditionalTable)
    {
      _dealerOnDb = dealerOnDb;
      _homeTable = homeTable;
      _conditionalTable = conditionalTable;
    }

    public Task ImportCampaigns(CampaignDbCall call) =>
      _dealerOnDb.ExecuteAction(async connection =>
      {
        await _homeTable.ImportBanners(call, connection);
        await _conditionalTable.ImportBanners(call, connection);
      });
  }
}