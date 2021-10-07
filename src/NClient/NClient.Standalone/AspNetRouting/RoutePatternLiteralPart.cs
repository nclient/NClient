﻿using System.Diagnostics;

namespace NClient.Standalone.AspNetRouting
{
    /// <summary>
    /// Resprents a literal text part of a route pattern. Instances of <see cref="RoutePatternLiteralPart"/>
    /// are immutable.
    /// </summary>
    internal sealed class RoutePatternLiteralPart : RoutePatternPart
    {
        internal RoutePatternLiteralPart(string content)
            : base(RoutePatternPartKind.Literal)
        {
            Debug.Assert(!string.IsNullOrEmpty(content));
            Content = content;
        }

        /// <summary>
        /// Gets the text content.
        /// </summary>
        public string Content { get; }

        internal override string DebuggerToString()
        {
            return Content;
        }
    }
}
