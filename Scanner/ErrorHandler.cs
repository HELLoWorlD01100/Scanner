using System;
using System.Diagnostics;

namespace Scanner
{
    public static class ErrorHandler
    {
        public static void ThrowError(string text)
        {
            Console.WriteLine(text);
            Process.GetCurrentProcess().Kill();
        }
    }
}