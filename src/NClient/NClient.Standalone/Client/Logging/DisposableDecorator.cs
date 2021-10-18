using System;
using System.Collections.Generic;

namespace NClient.Standalone.Client.Logging
{
    internal class DisposableDecorator : IDisposable
    {
        private readonly IEnumerable<IDisposable> _disposables;
        
        public DisposableDecorator(IEnumerable<IDisposable> disposables)
        {
            _disposables = disposables;
        }
        
        public void Dispose()
        {
            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }
        }
    }
}
