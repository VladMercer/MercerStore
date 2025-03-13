using MercerStore.Web.Application.Interfaces.Repositories;
using MercerStore.Web.Application.Interfaces;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Dtos.ResultDto;
using MercerStore.Web.Application.Dtos.UserDto;
using MercerStore.Web.Application.Requests.Users;
using CloudinaryDotNet.Actions;
using MercerStore.Web.Application.Models.Users;
using MercerStore.Web.Application.ViewModels.Users;
using MercerStore.Web.Infrastructure.Helpers;
using MercerStore.Web.Areas.Admin.ViewModels.Users;

namespace MercerStore.Web.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRedisCacheService _redisCacheService;
        private readonly IPhotoService _photoService;
        private readonly IUserRepository _profileRepository;
        private readonly IRequestContextService _requestContextService;
        public UserService(
            IUserRepository userRepository,
            IRedisCacheService redisCacheService,
            IPhotoService photoService,
            IUserRepository profileRepository,
            IRequestContextService requestContextService)
        {
            _userRepository = userRepository;
            _redisCacheService = redisCacheService;
            _photoService = photoService;
            _profileRepository = profileRepository;
            _requestContextService = requestContextService;
        }
        public async Task<PaginatedResultDto<UserDto>> GetFilteredUsers(UserFilterRequest request)
        {
            bool isDefaultQuery =
            request.PageNumber == 1 &&
            request.PageSize == 30 &&
            !request.SortOrder.HasValue &&
            !request.Period.HasValue &&
            !request.Filter.HasValue &&
            string.IsNullOrEmpty(request.Query);

            string cacheKey = $"users:page1";

            return await _redisCacheService.TryGetOrSetCacheAsync(
                cacheKey,
                () => FetchFilteredUsers(request),
                isDefaultQuery,
                TimeSpan.FromMinutes(10)
            );
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

        public async Task<Result<string>> UpdateUserProfile(UserProfileViewModel userProfileViewModel)
        {
            var user = await _profileRepository.GetUserByIdAsyncNoTracking(userProfileViewModel.Id);
            if (user == null) return Result<string>.Failure("Пользователь не найден");

            ImageUploadResult photoResult = null;
            if (!string.IsNullOrEmpty(userProfileViewModel.UserImage?.FileName))
            {
                if (!string.IsNullOrEmpty(user.UserImgUrl))
                {
                    try
                    {
                        await _photoService.DeletePhotoAsync(user.UserImgUrl);
                    }
                    catch (Exception)
                    {
                        return Result<string>.Failure("Не удалось удалить старое изображение");
                    }
                }

                photoResult = await _photoService.AddPhotoAsync(userProfileViewModel.UserImage);
            }

            MapUserProfileEdit(user, userProfileViewModel, photoResult);
            _profileRepository.UpdateUserProfile(user);

            _requestContextService.SetLogDetails(new
            {
                userProfileViewModel.UserName,
                UserImageUrl = photoResult?.Url.ToString(),
                userProfileViewModel.Address,
                userProfileViewModel.PhoneNumber,
                userProfileViewModel.EmailAddress
            });

            return Result<string>.Success(userProfileViewModel.Id);
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
        public async Task<Result<string>> UpdateUserProfile(UpdateUserProfileViewModel updateUserProfileViewModel)
        {
            var user = await _userRepository.GetUserByIdAsync(updateUserProfileViewModel.Id);
            if (user == null)
            {
                return Result<string>.Failure("Пользователь не найден");
            }

            user.UserImgUrl = updateUserProfileViewModel.UserImgUrl ?? "https://localhost:7208/img/default/default_user_image.jpg";
            user.PhoneNumber = updateUserProfileViewModel.PhoneNumber;
            user.Address = updateUserProfileViewModel.Address;
            user.Email = updateUserProfileViewModel.EmailAddress;
            user.UserName = updateUserProfileViewModel.UserName;

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

            return Result<string>.Success(user.Id);
        }
        private void MapUserProfileEdit(AppUser user, UserProfileViewModel viewModel, ImageUploadResult photoResult)
        {
            user.Id = viewModel.Id;
            user.UserName = viewModel.UserName;
            user.Email = viewModel.EmailAddress;
            user.PhoneNumber = viewModel.PhoneNumber;
            user.UserImgUrl = photoResult?.Url.ToString();
            user.Address = viewModel.Address;
        }
        private async Task<PaginatedResultDto<UserDto>> FetchFilteredUsers(UserFilterRequest request)
        {
            var (userDtos, totalItems) = await _userRepository.GetFilteredUsers(request);
            var result = new PaginatedResultDto<UserDto>(userDtos, totalItems, request.PageSize);
            return result;
        }
    }
}
