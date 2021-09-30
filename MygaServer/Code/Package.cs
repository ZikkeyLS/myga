using MessagePack;
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

    public static class PackageAddon
    {
        public static Package Copy(this Package package)
        {
            return new Package(package.buffer);
        }
    }

    [MessagePackObject]
    public class MyClass
    {
        // Key attributes take a serialization index (or string name)
        // The values must be unique and versioning has to be considered as well.
        // Keys are described in later sections in more detail.
        [Key(0)]
        public int Age { get; set; }

        [Key(1)]
        public string FirstName { get; set; }

        [Key(2)]
        public string LastName { get; set; }

        // All fields or properties that should not be serialized must be annotated with [IgnoreMember].
        [IgnoreMember]
        public string FullName { get { return FirstName + LastName; } }
    }
}
