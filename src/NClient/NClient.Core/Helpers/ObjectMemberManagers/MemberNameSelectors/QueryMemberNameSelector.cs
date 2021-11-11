using System;
using System.Linq;
using System.Reflection;
using NClient.Annotations;

namespace NClient.Core.Helpers.ObjectMemberManagers.MemberNameSelectors
{
    internal class QueryMemberNameSelector : IMemberNameSelector
    {
        public string GetName(MemberInfo memberInfo)
        {
            var fromQueryAttribute = FindFromQueryAttribute(memberInfo);
            var fromQueryName = fromQueryAttribute?
                .GetType()
                .GetProperty("Name")!
                .GetValue(fromQueryAttribute) as string;

            var queryParamName = (memberInfo
                    .GetCustomAttributes()
                    .SingleOrDefault(x => x is IPropertyParamAttribute)
                as IPropertyParamAttribute)?.Name;

            return fromQueryName ?? queryParamName ?? memberInfo.Name;
        }

        private static Attribute? FindFromQueryAttribute(MemberInfo memberInfo)
        {
            return memberInfo
                .GetCustomAttributes(false)
                .SingleOrDefault(x => x.GetType().FullName == "Microsoft.AspNetCore.Mvc.FromQueryAttribute") as Attribute;
        }
    }
}
