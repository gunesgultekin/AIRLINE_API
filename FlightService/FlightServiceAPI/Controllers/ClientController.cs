using FlightServiceAPI.Context;
using FlightServiceAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FlightServiceAPI.Controllers
{
    [ApiController]
    [Route("/api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ClientController : ControllerBase
    {
        private IClient _client;

        public ClientController(IClient client)
        {
            this._client = client;
        }

        [HttpGet("deleteMyTicket")]
        public async Task<ActionResult> deleteMyTicket(string clientEmail, string ticket_id)
        {
            try
            {
                await _client.deleteMyTicket(clientEmail, ticket_id);
                return Ok();
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("getMyTickets")]
        public async Task<ActionResult<List<AIRLINE_TICKETS>>> getMyTickets(string clientEmail)
        {
            try
            {
                return Ok(await _client.getMyTickets(clientEmail));
            }catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("loginMiles")]
        public async Task<ActionResult<int>> loginMiles([FromBody]MilesLoginModel credentials)
        {
            try
            {
                var result = await _client.loginMiles(credentials);
                if (result == "not exists")
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(401, "");
            }
        }

        [HttpPost("registerMiles")]
        public async Task<ActionResult<int>> registerMiles(AIRLINE_MILESMEMBERS credentials)
        {
            try
            {
                return Ok(await _client.registerMiles(credentials));
            }
            catch(Exception ex)
            {
                return BadRequest("register failed");
            }

        }


        [HttpPost("search")]
        public async Task<ActionResult<List<AIRLINE_FLIGHTS>>> search(SearchModel model)
        {
            try
            {
                var response = await _client.search(model); 
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("flexSearch")]
        public async Task<ActionResult<List<AIRLINE_FLIGHTS>>> flexSearch(SearchModel search)
        {
            try
            {
                return Ok(await _client.flexSearch(search));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("buyTicket")]
        public async Task<ActionResult<string>> buyTicket(string flightCode, string clientEmail)
        {
            try
            {
                var result = await _client.buyTicket(flightCode, clientEmail);
                return Ok(result);
            }catch (Exception ex)
            {
                return HttpStatusCode.InternalServerError.ToString();
            }

        }

        [HttpGet("buyWithMiles")]
        public async Task<ActionResult<string>> buyWithMiles(string flightCode, string clientEmail)
        {
            try
            {
                var result = await _client.buyWithMiles(flightCode, clientEmail);
                if (result != "success")
                {
                    return Ok();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok();
            }

        }
    }
}
