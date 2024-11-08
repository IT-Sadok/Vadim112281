namespace PrivateHospitals.Application.Responses;

public class ServiceResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public List<string> Errors { get; set; } = new List<string>();

    public static ServiceResponse<T> SuccessResponse(T data)
    {
        return new ServiceResponse<T>
        {
            Success = true,
            Data = data
        };
    }

    public static ServiceResponse<T> ErrorResponse(List<string> errors)
    {
        return new ServiceResponse<T>
        {
            Success = false,
            Errors = errors
        };
    }
}