using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace MygaClient
{
    public class Package : IDisposable
    {
        private string parsedPackageData;
        protected PackageReader reader;
        public string packageType { get; private set; }

        public Package(string typeName = "Package")
        {
            packageType = typeName;
            Write(typeName);
        }

        public Package(byte[] bytes)
        {
            parsedPackageData = Encoding.UTF8.GetString(bytes);
            reader = new PackageReader(parsedPackageData);

            packageType = reader.ReadString();
        }

        public void Dispose()
        {
            reader = null;
        }

        public void Write(object element)
        {
            parsedPackageData += $"{element};";
        }

        public void Clear()
        {
            parsedPackageData = string.Empty;
        }

        public byte[] ToBytes()
        {
            return Encoding.ASCII.GetBytes(parsedPackageData);
        }

        public bool typeOf(string packageType)
        {
            return this.packageType == packageType;
        }
    }

    public class CheckerPackage : IDisposable
    {
        private string parsedPackageData;
        protected PackageReader reader;

        public string packageType { get; private set; }

        public CheckerPackage(byte[] bytes)
        {
            parsedPackageData = Encoding.UTF8.GetString(bytes);
            reader = new PackageReader(parsedPackageData);

            packageType = reader.ReadString();
        }

        public bool typeOf(string packageType)
        {
            return this.packageType == packageType;
        }

        public void Dispose()
        {
            reader = null;
        }
    }

    public class PackageReader
    {
        public string[] values;
        public int index = 0;

        public PackageReader(string parsedPackageData)
        {
            values = parsedPackageData.Split(';');
        }

        public int ReadInt()
        {
            return Convert.ToInt32(ReadString());
        }

        public float ReadFloat()
        {
            return Convert.ToInt64(ReadString());
        }

        public long ReadLong()
        {
            return Convert.ToInt64(ReadString());
        }

        public string ReadString()
        {
            if (OverIndexException())
                return string.Empty;

            string result = values[index];
            index++;
            return result;
        }

        private bool OverIndexException()
        {
            bool result = values.Length - 1 == index;
            if (result)
                Console.WriteLine("Error: index equals to values count. Returning a default value...", ConsoleColor.Red);
            return result;
        }
    }

    public static class PackageAddon
    {
        public static Package Copy(this Package package) => new Package(package.ToBytes());
    }
}
