using System.ComponentModel.DataAnnotations;

namespace myFirstSchoolProject.DTOs.Admin
{
    public class CreateStudentDto
    {
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        public int Class { get; set; }
        [Required] public string FullName { get; set; } = string.Empty;

    }
}
