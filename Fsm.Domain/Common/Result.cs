namespace Fsm.Domain.Common
{
    public abstract class ResultBase
    {
        public bool IsSuccess { get; }
        public bool IsWarning { get; }
        public bool IsError { get; }
        public bool IsInfo { get; }  
        public string Message { get; }

        protected ResultBase(bool isSuccess, bool isWarning, bool isError, bool isInfo, string message)
        {
            IsSuccess = isSuccess;
            IsWarning = isWarning;
            IsError = isError;
            IsInfo = isInfo;        
            Message = message;
        }
    }

    public class Result<T> : ResultBase
    {
        public T? Value { get; }

        protected Result(T value, bool isSuccess, bool isWarning, bool isError, bool isInfo, string message)
            : base(isSuccess, isWarning, isError, isInfo, message)
        {
            Value = value;
        }

        public static Result<T> Success(T value, string message) => new(value, true, false, false, false, message);

        public static Result<T> Warning(string message) => new(default!, false, true, false, false, message);

        public static Result<T> Error(string message) => new(default!, false, false, true, false, message);

        public static Result<T> Info(string message) => new(default!, false, false, false, true, message);

        public static Result<T> Info(T value, string message) => new(value, false, false, false, true, message);
    }

    public class Result : ResultBase
    {
        protected Result(bool isSuccess, bool isWarning, bool isError, bool isInfo, string message)
            : base(isSuccess, isWarning, isError, isInfo, message)
        {
        }

        public static Result Success(string message) => new(true, false, false, false, message);

        public static Result Warning(string message) => new(false, true, false, false, message);

        public static Result Error(string message) => new(false, false, true, false, message);

        public static Result Info(string message) => new(false, false, false, true, message);
    }
}
