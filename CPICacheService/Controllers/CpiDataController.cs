using CPICacheService.Models;
using CPICacheService.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Reflection.Metadata.Ecma335;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CPICacheService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CpiDataController : ControllerBase
    {
        private readonly IRepository _repository;

        public CpiDataController(IRepository repository) {
            _repository = repository;
        }

        // GET <CpiDataController>/LAUCN040010000000005/2022/January
        [HttpGet("{seriesid}/{year}/{month}")]
        public async Task<ActionResult<Cpi>> GetAsync(string seriesid, string year, string month)
        {
            try
            {
                IPropertyValidator validator = new PropertyValidator();
                if (!validator.IsValidSeriesIdFormat(seriesid))
                    return BadRequest("Invalid series id format.");

                if (!validator.IsValidYear(year))
                    return BadRequest("Invalid year format.");

                if (!validator.IsValidMonth(month))
                    return BadRequest("Invalid month format.");

                Cpi response = await _repository.GetCpi(seriesid, year, month);

                if (response == null)
                {
                    return NotFound("Data not found.");
                }
                else
                {
                    return Ok(response);
                }
            }
            catch (Exception)
            {
                return Problem("Something unexpected happened.");
            }
        }
    }
}
