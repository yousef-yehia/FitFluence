﻿using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace Core.Interfaces
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddProfilePhotoAsync(IFormFile file);
        Task<string> GetPhotoAsync(string photoName);
        Task<DeletionResult> DeletePhotoAsync(string publicId);
        public Task<ImageUploadResult> AddExercisePhotoAsync(IFormFile file);
        public Task<ImageUploadResult> AddMusclePhotoAsync(IFormFile file);
        public Task<ImageUploadResult> AddFoodPhotoAsync(IFormFile file);
        public Task<RawUploadResult> UploadPdfAsync(IFormFile file);



    }
}
