using System;
using System.Collections.Generic;
using System.Text;

namespace NClient.Core.Helpers.TemplateParsers
{
    public static class TemplateParser
    {
        public static RouteTemplate Parse(string routeTemplate)
        {
            if (routeTemplate == null)
            {
                throw new ArgumentNullException(routeTemplate);
            }

            try
            {
                var inner = RoutePatternFactory.Parse(routeTemplate);
                return new RouteTemplate(inner);
            }
            catch (RoutePatternException ex)
            {
                // Preserving the existing behavior of this API even though the logic moved.
                throw new ArgumentException(ex.Message, nameof(routeTemplate), ex);
            }
        }
    }
}
