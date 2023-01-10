using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Architecture.API.Application.Models;
using Architecture.API.Application.Queries;
using Architecture.Domain.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Architecture.API.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILocationQueries _locationQueries;
        private readonly ILogger<LocationController> _logger;

        public LocationController(IMediator mediator, ILocationQueries queries, ILogger<LocationController> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _locationQueries = queries ?? throw new ArgumentNullException(nameof(queries));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [Route("getDistance/{location_one}/{location_two}")]
        [HttpGet]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Response>> getDistanceAsync(string location_one, string location_two)
        {
            try
            {

                if (string.IsNullOrEmpty(location_one) || string.IsNullOrEmpty(location_two))
                    return BadRequest();

                var commandResult = await _locationQueries.GetDistanceAsync(location_one, location_two);

                return Ok(commandResult);
            }
            catch (Exception ex)
            {
                Log.Logger.Here().Error($"Exception: {JsonConvert.SerializeObject(ex)}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }



    }
}
