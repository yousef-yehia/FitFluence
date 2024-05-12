using Core.Models;

namespace Core.Interfaces
{
    public interface IAuthRepository
    {
        public Task<string> SendResetPasswordEmailAsync(AppUser appUser);

        public Task<bool> SendVerificationEmailAsync(AppUser appUser);
        public Task<string> FacebookLogin(string accessToken);


    }
}
