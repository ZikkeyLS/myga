using System.IO;

namespace MygaClient
{
    public class Package
    {
        public int id { get; private set; } = 0;
        public byte[] buffer { get; private set; } = new byte[4096];
        public MemoryStream stream { get; private set; }
        public BinaryWriter writer { get; private set; }
        public BinaryReader reader { get; private set; }

        public Package(int id)
        {
            stream = new MemoryStream(buffer);
            writer = new BinaryWriter(stream);
            reader = new BinaryReader(stream);

            this.id = id;
            writer.Write(id);
        }

        public Package(byte[] bytes)
        {
            buffer = bytes;
            stream = new MemoryStream(buffer);
            writer = new BinaryWriter(stream);
            reader = new BinaryReader(stream);

            id = reader.ReadInt32();
        }
    }

    public static class PackageAddon
    {
        public static Package Copy(this Package package)
        {
            return new Package(package.buffer);
        }
    }
}
