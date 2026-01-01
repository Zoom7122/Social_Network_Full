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

            if (User.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("MainAccountView");
            }

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
                    AllowRefresh = true,
                    IssuedUtc = DateTimeOffset.UtcNow
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity),
                    authProperties);

                return RedirectToAction("MainAccountView");
            }
            else return StatusCode(404, "Не найден пользователь");
        }


        [Authorize]
        [HttpGet]
        [Route("EditProfile")]
        public async Task<IActionResult> EditProfile()
        {
            var userIdClaim = User.FindFirst("UserId");

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Login");
            }

            var us = await _correctDataUserBLL.GetUserByIdAsync(userId);

            if (us == null)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Login");
            }

            return View("EditProfile", us);
        }

        [Authorize]
        [HttpPost]
        [Route("EditProfile")]
        public async Task<IActionResult> EditProfile(string FirstName, string LastName, DateTime BirthDate,
            string Email, string Password)
        {
            var userIdClaim = User.FindFirst("UserId");

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Login");
            }

            var us = await _correctDataUserBLL.GetUserByIdAsync(userId);

            if (us == null)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Login");
            }

            var newUser = new User()
            {
                FirstName = FirstName,
                LastName = LastName,
                BirthDate = BirthDate,
                Email = Email,
                password = Password,
                FriendsID = us.FriendsID,
                MessegesFromUser = us.MessegesFromUser,
                MessegesToUser = us.MessegesToUser,
                Id = us.Id
            };

            bool result = await _validUserBLL.UpdateUser(newUser);

            return View("MainAccountView", newUser);
        }

        [Authorize]
        [HttpGet]
        [Route("MainAccountView")]
        public async Task<IActionResult> MainAccountView()
        {
            var userIdClaim = User.FindFirst("UserId");

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Login");
            }

            var us = await _correctDataUserBLL.GetUserByIdAsync(userId);

            if (us == null)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Login");
            }

            return View("MainAccountView", us);
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

        [HttpGet]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
