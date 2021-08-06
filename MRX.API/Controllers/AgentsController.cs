using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MRX.API.ViewModels;
using MRX.DAL;
using MRX.DAL.Entity;

namespace MRX.API.Controllers
{
    [ApiController]
    [Route("[controller]/api")] //Agents/api
        public class AgentsController : ControllerBase{
    private readonly UserManager<User> _userManager;
        public AgentsController(UserManager<User> userManager){
            _userManager = userManager;

        }
        
        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateAgentViewModel model){

            var agent = new User{
                UserName = model.Username,
                Email = model.Email
            };
            var result = await _userManager.CreateAsync(agent,model.Password);
            if(!result.Succeeded){
                return BadRequest(result);
            }
            return Ok(result);
        }

    }
}
