using CPICacheService.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CPICacheService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CpiDataController : ControllerBase
    {
        // GET <CpiDataController>/LAUCN040010000000005/2022/6
        [HttpGet("{seriesid}/{year:int}/{month:int}")]
        public ActionResult<Cpi> Get(string seriesid, int year, int month)
        {
            Cpi response = new Cpi { 
                SeriesId = seriesid,
                year = year,
                month = month
            };

            return Ok(response);
        }
    }
}
