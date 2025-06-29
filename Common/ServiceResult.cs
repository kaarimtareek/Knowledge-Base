namespace Common;

public abstract class ServiceResult
{
    public bool IsSuccess { get; init; }
    public string Message { get; init; }
}

public class ServiceResult<T> : ServiceResult
{
    public T Data { get; init; }

    public static ServiceResult<T> Success(T data, string message = null)
    {
        return new ServiceResult<T>
        {
            IsSuccess = true,
            Data = data,
            Message = message
        };
    }

    public static ServiceResult<T> Failure(string message)
    {
        return new ServiceResult<T>
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

    public ApiResponse<T> ToFailureApiResponse(string errorCode = null, object errorData = null)
    {
        return ApiResponse<T>.Failure(Message, errorCode, errorData);
    }
}