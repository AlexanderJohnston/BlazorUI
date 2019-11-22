using BlazorUI.Service.Models;
using BlazorUI.Shared.Data;
using BlazorUI.Shared.Services.Aspect;
using Microsoft.EntityFrameworkCore;
using PostSharp.Patterns.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorUI.Service.Data
{
    //[Log(AttributeExclude=true)]
    public class DatabaseService : ILegacyEventContext
    {
        public DatabaseService(string connection)
        {
            _connection = connection;
        }

        public TorqueQAContext Context { get; set; }

        private readonly string _connection;

        [Profile]
        public async Task<List<TotemV1Event>> GetEvents(int count, int checkpoint)
        {
            var contextBuilder = new DbContextOptionsBuilder<TorqueQAContext>();
            contextBuilder.UseSqlServer(_connection);
            using (var context = new TorqueQAContext(contextBuilder.Options))
            {
                return await context.Event.OrderBy(e => e.Position).Skip(checkpoint).Take(count).ToListAsync();
            }
        }
    }
}
