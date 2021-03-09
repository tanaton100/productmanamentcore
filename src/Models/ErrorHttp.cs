namespace ProductmanagementCore.Models
{
    public class ErrorHttp
    {
        public int Code { get; set; }
        public string Message { get; set; } = "";
        public bool Success { get; } = false;
        public object Result { get; set; }
    }
}
