using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorUI.Client.totem_timeline;

namespace BlazorUI.Client.totem_timeline_signalr
{
    public class QueryHub
    {
        /*private List<Query> _queries { get; set; }

        public async Task Enable(HubConnection connection, List<Query> queries)
        {
            _queries = queries;
            foreach (var query in _queries)
            {
                connection.On<string>(query.Type, async (message) => { await query.ReadQueryAsync(message); });
            }
            await connection.StartAsync();
        }*/
    }
}
