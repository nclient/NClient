// ReSharper disable once CheckNamespace

namespace NClient.Providers.Mapping.Results
{
    public interface IResult<TValue, TError>
    {
        public TValue Value { get; }
        public TError Error { get; }
    }

    public class Result<TValue, TError> : IResult<TValue, TError>
    {
        public TValue Value { get; }
        public TError Error { get; }
        
        public Result(TValue value, TError error)
        {
            Value = value;
            Error = error;
        }
    }
}
