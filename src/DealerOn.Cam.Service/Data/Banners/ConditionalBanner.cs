using System.Data;
using System.Threading.Tasks;
using Dapper;
using DealerOn.Cam.Data;
using Totem;

namespace DealerOn.Cam.Service.Data.Banners
{
  /// <summary>
  /// A banner associated with a dealer in the DealerConditionalBanners table
  /// </summary>
  public sealed class ConditionalBanner : IBanner
  {
    readonly IDbConnection _connection;
    readonly int _dealerConditionalBannerId;

    public ConditionalBanner(IDbConnection connection, int dealerConditionalBannerId, Id dealerId, int position, string path)
    {
      _connection = connection;
      _dealerConditionalBannerId = dealerConditionalBannerId;
      DealerId = dealerId;
      Position = position;
      Path = path;
    }

    public Id DealerId { get; }
    public int Position { get; }
    public bool IsCampaign => true;
    public string Path { get; }

    public Task Update(CampaignDbCall.Campaign campaign, string assetPath)
    {
      var sql = @"
declare @LibraryPhotoId int

set @LibraryPhotoId = (select top 1 id from VehiclePhotos where path = @AssetPath)

update
  DealerConditionalBanners
set
  LibraryPhotoId = @LibraryPhotoId,
  DqlFilter = @DqlFilter,
  IsOnSrp = @IsOnSrp,
  IsOnVdp = @IsOnVdp,
  WhenStarts = @WhenStarts,
  WhenEnds = @WhenEnds,
  AssetPath = @AssetPath,
  Link = @Link,
  AltText = @AltText
where
  DealerConditionalBannerId = @DealerConditionalBannerId
";

      return _connection.ExecuteAsync(sql, new
      {
        DealerConditionalBannerId = _dealerConditionalBannerId,
        DqlFilter = new ConditionalDqlFilterBuilder(campaign).Build(),
        IsOnSrp = IsOnSrp(campaign.Page),
        IsOnVdp = IsOnVdp(campaign.Page),
        campaign.WhenStarts,
        campaign.WhenEnds,
        AssetPath = assetPath,
        Link = campaign.Link.ToString(),
        campaign.AltText
      });
    }

    public Task Move(int position)
    {
      var sql = @"
update
  DealerConditionalBanners
set
  Position = @Position
where
  DealerConditionalBannerId = @DealerConditionalBannerId
";

      return _connection.ExecuteAsync(sql, new
      {
        DealerConditionalBannerId = _dealerConditionalBannerId,
        Position = position
      });
    }

    public Task Delete()
    {
      var sql = @"
delete from
  DealerConditionalBanners
where
  DealerConditionalBannerId = @DealerConditionalBannerId
";

      return _connection.ExecuteAsync(sql, new
      {
        DealerConditionalBannerId = _dealerConditionalBannerId
      });
    }

    public static bool IsOnSrp(DealerManifest.Page page) =>
      page == DealerManifest.Page.Srp || page == DealerManifest.Page.SrpVdp;

    public static bool IsOnVdp(DealerManifest.Page page) =>
      page == DealerManifest.Page.Vdp || page == DealerManifest.Page.SrpVdp;
  }
}