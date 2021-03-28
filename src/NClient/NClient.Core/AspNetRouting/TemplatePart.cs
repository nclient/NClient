﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace NClient.Core.AspNetRouting
{
    /// <summary>
    /// Represents a part of a route template segment.
    /// </summary>
    internal class TemplatePart
    {
        /// <summary>
        /// Constructs a new <see cref="TemplatePart"/> instance.
        /// </summary>
        public TemplatePart()
        {
        }

        /// <summary>
        /// Constructs a new <see cref="TemplatePart"/> instance given a <paramref name="other"/>.
        /// </summary>
        /// <param name="other">A <see cref="RoutePatternPart"/> instance representing the route part.</param>
        public TemplatePart(RoutePatternPart other)
        {
            IsLiteral = other.IsLiteral || other.IsSeparator;
            IsParameter = other.IsParameter;

            if (other.IsLiteral && other is RoutePatternLiteralPart literal)
            {
                Text = literal.Content;
            }
            else if (other.IsParameter && other is RoutePatternParameterPart parameter)
            {
                // Text is unused by TemplatePart and assumed to be null when the part is a parameter.
                Name = parameter.Name;
                IsCatchAll = parameter.IsCatchAll;
                IsOptional = parameter.IsOptional;
                DefaultValue = parameter.Default;
                InlineConstraints = parameter.ParameterPolicies?.Select(p => new InlineConstraint(p)) ?? Enumerable.Empty<InlineConstraint>();
            }
            else if (other.IsSeparator && other is RoutePatternSeparatorPart separator)
            {
                Text = separator.Content;
                IsOptionalSeperator = true;
            }
            else
            {
                // Unreachable
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Create a <see cref="TemplatePart"/> representing a literal route part.
        /// </summary>
        /// <param name="text">The text of the literate route part.</param>
        /// <returns>A <see cref="TemplatePart"/> instance.</returns>
        public static TemplatePart CreateLiteral(string text)
        {
            return new TemplatePart()
            {
                IsLiteral = true,
                Text = text,
            };
        }

        /// <summary>
        /// Creates a <see cref="TemplatePart"/> representing a paramter part.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="isCatchAll"><see langword="true"/> if the parameter is a catch-all parameter.</param>
        /// <param name="isOptional"><see langword="true"/> if the parameter is an optional parameter.</param>
        /// <param name="defaultValue">The default value of the parameter.</param>
        /// <param name="inlineConstraints">A collection of constraints associated with the parameter.</param>
        /// <returns>A <see cref="TemplatePart"/> instance.</returns>
        public static TemplatePart CreateParameter(
            string name,
            bool isCatchAll,
            bool isOptional,
            object? defaultValue,
            IEnumerable<InlineConstraint>? inlineConstraints)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            return new TemplatePart()
            {
                IsParameter = true,
                Name = name,
                IsCatchAll = isCatchAll,
                IsOptional = isOptional,
                DefaultValue = defaultValue,
                InlineConstraints = inlineConstraints ?? Enumerable.Empty<InlineConstraint>(),
            };
        }

        /// <summary>
        /// <see langword="true"/> if the route part is is a catch-all part (e.g. /*).
        /// </summary>
        public bool IsCatchAll { get; private set; }
        /// <summary>
        /// <see langword="true"/> if the route part is represents a literal value.
        /// </summary>
        public bool IsLiteral { get; private set; }
        /// <summary>
        /// <see langword="true"/> if the route part represents a parameterized value.
        /// </summary>
        public bool IsParameter { get; private set; }
        /// <summary>
        /// <see langword="true"/> if the route part represents an optional part.
        /// </summary>
        public bool IsOptional { get; private set; }
        /// <summary>
        /// <see langword="true"/> if the route part represents an optional seperator.
        /// </summary>
        public bool IsOptionalSeperator { get; set; }
        /// <summary>
        /// The name of the route parameter. Can be null.
        /// </summary>
        public string? Name { get; private set; }
        /// <summary>
        /// The textual representation of the route paramter. Can be null. Used to represent route seperators and literal parts.
        /// </summary>
        public string? Text { get; private set; }
        /// <summary>
        /// The default value for route paramters. Can be null.
        /// </summary>
        public object? DefaultValue { get; private set; }
        /// <summary>
        /// The constraints associates with a route paramter.
        /// </summary>
        public IEnumerable<InlineConstraint> InlineConstraints { get; private set; } = Enumerable.Empty<InlineConstraint>();

        internal string? DebuggerToString()
        {
            if (IsParameter)
            {
                return "{" + (IsCatchAll ? "*" : string.Empty) + Name + (IsOptional ? "?" : string.Empty) + "}";
            }
            else
            {
                return Text;
            }
        }

        /// <summary>
        /// Creates a <see cref="RoutePatternPart"/> for the route part designated by the <see cref="TemplatePart"/>.
        /// </summary>
        /// <returns>A <see cref="RoutePatternPart"/> instance.</returns>
        public RoutePatternPart ToRoutePatternPart()
        {
            if (IsLiteral && IsOptionalSeperator)
            {
                return RoutePatternFactory.SeparatorPart(Text!);
            }
            else if (IsLiteral)
            {
                return RoutePatternFactory.LiteralPart(Text!);
            }
            else
            {
                var kind = IsCatchAll ?
                    RoutePatternParameterKind.CatchAll :
                    IsOptional ?
                        RoutePatternParameterKind.Optional :
                        RoutePatternParameterKind.Standard;

                var constraints = InlineConstraints.Select(c => new RoutePatternParameterPolicyReference(c.Constraint));
                return RoutePatternFactory.ParameterPart(Name!, DefaultValue, kind, constraints);
            }
        }
    }
}
