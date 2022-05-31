using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

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

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> Register(Credentials credentials)
        {
            var user = new IdentityUser { UserName = credentials.Email, Email=credentials.Email };

            var result = await _userManager.CreateAsync(user, credentials.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await _signInManager.SignInAsync(user, isPersistent: false);

            var jwt = new JwtSecurityToken();

            //var j = new JwtSecurityTokenHandler().WriteToken(jwt);

            return Ok(new Token(jwt.EncodedHeader));
        }
    }
}
