﻿using System;

namespace NClient.Standalone.AspNetRouting
{
    /// <summary>
    /// The parsed representation of an inline constraint in a route parameter.
    /// </summary>
    internal class InlineConstraint
    {
        /// <summary>
        /// Creates a new instance of <see cref="InlineConstraint"/>.
        /// </summary>
        /// <param name="constraint">The constraint text.</param>
        public InlineConstraint(string constraint)
        {
            if (constraint == null)
            {
                throw new ArgumentNullException(nameof(constraint));
            }

            Constraint = constraint;
        }

        /// <summary>
        /// Creates a new <see cref="InlineConstraint"/> instance given a <see cref="RoutePatternParameterPolicyReference"/>.
        /// </summary>
        /// <param name="other">A <see cref="RoutePatternParameterPolicyReference"/> instance.</param>
        public InlineConstraint(RoutePatternParameterPolicyReference other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            Constraint = other.Content!;
        }

        /// <summary>
        /// Gets the constraint text.
        /// </summary>
        public string Constraint { get; }
    }
}
