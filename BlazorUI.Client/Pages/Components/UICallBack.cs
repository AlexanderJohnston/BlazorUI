using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BlazorUI.Client.Pages.Components
{
    public class UICallBack
    {
        public UICallBack(object instance, Type componentType, Type propertyType)
        {
            Instance = instance;
            InstanceType = componentType;
            AssignableProperty = componentType.GetProperties().Where(prop => prop.PropertyType == propertyType).FirstOrDefault();
            Debug.Assert(AssignableProperty != null, 
                $"{Environment.NewLine}[####]{Environment.NewLine}Couldn't create UICallBack binding of {propertyType.Name} in {componentType}. " +
                $"{Environment.NewLine}Check the AssignableProperty variable in the constructor for UICallBack.{Environment.NewLine}[####]");
        }
        public Type InstanceType;
        public object Instance;
        public PropertyInfo AssignableProperty;
    }
}
