namespace RADTest.Domain.Responses;

public interface IResponse<T>
{
    T? Model { get; }

    ResponseStatus Status { get; }

    string? ErrorMessage { get; }
}