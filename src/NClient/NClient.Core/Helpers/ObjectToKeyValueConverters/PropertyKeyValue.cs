namespace NClient.Core.Helpers.ObjectToKeyValueConverters
{
    public class PropertyKeyValue
    {
        public string Key { get; }
        public object? Value { get; }

        public PropertyKeyValue(string key, object? value)
        {
            Key = key;
            Value = value;
        }
    }
}