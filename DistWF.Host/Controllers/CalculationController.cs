using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using AdapterClient = DistWF.Host.DistWF_AdapterClient;

namespace DistWF.Host.Controllers
{
    [ApiController]
    public class CalculationController : ControllerBase
    {
        private readonly AdapterClient.IClient _adapterClient;

        public CalculationController(AdapterClient.IClient adapterClient)
        {
            _adapterClient = adapterClient;
        }

        [HttpPost("Calculate")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<AdapterClient.CalculationResponse>> Calculate([FromBody] AdapterClient.CalculationRequest request)
        {
            var result = await _adapterClient.CalculateAsync(request);
            return Ok(result);
        }

    }
}
