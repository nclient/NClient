using System;
using System.Collections.Generic;

namespace NClient.Standalone.Client.Logging
{
    internal class CompositeDisposable : IDisposable
    {
        private readonly IEnumerable<IDisposable> _disposables;
        
        public CompositeDisposable(IEnumerable<IDisposable> disposables)
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
