﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorUI.Server.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public class TimelineQueryAttribute : Attribute
    {
        public Type QueryType { get; set; } 
        public TimelineQueryAttribute(Type queryType)
        {
            QueryType = queryType;
        }
    }
}