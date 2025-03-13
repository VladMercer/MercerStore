using MercerStore.Web.Application.Interfaces;
using MercerStore.Web.Application.Interfaces.Repositories;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Areas.Admin.ViewModels.Users;
using MercerStore.Web.Infrastructure.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Manager")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult UserPage()
        {
            return View();
        }

        public IActionResult UsersList()
        {
            return View();
        }

        [HttpGet("[area]/[controller]/update/{userId}")]
        public async Task<IActionResult> UpdateUserProfile(string userId)
        {
            var updateUserProfileViewModel = await _userService.GetUpdateUserProfileViewModel(userId);
            return View(updateUserProfileViewModel);
        }

        [HttpPost("[area]/[controller]/update/{userId}")]
        [LogUserAction("Manager update user profile", "user")]
        public async Task<IActionResult> UpdateUserProfileAsync(UpdateUserProfileViewModel updateUserProfileViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(updateUserProfileViewModel);
            }
            var result = await _userService.UpdateUserProfile(updateUserProfileViewModel);
        
            return RedirectToAction("UpdateUserProfile", new { id = result.Data });
        }
    }
}
