using Agora.Core.Enums;

namespace Agora.Core.Common;

/* Describe the result of a business operation */
public class Result
{
    public bool IsSuccess { get; }
    public List<ErrorDetail>? Errors { get; }
    public bool IsFailure => !IsSuccess;

    protected Result(bool isSuccess, List<ErrorDetail>? errors)
    {
        IsSuccess = isSuccess;
        Errors = errors;
    }

    public static Result Success() => new Result(true, null);
    public static Result Failure(ErrorType errorType, string errorMsg) => new Result(false, [new ErrorDetail(errorType, errorMsg)]);
    public static Result Failure(List<ErrorDetail> errors) => new Result(false, errors);
}


public class Result<T> : Result where T : class
{
    public T? Value { get; }

    private Result(bool isSuccess, List<ErrorDetail>? errors, T? value)
        : base(isSuccess, errors)
    {
        Value = value;
    }

    public static Result<T> Success(T value) => new Result<T>(true, null, value);
    public static Result<T> Failure(ErrorType errorType, string errorMsg, T? value) 
        => new Result<T>(false, [new ErrorDetail(errorType, errorMsg)], value);
    public static Result<T> Failure(List<ErrorDetail> errors, T? value) => new Result<T>(false, errors, value);
    public new static Result<T> Failure(ErrorType errorType, string errorMsg) => new Result<T>(false, [new ErrorDetail(errorType, errorMsg)], null);
    public new static Result<T> Failure(List<ErrorDetail> errors) => new Result<T>(false, errors, null);
}