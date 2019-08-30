using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorUI.Client.Pages.Components
{
    public class ImportComponent : ComponentBase
    {
        [Inject] public HttpClient _http { get; set; }
        [Inject] public AppState _appState { get; set; }
        
    }
}
