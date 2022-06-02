using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace QuizBackend.Controllers
{
    public class Credentials
    {
        public Credentials(string email, string password)
        {
            Email = email;
            Password = password;
        }
        public string Email { get; set; }
        public string Password { get; set; }

    }
    public class Token
    {
        public Token ( string token)
        {
            Value = token;
        }
        public string Value { get; set; }
    }
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Register(Credentials credentials)
        {
            // Create new User at Microft Identity
            var user = new IdentityUser { UserName = credentials.Email, Email=credentials.Email };
            var result = await _userManager.CreateAsync(user, credentials.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // return Token with Jwt for user
            return Ok(CreateToken(user));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(Credentials credentials)
        {

            // Try to sing in with credentials 
            var signResult = await _signInManager.PasswordSignInAsync(credentials.Email, credentials.Password, false, false);

            // Check if succeded
            if (!signResult.Succeeded)
                return BadRequest();

            // Get user
            var user = await _userManager.FindByEmailAsync(credentials.Email);

            // return Token with Jwt for user
            return Ok(CreateToken(user));
        }

        // Create token for given user
        private Token CreateToken ( IdentityUser user)
        {
            // User Claims
            var claim = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id)
            };

            // Sign key
            var secret = _configuration.GetValue<string>($"JwtSecret");
            var singingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var signinCredentials = new SigningCredentials(singingKey, SecurityAlgorithms.HmacSha256);

            // Create Jwt
            var jwt = new JwtSecurityToken(signingCredentials: signinCredentials, claims: claim);

            // return Jwt
            return new Token(new JwtSecurityTokenHandler().WriteToken(jwt));

        }

    }
}
