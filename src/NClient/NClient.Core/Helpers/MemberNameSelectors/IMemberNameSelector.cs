using System.Reflection;

namespace NClient.Core.Helpers.MemberNameSelectors
{
    public interface IMemberNameSelector
    {
        string GetName(MemberInfo memberInfo);
    }
}