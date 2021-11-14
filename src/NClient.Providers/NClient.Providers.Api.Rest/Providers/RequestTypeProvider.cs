using System;
using NClient.Annotations;
using NClient.Providers.Api.Rest.Exceptions.Factories;
using NClient.Providers.Transport;

namespace NClient.Providers.Api.Rest.Providers
{
    internal interface ITransportMethodProvider
    {
        RequestType Get(IOperationAttribute operationAttribute);
    }

    internal class RequestTypeProvider : ITransportMethodProvider
    {
        private readonly IClientValidationExceptionFactory _clientValidationExceptionFactory;

        public RequestTypeProvider(IClientValidationExceptionFactory clientValidationExceptionFactory)
        {
            _clientValidationExceptionFactory = clientValidationExceptionFactory;
        }

        public RequestType Get(IOperationAttribute operationAttribute)
        {
            return operationAttribute switch
            {
                ICustomOperationAttribute => RequestType.Custom,
                IInfoOperationAttribute => RequestType.Info,
                ICheckOperationAttribute => RequestType.Check,
                IReadOperationAttribute => RequestType.Read,
                ICreateOperationAttribute => RequestType.Create,
                IUpdateOperationAttribute => RequestType.Update,
                IDeleteOperationAttribute => RequestType.Delete,
                #if !NETSTANDARD2_0
                IPartialUpdateOperationAttribute => RequestType.PartialUpdate,
                #endif
                { } => throw _clientValidationExceptionFactory.MethodAttributeNotSupported(operationAttribute.GetType().Name),
                _ => throw new ArgumentNullException(nameof(operationAttribute))
            };
        }
    }
}
