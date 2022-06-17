namespace DistWF.Common.Model
{
    public class CalculationResponse : ResponseBase
    {
        public CalculationResponse(string message = null) : base(message) { }
        public decimal Result { get; set; }
    }
}
