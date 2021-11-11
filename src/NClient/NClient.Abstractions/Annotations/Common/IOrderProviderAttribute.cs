// ReSharper disable once CheckNamespace

namespace NClient.Annotations
{
    public interface IOrderProviderAttribute
    {
        /// <summary>
        /// Gets the route order. The order determines the order of route execution. Routes with a lower
        /// order value are tried first. When a route doesn't specify a value, it gets a default value of 0.
        /// A null value for the Order property means that the user didn't specify an explicit order for the
        /// route.
        /// </summary>
        public int Order { get; }
    }
}
