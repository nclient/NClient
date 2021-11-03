// ReSharper disable once CheckNamespace

namespace NClient
{
    public static class AsHandlingSetterExtensions
    {
        public static INClientAdvancedHandlingSetter<TRequest, TResponse> AsAdvanced<TRequest, TResponse>(
            this INClientHandlingSetter<TRequest, TResponse> handlingSetter)
        {
            return (INClientAdvancedHandlingSetter<TRequest, TResponse>)handlingSetter;
        }
        
        public static INClientHandlingSetter<TRequest, TResponse> AsBasic<TRequest, TResponse>(
            this INClientAdvancedHandlingSetter<TRequest, TResponse> advancedHandlingSetter)
        {
            return advancedHandlingSetter;
        }
    }
}
