using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BlazorUI.Client.Pages.Components
{
    public class UICallBack
    {
        public UICallBack(object instance, Type type, PropertyInfo property)
        {
            Instance = instance;
            InstanceType = type;
            AssignableProperty = property;
        }
        public Type InstanceType;
        public object Instance;
        public PropertyInfo AssignableProperty;
    }
}
