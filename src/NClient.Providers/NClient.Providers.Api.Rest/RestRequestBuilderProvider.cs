using NClient.Core.Helpers.ObjectMemberManagers;
using NClient.Core.Helpers.ObjectToKeyValueConverters;
using NClient.Core.Helpers.ObjectToKeyValueConverters.Factories;
using NClient.Providers.Api.Rest.Exceptions.Factories;
using NClient.Providers.Api.Rest.Providers;
using NClient.Providers.Serialization;

namespace NClient.Providers.Api.Rest
{
    public class RestRequestBuilderProvider : IRequestBuilderProvider
    {
        private readonly IObjectMemberManager _objectMemberManager;
        private readonly IClientArgumentExceptionFactory _clientArgumentExceptionFactory;
        private readonly IClientValidationExceptionFactory _clientValidationExceptionFactory;
        private readonly IObjectToKeyValueConverterExceptionFactory _objectToKeyValueConverterExceptionFactory;

        public RestRequestBuilderProvider()
        {
            _objectMemberManager = new ObjectMemberManager(new ObjectMemberManagerExceptionFactory());
            _clientArgumentExceptionFactory = new ClientArgumentExceptionFactory();
            _clientValidationExceptionFactory = new ClientValidationExceptionFactory();
            _objectToKeyValueConverterExceptionFactory = new ObjectToKeyValueConverterExceptionFactory();
        }
        
        public IRequestBuilder Create(ISerializer serializer)
        {
            return new RestRequestBuilder(
                serializer,
                new RouteTemplateProvider(_clientValidationExceptionFactory),
                new RouteProvider(_objectMemberManager, _clientArgumentExceptionFactory, _clientValidationExceptionFactory),
                new RequestTypeProvider(_clientValidationExceptionFactory),
                new ObjectToKeyValueConverter(_objectMemberManager, _objectToKeyValueConverterExceptionFactory),
                _clientValidationExceptionFactory);
        }
    }
}
