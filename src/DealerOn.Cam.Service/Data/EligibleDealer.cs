using Totem;

namespace DealerOn.Cam.Service.Data
{
  /// <summary>
  /// A dealer in the DealerOn database eligible for enrollment for CAM enrollment
  /// </summary>
  public class EligibleDealer
  {
    public EligibleDealer(Id id, string code, string name, string region, string hostname)
    {
      Id = id;
      Code = code;
      Name = name;
      Region = region;
      Hostname = hostname;
    }

    public readonly Id Id;
    public readonly string Code;
    public readonly string Name;
    public readonly string Region;
    public readonly string Hostname;
  }
}