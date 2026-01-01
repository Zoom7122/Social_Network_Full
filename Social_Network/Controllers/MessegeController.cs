using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Social_network.BLL.ClassToViewFriends;
using Social_network.BLL.Intarface;
using Social_network.DAL.Interface;
using Social_network.DAL.Models;

namespace Social_Network.PLL.Controllers
{
    [Authorize]
    public class MessegeController : Controller
    {
        private readonly ICorrectDataUserValidation _correctDataUserBLL;
        private readonly IMessegeRepo _messegeRepo;
        private readonly IUserRepo _userRepo;

        public MessegeController(ICorrectDataUserValidation correctDataUserValidation,
            IFreindsBLL freindsBLL, IMessegeRepo messegeRepo, IUserRepo userRepo)
        {
            _correctDataUserBLL = correctDataUserValidation;
            _messegeRepo = messegeRepo;
            _userRepo = userRepo;
        }

        [HttpGet]
        [Route("SendMessege")]
        public async Task<IActionResult> SendMessege(Guid FriendID)
        { 
            var user = await _correctDataUserBLL.GetUserByIdAsync(FriendID);

            return View(user);
        }

        [HttpPost]
        [Route("SendMessege")]
        public async Task<IActionResult> SendMessepe(Guid friendId, string messegeText)
        {
            var userIdClaim = User.FindFirst("UserId");

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Login");
            }

            var user = await _correctDataUserBLL.GetUserByIdAsync(userId);

            if (user == null)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Login");
            }

            var friend = await _correctDataUserBLL.GetUserByIdAsync(friendId);
            if (friend == null)
            {
                return RedirectToAction("FriendsView", "Friends");
            }

            var answer = await _messegeRepo.EnterMessege(user.Id, friend.Id, messegeText);

            if (answer)
                return View("MessegeIsSend");
            else
                return View("MessegeIsNOTSend");
        }

        [HttpGet]
        [Route("MyMessages")]
        public async Task<IActionResult> MyMessages()
        {
            var userIdClaim = User.FindFirst("UserId");

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Login");
            }

            // Получаем ВСЕ сообщения, отправленные мне
            var receivedMessages = await _messegeRepo.GetAllMessegeUserByIDToWroteHim(userId);

            // Для каждого сообщения получаем информацию об отправителе
            var messagesWithSenders = new List<object>();

            foreach (var message in receivedMessages)
            {
                var sender = await _correctDataUserBLL.GetUserByIdAsync(message.FromUser);
                messagesWithSenders.Add(new
                {
                    Message = message,
                    SenderName = sender?.FirstName ?? "Неизвестный",
                    SenderEmail = sender?.Email ?? "Нет email"
                });
            }

            ViewBag.UserId = userId;
            return View(messagesWithSenders);
        }

    }
}
