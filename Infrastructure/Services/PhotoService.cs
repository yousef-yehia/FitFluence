using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Core.Interfaces;
using Infrastructure.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;

        public PhotoService(IOptions<CloudinarySettings> config)
        {
            var acc = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret);
            _cloudinary = new Cloudinary(acc);

        }

        public async Task<ImageUploadResult> AddProfilePhotoAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();
            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParms = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face"),
                    Folder = "ProfilePic" // Set the folder name here

                };
                uploadResult = await _cloudinary.UploadAsync(uploadParms);
            }
            return uploadResult;
        }
        public async Task<ImageUploadResult> AddExercisePhotoAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();
            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParms = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Crop("fill"),
                    Folder = "ExercisePhotos" // Set the folder name here

                };
                uploadResult = await _cloudinary.UploadAsync(uploadParms);
            }
            return uploadResult;
        }    
        public async Task<ImageUploadResult> AddMusclePhotoAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();
            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParms = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Crop("fill"),
                    Folder = "MusclePhotos" // Set the folder name here

                };
                uploadResult = await _cloudinary.UploadAsync(uploadParms);
            }
            return uploadResult;
        }  
        public async Task<ImageUploadResult> AddFoodPhotoAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();
            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParms = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Crop("fill"),
                    Folder = "FoodPhotos" // Set the folder name here

                };
                uploadResult = await _cloudinary.UploadAsync(uploadParms);
            }
            return uploadResult;
        }

        public async Task<RawUploadResult> UploadPdfAsync(IFormFile file)
        {
            var uploadResult = new RawUploadResult();
            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new RawUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "CoachsCv" // Set the folder name here
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }
            return uploadResult;
        }


        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deleteParams);

            return result;
        }

        public async Task<string> GetPhotoAsync(string photoName)
        {
            var result = await _cloudinary.GetResourceAsync(photoName);
            var url = result.Url.ToString();

            return url;
        }
    }
}
