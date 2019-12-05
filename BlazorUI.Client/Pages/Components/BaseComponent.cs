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
                Console.WriteLine($"Subscribing {query} to the app state!!");
                var type = query.PropertyType;
                var flags = BindingFlags.Public | BindingFlags.Instance;
                MethodInfo subscribe = typeof(AppState).GetMethod("Subscribe", flags);
                MethodInfo genericSubscribe = subscribe.MakeGenericMethod(type);
                MethodInfo read = GetType().GetMethod("ReadQuery", flags);
                MethodInfo genericRead = read.MakeGenericMethod(type);

                Console.WriteLine("Calling AppState.Subscription() with the ReadQuery callback as parameter 0.");
                var caller = this;
                object queryRouteId = null;
                genericSubscribe.Invoke(_appState, new object[]{ genericRead, caller, queryRouteId, GetType() });
            }
        }
        public BaseComponent() { }
    }
}