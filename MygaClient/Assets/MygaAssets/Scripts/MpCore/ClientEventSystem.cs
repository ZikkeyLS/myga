using MygaCross;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MygaClient
{
    public delegate void PackageRecieved(byte[] data);

    public class PackageRecievedData
    {
        public PackageRecieved packageRecieved;
        public string type;

        public PackageRecievedData(PackageRecieved packageRecieved, string type)
        {
            this.packageRecieved = packageRecieved;
            this.type = type;
        }
    }

    public static class ClientEventSystem
    {
        public static HashSet<PackageRecievedData> packageEvents = new HashSet<PackageRecievedData>();

        public static void OnPackageRecieved(PackageRecieved packageRecieved, string packageType = "Any")
        {
            packageEvents.Add(new PackageRecievedData(packageRecieved, packageType));
        }

        public static void PackageRecieved(byte[] data)
        {
            foreach (PackageRecievedData recievedData in packageEvents)
                if (new CheckerPackage(data).typeOf(recievedData.type) || recievedData.type == "Any")
                    recievedData.packageRecieved(data);
        }
    }
}
