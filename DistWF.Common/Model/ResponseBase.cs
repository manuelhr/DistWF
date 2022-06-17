namespace DistWF.Common.Model
{
    public abstract class ResponseBase
    {
        public ResponseBase(string message = null)
        {
            Message = message;
        }
        public bool Success { get; set; }
        public string Message { get; set; }
        public string BackEndName { get; set; }
    }
}
