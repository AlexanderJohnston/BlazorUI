using BlazorUI.Service.Models;
using BlazorUI.Shared.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorUI.Service.Data
{
    public class DatabaseService : ILegacyEventContext
    {
        public DatabaseService(IConfiguration configuration)
        {
            Configuration = configuration;
            _connection = Configuration["LegacyEvents:ConnectionString"];
        }

        public IConfiguration Configuration { get; set; }

        public TorqueQAContext Context { get; set; }

        private readonly string _connection;

        public async Task<List<Event>> GetEvents()
        {
            using (var context = new TorqueQAContext(_connection))
            {
                return await context.Event.Where(e => e.Position < 10).ToListAsync();
            }
        }
    }
}
