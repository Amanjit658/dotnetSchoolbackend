using System.ComponentModel.DataAnnotations;
namespace myFirstSchoolProject.DTOs.Admin
{
    public class AddSubjectDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}