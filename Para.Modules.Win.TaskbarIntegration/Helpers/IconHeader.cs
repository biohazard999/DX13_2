using System.IO;

namespace Para.Modules.Win.TaskbarIntegration.Helpers
{
    internal struct IconHeader
    {
        public IconHeader(BinaryReader reader)
            : this()
        {
            Reserved = reader.ReadInt16();
            Type = reader.ReadInt16();
            Count = reader.ReadInt16();
        }

        public short Reserved { get; set; }

        public short Type { get; set; }

        public short Count { get; set; }

        public void Save(BinaryWriter writer)
        {
            writer.Write(Reserved);
            writer.Write(Type);
            writer.Write(Count);
        }
    }
}
