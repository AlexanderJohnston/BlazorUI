using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorUI.Server.Attributes
{
    /// <summary>
    ///     Place this attribute on the same property as a <see cref="HttpMethodAttribute"/> within a <see cref="Controller"/> class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public class TimelineQueryAttribute : Attribute
    {
        public Type QueryType { get; set; }
        /// <summary>
        ///     Creates metadata attribute which maps a Route on a Controller to a specific Query type.
        ///     Clients can then match Query types back to a Route if they have a reference to <see cref="BlazorUI.Shared"/>.
        /// </summary>
        /// <param name="queryType"></param>
        public TimelineQueryAttribute(Type queryType)
        {
            QueryType = queryType;
        }
    }
}
