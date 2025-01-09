namespace TaxiDigital.SharedKernel;

public class ApiResult
{
    public static readonly ApiResult Empty = new();

    public int StatusCode { get; }
    public string Message { get; }

    public bool OK => string.IsNullOrEmpty(Message);
}

public class ApiResult<T> : ApiResult
{
    public T Result { get; set; }

    public ApiResult() { }

    public ApiResult(T value)
    {
        Result = value;
    }
}