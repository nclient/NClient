using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using NClient.Common.Helpers;
using NClient.Providers.Handling;
using NClient.Providers.Results;
using NClient.Providers.Serialization;
using NClient.Providers.Validation;
using NClient.Standalone.ClientProxy.Building.Configuration.Handling;
using NClient.Standalone.ClientProxy.Building.Configuration.Resilience;
using NClient.Standalone.ClientProxy.Building.Configuration.Results;
using NClient.Standalone.ClientProxy.Building.Configuration.Validation;
using NClient.Standalone.ClientProxy.Building.Context;

namespace NClient.Standalone.ClientProxy.Building.Factory
{
    internal class NClientFactoryOptionalBuilder<TRequest, TResponse> 
        : INClientFactoryAdvancedOptionalBuilder<TRequest, TResponse>, INClientFactoryOptionalBuilder<TRequest, TResponse>
    {
        private readonly string _factoryName;
        private readonly BuilderContext<TRequest, TResponse> _context;

        public NClientFactoryOptionalBuilder(string factoryName, BuilderContext<TRequest, TResponse> context)
        {
            _factoryName = factoryName;
            _context = context;
        }

        #region INClientFactoryOptionalBuilder
        
        INClientFactoryOptionalBuilder<TRequest, TResponse> INClientFactoryOptionalBuilder<TRequest, TResponse>.WithResponseValidation(IEnumerable<IResponseValidator<TRequest, TResponse>> validators)
        {
            return WithResponseValidation(x => x
                    .ForTransport().Use(validators))
                .AsBasic();
        }
        
        INClientFactoryOptionalBuilder<TRequest, TResponse> INClientFactoryOptionalBuilder<TRequest, TResponse>.WithoutResponseValidation()
        {
            return WithoutResponseValidation().AsBasic();
        }
        
        INClientFactoryOptionalBuilder<TRequest, TResponse> INClientFactoryOptionalBuilder<TRequest, TResponse>.WithHandling(IEnumerable<IClientHandler<TRequest, TResponse>> handlers)
        {
            return WithHandling(x => x
                    .ForTransport().Use(handlers))
                .AsBasic();
        }
        
        INClientFactoryOptionalBuilder<TRequest, TResponse> INClientFactoryOptionalBuilder<TRequest, TResponse>.WithoutHandling()
        {
            return WithoutHandling().AsBasic();
        }

        INClientFactoryOptionalBuilder<TRequest, TResponse> INClientFactoryOptionalBuilder<TRequest, TResponse>.WithResults(IEnumerable<IResultBuilder<TRequest, TResponse>> builders)
        {
            return WithResults(x => x
                    .ForTransport().Use(builders))
                .AsBasic();
        }
        
        INClientFactoryOptionalBuilder<TRequest, TResponse> INClientFactoryOptionalBuilder<TRequest, TResponse>.WithoutResults()
        {
            return WithoutResults().AsBasic();
        }
        
        INClientFactoryOptionalBuilder<TRequest, TResponse> INClientFactoryOptionalBuilder<TRequest, TResponse>.WithResilience(Action<INClientFactoryResilienceMethodSelector<TRequest, TResponse>> configure)
        {
            return WithResilience(configure).AsBasic();
        }
        
        INClientFactoryOptionalBuilder<TRequest, TResponse> INClientFactoryOptionalBuilder<TRequest, TResponse>.WithoutResilience()
        {
            return WithoutResilience().AsBasic();
        }
        
        INClientFactoryOptionalBuilder<TRequest, TResponse> INClientFactoryOptionalBuilder<TRequest, TResponse>.WithLogging(ILoggerFactory loggerFactory)
        {
            return WithLogging(loggerFactory).AsBasic();
        }
        
        INClientFactoryOptionalBuilder<TRequest, TResponse> INClientFactoryOptionalBuilder<TRequest, TResponse>.WithLogging(IEnumerable<ILogger> loggers)
        {
            return WithLogging(loggers).AsBasic();
        }

        INClientFactoryOptionalBuilder<TRequest, TResponse> INClientFactoryOptionalBuilder<TRequest, TResponse>.WithoutLogging()
        {
            return WithoutLogging().AsBasic();
        }
        
        INClientFactory INClientFactoryOptionalBuilder<TRequest, TResponse>.Build()
        {
            return Build();
        }
        
        #endregion

        public INClientFactoryAdvancedOptionalBuilder<TRequest, TResponse> WithResponseValidation(Action<INClientResponseValidationSelector<TRequest, TResponse>> configure)
        {
            Ensure.IsNotNull(configure, nameof(configure));

            var builderContextModifier = new BuilderContextModifier<TRequest, TResponse>();
            configure(new NClientResponseValidationSelector<TRequest, TResponse>(builderContextModifier));
            return new NClientFactoryOptionalBuilder<TRequest, TResponse>(_factoryName, builderContextModifier.Invoke(_context));
        }

        public INClientFactoryAdvancedOptionalBuilder<TRequest, TResponse> WithoutResponseValidation()
        {
            return new NClientFactoryOptionalBuilder<TRequest, TResponse>(_factoryName, _context
                .WithoutResponseValidation());
        }

        public INClientFactoryAdvancedOptionalBuilder<TRequest, TResponse> WithCustomSerialization(ISerializerProvider provider)
        {
            Ensure.IsNotNull(provider, nameof(provider));
            
            return new NClientFactoryOptionalBuilder<TRequest, TResponse>(_factoryName, _context
                .WithSerializer(provider));
        }
        
        public INClientFactoryAdvancedOptionalBuilder<TRequest, TResponse> WithHandling(Action<INClientHandlingSelector<TRequest, TResponse>> configure)
        {
            Ensure.IsNotNull(configure, nameof(configure));

            var builderContextModifier = new BuilderContextModifier<TRequest, TResponse>();
            configure(new NClientHandlingSelector<TRequest, TResponse>(builderContextModifier));
            return new NClientFactoryOptionalBuilder<TRequest, TResponse>(_factoryName, builderContextModifier.Invoke(_context));
        }

        public INClientFactoryAdvancedOptionalBuilder<TRequest, TResponse> WithoutHandling()
        {
            return new NClientFactoryOptionalBuilder<TRequest, TResponse>(_factoryName, _context
                .WithoutHandlers());
        }

        public INClientFactoryAdvancedOptionalBuilder<TRequest, TResponse> WithResults(Action<INClientResultsSelector<TRequest, TResponse>> configure)
        {
            Ensure.IsNotNull(configure, nameof(configure));

            var builderContextModifier = new BuilderContextModifier<TRequest, TResponse>();
            configure(new NClientResultsSelector<TRequest, TResponse>(builderContextModifier));
            return new NClientFactoryOptionalBuilder<TRequest, TResponse>(_factoryName, builderContextModifier.Invoke(_context));
        }

        public INClientFactoryAdvancedOptionalBuilder<TRequest, TResponse> WithoutResults()
        {
            return new NClientFactoryOptionalBuilder<TRequest, TResponse>(_factoryName, _context
                .WithoutResultBuilders());
        }
        
        public INClientFactoryAdvancedOptionalBuilder<TRequest, TResponse> WithResilience(Action<INClientFactoryResilienceMethodSelector<TRequest, TResponse>> configure)
        {
            Ensure.IsNotNull(configure, nameof(configure));

            var builderContextModifier = new BuilderContextModifier<TRequest, TResponse>();
            configure(new NClientFactoryResilienceMethodSelector<TRequest, TResponse>(builderContextModifier));
            return new NClientFactoryOptionalBuilder<TRequest, TResponse>(_factoryName, builderContextModifier.Invoke(_context));
        }

        public INClientFactoryAdvancedOptionalBuilder<TRequest, TResponse> WithoutResilience()
        {
            return new NClientFactoryOptionalBuilder<TRequest, TResponse>(_factoryName, _context
                .WithoutResiliencePolicy());
        }

        public INClientFactoryAdvancedOptionalBuilder<TRequest, TResponse> WithLogging(ILoggerFactory loggerFactory)
        {
            Ensure.IsNotNull(loggerFactory, nameof(loggerFactory));
            
            return new NClientFactoryOptionalBuilder<TRequest, TResponse>(_factoryName, _context
                .WithLogging(loggerFactory));
        }

        public INClientFactoryAdvancedOptionalBuilder<TRequest, TResponse> WithLogging(ILogger logger, params ILogger[] extraLoggers)
        {
            Ensure.IsNotNull(logger, nameof(logger));
            Ensure.AreNotNullItems(extraLoggers, nameof(extraLoggers));
            
            return new NClientFactoryOptionalBuilder<TRequest, TResponse>(_factoryName, _context
                .WithLogging(extraLoggers.Concat(new[] { logger })));
        }
        
        public INClientFactoryAdvancedOptionalBuilder<TRequest, TResponse> WithLogging(IEnumerable<ILogger> loggers)
        {
            var loggerCollection = loggers as ICollection<ILogger> ?? loggers.ToArray();
            Ensure.AreNotNullItems(loggerCollection, nameof(loggerCollection));
            
            return new NClientFactoryOptionalBuilder<TRequest, TResponse>(_factoryName, _context
                .WithLogging(loggerCollection));
        }

        public INClientFactoryAdvancedOptionalBuilder<TRequest, TResponse> WithoutLogging()
        {
            return new NClientFactoryOptionalBuilder<TRequest, TResponse>(_factoryName, _context
                .WithoutLogging());
        }

        public INClientFactory Build()
        {
            return new NClientFactory<TRequest, TResponse>(_factoryName, _context);
        }
    }
}
