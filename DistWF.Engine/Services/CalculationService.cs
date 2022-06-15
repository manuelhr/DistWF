using DistWF.Common.Model;
using DistWF.Common.Services;

namespace DistWF.Engine.Services
{
    public class CalculationService : ICalculationService
    {
        public CalculationResponse Divide(CalculationRequest request)
        {
            if (request == null) return new CalculationResponse() { Message = "Solicitud no válida." };
            if (request.Operand2 == 0) return new CalculationResponse() { Message = "Solicitud no válida. El divisor no puede ser cero." };

            var response = new CalculationResponse
            {
                Result = request.Operand1 / request.Operand2,
                Success = true
            };
            return response;
        }

        public CalculationResponse Multiply(CalculationRequest request)
        {
            if (request == null) return new CalculationResponse() { Message = "Solicitud no válida." };
            var response = new CalculationResponse
            {
                Result = request.Operand1 * request.Operand2,
                Success = true
            };
            return response;
        }

        public CalculationResponse Substract(CalculationRequest request)
        {
            if (request == null) return new CalculationResponse() { Message = "Solicitud no válida." };
            var response = new CalculationResponse
            {
                Result = request.Operand1 - request.Operand2,
                Success = true
            };
            return response;
        }

        public CalculationResponse Sum(CalculationRequest request)
        {
            if (request == null) return new CalculationResponse() { Message = "Solicitud no válida." };
            var response = new CalculationResponse
            {
                Result = request.Operand1 + request.Operand2,
                Success = true
            };
            return response;

        }
    }
}
