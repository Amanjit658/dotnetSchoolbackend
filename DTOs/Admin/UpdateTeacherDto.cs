using System.ComponentModel.DataAnnotations;
namespace myFirstSchoolProject.DTOs.Admin
{
    public class UpdateTeacherDto
    {
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Subject { get; set; } = string.Empty;

        [Required]
        public string FullName { get; set; } = string.Empty;
    }
}