namespace NClient.Abstractions.Serialization
{
    public interface ISerializerProvider
    {
        ISerializer Create();
    }
}