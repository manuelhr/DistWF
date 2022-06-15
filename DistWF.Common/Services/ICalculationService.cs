using DistWF.Common.Model;

namespace DistWF.Common.Services
{
    public interface ICalculationService
    {
        CalculationResponse Sum(CalculationRequest request);
        CalculationResponse Substract(CalculationRequest request);
        CalculationResponse Multiply(CalculationRequest request);
        CalculationResponse Divide(CalculationRequest request);
    }
}
