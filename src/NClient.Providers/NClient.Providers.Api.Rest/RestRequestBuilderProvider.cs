using NClient.Core.Helpers.ObjectMemberManagers;
using NClient.Core.Helpers.ObjectToKeyValueConverters;
using NClient.Core.Helpers.ObjectToKeyValueConverters.Factories;
using NClient.Providers.Api.Rest.Exceptions.Factories;
using NClient.Providers.Api.Rest.Helpers;
using NClient.Providers.Api.Rest.Providers;

namespace NClient.Providers.Api.Rest
{
    /// <summary>The provider of the request builder that turns a method call into a REST request.</summary>
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
        
        /// <summary>Creates the request builder that turns a method call into a REST request.</summary>
        /// <param name="toolset">Tools that help implement providers.</param>
        public IRequestBuilder Create(IToolset toolset)
        {
            return new RestRequestBuilder(
                new RouteTemplateProvider(_clientValidationExceptionFactory),
                new RouteProvider(_objectMemberManager, _clientArgumentExceptionFactory, _clientValidationExceptionFactory),
                new RequestTypeProvider(_clientValidationExceptionFactory),
                new FormUrlEncoder(),
                new ObjectToKeyValueConverter(_objectMemberManager, _objectToKeyValueConverterExceptionFactory),
                _clientValidationExceptionFactory,
                toolset);
        }
    }
}
