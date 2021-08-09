using System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MRX.DAL;
using Microsoft.AspNetCore.Authorization;

namespace MRX.API.Controllers
{
    [ApiController]
    [Route("[controller]/api")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        public WeatherForecastController(ApplicationDbContext context)
        {
            _context = context;
        }
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };


        [HttpGet]
        [Authorize(Policy = "EmployerPolicy")]
        public async Task<IActionResult> GetForcasts()
        {
            var forcasts = await _context.Values.ToListAsync();
            return Ok(forcasts);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetForcast(int id)
        {
            var value = await _context.Values.FirstOrDefaultAsync(v => v.Id == id);
            return Ok(value);
        }
    }
}
