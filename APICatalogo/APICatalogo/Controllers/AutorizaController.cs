using APICatalogo.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutorizaController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AutorizaController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            return "AutoriazaController :: Acesso em : " + DateTime.Now.ToLongDateString();
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser([FromBody] UsuarioDTO model)
        {
            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            await _signInManager.SignInAsync(user, isPersistent: false);

            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] UsuarioDTO userInfo)
        {
            var result = await _signInManager.PasswordSignInAsync(userInfo.Email, 
                userInfo.Password, isPersistent: false, lockoutOnFailure: false); // lockoutOnFailure: false -> se tentar mais de 3 vezes NÃO bloqueia

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                ModelState.AddModelError(string.Empty, "login Inválido...");
                return BadRequest(ModelState);
            }
        }
    }
}
