using Architecture.API.Application.Commands;
using Architecture.API.Application.Models;
using Architecture.API.Application.Queries;
using Architecture.Domain.AggregatesModel;
using Architecture.Domain.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Architecture.API.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICountryQueries _countryQueries;
        private readonly ILogger<CountryController> _logger;

        public CountryController(IMediator mediator, ICountryQueries queries, ILogger<CountryController> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _countryQueries = queries ?? throw new ArgumentNullException(nameof(queries));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [Route("create")]
        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> CreateCountryAsync(CreateCountryCommand createCommand)
        {
            try
            {
                bool commandResult = false;

                if (createCommand == null)
                {
                    return BadRequest();
                }

                commandResult = await _mediator.Send(createCommand);

                return Ok(commandResult);
            }
            catch (Exception ex)
            {
                Log.Logger.Here().Error($"Exception: {JsonConvert.SerializeObject(ex)}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route("replace")]
        [HttpPut]
        [ProducesResponseType(typeof(string), StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> ReplaceCountryAsync([FromBody] CountryDto Country)
        {
            try
            {
                bool commandResult = false;
                if (Country == null)
                    return BadRequest();

                commandResult = await _mediator.Send(new ReplaceCountryCommand(Country));

                if (!commandResult)
                {
                    return NotFound();
                }

                return Ok(commandResult);
            }
            catch (Exception ex)
            {
                Log.Logger.Here().Error($"Exception: {JsonConvert.SerializeObject(ex)}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route("delete/{id}")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> DeleteCountryAsync(int id)
        {
            try
            {
                bool commandResult = false;
                if (id == 0)
                    return BadRequest();

                commandResult = await _mediator.Send(new DeleteCountryCommand(id));

                return Ok(commandResult);
            }
            catch (Exception ex)
            {
                Log.Logger.Here().Error($"Exception: {JsonConvert.SerializeObject(ex)}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [Route("get/{id}")]
        [HttpGet]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Country>> GetCountryByIdAsync([FromRoute] int id)
        {
            try
            {
                var entity = await _countryQueries.GetByIdAsync(id);

                if (entity == null)
                    return NotFound();

                return Ok(entity);
            }
            catch (Exception ex)
            {
                Log.Logger.Here().Error($"Exception: {JsonConvert.SerializeObject(ex)}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route("getAll")]
        [HttpGet]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Country>> GetCountrysAsync()
        {
            try
            {
                //Please setup sql connection to call the method of the query
                //var result = await _countryQueries.GetAll();

                //Dummy country list
                var result = new List<CountryDto>()
                    {
                        new CountryDto{Id=1, Name="Canada", Code= "CA" },
                        new CountryDto { Id = 2, Name = "United States", Code = "USA" }
                    };
                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Logger.Here().Error($"Exception: {JsonConvert.SerializeObject(ex)}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        } 
    }
}