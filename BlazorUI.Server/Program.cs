using System;
using System.Reflection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Totem.App.Web;
using DealerOn.Cam;
using Microsoft.AspNetCore.Builder;

namespace BlazorUI.Server
{
    public class Program
    {
        public static Task Main()
        {
            Type type = MethodBase.GetCurrentMethod().DeclaringType;
            /*WebApp.Run<CamArea>(Configure.App(configure => 
            { 
                configure.UseClientSideBlazorFiles<BlazorUI.Client.Startup>(); 
            }));*/
            return WebApp.Run<CamArea>(Assembly.GetAssembly(type));
        }
    }

}
