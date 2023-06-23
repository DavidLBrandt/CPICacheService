using CPICacheService.Models;
using CPICacheService.Utilities;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CPICacheService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CpiDataController : ControllerBase
    {
        // GET <CpiDataController>/LAUCN040010000000005/2022/January
        [HttpGet("{seriesid}/{year}/{month}")]
        public ActionResult<Cpi> Get(string seriesid, string year, string month)
        {
            if (!PropertyValidator.IsValidSeriesIdFormat(seriesid))
                return BadRequest("Invalid series id format.");

            if (!PropertyValidator.IsValidYear(year))
                return BadRequest("Invalid year format.");

            if (!PropertyValidator.IsValidMonth(month))
                return BadRequest("Invalid month format.");

            Cpi response = new Cpi { 
                SeriesId = seriesid,
                year = year,
                month = month
            };

            throw new NotImplementedException(); // work in progress

            return Ok(response);
        }
    }
}
