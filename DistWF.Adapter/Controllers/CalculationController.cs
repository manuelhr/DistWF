using DistWF.Common.Model;
using DistWF.Common.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace DistWF.Adapter.Controllers
{
    [ApiController]
    public class CalculationController : ControllerBase
    {
        private readonly ICalculationBackend _calculationBackend;
        public CalculationController(ICalculationBackend calculationBackend)
        {
            _calculationBackend = calculationBackend;
        }
        [HttpPost("Calculate")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<CalculationResponse>> Calculate([FromBody] CalculationRequest request)
        {
            var result = _calculationBackend.Calculate(request);
            return Ok(result);
        }
    }
}
