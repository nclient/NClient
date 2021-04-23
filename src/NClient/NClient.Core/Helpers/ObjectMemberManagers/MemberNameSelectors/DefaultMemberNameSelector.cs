using System.Reflection;

namespace NClient.Core.Helpers.ObjectMemberManagers.MemberNameSelectors
{
    public class DefaultMemberNameSelector : IMemberNameSelector
    {
        public string GetName(MemberInfo memberInfo)
        {
            return memberInfo.Name;
        }
    }
}