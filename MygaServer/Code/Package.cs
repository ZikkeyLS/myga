using System.IO;

namespace MygaServer
{
    public class Package
    {
        public byte[] buffer { get; private set; } = new byte[4096];
        public MemoryStream stream { get; private set; }
        public BinaryWriter writer { get; private set; }
        public BinaryReader reader { get; private set; }

        public Package(int id)
        {
            stream = new MemoryStream(buffer);
            writer = new BinaryWriter(stream);
            reader = new BinaryReader(stream);

            writer.Write(id);
        }

        public Package(byte[] bytes)
        {
            buffer = bytes;
            stream = new MemoryStream(buffer);
            writer = new BinaryWriter(stream);
            reader = new BinaryReader(stream);
        }
    }
}
