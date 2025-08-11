//using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.EntityFramework;
//using Microsoft.AspNet.Identity;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MoyoBusinessAdvisory.Models;
using MoyoBusinessAdvisory.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Numerics;
using System.Security.Claims;
using System.Text;
namespace MoyoBusinessAdvisory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly DataContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IUserClaimsPrincipalFactory<AppUser> _claimsPrincipalFactory;
        public UserController(DataContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IUserClaimsPrincipalFactory<AppUser> claimsPrincipalFactory)
        {

            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
            _configuration = configuration;
            _claimsPrincipalFactory = claimsPrincipalFactory;
        }
        // GET: UserController
        

        [HttpGet]
        [Route("CreateUserRoles")]
        public async Task<ActionResult<Product>> CreateUserRoles()
        {
          var result =  await _roleManager.CreateAsync(new IdentityRole("vendor"));
            var result2 = await _roleManager.CreateAsync(new IdentityRole("client"));
            var result3 = await _roleManager.CreateAsync(new IdentityRole("capturer"));
            var result4 = await _roleManager.CreateAsync(new IdentityRole("manager"));
            return CreatedAtAction("CreateVendor", new { id = result }, result);
        }

        [HttpGet]
        [Route("CheckAuthentication")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CheckAuthentication()
        {
       // https://stackoverflow.com/questions/46112258/how-do-i-get-current-user-in-net-core-web-api-from-jwt-token
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var role = "";
            if(userId != null)
            {
                var currentUser = await _userManager.FindByIdAsync(userId);
                var roles = await _userManager.GetRolesAsync(currentUser);
                role = roles.FirstOrDefault();
            }

            // maybe return the user role to the front end. then make it a global variable.
            // https://stackoverflow.com/questions/76518758/how-to-return-plain-string-as-json-in-asp-net-core-7
            return Ok(new { role = role });
        }

       

        [HttpPost]
        [Route("postVendor")]
        public async Task<ActionResult<Product>> CreateVendor(Vendor vendor)
        {
            // https://stackoverflow.com/questions/19689183/add-user-to-role-asp-net-identity
            // https://stackoverflow.com/questions/22486489/entity-framework-6-transaction-rollback
            var result = await vendor.AddUser(_userManager, _context, _roleManager, "vendor");
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Oauth")]

        //https://rajasekar.dev/blog/how-to-implement-google-authentication-in-aspnet-using-angular-and-jwt-a-detailed-guide
        public async Task<ActionResult> OAuth(GoogleSignInVM? googleSignIn) // if vendor wasnt connected then would have to pass thats as well...
        {
            //var payload;
            GoogleJsonWebSignature.Payload payload = null;
            try
            {
                
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new List<string>() { _configuration["Authentication:Google:ClientId"] }
                };
                payload = await GoogleJsonWebSignature.ValidateAsync(googleSignIn.IdToken, settings); // check if signed by google i.e. google is accessing this
                                                                                                      //return payload;

                if (payload != null)
                {
                    var user = await _userManager.FindByEmailAsync(payload.Email);
                    if (user != null)
                    {
                        return GenerateJWTToken(user);
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                                       return BadRequest("Invalid Google Sign-In");
                }
            }
            catch (Exception ex)
            {
                return BadRequest();
                //log an exception

              


            }
        }

            [HttpPost]
        [Route("Login")]
        public async Task<ActionResult> Login(SignInViewModel user) // if vendor wasnt connected then would have to pass thats as well...
        {
            var appUser = await _userManager.FindByNameAsync(user.UserName);

            if (user != null && await _userManager.CheckPasswordAsync(appUser, user.Password))
            {
                return GenerateJWTToken(appUser);
            }
            else
            {
                return NotFound("Does not exist");
            }
        }

        [HttpGet]
        private ActionResult GenerateJWTToken(AppUser user)
        {
            // Create JWT Token
            //https://stackoverflow.com/questions/71646794/how-to-put-user-id-in-registered-claims
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
               new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName), // userName needs to be unq
              // new Claim(JwtRegisteredClaimNames.Email, user.Email) // userName needs to be unq
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Tokens:Issuer"],
                _configuration["Tokens:Audience"],
                claims,
                signingCredentials: credentials,
                expires: DateTime.UtcNow.AddHours(3)
            );

            return Created("", new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                user = user.UserName
            });
        }

        [HttpPost]
        [Route("postCapturer")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<Product>> CreateCapturer(AppUser user)
        {
            var result = await user.AddUser(_userManager, _context, _roleManager, "capturer");
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result);
        }

        [HttpPost]
        [Route("postClient")]
        public async Task<ActionResult<Product>> CreateClient(Client client)
        {
            var result = await client.AddUser(_userManager, _context, _roleManager, "client");
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result);
        }
        // return View();


        [HttpPost]
        [Route("postManager")]
        // only to be run in swagger
        public async Task<ActionResult<Product>> CreateManager(AppUser user)
        {
            //UserManager.
            //AppUser.Children.Add(vendor);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("CreateVendor", new { id = user.Id }, user);
            // return View();
        }


        [HttpGet]
        [Route("getVendors")]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetVendors()
        {
            // returning children of AppUser?

            // NB FIX THE RETURNING OF PASSWORD
           var vendors = await _context.Users.Where(x => x is Vendor).ToArrayAsync();
            //var users = await _context.Users.ToListAsync();
            //Vendor[] vendors = await _context.Users.OfType<Vendor>().ToArrayAsync();
            return vendors;
        }

        [HttpGet]
        [Route("CreateCapturerManually")]
        public async Task<ActionResult<IEnumerable<AppUser>>> CreateCapturerManually()
        {
            var appUser = new AppUser
            {
                
                UserName = "ma.gaitsmith@gmail.com",
                Name = "Mike",
                Email = "ma.gaitsmith@gmail.com",
                Password = "123456"
            };

            var result = await appUser.AddUser(_userManager, _context, _roleManager, "capturer");
            // returning children of AppUser?

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result);

            // NB FIX THE RETURNING OF PASSWORD
            var vendors = await _context.Users.Where(x => x is Vendor).ToArrayAsync();
            //var users = await _context.Users.ToListAsync();
            //Vendor[] vendors = await _context.Users.OfType<Vendor>().ToArrayAsync();
            return vendors;
        }


    }
}
