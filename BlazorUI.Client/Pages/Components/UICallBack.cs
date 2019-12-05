using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BlazorUI.Client.Pages.Components
{
    public class UICallBack
    {
        public UICallBack(MethodInfo handler, object instance, Type type)
        {
            Handler = handler;
            Instance = instance;
            InstanceType = type;
        }
        public Type InstanceType;
        public MethodInfo Handler;
        public object Instance;
    }
}
