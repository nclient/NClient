using System.Reflection;

namespace NClient.Core.Helpers.MemberNameSelectors
{
    public class DefaultMemberNameSelector : IMemberNameSelector
    {
        public string GetName(MemberInfo memberInfo)
        {
            return memberInfo.Name;
        }
    }
}