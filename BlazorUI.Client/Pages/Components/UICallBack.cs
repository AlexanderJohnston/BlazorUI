using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BlazorUI.Client.Pages.Components
{
    public class UICallBack
    {
        public UICallBack(MethodInfo handler, object instance, Type type, PropertyInfo property)
        {
            Handler = handler;
            Instance = instance;
            InstanceType = type;
            AssignableProperty = property;
        }
        public Type InstanceType;
        public MethodInfo Handler;
        public object Instance;
        public PropertyInfo AssignableProperty;
    }
}
