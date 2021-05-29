using System;

namespace NClient.Abstractions
{
    public interface INClientBuilder
    {
        IOptionalNClientBuilder<TInterface> Use<TInterface>(string host) 
            where TInterface : class;

        [Obsolete("The right way is to add NClient controllers (see AddNClientControllers) and use Use<T> method.")]
        IOptionalNClientBuilder<TInterface> Use<TInterface, TController>(string host)
            where TInterface : class
            where TController : TInterface;
    }
}