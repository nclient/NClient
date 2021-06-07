using NClient.Core.MethodBuilders.Models;

namespace NClient.Core.Exceptions
{
    public class ClientValidationException : ClientException
    {
        public ClientValidationException(string message) : base(message)
        {
        }

        public ClientValidationException(string message, Method method) : base(message, method)
        {
        }
    }
}