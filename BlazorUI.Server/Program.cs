using DealerOn.Cam;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Totem.App.Web;

namespace BlazorUI.Server
{
    public class Program
    {
        public static Task Main() => WebApp.Run<CamArea>();
    }

}
