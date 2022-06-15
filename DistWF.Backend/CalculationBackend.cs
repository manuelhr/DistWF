using DistWF.Common.Model;
using DistWF.Common.Services;
using Microsoft.Extensions.Logging;

namespace DistWF.Backend
{
    public class CalculationBackend : ICalculationBackend
    {
        private readonly string _currentBackEndName;
        private readonly ICalculationService _calculationService;
        private readonly ILogger _logger;

        public CalculationBackend(string currentBackEndName,
                                                    ICalculationService calculationService,
                                                    ILogger logger)
        {
            _currentBackEndName = currentBackEndName;
            _calculationService = calculationService;
            _logger = logger;
        }

        public CalculationResponse Calculate(CalculationRequest request)
        {
            var response = new CalculationResponse() { BackEndName = _currentBackEndName };
            switch (request.ServiceName)
            {
                case CalculationServiceNames.Sum:
                    response = _calculationService.Sum(request);
                    break;
                case CalculationServiceNames.Substract:
                    response = _calculationService.Substract(request);
                    break;
                case CalculationServiceNames.Multiply:
                    response = _calculationService.Multiply(request);
                    break;
                case CalculationServiceNames.Divide:
                    response = _calculationService.Divide(request);
                    break;
                default:
                    response.Message = "Solicitud no válida. Servicio no reconocido.";
                    break;
            }
            response.BackEndName = _currentBackEndName;
            #region Logueo de resultados
            if (response.Success)
            {
                _logger.Log(LogLevel.Warning, $"backend={_currentBackEndName}; service={request.ServiceName}; op1={request.Operand1}; op2:{request.Operand2}; result={response.Result}");
            }
            else
            {
                _logger.Log(LogLevel.Error, $"backend={_currentBackEndName}; service={request.ServiceName}; op1={request.Operand1}; op2:{request.Operand2}; result={response.Result}; msg={response.Message}");
            }
            #endregion

            return response;
        }

    }
}
