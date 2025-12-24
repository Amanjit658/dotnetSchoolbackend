using System.ComponentModel.DataAnnotations;

namespace myFirstSchoolProject.DTOs.Admin
{
    public class CreateTeacherDto
    {
        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string Subject { get; set; } = string.Empty;

        [Required]
        public string FullName { get; set; } = string.Empty;
    }
}
