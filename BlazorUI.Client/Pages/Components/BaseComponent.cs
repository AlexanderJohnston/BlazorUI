using System.Security.AccessControl;
using System.Runtime.CompilerServices;
using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Totem.Timeline;
using System.Diagnostics;

namespace BlazorUI.Client.Pages.Components
{
    public class BaseComponent<T> : ComponentBase
    {
        [Inject] public HttpClient _http { get; set; }
        [Inject] public AppState _appState { get; set; }
        protected async override Task OnInitializedAsync()
        {
            Console.WriteLine("Initialized Properties");
            var properties = GetType().GetProperties();
            var queries = properties.Where(prop => prop.PropertyType.IsSubclassOf(typeof(Query)));
            foreach (var query in queries) 
            {
                Console.WriteLine($"Subscribing {query.Name} to the app state from {GetType().Name}.");
                var type = query.PropertyType;
                var flags = BindingFlags.Public | BindingFlags.Instance;
                MethodInfo subscribe = typeof(AppState).GetMethod("Subscribe", flags);
                MethodInfo genericSubscribe = subscribe.MakeGenericMethod(type);

                Console.WriteLine("Calling AppState.Subscription() with the ReadQuery callback as parameter 0.");
                var caller = this;
                object queryRouteId = null;
                var finalizedCallback = new UICallBack(caller, GetType(), query);
                genericSubscribe.Invoke(_appState, new object[]{ finalizedCallback, queryRouteId});
            }
        }
        public BaseComponent() { }
    }
}