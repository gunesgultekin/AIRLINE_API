
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NotificationServiceAPI.Context;

namespace NotificationServiceAPI.Controllers
{
    [ApiController]
    [Route("/api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class MailServiceController : ControllerBase
    {
        private MailService _service;
        public MailServiceController(MailService service)
        {
            this._service = service;
        }

        [HttpPost("sendMail")]
        public async Task<ActionResult> sendMail([FromBody] MailModel model)
        {
            try
            {
                await _service.sendMail(model);
                return Ok();
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
