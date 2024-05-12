using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace Core.Interfaces
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
        Task<string> GetPhotoAsync(string photoName);
        Task<DeletionResult> DeletePhotoAsync(string publicId);
    }
}
