using System.Threading.Tasks;
using Dapper;
using DealerOn.Cam.Data;

namespace DealerOn.Cam.Service.Data
{
  /// <summary>
  /// The database containing campaign assets
  /// </summary>
  public sealed class AssetDb : IAssetDb
  {
    readonly IDealerOnDb _dealerOnDb;
    readonly IBannerPath _bannerPath;

    public AssetDb(IDealerOnDb dealerOnDb, IBannerPath bannerPath)
    {
      _dealerOnDb = dealerOnDb;
      _bannerPath = bannerPath;
    }

    public Task Merge(DealerManifest.Asset asset) =>
      _dealerOnDb.ExecuteAction(connection =>
      {
        var sql = $@"
update
	VehiclePhotos
set
	caption = @Caption
where
	path = @Path

if @@rowcount = 0
begin
  insert into VehiclePhotos
  (
	  year,
    make,
    model,
    number,
    path,
    caption,
    responsive,
	  is_campaign
  )
  values
  (
	  null,
	  'toyota',
	  null,
	  1,
	  @Path,
	  @Caption,
	  0,
	  1
  )
end
";

        return connection.ExecuteAsync(sql, new
        {
          Path = _bannerPath.GetPath(asset.File.Name),
          asset.Caption
        });
      });
  }
}