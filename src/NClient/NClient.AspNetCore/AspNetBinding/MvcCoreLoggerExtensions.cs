using System;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.Extensions.Logging;

namespace NClient.AspNetCore.AspNetBinding
{
    internal static class MvcCoreLoggerExtensions
    {
        private static readonly Action<ILogger, IInputFormatter, string, Exception?> _inputFormatterSelected;
        private static readonly Action<ILogger, IInputFormatter, string, Exception?> _inputFormatterRejected;
        private static readonly Action<ILogger, string, Exception?> _noInputFormatterSelected;

        private static readonly Action<ILogger, string, string, Exception?> _removeFromBodyAttribute;

        private static readonly Action<ILogger, string, Type, string, Exception?> _attemptingToBindParameterModel;
        private static readonly Action<ILogger, Type, string, Type, string, Exception?> _attemptingToBindPropertyModel;
        private static readonly Action<ILogger, Type, string, Exception?> _attemptingToBindModel;

        private static readonly Action<ILogger, string, Type, Exception?> _doneAttemptingToBindParameterModel;
        private static readonly Action<ILogger, Type, string, Type, Exception?> _doneAttemptingToBindPropertyModel;
        private static readonly Action<ILogger, Type, string, Exception?> _doneAttemptingToBindModel;

        private static readonly Action<ILogger, string, Type, Exception?> _noPublicSettableProperties;

        private static readonly Action<ILogger, Type, Exception?> _cannotBindToComplexType;

        static MvcCoreLoggerExtensions()
        {
            _inputFormatterSelected = LoggerMessage.Define<IInputFormatter, string>(
                LogLevel.Debug,
                new EventId(1, "InputFormatterSelected"),
                "Selected input formatter '{InputFormatter}' for content type '{ContentType}'.");
            _inputFormatterRejected = LoggerMessage.Define<IInputFormatter, string>(
                LogLevel.Debug,
                new EventId(2, "InputFormatterRejected"),
                "Rejected input formatter '{InputFormatter}' for content type '{ContentType}'.");
            _noInputFormatterSelected = LoggerMessage.Define<string>(
                LogLevel.Debug,
                new EventId(3, "NoInputFormatterSelected"),
                "No input formatter was found to support the content type '{ContentType}' for use with the [FromBody] attribute.");

            _removeFromBodyAttribute = LoggerMessage.Define<string, string>(
                LogLevel.Debug,
                new EventId(4, "RemoveFromBodyAttribute"),
                "To use model binding, remove the [FromBody] attribute from the property or parameter named '{ModelName}' with model type '{ModelType}'.");

            _attemptingToBindParameterModel = LoggerMessage.Define<string, Type, string>(
                LogLevel.Debug,
                new EventId(44, "AttemptingToBindParameterModel"),
                "Attempting to bind parameter '{ParameterName}' of type '{ModelType}' using the name '{ModelName}' in request data ...");
            _attemptingToBindPropertyModel = LoggerMessage.Define<Type, string, Type, string>(
                LogLevel.Debug,
                new EventId(13, "AttemptingToBindPropertyModel"),
                "Attempting to bind property '{PropertyContainerType}.{PropertyName}' of type '{ModelType}' using the name '{ModelName}' in request data ...");
            _attemptingToBindModel = LoggerMessage.Define<Type, string>(
                LogLevel.Debug,
                new EventId(24, "AttemptingToBindModel"),
                "Attempting to bind model of type '{ModelType}' using the name '{ModelName}' in request data ...");

            _doneAttemptingToBindParameterModel = LoggerMessage.Define<string, Type>(
                LogLevel.Debug,
                new EventId(45, "DoneAttemptingToBindParameterModel"),
                "Done attempting to bind parameter '{ParameterName}' of type '{ModelType}'.");
            _doneAttemptingToBindPropertyModel = LoggerMessage.Define<Type, string, Type>(
                LogLevel.Debug,
                new EventId(14, "DoneAttemptingToBindPropertyModel"),
                "Done attempting to bind property '{PropertyContainerType}.{PropertyName}' of type '{ModelType}'.");
            _doneAttemptingToBindModel = LoggerMessage.Define<Type, string>(
                LogLevel.Debug,
                new EventId(25, "DoneAttemptingToBindModel"),
                "Done attempting to bind model of type '{ModelType}' using the name '{ModelName}'.");

            _noPublicSettableProperties = LoggerMessage.Define<string, Type>(
                LogLevel.Debug,
                new EventId(17, "NoPublicSettableProperties"),
                "Could not bind to model with name '{ModelName}' and type '{ModelType}' as the type has no public settable properties.");

            _cannotBindToComplexType = LoggerMessage.Define<Type>(
                LogLevel.Debug,
                new EventId(18, "CannotBindToComplexType"),
                "Could not bind to model of type '{ModelType}' as there were no values in the request for any of the properties.");
        }

        public static void InputFormatterSelected(
            this ILogger logger,
            IInputFormatter inputFormatter,
            InputFormatterContext formatterContext)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                var contentType = formatterContext.HttpContext.Request.ContentType;
                _inputFormatterSelected(logger, inputFormatter, contentType, null);
            }
        }

        public static void InputFormatterRejected(
            this ILogger logger,
            IInputFormatter inputFormatter,
            InputFormatterContext formatterContext)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                var contentType = formatterContext.HttpContext.Request.ContentType;
                _inputFormatterRejected(logger, inputFormatter, contentType, null);
            }
        }

        public static void NoInputFormatterSelected(
            this ILogger logger,
            InputFormatterContext formatterContext)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                var contentType = formatterContext.HttpContext.Request.ContentType;
                _noInputFormatterSelected(logger, contentType, null);
                if (formatterContext.HttpContext.Request.HasFormContentType)
                {
                    var modelType = formatterContext.ModelType.FullName;
                    var modelName = formatterContext.ModelName;
                    _removeFromBodyAttribute(logger, modelName, modelType!, null);
                }
            }
        }

        public static void AttemptingToBindModel(this ILogger logger, ModelBindingContext bindingContext)
        {
            if (!logger.IsEnabled(LogLevel.Debug))
            {
                return;
            }

            var modelMetadata = bindingContext.ModelMetadata;
            switch (modelMetadata.MetadataKind)
            {
                case ModelMetadataKind.Parameter:
                    _attemptingToBindParameterModel(
                        logger,
                        modelMetadata.ParameterName!,
                        modelMetadata.ModelType,
                        bindingContext.ModelName,
                        null);
                    break;
                case ModelMetadataKind.Property:
                    _attemptingToBindPropertyModel(
                        logger,
                        modelMetadata.ContainerType!,
                        modelMetadata.PropertyName!,
                        modelMetadata.ModelType,
                        bindingContext.ModelName,
                        null);
                    break;
                case ModelMetadataKind.Type:
                    _attemptingToBindModel(logger, bindingContext.ModelType, bindingContext.ModelName, null);
                    break;
            }
        }

        public static void DoneAttemptingToBindModel(this ILogger logger, ModelBindingContext bindingContext)
        {
            if (!logger.IsEnabled(LogLevel.Debug))
            {
                return;
            }

            var modelMetadata = bindingContext.ModelMetadata;
            switch (modelMetadata.MetadataKind)
            {
                case ModelMetadataKind.Parameter:
                    _doneAttemptingToBindParameterModel(
                        logger,
                        modelMetadata.ParameterName!,
                        modelMetadata.ModelType,
                        null);
                    break;
                case ModelMetadataKind.Property:
                    _doneAttemptingToBindPropertyModel(
                        logger,
                        modelMetadata.ContainerType!,
                        modelMetadata.PropertyName!,
                        modelMetadata.ModelType,
                        null);
                    break;
                case ModelMetadataKind.Type:
                    _doneAttemptingToBindModel(logger, bindingContext.ModelType, bindingContext.ModelName, null);
                    break;
            }
        }

        public static void NoPublicSettableProperties(this ILogger logger, ModelBindingContext bindingContext)
        {
            _noPublicSettableProperties(logger, bindingContext.ModelName, bindingContext.ModelType, null);
        }

        public static void CannotBindToComplexType(this ILogger logger, ModelBindingContext bindingContext)
        {
            _cannotBindToComplexType(logger, bindingContext.ModelType, null);
        }
    }
}
