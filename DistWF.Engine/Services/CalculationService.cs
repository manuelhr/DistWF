using DistWF.Common.Model;
using DistWF.Common.Services;

namespace DistWF.Engine.Services
{
    public class CalculationService : ICalculationService
    {
        public CalculationResponse Divide(CalculationRequest request)
        {
            if (!IsValidCalculationRequest(request, CalculationServiceOperations.Divide, out string message))
                return new CalculationResponse(message);

            var response = new CalculationResponse()
            {
                Result = request.Operand1 / request.Operand2,
                Success = true
            };
            return response;
        }

        public CalculationResponse Multiply(CalculationRequest request)
        {
            if (!IsValidCalculationRequest(request, CalculationServiceOperations.Multiply, out string message))
                return new CalculationResponse(message);

            var response = new CalculationResponse()
            {
                Result = request.Operand1 * request.Operand2,
                Success = true
            };
            return response;
        }

        public CalculationResponse Substract(CalculationRequest request)
        {
            if (!IsValidCalculationRequest(request, CalculationServiceOperations.Substract, out string message))
                return new CalculationResponse(message);

            var response = new CalculationResponse()
            {
                Result = request.Operand1 - request.Operand2,
                Success = true
            };
            return response;
        }

        public CalculationResponse Sum(CalculationRequest request)
        {
            if (!IsValidCalculationRequest(request, CalculationServiceOperations.Sum, out string message))
                return new CalculationResponse(message);

            var response = new CalculationResponse()
            {
                Result = request.Operand1 + request.Operand2,
                Success = true
            };
            return response;

        }

        private bool IsValidCalculationRequest(CalculationRequest request,
                                                                        CalculationServiceOperations operation,
                                                                        out string message)
        {
            if (request == null)
            {
                message = Messages.InvalidRequest;
                return false;
            }
            switch (operation)
            {
                case CalculationServiceOperations.Divide:
                    if (request.Operand2 == 0)
                    {
                        message = $"{Messages.InvalidRequest}: {Messages.DivisorCannotBeZero}";
                        return false;
                    }
                    break;
            }
            message = null;
            return true;
        }
    }
}
