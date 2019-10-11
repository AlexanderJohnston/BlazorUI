using PostSharp.Patterns.Diagnostics;
using System.Linq;
using System.Reflection;

namespace BlazorUI.Shared.Services.Metrics
{
    [Log(AttributeExclude = true)]
    public class CapturedMetadata
    {
        internal int Index { get; }
        public string Name { get; }

        public CapturedMetadata(MethodBase method, int index)
        {
            Index = index;
            Name = method.DeclaringType.FullName
                 + "."
                 + method.Name
                 + "("
                 + string.Join(",", method.GetParameters().Select(p => p.ParameterType.FullName))
                 + ")";
        }
    }
}
