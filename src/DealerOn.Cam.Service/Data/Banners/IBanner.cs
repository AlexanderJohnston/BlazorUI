using System.Threading.Tasks;
using DealerOn.Cam.Data;
using Totem;

namespace DealerOn.Cam.Service.Data.Banners
{
  /// <summary>
  /// Describes a banner record in the DealerVehiclePhotos or DealerConditionalBanners table
  /// </summary>
  public interface IBanner
  {
    Id DealerId { get; }
    int Position { get; }
    bool IsCampaign { get; }
    string Path { get; }

    Task Update(CampaignDbCall.Campaign campaign, string path);

    Task Move(int position);

    Task Delete();
  }
}