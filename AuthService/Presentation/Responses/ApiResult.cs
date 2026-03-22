namespace AuthService.Presentation.Responses;

public class ApiResult
{
    public bool IsSuccess { get; set; }
    public bool IsFailure => !IsSuccess;

    public string Error { get; set; }

    public double ExecutionTimeMs { get; set; }

    public string TraceId { get; set; }
}

public class ApiResult<T> : ApiResult
{
    public T Data { get; set; }
}