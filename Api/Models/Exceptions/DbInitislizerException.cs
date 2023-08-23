namespace Api.Models.Exceptions
{
    public class DbInitislizerException : Exception
    {
        public DbInitislizerException(string message) : base(message)
        {
        }

        protected DbInitislizerException(string message, object error) : base(message)
        {
            Error = error;
        }

        protected DbInitislizerException(string message, object error, Exception innerException) : base(message, innerException)
        {
            Error = error;
        }

        public int StatusCode { get; set; }
        public object? Error { get; set; }
    }
}
