using Microsoft.AspNetCore.Identity;

namespace myFirstSchoolProject.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
    }
}
