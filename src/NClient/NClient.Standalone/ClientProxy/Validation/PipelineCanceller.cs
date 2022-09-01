using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using NClient.Providers.Transport.Common;

namespace NClient.Standalone.ClientProxy.Validation
{
    internal class PipelineCanceller : IPipelineCanceller
    {
        internal bool _cancellationRequested;
        public PipelineCanceller()
        {
            _cancellationRequested = false;
        }
        public void Renew()
        {
            _cancellationRequested = false;
        }
        public void Cancel()
        {
            _cancellationRequested = true;
        }

        public bool IsCancellationRequested
        {
            get
            {
                return _cancellationRequested;
            }
        }
    }
}
