namespace NClient.Abstractions.Ensuring
{
    public class NoEnsuringSettings<TRequest, TResponse> : EnsuringSettings<TRequest, TResponse>
    {
        public NoEnsuringSettings() : base(successCondition: _ => true, onFailure: _ => { })
        {
        }
    }
}
