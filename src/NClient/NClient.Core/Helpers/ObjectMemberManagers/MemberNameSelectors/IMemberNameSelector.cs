using System.Reflection;

namespace NClient.Core.Helpers.ObjectMemberManagers.MemberNameSelectors
{
    public interface IMemberNameSelector
    {
        string GetName(MemberInfo memberInfo);
    }
}