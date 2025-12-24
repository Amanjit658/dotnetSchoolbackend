using myFirstSchoolProject.Models;

namespace myFirstSchoolProject.Services
{
    public interface ITokenService
    {
        Task<string> CreateTokenAsync(ApplicationUser user);
    }
}
