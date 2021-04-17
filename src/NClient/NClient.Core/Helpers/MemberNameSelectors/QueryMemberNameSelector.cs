using System;
using System.Linq;
using System.Reflection;
using NClient.Annotations.Parameters;

namespace NClient.Core.Helpers.MemberNameSelectors
{
    public class QueryMemberNameSelector : IMemberNameSelector
    {
        public string GetName(MemberInfo memberInfo)
        {
            var fromQueryAttribute = FindFromQueryAttribute(memberInfo);
            var fromQueryName = fromQueryAttribute?
                .GetType()
                .GetProperty("Name")!
                .GetValue(fromQueryAttribute) as string;
                    
            var queryParamName = memberInfo
                .GetCustomAttribute<QueryParamAttribute>()?
                .Name;
                    
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