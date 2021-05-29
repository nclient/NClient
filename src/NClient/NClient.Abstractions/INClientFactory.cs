using System;

namespace NClient.Abstractions
{
    public interface INClientFactory
    {
        TInterface Create<TInterface>(string host)
            where TInterface : class;

        [Obsolete("The right way is to add NClient controllers (see AddNClientControllers) and use Create<T> method.")]
        TInterface Create<TInterface, TController>(string host)
            where TInterface : class
            where TController : TInterface;
    }
}