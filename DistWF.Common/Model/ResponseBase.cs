namespace DistWF.Common.Model
{
    public abstract class ResponseBase
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string BackEndName { get; set; }
    }
}
