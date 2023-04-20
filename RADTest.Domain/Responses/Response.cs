namespace RADTest.Domain.Responses;

public sealed class Response<T> : IResponse<T>
{
    public static Response<T> Conflict(string? errorMessage)
    {
        return new(ResponseStatus.Conflict, errorMessage);
    }

    public static Response<T> NotFound(string? errorMessage)
    {
        return new(ResponseStatus.NotFound, errorMessage);
    }

    public static Response<T> Success(T? model)
    {
        return new(ResponseStatus.Success, model);
    }

    public static Response<T> Created(T? model)
    {
        return new(ResponseStatus.Created, model);
    }

    public static Response<T> NoContent()
    {
        return new(ResponseStatus.NoContent);
    }

    public ResponseStatus Status { get; private set; }

    public T? Model { get; private set; }

    public string? ErrorMessage { get; private set; }

    private Response(ResponseStatus status)
    {
        Status = status;
    }

    private Response(ResponseStatus status, T? model)
    {
        Model = model;
        Status = status;
    }

    private Response(ResponseStatus status, string? errorMessage)
    {
        ErrorMessage = errorMessage;
        Status = status;
    }
}