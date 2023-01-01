namespace NClient.Providers.Transport.Common
{
    public interface IPipelineCanceller
    {
        void Renew();
        void Cancel();
        bool IsCancellationRequested { get; }
        void Dispose();
    }
}
