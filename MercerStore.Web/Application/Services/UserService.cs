using MercerStore.Web.Application.Interfaces.Repositories;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Dtos.ResultDto;
using MercerStore.Web.Application.Dtos.UserDto;
using MercerStore.Web.Application.Requests.Users;
using MercerStore.Web.Application.Models.Users;
using MercerStore.Web.Application.ViewModels.Users;
using MercerStore.Web.Infrastructure.Helpers;
using MercerStore.Web.Areas.Admin.ViewModels.Users;

namespace MercerStore.Web.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserRepository _profileRepository;

        public UserService(IUserRepository userRepository, IUserRepository profileRepository)
        {
            _userRepository = userRepository;
            _profileRepository = profileRepository;
        }

        public async Task<PaginatedResultDto<UserDto>> GetFilteredSuppliersWithoutCache(UserFilterRequest request)
        {
            var (userDtos, totalItems) = await _userRepository.GetFilteredUsers(request);
            var result = new PaginatedResultDto<UserDto>(userDtos, totalItems, request.PageSize);
            return result;
        }
        public async Task<UserProfileViewModel> GetUserProfile(string userId)
        {
            var user = await _profileRepository.GetUserByIdAsync(userId);
            if (user == null) return null;

            return new UserProfileViewModel
            {
                Id = userId,
                UserName = user.UserName,
                EmailAddress = user.Email,
                PhoneNumber = user.PhoneNumber,
                UserImgUrl = user.UserImgUrl,
                Address = user.Address,
                Orders = user.Orders.ToList(),
                Reviews = user.Reviews.ToList(),
                CountOrders = user.Orders.Count(),
                CountReviews = user.Reviews.Count(),
                CreateDate = user.DateCreated,
            };
        }

        public async Task<UserProfileViewModel> GetUserProfileForEdit(string userId)
        {
            var user = await _profileRepository.GetUserByIdAsync(userId);
            if (user == null) return null;

            return new UserProfileViewModel
            {
                Id = userId,
                UserName = user.UserName,
                EmailAddress = user.Email,
                PhoneNumber = user.PhoneNumber,
                UserImgUrl = user.UserImgUrl,
                Address = user.Address
            };
        }

        public async Task<UpdateUserProfileViewModel> GetUpdateUserProfileViewModel(string userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);

            return new UpdateUserProfileViewModel
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
        }
        public async Task<Result<AppUser>> GetUserById(string userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return Result<AppUser>.Failure("Пользователь не найден");
            }
            return Result<AppUser>.Success(user);
        }
        public async Task UpdateUserProfile(AppUser user)
        {
            await _userRepository.UpdateUserProfile(user);
        }
    }
}
