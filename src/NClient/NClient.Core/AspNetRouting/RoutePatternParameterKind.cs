namespace NClient.Core.AspNetRouting
{
    /// <summary>
    /// Defines the kinds of <see cref="RoutePatternParameterPart"/> instances.
    /// </summary>
    public enum RoutePatternParameterKind
    {
        /// <summary>
        /// The <see cref="RoutePatternParameterKind"/> of a standard parameter
        /// without optional or catch all behavior.
        /// </summary>
        Standard,

        /// <summary>
        /// The <see cref="RoutePatternParameterKind"/> of an optional parameter.
        /// </summary>
        Optional,

        /// <summary>
        /// The <see cref="RoutePatternParameterKind"/> of a catch-all parameter.
        /// </summary>
        CatchAll,
    }
}
