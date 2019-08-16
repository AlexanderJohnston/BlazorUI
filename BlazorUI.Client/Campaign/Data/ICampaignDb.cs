using System.Threading.Tasks;

namespace BlazorUI.Client.Campaign.Data
{
  /// <summary>
  /// Describes the database containing campaign data for CAM dealers
  /// </summary>
  public interface ICampaignDb
  {
    Task ImportCampaigns(CampaignDbCall call);
  }
}