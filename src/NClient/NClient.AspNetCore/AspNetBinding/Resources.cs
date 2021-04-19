using System.Globalization;

namespace NClient.AspNetCore.AspNetBinding
{
    internal static class Resources
    {
        internal static string FormatUnsupportedContentType(object p0)
            => string.Format(CultureInfo.CurrentCulture, "Unsupported content type '{0}'.", p0);

        internal static string FormatInputFormattersAreRequired(object p0, object p1, object p2)
            => string.Format(CultureInfo.CurrentCulture, "'{0}.{1}' must not be empty. At least one '{2}' is required to bind from the body.", p0, p1, p2);

        internal static string FormatComplexTypeModelBinder_NoParameterlessConstructor_ForParameter(object p0, object p1)
            => string.Format(CultureInfo.CurrentCulture, "Could not create an instance of type '{0}'. Model bound complex types must not be abstract or value types and must have a parameterless constructor. Alternatively, give the '{1}' parameter a non-null default value.", p0, p1);

        internal static string FormatComplexTypeModelBinder_NoParameterlessConstructor_ForProperty(object p0, object p1, object p2)
            => string.Format(CultureInfo.CurrentCulture, "Could not create an instance of type '{0}'. Model bound complex types must not be abstract or value types and must have a parameterless constructor. Alternatively, set the '{1}' property to a non-null value in the '{2}' constructor.", p0, p1, p2);

        internal static string FormatComplexTypeModelBinder_NoParameterlessConstructor_ForType(object p0)
            => string.Format(CultureInfo.CurrentCulture, "Could not create an instance of type '{0}'. Model bound complex types must not be abstract or value types and must have a parameterless constructor.", p0);
    }
}
