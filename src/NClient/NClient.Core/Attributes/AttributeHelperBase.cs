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

        Type FromUriAttributeType { get; }
        Type FromHeaderAttributeType { get; }
        Type FromBodyAttributeType { get; }

        IReadOnlyCollection<Type> NotSupportedMethodAttributes { get; }

        bool IsParameterAttribute(Attribute attribute);
        bool IsFromUriAttribute(Attribute attribute);
        bool IsFromHeaderAttribute(Attribute attribute);
        bool IsFromBodyAttributeType(Attribute attribute);

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

        public abstract Type FromUriAttributeType { get; }
        public abstract Type FromHeaderAttributeType { get; }
        public abstract Type FromBodyAttributeType { get; }

        public abstract IReadOnlyCollection<Type> NotSupportedMethodAttributes { get; }

        public bool IsParameterAttribute(Attribute attribute) => IsFromUriAttribute(attribute) || IsFromHeaderAttribute(attribute) || IsFromBodyAttributeType(attribute);
        public bool IsFromUriAttribute(Attribute attribute) => attribute.GetType() == FromUriAttributeType;
        public bool IsFromHeaderAttribute(Attribute attribute) => attribute.GetType() == FromHeaderAttributeType;
        public bool IsFromBodyAttributeType(Attribute attribute) => attribute.GetType() == FromBodyAttributeType;

        public bool IsNotSupportedMethodAttribute(Attribute attribute) => NotSupportedMethodAttributes.Contains(attribute.GetType());

        public Attribute CreateAttributeInstance(Type type)
        {
            if (type.IsAssignableFrom(typeof(Attribute)))
                throw InnerExceptionFactory.TypeMustBeAttribute(nameof(type));

            return (Attribute)Activator.CreateInstance(type);
        }
    }
}
