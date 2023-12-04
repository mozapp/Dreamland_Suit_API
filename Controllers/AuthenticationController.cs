using Dreamland_Suit_API.EmailService;
using Dreamland_Suit_API.Models;
using Dreamland_Suit_API.Models.Account.SignUp;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;

namespace Dreamland_Suit_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;

        public AuthenticationController(UserManager<IdentityUser> userManager, 
            RoleManager<IdentityRole> roleManager, IEmailService emailService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _emailService = emailService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegister userRegister, string role) 
        {
            var userExist = await _userManager.FindByEmailAsync(userRegister.Email);
            if(userExist != null)
            {
                return StatusCode(StatusCodes.Status403Forbidden,
                    new Response { Status = "Error", Message = "User already exist!" });
            }

            //Add the user in the database
            IdentityUser user = new ()
            {
                Email = userRegister.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = userRegister.Username
            };

            if(await _roleManager.RoleExistsAsync(role))
            {
                var result = await _userManager.CreateAsync(user, userRegister.Password);
                if (!result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new Response { Status = "Error", Message = "User failed to create" });
                }

                //Add role to the user table...
                await _userManager.AddToRoleAsync(user, role);
                return StatusCode(StatusCodes.Status200OK,
                    new Response { Status = "Success", Message = "User created successfully" });

                //return StatusCode(StatusCodes.Status201Created,
                //    new Response { Status = "Success", Message = "User created successfully" });
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                        new Response { Status = "Error", Message = "This role doesnt exist" });
            }
        }

        [HttpPost]
        public IActionResult SendEmail(EmailDto request)
        {
            _emailService.SendEmail(request);
            return Ok();
        }


        //[HttpGet]
        //public IActionResult TestEmail()
        //{
        //    var message =
        //        new Message(new string[]
        //        {"mosesizu4@gmail.com"}, "Test", "<h1>Subscribe to my channel</h1>");

        //    _emailService.SendEmail(message);
        //    return StatusCode(StatusCodes.Status200OK,
        //        new Response { Status = "Success", Message = "Email sent successfully" });

        //}



        //[HttpPost("register")]
        //public async Task<IActionResult> Register([FromBody] UserRegister userRegister, string role)
        //{
        //    try
        //    {
        //        // Validation
        //        if (userRegister == null)
        //        {
        //            return BadRequest(new Response { Status = "Error", Message = "Invalid input" });
        //        }

        //        // Check if the user already exists
        //        var userExist = await _userManager.FindByEmailAsync(userRegister.Email);
        //        if (userExist != null)
        //        {
        //            return StatusCode(StatusCodes.Status403Forbidden,
        //                new Response { Status = "Error", Message = "User already exists" });
        //        }

        //        // Create user
        //        var user = new IdentityUser
        //        {
        //            Email = userRegister.Email,
        //            SecurityStamp = Guid.NewGuid().ToString(),
        //            UserName = userRegister.Username
        //        };

        //        var result = await _userManager.CreateAsync(user, userRegister.Password);

        //        if (result.Succeeded)
        //        {
        //            // Add user to the specified role if a role is provided
        //            if (!string.IsNullOrEmpty(role))
        //            {
        //                await _userManager.AddToRoleAsync(user, role);
        //            }

        //            return StatusCode(StatusCodes.Status201Created,
        //                new Response { Status = "Success", Message = "User created successfully" });
        //        }
        //        else
        //        {
        //            // Log errors for debugging
        //            foreach (var error in result.Errors)
        //            {
        //                _logger.LogError($"User creation error: {error.Description}");
        //            }

        //            return StatusCode(StatusCodes.Status500InternalServerError,
        //                new Response { Status = "Error", Message = "User failed to create" });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log unexpected exceptions
        //        _logger.LogError($"Unexpected error during user registration: {ex.Message}");
        //        return StatusCode(StatusCodes.Status500InternalServerError,
        //            new Response { Status = "Error", Message = "Unexpected error during user registration" });
        //    }
        //}


    }
}
