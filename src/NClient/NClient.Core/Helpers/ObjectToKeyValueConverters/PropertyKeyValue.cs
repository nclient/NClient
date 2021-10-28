namespace NClient.Core.Helpers.ObjectToKeyValueConverters
{
    internal class PropertyKeyValue
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
