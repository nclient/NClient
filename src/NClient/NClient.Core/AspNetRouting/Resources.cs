﻿using System.Globalization;
using System.Reflection;
using System.Resources;

namespace NClient.Core.AspNetRouting
{
    internal static class Resources
    {
        private static readonly ResourceManager _resourceManager
            = new ResourceManager("Microsoft.AspNetCore.Http.Abstractions.Resources", typeof(Resources).GetTypeInfo().Assembly);

        /// <summary>
        /// '{0}' is not available.
        /// </summary>
        internal static string Exception_UseMiddlewareIServiceProviderNotAvailable
        {
            get => GetString("Exception_UseMiddlewareIServiceProviderNotAvailable");
        }

        /// <summary>
        /// '{0}' is not available.
        /// </summary>
        internal static string FormatException_UseMiddlewareIServiceProviderNotAvailable(object p0)
            => string.Format(CultureInfo.CurrentCulture, GetString("Exception_UseMiddlewareIServiceProviderNotAvailable"), p0);

        /// <summary>
        /// No public '{0}' or '{1}' method found for middleware of type '{2}'.
        /// </summary>
        internal static string Exception_UseMiddlewareNoInvokeMethod
        {
            get => GetString("Exception_UseMiddlewareNoInvokeMethod");
        }

        /// <summary>
        /// No public '{0}' or '{1}' method found for middleware of type '{2}'.
        /// </summary>
        internal static string FormatException_UseMiddlewareNoInvokeMethod(object p0, object p1, object p2)
            => string.Format(CultureInfo.CurrentCulture, GetString("Exception_UseMiddlewareNoInvokeMethod"), p0, p1, p2);

        /// <summary>
        /// '{0}' or '{1}' does not return an object of type '{2}'.
        /// </summary>
        internal static string Exception_UseMiddlewareNonTaskReturnType
        {
            get => GetString("Exception_UseMiddlewareNonTaskReturnType");
        }

        /// <summary>
        /// '{0}' or '{1}' does not return an object of type '{2}'.
        /// </summary>
        internal static string FormatException_UseMiddlewareNonTaskReturnType(object p0, object p1, object p2)
            => string.Format(CultureInfo.CurrentCulture, GetString("Exception_UseMiddlewareNonTaskReturnType"), p0, p1, p2);

        /// <summary>
        /// The '{0}' or '{1}' method's first argument must be of type '{2}'.
        /// </summary>
        internal static string Exception_UseMiddlewareNoParameters
        {
            get => GetString("Exception_UseMiddlewareNoParameters");
        }

        /// <summary>
        /// The '{0}' or '{1}' method's first argument must be of type '{2}'.
        /// </summary>
        internal static string FormatException_UseMiddlewareNoParameters(object p0, object p1, object p2)
            => string.Format(CultureInfo.CurrentCulture, GetString("Exception_UseMiddlewareNoParameters"), p0, p1, p2);

        /// <summary>
        /// Multiple public '{0}' or '{1}' methods are available.
        /// </summary>
        internal static string Exception_UseMiddleMutlipleInvokes
        {
            get => GetString("Exception_UseMiddleMutlipleInvokes");
        }

        /// <summary>
        /// Multiple public '{0}' or '{1}' methods are available.
        /// </summary>
        internal static string FormatException_UseMiddleMutlipleInvokes(object p0, object p1)
            => string.Format(CultureInfo.CurrentCulture, GetString("Exception_UseMiddleMutlipleInvokes"), p0, p1);

        /// <summary>
        /// The path in '{0}' must start with '/'.
        /// </summary>
        internal static string Exception_PathMustStartWithSlash
        {
            get => GetString("Exception_PathMustStartWithSlash");
        }

        /// <summary>
        /// The path in '{0}' must start with '/'.
        /// </summary>
        internal static string FormatException_PathMustStartWithSlash(object p0)
            => string.Format(CultureInfo.CurrentCulture, GetString("Exception_PathMustStartWithSlash"), p0);

        /// <summary>
        /// Unable to resolve service for type '{0}' while attempting to Invoke middleware '{1}'.
        /// </summary>
        internal static string Exception_InvokeMiddlewareNoService
        {
            get => GetString("Exception_InvokeMiddlewareNoService");
        }

        /// <summary>
        /// Unable to resolve service for type '{0}' while attempting to Invoke middleware '{1}'.
        /// </summary>
        internal static string FormatException_InvokeMiddlewareNoService(object p0, object p1)
            => string.Format(CultureInfo.CurrentCulture, GetString("Exception_InvokeMiddlewareNoService"), p0, p1);

        /// <summary>
        /// The '{0}' method must not have ref or out parameters.
        /// </summary>
        internal static string Exception_InvokeDoesNotSupportRefOrOutParams
        {
            get => GetString("Exception_InvokeDoesNotSupportRefOrOutParams");
        }

        /// <summary>
        /// The '{0}' method must not have ref or out parameters.
        /// </summary>
        internal static string FormatException_InvokeDoesNotSupportRefOrOutParams(object p0)
            => string.Format(CultureInfo.CurrentCulture, GetString("Exception_InvokeDoesNotSupportRefOrOutParams"), p0);

        /// <summary>
        /// The value must be greater than zero.
        /// </summary>
        internal static string Exception_PortMustBeGreaterThanZero
        {
            get => GetString("Exception_PortMustBeGreaterThanZero");
        }

        /// <summary>
        /// The value must be greater than zero.
        /// </summary>
        internal static string FormatException_PortMustBeGreaterThanZero()
            => GetString("Exception_PortMustBeGreaterThanZero");

        /// <summary>
        /// No service for type '{0}' has been registered.
        /// </summary>
        internal static string Exception_UseMiddlewareNoMiddlewareFactory
        {
            get => GetString("Exception_UseMiddlewareNoMiddlewareFactory");
        }

        /// <summary>
        /// No service for type '{0}' has been registered.
        /// </summary>
        internal static string FormatException_UseMiddlewareNoMiddlewareFactory(object p0)
            => string.Format(CultureInfo.CurrentCulture, GetString("Exception_UseMiddlewareNoMiddlewareFactory"), p0);

        /// <summary>
        /// '{0}' failed to create middleware of type '{1}'.
        /// </summary>
        internal static string Exception_UseMiddlewareUnableToCreateMiddleware
        {
            get => GetString("Exception_UseMiddlewareUnableToCreateMiddleware");
        }

        /// <summary>
        /// '{0}' failed to create middleware of type '{1}'.
        /// </summary>
        internal static string FormatException_UseMiddlewareUnableToCreateMiddleware(object p0, object p1)
            => string.Format(CultureInfo.CurrentCulture, GetString("Exception_UseMiddlewareUnableToCreateMiddleware"), p0, p1);

        /// <summary>
        /// Types that implement '{0}' do not support explicit arguments.
        /// </summary>
        internal static string Exception_UseMiddlewareExplicitArgumentsNotSupported
        {
            get => GetString("Exception_UseMiddlewareExplicitArgumentsNotSupported");
        }

        /// <summary>
        /// Types that implement '{0}' do not support explicit arguments.
        /// </summary>
        internal static string FormatException_UseMiddlewareExplicitArgumentsNotSupported(object p0)
            => string.Format(CultureInfo.CurrentCulture, GetString("Exception_UseMiddlewareExplicitArgumentsNotSupported"), p0);

        /// <summary>
        /// Argument cannot be null or empty.
        /// </summary>
        internal static string ArgumentCannotBeNullOrEmpty
        {
            get => GetString("ArgumentCannotBeNullOrEmpty");
        }

        /// <summary>
        /// Argument cannot be null or empty.
        /// </summary>
        internal static string FormatArgumentCannotBeNullOrEmpty()
            => GetString("ArgumentCannotBeNullOrEmpty");

        /// <summary>
        /// An element with the key '{0}' already exists in the {1}.
        /// </summary>
        internal static string RouteValueDictionary_DuplicateKey
        {
            get => GetString("RouteValueDictionary_DuplicateKey");
        }

        /// <summary>
        /// An element with the key '{0}' already exists in the {1}.
        /// </summary>
        internal static string FormatRouteValueDictionary_DuplicateKey(object p0, object p1)
            => string.Format(CultureInfo.CurrentCulture, GetString("RouteValueDictionary_DuplicateKey"), p0, p1);

        /// <summary>
        /// The type '{0}' defines properties '{1}' and '{2}' which differ only by casing. This is not supported by {3} which uses case-insensitive comparisons.
        /// </summary>
        internal static string RouteValueDictionary_DuplicatePropertyName
        {
            get => GetString("RouteValueDictionary_DuplicatePropertyName");
        }

        /// <summary>
        /// The type '{0}' defines properties '{1}' and '{2}' which differ only by casing. This is not supported by {3} which uses case-insensitive comparisons.
        /// </summary>
        internal static string FormatRouteValueDictionary_DuplicatePropertyName(object p0, object p1, object p2, object p3)
            => string.Format(CultureInfo.CurrentCulture, GetString("RouteValueDictionary_DuplicatePropertyName"), p0, p1, p2, p3);

        private static string GetString(string name, params string[] formatterNames)
        {
            var value = _resourceManager.GetString(name);

            System.Diagnostics.Debug.Assert(value != null);

            if (formatterNames != null)
            {
                for (var i = 0; i < formatterNames.Length; i++)
                {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    value = value.Replace("{" + formatterNames[i] + "}", "{" + i + "}");
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                }
            }

#pragma warning disable CS8603 // Possible null reference return.
            return value;
#pragma warning restore CS8603 // Possible null reference return.
        }
    }
}
