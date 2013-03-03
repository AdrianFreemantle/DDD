using System.IO;
using System.Runtime.Serialization;
using System.Text;

using Newtonsoft.Json;

namespace Infrastructure
{
    public class JsonSerializer
    {
        private readonly Newtonsoft.Json.JsonSerializer serializer;
        private readonly Encoding encoding = Encoding.UTF8;

        public JsonSerializer()
        {
            serializer = new Newtonsoft.Json.JsonSerializer
            {
                TypeNameHandling = TypeNameHandling.All,
                DefaultValueHandling = DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple
            };
        }

        public JsonSerializer(Newtonsoft.Json.JsonSerializer serializer)
        {
            this.serializer = serializer;
        }

        public virtual void Serialize<T>(Stream output, T graph)
        {
            using (TextWriter writer = new StreamWriter(output, encoding))
            {
                Serialize(writer, graph);
            }
        }

        public virtual void Serialize<T>(TextWriter writer, T graph)
        {
            using (var jsonWriter = new JsonTextWriter(writer) { Formatting = Formatting.Indented })
            {
                serializer.Serialize(jsonWriter, graph);
            }
        }

        public virtual T Deserialize<T>(Stream input)
        {
            using (var streamReader = new StreamReader(input, encoding))
            {
                return (T)Deserialize(streamReader);
            }
        }

        protected virtual object Deserialize(StreamReader reader)
        {
            using (var jsonReader = new JsonTextReader(reader))
            {
                return Deserialize(jsonReader);
            }
        }

        protected virtual object Deserialize(JsonReader jsonReader)
        {
            try
            {
                return serializer.Deserialize(jsonReader);
            }
            catch (JsonSerializationException e)
            {
                // Wrap in a standard .NET exception.
                throw new SerializationException(e.Message, e);
            }
        }

        public byte[] Serialize(object value)
        {
            using (var stream = new MemoryStream())
            {
                Serialize(stream, value);
                return stream.ToArray();
            }
        }

        public T Deserialize<T>(byte[] serialized)
        {
            serialized = serialized ?? new byte[] { };

            if (serialized.Length == 0)
                return default(T);

            using (var stream = new MemoryStream(serialized))
                return Deserialize<T>(stream);
        }
    }
}