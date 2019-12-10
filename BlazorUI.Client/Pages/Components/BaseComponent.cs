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
                Console.WriteLine($"Subscribing the property {query.Name} to receive queries of {query.PropertyType.Name} on the {typeof(T).Name}.");
                var propType = query.PropertyType;
                var flags = BindingFlags.Public | BindingFlags.Instance;
                MethodInfo subscribe = typeof(AppState).GetMethod("Subscribe", flags);
                MethodInfo genericSubscribe = subscribe.MakeGenericMethod(propType);

                Console.WriteLine($"Calling AppState.Subscription() with the relevant callback information for {query.Name} on {typeof(T).Name}.");
                var caller = this;
                string queryRouteId = null;
                var finalizedCallback = new UICallBack(caller, GetType(), propType);
                genericSubscribe.Invoke(_appState, new object[]{ finalizedCallback, queryRouteId});
            }
        }
        public BaseComponent() { }
    }
}