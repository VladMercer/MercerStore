using MercerStore.Areas.Admin.ViewModels;
using MercerStore.Infrastructure.Extentions;
using MercerStore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Manager")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IRequestContextService _requestContextService;

        public UserController(IUserRepository userRepository, IRequestContextService requestContextService)
        {
            _userRepository = userRepository;
            _requestContextService = requestContextService;
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
            var user = await _userRepository.GetUserByIdAsync(userId);

            var viewModel = new UpdateUserProfileViewModel
            {
                Id = user.Id,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
                CountOrders = user.Orders.Count(),
                CountReviews = user.Reviews.Count(),
                CreateDate = user.DateCreated,
                EmailAddress = user.Email,
                LastActivityDate = user.LastActivity,
                Orders = user.Orders?.ToList(),
                Reviews = user.Reviews?.ToList(),
                Roles = user.UserRoles.Select(u => u.Role.Name).ToList(),
                UserImgUrl = user.UserImgUrl,
                UserName = user.UserName

            };

            return View(viewModel);
        }
        [HttpPost("[area]/[controller]/update/{userId}")]
        [LogUserAction("Manager update user profile", "user")]
        public async Task<IActionResult> UpdateUserProfileAsync(UpdateUserProfileViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var user = await _userRepository.GetUserByIdAsync(viewModel.Id);
            if (user == null)
            {
                return NotFound();
            }

            user.UserImgUrl = viewModel.UserImgUrl ?? "https://localhost:7208/img/default/default_user_image.jpg";
            user.PhoneNumber = viewModel.PhoneNumber;
            user.Address = viewModel.Address;
            user.Email = viewModel.EmailAddress;
            user.UserName = viewModel.UserName;

            var logDetails = new
            {
                user.UserImgUrl,
                user.PhoneNumber,
                user.Address,
                user.Email,
                user.UserName
            };

            _requestContextService.SetLogDetails(logDetails);

            await _userRepository.UpdateUserProfile(user);

            return RedirectToAction("UpdateUserProfile", new { id = user.Id });
        }
    }
}
