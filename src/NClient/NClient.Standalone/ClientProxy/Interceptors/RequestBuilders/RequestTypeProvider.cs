using System;
using NClient.Annotations.Operations;
using NClient.Providers.Transport;
using NClient.Standalone.Exceptions.Factories;

namespace NClient.Standalone.ClientProxy.Interceptors.RequestBuilders
{
    internal interface ITransportMethodProvider
    {
        RequestType Get(OperationAttribute operationAttribute);
    }

    internal class RequestTypeProvider : ITransportMethodProvider
    {
        private readonly IClientValidationExceptionFactory _clientValidationExceptionFactory;

        public RequestTypeProvider(IClientValidationExceptionFactory clientValidationExceptionFactory)
        {
            _clientValidationExceptionFactory = clientValidationExceptionFactory;
        }

        public RequestType Get(OperationAttribute operationAttribute)
        {
            return operationAttribute switch
            {
                CustomOperationAttribute => RequestType.Custom,
                InfoOperationAttribute => RequestType.Info,
                CheckOperationAttribute => RequestType.Check,
                ReadOperationAttribute => RequestType.Read,
                CreateOperationAttribute => RequestType.Create,
                UpdateOperationAttribute => RequestType.Update,
                DeleteOperationAttribute => RequestType.Delete,
                #if !NETSTANDARD2_0
                PartialUpdateOperationAttribute => RequestType.PartialUpdate,
                #endif
                { } => throw _clientValidationExceptionFactory.MethodAttributeNotSupported(operationAttribute.GetType().Name),
                _ => throw new ArgumentNullException(nameof(operationAttribute))
            };
        }
    }
}
