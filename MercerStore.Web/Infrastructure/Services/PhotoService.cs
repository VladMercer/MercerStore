using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Infrastructure.Helpers;
using Microsoft.Extensions.Options;

namespace MercerStore.Web.Infrastructure.Services;

public class PhotoService : IPhotoService
{
    private readonly Cloudinary _cloundinary;

    public PhotoService(IOptions<CloudinarySettings> config)
    {
        var acc = new Account(
            config.Value.CloudName,
            config.Value.ApiKey,
            config.Value.ApiSecret
        );
        _cloundinary = new Cloudinary(acc);
    }

    public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file, CancellationToken ct)
    {
        var uploadResult = new ImageUploadResult();
        if (file.Length > 0)
        {
            await using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face"),
                UseFilename = true,
                Overwrite = true
            };
            uploadResult = await _cloundinary.UploadAsync(uploadParams, ct);
        }


        if (!string.IsNullOrEmpty(uploadResult.Url?.ToString()))
            uploadResult.Url = new Uri(uploadResult.Url.ToString().Replace("http://", "https://"));

        return uploadResult;
    }

    public async Task<DeletionResult> DeletePhotoAsync(string publicUrl)
    {
        var publicId = publicUrl.Split('/').Last().Split('.')[0];
        var deleteParams = new DeletionParams(publicId);
        return await _cloundinary.DestroyAsync(deleteParams);
    }
}
