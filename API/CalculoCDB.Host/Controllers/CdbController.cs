using CalculoCDB.API.Models.CDB;
using CalculoCDB.API.Services.CDB;
using Microsoft.AspNetCore.Mvc;

namespace CalculoCDB.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CdbController : ControllerBase
    {
        private readonly ICdbService _cdbService;

        public CdbController(ICdbService cdbService)
        {
            _cdbService = cdbService;
        }

        [HttpGet("Calculate")]
        public async Task<ActionResult<CdbResult>> CalculateCDBAsync([FromQuery] CdbRequest request)
        {
            var validation = await _cdbService.ValidateRequestAsync(request);

            if (validation.IsValid)
            {
                var result = await _cdbService.CalculateCDBAsync(request.InitialValue, request.Months);
                if (result.Success)
                    return Ok(result);

                return BadRequest(result);
            }

            var invalidResult = new CdbResult()
            {
                Errors = validation.Errors
                    .Select(error => error.ErrorMessage)
                    .ToList()
            };

            return UnprocessableEntity(invalidResult);
        }
    }
}
