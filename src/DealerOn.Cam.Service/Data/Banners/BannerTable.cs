using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using DealerOn.Cam.Data;
using Totem;

namespace DealerOn.Cam.Service.Data.Banners
{
  /// <summary>
  /// Base implementation of a table containing CAM banners
  /// </summary>
  public abstract class BannerTable : IBannerTable
  {
    readonly IBannerPath _path;

    protected BannerTable(IBannerPath path)
    {
      _path = path;
    }

    public async Task ImportBanners(CampaignDbCall call, IDbConnection connection)
    {
      await ImportDealers(call, connection);

      await DeleteUnenrolledDealers(call, connection);
    }

    async Task ImportDealers(CampaignDbCall call, IDbConnection connection)
    {
      var banners = await SelectBanners(connection);

      var dealers =
        from dealer in call.Dealers
        join banner in banners on dealer.Id equals banner.DealerId into dealerBanners
        select (dealer.Id, new BannerImporter(this, dealer, dealerBanners, connection));

      foreach(var (dealerId, importer) in dealers)
      {
        try
        {
          await importer.Import();
        }
        catch(Exception error)
        {
          call.AddError(dealerId, error);
        }
      }
    }

    async Task DeleteUnenrolledDealers(CampaignDbCall call, IDbConnection connection)
    {
      foreach(var unenrolledDealerId in call.UnenrolledDealerIds)
      {
        try
        {
          await DeleteDealerBanners(unenrolledDealerId, connection);
        }
        catch(Exception error)
        {
          call.AddError(unenrolledDealerId, error);
        }
      }
    }

    protected abstract Task<IEnumerable<IBanner>> SelectBanners(IDbConnection connection);

    protected abstract bool ContainsCampaign(CampaignDbCall.Campaign campaign);

    protected abstract Task InsertBanner(
      Id dealerId,
      CampaignDbCall.Campaign campaign,
      int position,
      string path,
      IDbConnection connection);

    protected abstract Task DeleteDealerBanners(Id dealerId, IDbConnection connection);

    class BannerImporter
    {
      readonly BannerTable _table;
      readonly Id _dealerId;
      readonly IEnumerable<IBanner> _banners;
      readonly IDbConnection _connection;
      readonly Dictionary<CampaignDbCall.Campaign, string> _pathsByCampaign;

      internal BannerImporter(
        BannerTable table,
        CampaignDbCall.Dealer dealer,
        IEnumerable<IBanner> banners,
        IDbConnection connection)
      {
        _table = table;
        _dealerId = dealer.Id;
        _banners = banners;
        _connection = connection;

        _pathsByCampaign = dealer
          .Campaigns
          .Where(table.ContainsCampaign)
          .ToDictionary(
            campaign => campaign,
            campaign => table._path.GetPath(campaign.AssetName));
      }

      internal async Task Import()
      {
        var banners =
          from banner in _banners
          join path in _pathsByCampaign.Values on banner.Path equals path into campaignPaths
          from campaignPath in campaignPaths.DefaultIfEmpty()
          orderby banner.Position
          select (banner, campaignPath == null);

        var remainingBanners = new List<IBanner>();

        foreach(var (banner, removed) in banners)
        {
          if(banner.IsCampaign && removed)
          {
            await banner.Delete();
          }
          else
          {
            remainingBanners.Add(banner);
          }
        }

        await AdjustBanners(remainingBanners);
      }

      async Task AdjustBanners(List<IBanner> banners)
      {
        var addedCampaigns = new List<CampaignDbCall.Campaign>();

        var campaigns =
          from campaign in _pathsByCampaign.Keys
          let path = _pathsByCampaign[campaign]
          join banner in banners on path equals banner.Path into campaignBanners
          from banner in campaignBanners.DefaultIfEmpty()
          select (campaign, path, banner);

        foreach(var (campaign, path, banner) in campaigns)
        {
          if(banner != null)
          {
            await banner.Update(campaign, path);
          }
          else
          {
            addedCampaigns.Add(campaign);
          }
        }

        await InsertOrMoveBanners(banners, addedCampaigns);
      }

      async Task InsertOrMoveBanners(List<IBanner> banners, List<CampaignDbCall.Campaign> addedCampaigns)
      {
        var position = 1;

        for(int i = 0, n = banners.Count + addedCampaigns.Count; i < n; i++)
        {
          var banner = banners.FirstOrDefault();
          var addedCampaign = addedCampaigns.FirstOrDefault();

          if(addedCampaign != null && (banner == null || !banner.IsCampaign || banner.Position >= position))
          {
            addedCampaigns.RemoveAt(0);

            if(!addedCampaign.AssetFailed)
            {
              await _table.InsertBanner(
                _dealerId,
                addedCampaign,
                position,
                _pathsByCampaign[addedCampaign],
                _connection);

              position++;
            }
          }
          else
          {
            banners.RemoveAt(0);

            if(banner.Position != position)
            {
              await banner.Move(position);
            }

            position++;
          }
        }
      }
    }
  }
}