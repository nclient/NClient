using System.Reflection;

namespace NClient.Core.Helpers.ObjectMemberManagers.MemberNameSelectors
{
    internal class DefaultMemberNameSelector : IMemberNameSelector
    {
        public string GetName(MemberInfo memberInfo)
        {
            return memberInfo.Name;
        }
    }
}