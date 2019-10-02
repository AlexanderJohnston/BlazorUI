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
        public DatabaseService(string connection)
        {
            _connection = connection;
        }

        public TorqueQAContext Context { get; set; }

        private readonly string _connection;

        public async Task<List<TotemV1Event>> GetEvents()
        {
            using (var context = new TorqueQAContext(_connection))
            {
                return await context.Event.Where(e => e.Position < 10).ToListAsync();
            }
        }
    }
}
