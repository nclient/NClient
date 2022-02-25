using NClient.Providers.Transport;

namespace NClient.Providers.Mapping.LanguageExt
{
    /// <summary>The provider of the mapper that converts NClient responses into Either monad from LanguageExt.</summary>
    public class ResponseToEitherBuilderProvider : IResponseMapperProvider<IRequest, IResponse>
    {
        /// <summary>Creates the mapper that converts NClient responses into Either monad from LanguageExt.</summary>
        /// <param name="toolset">Tools that help implement providers.</param>
        public IResponseMapper<IRequest, IResponse> Create(IToolset toolset)
        {
            return new ResponseToEitherBuilder();
        }
    }
}
