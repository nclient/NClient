using System;

namespace NClient.Abstractions
{
    public interface INClientBuilder
    {
        IInterfaceBasedClientBuilder<T> Use<T>(string host) where T : class;

        [Obsolete("The right way is to add NClient controllers (see AddNClientControllers) and use Use<T> method.")]
        IControllerBasedClientBuilder<TInterface, TController> Use<TInterface, TController>(string host)
            where TInterface : class
            where TController : TInterface;
    }
}