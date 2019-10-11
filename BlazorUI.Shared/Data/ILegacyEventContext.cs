using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorUI.Shared.Data
{
    public interface ILegacyEventContext
    {
        Task<List<TotemV1Event>> GetEvents(int count, int checkpoint);
    }
}
