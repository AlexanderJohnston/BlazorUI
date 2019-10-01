using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorUI.Shared.Data
{
    public interface ILegacyEventContext
    {
        Task<List<Event>> GetEvents();
    }
}
