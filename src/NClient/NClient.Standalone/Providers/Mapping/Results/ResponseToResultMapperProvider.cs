using NClient.Providers.Transport;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.Mapping.Results
{
    /// <summary>The provider of the mapper that converts NClient requests and responses into NClient results with deserialized data.</summary>
    public class ResponseToResultMapperProvider : IResponseMapperProvider<IRequest, IResponse>
    {
        /// <summary>Creates the mapper that converts NClient requests and responses into NClient results with deserialized data.</summary>
        /// <param name="toolset">Tools that help implement providers.</param>
        public IResponseMapper<IRequest, IResponse> Create(IToolset toolset)
        {
            return new ResponseToResultMapper();
        }
    }
}
