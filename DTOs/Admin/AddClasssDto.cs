using System.ComponentModel.DataAnnotations;
namespace myFirstSchoolProject.DTOs.Admin
{
    public class AddClasssDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}