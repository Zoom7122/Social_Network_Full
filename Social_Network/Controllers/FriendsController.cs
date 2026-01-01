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
    [Authorize]
    public class FriendsController : Controller
    {
        private readonly ICorrectDataUserValidation _correctDataUserBLL;
        private readonly IFreindsBLL _freindsBLL;
        public FriendsController(ICorrectDataUserValidation correctDataUserValidation, IFreindsBLL freindsBLL)
        {
            _correctDataUserBLL = correctDataUserValidation;
            _freindsBLL = freindsBLL;
        }


        [HttpGet]
        [Route("FriendsView")]
        public async Task<IActionResult> FriendsView()
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

            var listFriends = us.FriendsID.ToList();

            var friends = await _freindsBLL.FindFriendsUser(listFriends);

            return View(friends);
        }

        [HttpGet]
        [Route("AddFriends")]
        public async Task<IActionResult> AddFriends()
        {
            return View();
        }

        [HttpPost]
        [Route("AddFriends")]
        public async Task<IActionResult> AddFriends(string email)
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

            var answer = await _freindsBLL.GetuserByEmailToFriends(email, us.Id); 

            return View(answer);
        }

        [HttpGet]
        [Route("FriendIsDeleted")]
        public async Task<IActionResult> FriendIsDeleted(Guid FriendID)
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

            var friend = await _correctDataUserBLL.GetUserByIdAsync(FriendID);
            var answer = await _freindsBLL.DeletedUserBLL(us.Id, FriendID);

            if (answer && friend != null)
            {
                return View(friend);
            }
            else
            {
                return View();
            }
        }
    }
}
