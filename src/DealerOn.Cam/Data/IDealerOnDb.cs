using System;
using System.Data;
using System.Threading.Tasks;

namespace DealerOn.Cam.Data
{
  /// <summary>
  /// Describes the Dealeron database containing dealer and campaign data
  /// </summary>
  public interface IDealerOnDb
  {
    Task<bool> TestConnection();

    Task ExecuteAction(Func<IDbConnection, Task> action);

    Task<TResult> ExecuteFunc<TResult>(Func<IDbConnection, Task<TResult>> func);
  }
}