using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace NClient.AspNetCore.AspNetBinding
{
    /// <summary>
    /// An <see cref="IModelBinderProvider"/> for complex types.
    /// </summary>
    internal class ComplexTypeModelBinderProvider : IModelBinderProvider
    {
        /// <inheritdoc />
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.IsComplexType && !context.Metadata.IsCollectionType)
            {
                var propertyBinders = new Dictionary<ModelMetadata, IModelBinder>();
                for (var i = 0; i < context.Metadata.Properties.Count; i++)
                {
                    var property = context.Metadata.Properties[i];
                    propertyBinders.Add(property, context.CreateBinder(property));
                }

                var loggerFactory = context.Services.GetRequiredService<ILoggerFactory>();
                return new ComplexTypeModelBinder(
                    propertyBinders,
                    loggerFactory,
                    allowValidatingTopLevelNodes: true);
            }

            return null;
        }
    }
}
