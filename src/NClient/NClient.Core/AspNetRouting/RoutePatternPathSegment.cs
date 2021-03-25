using System.Collections.Generic;
using System.Linq;

namespace NClient.Core.AspNetRouting
{
    /// <summary>
    /// Represents a path segment in a route pattern. Instances of <see cref="RoutePatternPathSegment"/> are
    /// immutable.
    /// </summary>
    /// <remarks>
    /// Route patterns are made up of URL path segments, delimited by <c>/</c>. A
    /// <see cref="RoutePatternPathSegment"/> contains a group of 
    /// <see cref="RoutePatternPart"/> that represent the structure of a segment
    /// in a route pattern.
    /// </remarks>
    internal sealed class RoutePatternPathSegment
    {
        internal RoutePatternPathSegment(IReadOnlyList<RoutePatternPart> parts)
        {
            Parts = parts;
        }

        /// <summary>
        /// Returns <c>true</c> if the segment contains a single part;
        /// otherwise returns <c>false</c>.
        /// </summary>
        public bool IsSimple => Parts.Count == 1;

        /// <summary>
        /// Gets the list of parts in this segment.
        /// </summary>
        public IReadOnlyList<RoutePatternPart> Parts { get; }

        internal string DebuggerToString()
        {
            return DebuggerToString(Parts);
        }

        internal static string DebuggerToString(IReadOnlyList<RoutePatternPart> parts)
        {
            return string.Join(string.Empty, parts.Select(p => p.DebuggerToString()));
        }
    }
}
