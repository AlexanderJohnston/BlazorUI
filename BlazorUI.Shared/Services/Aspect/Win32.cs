using PostSharp.Patterns.Diagnostics;
using System;
using System.Runtime.InteropServices;
using System.Security;

namespace BlazorUI.Shared.Services.Aspect
{
    //[Log(AttributeExclude = true)]
    internal static class Win32
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [SuppressUnmanagedCodeSecurity]
        internal static extern bool GetThreadTimes(IntPtr threadHandle, out long creationTime, out long exitTime, out long kernelTime, out long userTime);

        [DllImport("kernel32.dll")]
        [SuppressUnmanagedCodeSecurity]
        internal static extern IntPtr GetCurrentThread();

        internal const double GetThreadTimesFrequency = 10_000_000;

    }
}
