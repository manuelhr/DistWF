using DistWF.Common.Model;
namespace DistWF.Common.Services
{
    public interface ICalculationBackend
    {
        CalculationResponse Calculate(CalculationRequest request);
    }
}
