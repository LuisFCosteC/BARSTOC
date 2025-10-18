namespace BARSTOC_API.Utility
{
    public class Response<T>
    {
        public bool Status { get; set; }
        public T? Value { get; set; }
        public string? Msg { get; set; }
    }
}