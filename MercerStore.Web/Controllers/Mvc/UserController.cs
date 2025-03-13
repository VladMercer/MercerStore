using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.ViewModels.Users;
using MercerStore.Web.Infrastructure.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Web.Controllers.Mvc
{
    [Authorize(Policy = "BlacklistRolesPolicy")]
    public class UserController : Controller
    {
        private readonly HttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;

        public UserController(IUserService userService, HttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> UserProfile()
        {
            var curUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            var userProfileViewModel = await _userService.GetUserProfile(curUserId);
            if (userProfileViewModel == null) return NotFound();

            return View(userProfileViewModel);
        }

        public async Task<IActionResult> EditUserProfile()
        {
            var curUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            var userProfileViewModel = await _userService.GetUserProfileForEdit(curUserId);
            if (userProfileViewModel == null) return NotFound();

            return View(userProfileViewModel);
        }

        [HttpPost]
        [LogUserAction("User update profile", "user")]
        public async Task<IActionResult> EditUserProfile(UserProfileViewModel userProfileViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Ошибка редактирования профиля");
                return View(userProfileViewModel);
            }

            var result = await _userService.UpdateUserProfile(userProfileViewModel);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.ErrorMessage);
                return View(userProfileViewModel);
            }

            return RedirectToAction("UserProfile");
        }
    }
}