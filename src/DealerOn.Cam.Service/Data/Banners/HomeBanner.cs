using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using DealerOn.Cam.Data;
using Totem;

namespace DealerOn.Cam.Service.Data.Banners
{
  /// <summary>
  /// A banner associated with a dealer in the DealerVehiclePhotos table
  /// </summary>
  public sealed class HomeBanner : IBanner
	{
    readonly IDbConnection _connection;
    readonly int _dealerVehiclePhotoId;
    readonly string _link;
    readonly string _altText;
    readonly DateTime? _whenStarts;
    readonly DateTime? _whenEnds;
    readonly bool _originalLink;

    public HomeBanner(
      IDbConnection connection,
      int dealerVehiclePhotoId,
      Id dealerId,
      int position,
      bool isCampaign,
      string path,
      string link,
			string altText,
			DateTime? whenStarts,
			DateTime? whenEnds,
			bool originalLink)
		{
      _connection = connection;
      _dealerVehiclePhotoId = dealerVehiclePhotoId;
      DealerId = dealerId;
			Position = position;
      IsCampaign = isCampaign;
      Path = path;
      _link = link;
      _altText = altText;
      _whenStarts = whenStarts;
      _whenEnds = whenEnds;
      _originalLink = originalLink;
		}

    public Id DealerId { get; }
    public int Position { get; }
    public bool IsCampaign { get; }
    public string Path { get; }

    public Task Update(CampaignDbCall.Campaign campaign, string assetPath)
    {
      var sql = @"
declare @LibraryPhotoId int

set @LibraryPhotoId = (select top 1 id from VehiclePhotos where path = @AssetPath)

update
  DealerVehiclePhotos
set
  library_photo_id = @LibraryPhotoId,
  path = @AssetPath,
  link = @Link,
  start_dt = @WhenStarts,
  end_dt = @WhenEnds,
  alttext = @AltText,
  cam_is_optional = @IsOptional
where
  dealer_vehicle_photo_id = @DealerVehiclePhotoId
";

      return _connection.ExecuteAsync(sql, new
      {
        AssetPath = assetPath,
        Link = !_originalLink ? _link : campaign.Link.ToString(),
        campaign.WhenStarts,
        campaign.WhenEnds,
        campaign.AltText,
        IsOptional = !campaign.Required,
        DealerVehiclePhotoId = _dealerVehiclePhotoId
      });
    }

    public Task Move(int position)
    {
      var sql = @"
update
  DealerVehiclePhotos
set
  display_order = @Position
where
  dealer_vehicle_photo_id = @DealerPhotoId
";

      return  _connection.ExecuteAsync(sql, new
      {
        Position = position,
        DealerPhotoId = _dealerVehiclePhotoId
      });
    }

    public Task Delete()
    {
      var sql = @"
delete from
  DealerVehiclePhotos
where
  dealer_vehicle_photo_id = @DealerVehiclePhotoId
";

      return _connection.ExecuteAsync(sql, new
      {
        DealerVehiclePhotoId = _dealerVehiclePhotoId
      });
    }
  }
}