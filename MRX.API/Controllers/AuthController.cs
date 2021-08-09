using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MRX.API.ViewModels;
using MRX.DAL;
using MRX.DAL.Entity;
using MRX.SER.DTO;
using MRX.SER.Interfaces;

namespace MRX.API.Controllers
{
    [ApiController]
    [Route("[controller]/api")] //Auth/api
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _config;
        private readonly EmailOptionsDTO _emailOptions;
        /*private readonly IEmail _email;
        public EmailOptionsDTO EmailOptionsDTO { get; }*/

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration config/*, EmailOptionsDTO emailOptionsDTO, IEmail email*/)
        {
            /*
            _email = email;
            this.EmailOptionsDTO = emailOptionsDTO;
            */
            _config = config;
            _userManager = userManager;
            _signInManager = signInManager;

        }

        //Post api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                return BadRequest();
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }
            return Ok(new
            {
                result = result,
                token = JwtTokenGeneratorMachine(user).Result

            });
        }

        //Post api/auth/confirmemail
        [HttpPost("confirmemail")]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailViewModel model)
        {
            var employer = await _userManager.FindByIdAsync(model.UserId);
            var confirm = await _userManager.ConfirmEmailAsync(employer, Uri.UnescapeDataString(model.Token));

            if (confirm.Succeeded)
            {
                return Ok();
            }

            return Unauthorized();

        }

        //Post api/auth/resetpassword
        [HttpPost("resetpassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null || user.EmailConfirmed)
            {
                //Send Email
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var changePasswordUrl = Request.Headers["changePasswordUrl"];//Http:localhost:4200/resetpassword

                var uriBuilder = new UriBuilder(changePasswordUrl);
                var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                query["token"] = token;
                query["userId"] = user.Id;
                uriBuilder.Query = query.ToString();
                var urlString = uriBuilder.ToString();

                var emailBody = $"Click on link to change password</br>{urlString}";
                //Cant really send emails?

                if (1 == 1)
                {
                    Console.Write($"This is the EMAIL Body -->> {emailBody} <<--");
                }
                else
                {
                    //await _email.Send(model.Email, emailBody, _emailOptions.Value);
                }
                ////

                return Ok(new
                {
                    emailBody
                });
            }
            return Unauthorized();
        }

        //Post api/auth/changepassword
        [HttpPost("changepassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            var resetPasswordResult = await _userManager.ResetPasswordAsync(user, Uri.UnescapeDataString(model.Token), model.Password);

            if (resetPasswordResult.Succeeded)
            {
                return Ok();
            }

            return Unauthorized();
        }


        private async Task<string> JwtTokenGeneratorMachine(User userInfo)
        {
            var claims = new List<Claim>{
                new Claim(ClaimTypes.NameIdentifier, userInfo.Id),
                new Claim(ClaimTypes.Name, userInfo.UserName)
            };
            var roles = await _userManager.GetRolesAsync(userInfo);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Key").Value));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

    }
}
