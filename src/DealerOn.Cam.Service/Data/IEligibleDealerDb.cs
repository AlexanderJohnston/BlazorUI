using System.Threading.Tasks;
using Totem;

namespace DealerOn.Cam.Service.Data
{
  /// <summary>
  /// Describes the DealerOn database containing dealers eligible for CAM enrollment
  /// </summary>
  public interface IEligibleDealerDb
  {
    Task<Many<EligibleDealer>> GetEligibleDealers();
  }
}