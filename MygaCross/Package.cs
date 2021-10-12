using System;
using System.Text;

namespace MygaCross
{
    public class Package : IDisposable
    {
        protected string parsedPackageData;
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
            return Encoding.UTF8.GetBytes(parsedPackageData);
        }

        public bool typeOf(string packageType)
        {
            return this.packageType == packageType;
        }

        public void Dispose()
        {
            reader = null;
            parsedPackageData = null;
            packageType = null;
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
            parsedPackageData = null;
            packageType = null;
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
            return values.Length - 1 == index;
        }
    }

    public static class PackageAddon
    {
        public static Package Copy(this Package package) => new Package(package.ToBytes());
    }
}
