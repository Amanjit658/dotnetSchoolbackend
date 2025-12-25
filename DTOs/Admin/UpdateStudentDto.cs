using System.ComponentModel.DataAnnotations;
namespace myFirstSchoolProject.DTOs.Admin
{
    public class UpdateStudentDto
    {
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public int? Class { get; set; }

        [Required]
        public string FullName { get; set; } = string.Empty;
    }
}