using CloudinaryDotNet.Actions;

namespace MercerStore.Web.Application.Interfaces.Services;

public interface IPhotoService
{
    Task<ImageUploadResult> AddPhotoAsync(IFormFile file, CancellationToken ct);

    Task<DeletionResult> DeletePhotoAsync(string publicUrl);
}
