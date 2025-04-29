using CloudinaryDotNet.Actions;
using MediatR;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Models.Users;
using MercerStore.Web.Application.Requests.Log;
using MercerStore.Web.Application.ViewModels.Users;
using MercerStore.Web.Infrastructure.Helpers;

namespace MercerStore.Web.Application.Handlers.Users.Commands;

public record UpdateUserProfileCommand(UserProfileViewModel UserProfileViewModel) :
    LoggableRequest<Result<Unit>>("User update profile", "user");

public class UpdateUserProfileHandler : IRequestHandler<UpdateUserProfileCommand, Result<Unit>>
{
    private readonly IPhotoService _photoService;
    private readonly IUserService _userService;

    public UpdateUserProfileHandler(IUserService userService, IPhotoService photoService)
    {
        _userService = userService;
        _photoService = photoService;
    }

    public async Task<Result<Unit>> Handle(UpdateUserProfileCommand request, CancellationToken ct)
    {
        var result = await _userService.GetUserById(request.UserProfileViewModel.Id, ct);
        ImageUploadResult photoResult = null;

        if (!result.IsSuccess) return Result<Unit>.Failure(result.ErrorMessage);

        var user = result.Data;

        if (!string.IsNullOrEmpty(request.UserProfileViewModel.UserImage?.FileName))
        {
            if (!string.IsNullOrEmpty(user.UserImgUrl))
                try
                {
                    await _photoService.DeletePhotoAsync(user.UserImgUrl);
                }
                catch (Exception)
                {
                    return Result<Unit>.Failure("Не удалось удалить старое изображение");
                }

            photoResult = await _photoService.AddPhotoAsync(request.UserProfileViewModel.UserImage, ct);
        }

        MapUserProfileEdit(user, request.UserProfileViewModel, photoResult);
        await _userService.UpdateUserProfile(user, ct);

        var logDetails = new
        {
            request.UserProfileViewModel.UserName,
            UserImageUrl = photoResult?.Url.ToString(),
            request.UserProfileViewModel.Address,
            request.UserProfileViewModel.PhoneNumber,
            request.UserProfileViewModel.EmailAddress
        };

        request.EntityId = request.UserProfileViewModel.Id;
        request.Details = logDetails;

        return Result<Unit>.Success(Unit.Value);
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
}
