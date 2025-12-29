using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Social_network.BLL;
using Social_network.BLL.Intarface;
using Social_network.BLL.Models;
using Social_network.DAL.Models;
using Social_Network.Models;
using System.Diagnostics;
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

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        [Route("AddFriends")]
        public IActionResult AddFriends()
        {
            return View();
        }

        //[HttpPost]
        //public IActionResult AddFriends()
        //{
        //    return View();
        //}

        [HttpGet]
        [Route("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(ForLoginUser user)
        {
            var us = await _correctDataUserBLL.UserCanLOgINAccount(user);
            if (us != null)
            {
                return View("MainAccountView",us);
            }
            else return StatusCode(404, "Не найден пользователь");
        }

        [HttpGet]
        [Route("Register")]
        public IActionResult Register()
        {
            return View();
        }

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
