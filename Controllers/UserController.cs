using CloudinaryDotNet.Actions;
using MercerStore.Data;
using MercerStore.Infrastructure.Extentions;
using MercerStore.Interfaces;
using MercerStore.Models;
using MercerStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Controllers
{
	[Authorize(Policy = "BlacklistRolesPolicy")]
	public class UserController : Controller
    {
        private readonly HttpContextAccessor _httpContextAccessor;
        private readonly IUserProfileRepository _profileRepository;
        private readonly IPhotoService _photoService;
        public UserController(IUserProfileRepository repository, HttpContextAccessor httpContextAccessor, IPhotoService photoService)
        {
            _profileRepository = repository;
           
            _httpContextAccessor = httpContextAccessor;
            _photoService = photoService;
        }
        private void MapUserProfileEdit(AppUser user, UserProfileViewModel userProfileViewModel, ImageUploadResult photoResult)
        {
            user.Id = userProfileViewModel.Id;
            user.UserName = userProfileViewModel.UserName;
            user.Email = userProfileViewModel.EmailAddress;
            user.PhoneNumber = userProfileViewModel.PhoneNumber;
            user.UserImgUrl = photoResult.Url.ToString();
            user.Adress = userProfileViewModel.Address;
        }
        public async Task<IActionResult> UserProfile()
        {
            var curUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            var user = await _profileRepository.GetUserByIdAsync(curUserId);
            var userProfileViewModel = new UserProfileViewModel()
            {
                Id = curUserId,
                UserName = user.UserName,
                EmailAddress = user.Email,
                PhoneNumber = user.PhoneNumber,
                UserImgUrl = user.UserImgUrl,
                Address = user.Adress
            };
            return View(userProfileViewModel);
        }

        public async Task<IActionResult> EditUserProfile()
        {
            var curUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            var user = await _profileRepository.GetUserByIdAsync(curUserId);
            var userProfileViewModel = new UserProfileViewModel()
            {
                Id = curUserId,
                UserName = user.UserName,
                EmailAddress = user.Email,
                PhoneNumber = user.PhoneNumber,
                UserImgUrl = user.UserImgUrl,
                Address = user.Adress
            };
            return View(userProfileViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> EditUserProfile(UserProfileViewModel userProfileViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Ошибка редоктирвания профиля");
                return View("EditUserProfile", userProfileViewModel);

            }
            var user = await _profileRepository.GetUserByIdAsyncNoTracking(userProfileViewModel.Id);

            if (user.UserImgUrl == "" || user.UserImgUrl == null)
            {
                var photoResult = await _photoService.AddPhotoAsync(userProfileViewModel.UserImage);
                MapUserProfileEdit(user, userProfileViewModel, photoResult);
                _profileRepository.Update(user);
                return RedirectToAction("UserProfile");
            }
            else
            {
                try
                {
                    await _photoService.DeletePhotoAsync(user.UserImgUrl);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Не удалось удалить фотографию");
                    return View(userProfileViewModel);
                }
                var photoResult = await _photoService.AddPhotoAsync(userProfileViewModel.UserImage);
                MapUserProfileEdit(user, userProfileViewModel, photoResult);
                _profileRepository.Update(user);
                return RedirectToAction("UserProfile");
            }
        }
    }
}
