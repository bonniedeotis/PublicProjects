namespace LibraryManagement.Core.Entities
{
    public class Result
    {
        public bool Ok { get; }
        public string Message { get; private set; }

        public Result(bool success, string message)
        {
            Ok = success;
            Message = message;
        }
    }

    public class Result<T> : Result // <T> is a type - any type
    {
        public T? Data { get; set; } // nullable - may or may not have data

        public Result(T data, bool success, string message) : base(success, message)
        {
            Data = data;
            //constructor will always return an Ok and a Message, even if no data
        }
    }

    public class ResultFactory
    {
        public static Result Success()
        {
            return new Result(true, string.Empty);
        }

        public static Result Fail(string message)
        {
            return new Result(false, message);
        }

        public static Result<T> Success<T>(T data)
        {
            return new Result<T>(data, true, string.Empty);
        }

        public static Result<T> Fail<T>(string message)
        {
            return new Result<T>(default, false, message);
        }
    }
}
