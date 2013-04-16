using System.IO;
using Domain.Core.Infrastructure;

namespace Infrastructure
{
    internal static class SerializeExtensions
    {
        public static byte[] Serialize(this ISerialize serializer, object value)
        {
            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream, value);
                return stream.ToArray();
            }
        }

        public static T Deserialize<T>(this ISerialize serializer, byte[] serialized)
        {
            serialized = serialized ?? new byte[] { };

            if (serialized.Length == 0)
                return default(T);

            using (var stream = new MemoryStream(serialized))
                return serializer.Deserialize<T>(stream);
        }
    }
}