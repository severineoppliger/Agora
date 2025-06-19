using Agora.Core.Enums;

namespace Agora.Core.Shared;

/// <summary>
/// Represents the outcome of a business operation, indicating success or failure and containing potential errors.
/// </summary>
public class Result
{
    /// <summary>
    /// Indicates whether the operation was successful.
    /// </summary>
    public bool IsSuccess { get; }
    
    /// <summary>
    /// Indicates whether the operation failed.
    /// </summary>
    public bool IsFailure => !IsSuccess;
    
    /// <summary>
    /// A list of error details if the operation failed.
    /// </summary>
    public List<ErrorDetail>? Errors { get; }
    
    protected Result(bool isSuccess, List<ErrorDetail>? errors)
    {
        IsSuccess = isSuccess;
        Errors = errors;
    }

    /// <summary>
    /// Creates a successful result.
    /// </summary>
    public static Result Success() => new Result(true, null);
    
    /// <summary>
    /// Creates a failed result with a single error.
    /// </summary>
    /// <param name="errorType">The type of the error.</param>
    /// <param name="errorMsg">The error message.</param>
    public static Result Failure(ErrorType errorType, string errorMsg) => new Result(false, [new ErrorDetail(errorType, errorMsg)]);
    
    /// <summary>
    /// Creates a failed result with multiple errors.
    /// </summary>
    /// <param name="errors">The list of error details.</param>
    public static Result Failure(List<ErrorDetail> errors) => new Result(false, errors);
}


/// <summary>
/// Represents the outcome of a business operation that returns a value of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of the value returned by the operation (if it's a success).</typeparam>
public class Result<T> : Result where T : class
{
    /// <summary>
    /// The value returned by the operation if successful.
    /// </summary>
    public T? Value { get; }
    
    private Result(bool isSuccess, List<ErrorDetail>? errors, T? value)
        : base(isSuccess, errors)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a successful result with a value.
    /// </summary>
    /// <param name="value">The value of the result.</param>
    public static Result<T> Success(T value) => new(true, null, value);
    
    /// <summary>
    /// Creates a failed result with a single error.
    /// </summary>
    /// <param name="errorType">The type of the error.</param>
    /// <param name="errorMsg">The error message.</param>
    public new static Result<T> Failure(ErrorType errorType, string errorMsg) => new(false, [new ErrorDetail(errorType, errorMsg)], null);
    
    /// <summary>
    /// Creates a failed result with multiple errors.
    /// </summary>
    /// <param name="errors">The list of error details.</param>
    public new static Result<T> Failure(List<ErrorDetail> errors) => new(false, errors, null);
}