using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Social_network.BLL;
using Social_network.BLL.Intarface;
using Social_network.BLL.Models;
using Social_network.DAL.Models;
using Social_Network.Models;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Social_Network.Controllers
{
    public class HomeController : Controller
    {
        private readonly IValidationUser _validUserBLL;
        private readonly ICorrectDataUserValidation _correctDataUserBLL;
        public HomeController(IValidationUser validationUser, ICorrectDataUserValidation correctDataUserValidation) 
        {
            _validUserBLL = validationUser;
            _correctDataUserBLL = correctDataUserValidation;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            return RedirectToAction("Login","Home");
        }
        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(ForLoginUser user)
        {
            var us = await _correctDataUserBLL.UserCanLOgINAccount(user);
            if (us != null)
            {
                var claims = new List<Claim>
                {
                    new Claim("UserId", us.Id.ToString())
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    // ВАЖНО: IsPersistent = true создаёт постоянную куку
                    IsPersistent = true, // ? Это ключевая настройка!
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7),
                    AllowRefresh = true,
                    IssuedUtc = DateTimeOffset.UtcNow
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity),
                    authProperties);

                return View("MainAccountView",us);
            }
            else return StatusCode(404, "Не найден пользователь");
        }

        [Authorize]
        [HttpGet]
        [Route("MainAccountView")]
        public async Task<IActionResult> MainAccountView(User user)
        {
            return View("MainAccountView", user);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(User newUser)
        {
            try
            {
                await _validUserBLL.AccreptUser(newUser);
                return View(newUser);
            }
            catch (ExceptionUser ex)
            {
                return StatusCode(401, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(401, ex.Message);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
