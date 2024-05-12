﻿using CloudinaryDotNet;
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

        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
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
