using System;
using System.Collections.Generic;
using System.Linq;
using NClient.Core.Exceptions.Factories;

namespace NClient.Core.Attributes
{
    public interface IAttributeHelper
    {
        Type ApiAttributeType { get; }

        Type MethodAttributeType { get; }
        Type GetAttributeType { get; }
        Type PostAttributeType { get; }
        Type PutAttributeType { get; }
        Type DeleteAttributeType { get; }

        Type RouteParamAttributeType { get; }
        Type UriParamAttributeType { get; }
        Type HeaderParamAttributeType { get; }
        Type BodyParamAttributeType { get; }

        IReadOnlyCollection<Type> NotSupportedMethodAttributes { get; }

        bool IsParameterAttribute(Attribute attribute);
        bool IsRouteParamAttribute(Attribute attribute);
        bool IsUriParamAttribute(Attribute attribute);
        bool IsHeaderParamAttribute(Attribute attribute);
        bool IsBodyParamAttributeType(Attribute attribute);

        bool IsNotSupportedMethodAttribute(Attribute attribute);

        Attribute CreateAttributeInstance(Type type);
    }

    public abstract class AttributeHelperBase : IAttributeHelper
    {
        public abstract Type ApiAttributeType { get; }

        public abstract Type MethodAttributeType { get; }
        public abstract Type GetAttributeType { get; }
        public abstract Type PostAttributeType { get; }
        public abstract Type PutAttributeType { get; }
        public abstract Type DeleteAttributeType { get; }

        public abstract Type RouteParamAttributeType { get; }
        public abstract Type UriParamAttributeType { get; }
        public abstract Type HeaderParamAttributeType { get; }
        public abstract Type BodyParamAttributeType { get; }

        public abstract IReadOnlyCollection<Type> NotSupportedMethodAttributes { get; }

        public bool IsParameterAttribute(Attribute attribute) =>
            IsRouteParamAttribute(attribute)
            || IsUriParamAttribute(attribute) 
            || IsHeaderParamAttribute(attribute) 
            || IsBodyParamAttributeType(attribute);

        public bool IsRouteParamAttribute(Attribute attribute) => attribute.GetType() == RouteParamAttributeType;
        public bool IsUriParamAttribute(Attribute attribute) => attribute.GetType() == UriParamAttributeType;
        public bool IsHeaderParamAttribute(Attribute attribute) => attribute.GetType() == HeaderParamAttributeType;
        public bool IsBodyParamAttributeType(Attribute attribute) => attribute.GetType() == BodyParamAttributeType;

        public bool IsNotSupportedMethodAttribute(Attribute attribute) => NotSupportedMethodAttributes.Contains(attribute.GetType());

        public Attribute CreateAttributeInstance(Type type)
        {
            if (type.IsAssignableFrom(typeof(Attribute)))
                throw InnerExceptionFactory.TypeMustBeAttribute(nameof(type));

            return (Attribute)Activator.CreateInstance(type);
        }
    }
}
