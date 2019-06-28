using System.Threading.Tasks;
using Dapper;
using DealerOn.Cam.Data;
using Totem;

namespace DealerOn.Cam.Service.Data
{
  /// <summary>
  /// The database containing dealer campaign data
  /// </summary>
  public sealed class EligibleDealerDb : IEligibleDealerDb
  {
    readonly IDealerOnDb _dealerOnDb;

    public EligibleDealerDb(IDealerOnDb dealerOnDb)
    {
      _dealerOnDb = dealerOnDb;
    }

    public async Task<Many<EligibleDealer>> GetEligibleDealers()
    {
      // Dealer 10919 has two host headers with order 1, causing it to appear twice in
      // the results. When this happens, we choose the first one. We do this in a
      // subquery to filter dealers without any host headers (usually example sites).

      var results = await _dealerOnDb.ExecuteFunc(connection => connection.QueryAsync(@"
select
	DealerID,
	RegionCode,
	DealerCode,
	Name,
	Hostname
from
	(select
		DOP.DealerID,
		DOP.RegionCode,
		DOP.DealerCode,
		D.Name,
		(
			select top 1
				hostname
			from
				DealerHeader
			where
				dealer_id = DOP.DealerID
				and hostname is not null
				and hostname <> ''
				and host_order = 1
		) as Hostname
	from
		EDealer..DealerOemPrograms DOP
		inner join EDealer..Dealers D on DOP.DealerID = D.DealerID
		inner join Dealeron_Setup DS on DOP.DealerID = DS.dealer_id
		inner join WebTemplate WT on DS.Template_ID = WT.Template_ID
	where
		DOP.Make = 'Toyota'
		and DOP.DealerCode <> ''
		and DS.status = 1
		and DS.dealer_type = 1
		and WT.Version = 4
	) as Dealers
where
	Hostname is not null
order by
	DealerID
"));

      return results.ToMany(result => new EligibleDealer(
        Id.From((int) result.DealerID),
        ((string) result.DealerCode).PadLeft(5, '0'),
        (string) result.Name,
        (string) result.RegionCode,
        (string) result.Hostname));
    }
  }
}