using Microsoft.AspNetCore.Routing;

namespace NClient.Core.AspNetRouting
{
    /// <summary>
    /// The parsed representation of a policy in a <see cref="RoutePattern"/> parameter. Instances
    /// of <see cref="RoutePatternParameterPolicyReference"/> are immutable.
    /// </summary>
    internal sealed class RoutePatternParameterPolicyReference
    {
        internal RoutePatternParameterPolicyReference(string content)
        {
            Content = content;
        }

        internal RoutePatternParameterPolicyReference(IParameterPolicy parameterPolicy)
        {
            ParameterPolicy = parameterPolicy;
        }

        /// <summary>
        /// Gets the constraint text.
        /// </summary>
        public string? Content { get; }

        /// <summary>
        /// Gets a pre-existing <see cref="IParameterPolicy"/> that was used to construct this reference.
        /// </summary>
        public IParameterPolicy? ParameterPolicy { get; }

        private string? DebuggerToString()
        {
            return Content;
        }
    }
}
