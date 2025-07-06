namespace Common;

public abstract class OperationResult
{
    public bool IsSuccess { get; init; }
    public string Message { get; init; }
    public List<string> Errors { get; init; } = new List<string>();
}
public class ErrorObject
{
    public string ErrorCode { get; set; }
    public string Message { get; set; }
    public object Data { get; set; }

    public ErrorObject(string errorCode, string message, object data = null)
    {
        ErrorCode = errorCode;
        Message = message;
        Data = data;
    }
}

public class OperationResult<T> : OperationResult
{
    public T Data { get; init; }

    public static OperationResult<T> Success(T data, string message = null)
    {
        return new OperationResult<T>
        {
            IsSuccess = true,
            Data = data,
            Message = message
        };
    }

    public static OperationResult<T> Failure(string message)
    {
        return new OperationResult<T>
        {
            IsSuccess = false,
            Message = message
        };
    }

    public ApiResponse<T> ToSuccessApiResponse()
    {
        return ApiResponse<T>.Success(Data, Message);
    }

    public ApiResponse<T> ToSuccessApiResponse(string message)
    {
        return ApiResponse<T>.Success(Data, message);
    }

    public ApiResponse<T> ToFailureApiResponse()
    {
        return ApiResponse<T>.Failure(Message);
    }

    public ApiResponse<T> ToFailureApiResponse(string message, string errorCode = null, object errorData = null)
    {
        return ApiResponse<T>.Failure(message ?? Message, errorCode, errorData);
    }
}