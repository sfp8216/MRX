using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MRX.API.ViewModels;
using MRX.DAL;
using MRX.DAL.Entity;
using MRX.SER.DTO;
using MRX.SER.Interfaces;

namespace MRX.API.Controllers
{
    [ApiController]
    [Route("[controller]/api")] //Agents/api
    public class AgentsController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IOptions<EmailOptionsDTO> _emailOptions;
        private readonly IEmail _email;
        public AgentsController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IOptions<EmailOptionsDTO> emailOptions, IEmail email)
        {
            _email = email;
            _emailOptions = emailOptions;
            _roleManager = roleManager;
            _userManager = userManager;

        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateAgentViewModel model)
        {
            if (!await _roleManager.RoleExistsAsync("Employer"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Employer"));
            }

            var agent = new User
            {
                UserName = model.Username,
                Email = model.Email
            };
            var result = await _userManager.CreateAsync(agent, model.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result);
            }
            //Send Emails
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(agent);
            var confirmEmailUrl = Request.Headers["confirmEmailUrl"];//Http:localhost:4200/email-confirm

            var uriBuilder = new UriBuilder(confirmEmailUrl);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["token"] = token;
            query["userId"] = agent.Id;
            uriBuilder.Query = query.ToString();
            var urlString = uriBuilder.ToString();

            var emailBody = $"Please confirm your email by clicking on the link</br>{urlString}";
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





            var userFromDB = await _userManager.FindByNameAsync(agent.UserName);
            await _userManager.AddToRoleAsync(userFromDB, "Employer");


            return Ok(new
            {
                result,
                emailBody
            });
        }

    }
}