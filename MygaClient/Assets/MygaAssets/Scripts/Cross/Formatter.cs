using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MygaClient
{
    public static class Formatter
    {
        public static bool OfType<T>(object obj)
        {
            return obj is T;   
        }
    }
}
