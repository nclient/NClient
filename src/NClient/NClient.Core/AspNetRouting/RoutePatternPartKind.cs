namespace NClient.Core.AspNetRouting
{
    /// <summary>
    /// Defines the kinds of <see cref="RoutePatternPart"/> instances.
    /// </summary>
    internal enum RoutePatternPartKind
    {
        /// <summary>
        /// The <see cref="RoutePatternPartKind"/> of a <see cref="RoutePatternLiteralPart"/>.
        /// </summary>
        Literal,

        /// <summary>
        /// The <see cref="RoutePatternPartKind"/> of a <see cref="RoutePatternParameterPart"/>.
        /// </summary>
        Parameter,

        /// <summary>
        /// The <see cref="RoutePatternPartKind"/> of a <see cref="RoutePatternSeparatorPart"/>.
        /// </summary>
        Separator,
    }
}
