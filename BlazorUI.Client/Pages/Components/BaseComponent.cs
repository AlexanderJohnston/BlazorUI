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
            var properties = this.GetType().GetProperties();
            var queries = properties.Where(prop => prop.PropertyType.IsSubclassOf(typeof(Query)));
            foreach (var query in queries) 
            {
                Console.WriteLine($"Subscribing {query} to the app state!!");
                var type = query.PropertyType;
                Console.WriteLine($"Type is {type}");
                MethodInfo subscribe = typeof(AppState).GetMethod("Subscribe", BindingFlags.Public | BindingFlags.Instance);
                MethodInfo genericSubscribe = subscribe.MakeGenericMethod(type);
                Console.WriteLine($"Subscription Invoke: {genericSubscribe}");

                Console.WriteLine("Read Query Method");
                MethodInfo read = typeof(BaseComponent<T>).GetMethod("ReadQuery", BindingFlags.Public | BindingFlags.Instance);
                MethodInfo genericRead = read.MakeGenericMethod(type);
                Console.WriteLine($"Read Invoke: {genericRead}");

                Console.WriteLine($"AppState Not Null: {_appState != null}");

                //var expression = Expression.Lambda<Func<object, Task>>(Expression.Call(genericRead)).Compile();
                Console.WriteLine("Calling AppState.Subscription() with the ReadQuery callback as parameter 0.");
                genericSubscribe.Invoke(_appState, new object[]{ genericRead, this, null });


                // It's compile time baby!!
                //var input = Expression.Parameter(typeof(object), "input");
                //var expression = Expression.Lambda<Func<object, Task>>(
                //    Expression.Call(
                //        Expression.Constant(_appState), genericSubscribe))
                //    .Compile()((BaseComponent<T>)this, new object[2] { genericRead, null });
                //generic.Invoke((BaseComponent<T>)this, new object[2]{genericRead, null});
            }
        }

        public BaseComponent() 
        {
            
        }

        // protected async override Task OnInitializedAsync()
        // {
        //     await _appState.Subscribe<EchoQuery>(ReadQuery<EchoQuery>);
        //     StateHasChanged();
        // }

        public async Task ReadQuery<T>(object query)
        {
            //this.Query = (T)query;
            Console.WriteLine("Are we good to?");
            //StateHasChanged();
            var type = typeof(T);
            Console.WriteLine(type);
            // MyObject obj = new MyObject();
            // obj.GetType().InvokeMember(type.ToString(),
            //     BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty,
            //     Type.DefaultBinder, obj, "Value");
        }
    }
}