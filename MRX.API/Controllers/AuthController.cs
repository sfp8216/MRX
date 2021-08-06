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
    [Route("[controller]/api")] //Auth/api
        public class AuthController : ControllerBase{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager){
            _userManager = userManager;
            _signInManager = signInManager;

        }
        
        //Post api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel model){
            var user = await _userManager.FindByNameAsync(model.Username);
            if(user == null){
                return BadRequest();
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user,model.Password,false);
            
            if(!result.Succeeded){
                return BadRequest(result);
            }
            return Ok(result);
        }

    }
}
