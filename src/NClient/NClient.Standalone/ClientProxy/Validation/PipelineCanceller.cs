using NClient.Providers.Transport.Common;
using System.Threading;

namespace NClient.Standalone.ClientProxy.Validation
{
    internal class PipelineCanceller : IPipelineCanceller
    {
        internal CancellationTokenSource _canceller;
        internal bool _cancellationRequested;
        public PipelineCanceller()
        {
            _cancellationRequested = false;
            _canceller = new();
        }
        public void Renew()
        {
            _canceller = new();
        }
        public void Cancel()
        {
            _canceller.Cancel();
        }

        public bool IsCancellationRequested
        {
            get
            {
                return _canceller.IsCancellationRequested;
            }
        }
    }
}
