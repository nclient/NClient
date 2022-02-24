using System;
using System.ComponentModel;

namespace NClient.Common.Helpers
{
    internal static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            var resultDescription = string.Empty;
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            Ensure.IsNotNull(name, nameof(name));
            Ensure.IsNotNullOrEmpty(name, nameof(name));
            var field = type.GetField(name);
            Ensure.IsNotNull(name, nameof(field));
            if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attr)
                resultDescription = attr.Description;
            Ensure.IsNotNullOrEmpty(resultDescription, nameof(resultDescription));
            return resultDescription;
        }
    }
}
