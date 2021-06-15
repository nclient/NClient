using System.Reflection;

namespace NClient.Core.Helpers.ObjectMemberManagers.MemberNameSelectors
{
    internal interface IMemberNameSelector
    {
        string GetName(MemberInfo memberInfo);
    }
}