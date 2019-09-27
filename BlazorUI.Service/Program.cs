using BlazorUI.Shared;
using System;
using System.Threading.Tasks;
using Totem.App.Service;

namespace BlazorUI.Service
{
    class Program
    {
        public static Task Main(string[] args) => ServiceApp.Run<ApplicationArea>(services => services.AddApplication());
    }
}
