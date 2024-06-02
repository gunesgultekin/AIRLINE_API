using FlightServiceAPI.Context;
using FlightServiceAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightServiceAPI.Controllers
{

    [ApiController]
    [Route("/api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class FlightManagerController : ControllerBase
    {
        public IFlightManager _flightManager;
        public FlightManagerController(IFlightManager flightManager) 
        {
            this._flightManager = flightManager;
        }

        [HttpGet("getAirports")]
        public async Task<ActionResult<List<AIRLINE_AIRPORTS>>> getAirports()
        {
            try
            {
                return Ok(await _flightManager.getAirports());
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("getCities")]
        public async Task<ActionResult<List<AIRLINE_CITIES>>> getCities()
        {
            try
            {
                return Ok(await _flightManager.getCities());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }



        [HttpPost("adminLogin")]
        public async Task<ActionResult<string>> adminLogin([FromBody]AdminLoginModel credentials)
        {
            try
            {
                var result = await _flightManager.adminLogin(credentials);
                if (result == "not exists")
                {
                    return BadRequest(result);

                }
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest("Unauthorized Login Attempt");

            }

        }

        [Authorize]
        [HttpGet("getAll")]
        public async Task<ActionResult<List<AIRLINE_FLIGHTS>>> getAll()
        {
            try
            {
                return Ok(await _flightManager.getAll());    
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [Authorize]
        [HttpGet("deleteFlight")]
        public async Task<ActionResult> deleteFlight(string flightCode)
        {
            try
            {
                await _flightManager.deleteFlight(flightCode);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

        [Authorize]
        [HttpPost("addFlight")]
        public async Task<ActionResult<string>> addFlight([FromBody] AddFlightModel flight)
        {
            try
            {
                return Ok(await _flightManager.addFlight(flight));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
