using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DealerOn.Cam.Data;
using Totem;

namespace DealerOn.Cam.Service.Data.Banners
{
  /// <summary>
  /// The DealerConditionalBanners table containing CAM banners for the SRP and VDP
  /// </summary>
  public class ConditionalBannerTable : BannerTable
  {
    public ConditionalBannerTable(IBannerPath path) : base(path)
    {}

    protected override bool ContainsCampaign(CampaignDbCall.Campaign campaign) =>
      campaign.Page != DealerManifest.Page.Home;

    protected override async Task<IEnumerable<IBanner>> SelectBanners(IDbConnection connection)
    {
      var sql = @"
select
  DealerConditionalBannerId,
  DealerId,
  Position,
  AssetPath
from
  dbo.DealerConditionalBanners
order by
  Position
";

      return
        from result in await connection.QueryAsync(sql)
        select new ConditionalBanner(
          connection,
          (int) result.DealerConditionalBannerId,
          Id.From((int) result.DealerId),
          (int) result.Position,
          (string) result.AssetPath);
    }

    protected override Task InsertBanner(Id dealerId, CampaignDbCall.Campaign campaign, int position, string path, IDbConnection connection)
    {
      var sql = @"
declare @LibraryPhotoId int

set @LibraryPhotoId = (select top 1 id from VehiclePhotos where path = @AssetPath)

insert into DealerConditionalBanners
(
  DealerId,
  LibraryPhotoId,
  DqlFilter,
  IsOnSrp,
  IsOnVdp,
  WhenStarts,
  WhenEnds,
  Position,
  AssetPath,
  Link,
  AltText
)
values
(
  @DealerId,
  @LibraryPhotoId,
  @DqlFilter,
  @IsOnSrp,
  @IsOnVdp,
  @WhenStarts,
  @WhenEnds,
  @Position,
  @AssetPath,
  @Link,
  @AltText
)
";
      return connection.ExecuteAsync(sql, new
      {
        DealerId = int.Parse(dealerId.ToString()),
        DqlFilter = new ConditionalDqlFilterBuilder(campaign).Build(),
        IsOnSrp = ConditionalBanner.IsOnSrp(campaign.Page),
        IsOnVdp = ConditionalBanner.IsOnVdp(campaign.Page),
        campaign.WhenStarts,
        campaign.WhenEnds,
        Position = position,
        AssetPath = path,
        Link = campaign.Link.ToString(),
        campaign.AltText
      });
    }

    protected override Task DeleteDealerBanners(Id dealerId, IDbConnection connection)
    {
      var sql = @"
delete from
  DealerConditionalBanners
where
  DealerId = @DealerId
";

      return connection.ExecuteAsync(sql, new
      {
        DealerId = int.Parse(dealerId.ToString())
      });
    }
  }
}