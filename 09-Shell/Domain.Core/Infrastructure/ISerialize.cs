using System.IO;

namespace Domain.Core.Infrastructure
{
    public interface ISerialize
    {
        void Serialize<T>(Stream output, T graph);
        T Deserialize<T>(Stream input);
    }
}