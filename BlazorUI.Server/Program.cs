using System;
using System.Reflection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Totem.App.Web;
using BlazorUI.Client.Campaign;

namespace BlazorUI.Server
{
    public class Program
    {
        public static Task Main()
        {
            Type type = MethodBase.GetCurrentMethod().DeclaringType;
            return WebApp.Run<CamArea>(Assembly.GetAssembly(type));
        }
    }

}
