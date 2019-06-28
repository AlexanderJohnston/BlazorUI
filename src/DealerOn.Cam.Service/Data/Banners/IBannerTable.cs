using System.Data;
using System.Threading.Tasks;
using DealerOn.Cam.Data;

namespace DealerOn.Cam.Service.Data.Banners
{
  /// <summary>
  /// Describes the tables containing CAM banners
  /// </summary>
  public interface IBannerTable
  {
    Task ImportBanners(CampaignDbCall call, IDbConnection connection);
  }
}