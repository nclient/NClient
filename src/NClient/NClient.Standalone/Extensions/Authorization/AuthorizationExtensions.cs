using NClient.Common.Helpers;
using NClient.Providers.Authorization;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class AuthorizationExtensions
    {
        /// <summary>Sets token for client authorization.</summary>
        /// <param name="optionalBuilder"></param>
        /// <param name="scheme">The scheme to use for authorization.</param>
        /// <param name="token">The access token value.</param>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithTokenAuthorization<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> optionalBuilder,
            string scheme, string token) 
            where TClient : class
        {
            Ensure.IsNotNull(scheme, nameof(scheme));
            Ensure.IsNotNull(token, nameof(token));

            return WithTokenAuthorization(optionalBuilder, new Token(scheme, token));
        }

        /// <summary>Sets token for client authorization.</summary>
        /// <param name="optionalBuilder"></param>
        /// <param name="scheme">The scheme to use for authorization.</param>
        /// <param name="token">The access token value.</param>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithTokenAuthorization<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> optionalBuilder,
            string scheme, string token)
        {
            Ensure.IsNotNull(scheme, nameof(scheme));
            Ensure.IsNotNullOrEmpty(token, nameof(token));

            return WithTokenAuthorization(optionalBuilder, new Token(scheme, token));
        }
        
        /// <summary>Sets token for client authorization.</summary>
        /// <param name="optionalBuilder"></param>
        /// <param name="token">The access token to use for authorization.</param>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithTokenAuthorization<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> optionalBuilder,
            IToken token) 
            where TClient : class
        {
            Ensure.IsNotNull(token, nameof(token));

            return optionalBuilder.WithTokenAuthorization(new SingleToken(token));
        }

        /// <summary>Sets token for client authorization.</summary>
        /// <param name="optionalBuilder"></param>
        /// <param name="token">The access token to use for authorization.</param>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithTokenAuthorization<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> optionalBuilder,
            IToken token)
        {
            Ensure.IsNotNull(token, nameof(token));

            return optionalBuilder.WithTokenAuthorization(new SingleToken(token));
        }
    }
}
