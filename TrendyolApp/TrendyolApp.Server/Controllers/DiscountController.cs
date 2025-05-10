using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrendyolApp.Server.Services;

namespace TrendyolApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiscountController : ControllerBase
    {
        private readonly IQueueService _queueService;

        public DiscountController(IQueueService queueService)
        {
            _queueService = queueService;
        }

        [HttpGet("get-discount")]
        public async Task<IActionResult> GetDiscount()
        {
            var code = await _queueService.GetDiscountCodeAsync();
            if (code == null)
                return NotFound("No valid discount code available.");
            return Ok(new { code });
        }
    }

}
