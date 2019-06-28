using System;
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
  /// The DealerVehiclePhotos table containing CAM banners for the home page
  /// </summary>
  public class HomeBannerTable : BannerTable
  {
    public HomeBannerTable(IBannerPath path) : base(path)
    {}

    protected override bool ContainsCampaign(CampaignDbCall.Campaign campaign) =>
      campaign.Page == DealerManifest.Page.Home;

    protected override async Task<IEnumerable<IBanner>> SelectBanners(IDbConnection connection)
    {
      var sql = @"
select
  DVP.dealer_vehicle_photo_id,
  DVP.dealer_id,
  DVP.link,
  DVP.alttext,
  DVP.start_dt,
  DVP.end_dt,
  DVP.OriginalLink,
  DVP.display_order,
  DVP.is_campaign,
  DVP.path
from
  dbo.DealerVehiclePhotos DVP
  inner join Dealeron..DealerMake DM on DVP.dealer_id = DM.DealerID
where
  DM.Make = 'Toyota'
order by
  DVP.display_order
";

      return
        from result in await connection.QueryAsync(sql)
        select new HomeBanner(
          connection,
          (int) result.dealer_vehicle_photo_id,
          Id.From((int) result.dealer_id),
          (int) result.display_order,
          (bool) result.is_campaign,
          (string) result.path,
          (string) result.link,
          (string) result.alttext,
          (DateTime?) result.start_dt,
          (DateTime?) result.end_dt,
          (bool) result.OriginalLink);
    }

    protected override Task InsertBanner(Id dealerId, CampaignDbCall.Campaign campaign, int position, string path, IDbConnection connection)
    {
      var sql = @"
declare @LibraryPhotoId int

set @LibraryPhotoId = (select top 1 id from VehiclePhotos where path = @Path)

insert into DealerVehiclePhotos
(
  dealer_id,
  is_library_photo,
  library_photo_id,
  gallery_photo_id,
  display_order,
  is_campaign,
  path,
  link,
  delay,
  start_dt,
  end_dt,
  comments,
  float_Position,
  float_width,
  link_blank,
  alttext,
  hideMobile,
  hideDesktop,
  cam_is_optional,
  cam_is_opted_out
)
values
(
  @DealerId,
  1,
  @LibraryPhotoId,
  0,
  @Position,
  1,
  @Path,
  @Link,
  5000,
  @WhenStarts,
  @WhenEnds,
  '',
  '',
  '',
  0,
  @AltText,
  0,
  0,
  @IsOptional,
  @IsOptedOut
)
";

      return connection.ExecuteAsync(sql, new
      {
        DealerId = int.Parse(dealerId.ToString()),
        Path = path,
        Position = position,
        Link = campaign.Link.ToString(),
        campaign.WhenStarts,
        campaign.WhenEnds,
        campaign.AltText,
        IsOptional = !campaign.Required,
        IsOptedOut = !campaign.Required
      });
    }

    protected override Task DeleteDealerBanners(Id dealerId, IDbConnection connection)
    {
      var sql = @"
delete from
  DealerVehiclePhotos
where
  dealer_id = @DealerId and is_campaign = 1

update
  DealerVehiclePhotos
set
  display_order = RowNumber
from
(
  select
    dealer_vehicle_photo_id,
    row_number() over (order by display_order) as RowNumber
  from
    DealerVehiclePhotos
  where
    dealer_id = @DealerId
) as Numbered
where
    DealerVehiclePhotos.dealer_vehicle_photo_id = Numbered.dealer_vehicle_photo_id
";

      return connection.ExecuteAsync(sql, new
      {
        DealerId = int.Parse(dealerId.ToString())
      });
    }
  }
}