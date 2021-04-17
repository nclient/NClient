using System;
using System.Linq;
using System.Reflection;

namespace NClient.Core.Helpers.MemberNameSelectors
{
    public class BodyMemberNameSelector : IMemberNameSelector
    {
        public string GetName(MemberInfo memberInfo)
        {
            var jsonPropertyNameAttribute = FindJsonPropertyNameAttribute(memberInfo);
            var sonPropertyName = jsonPropertyNameAttribute?
                .GetType()
                .GetProperty("Name")!
                .GetValue(jsonPropertyNameAttribute) as string;

            return sonPropertyName ?? memberInfo.Name;
        }
        
        private static Attribute? FindJsonPropertyNameAttribute(MemberInfo memberInfo)
        {
            return memberInfo
                .GetCustomAttributes(false)
                .SingleOrDefault(x => x.GetType().FullName == "System.Text.Json.Serialization.JsonPropertyNameAttribute") as Attribute;
        }
    }
}