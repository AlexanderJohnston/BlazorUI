using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorUI.Service.Native
{
    public interface INativeClass
    {
        Task Initialize<T>([CanBeNull]T parent);

        Task Terminate(string reason);
    }
}
