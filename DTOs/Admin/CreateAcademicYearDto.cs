using System.ComponentModel.DataAnnotations;

namespace myFirstSchoolProject.DTOs.Admin
{

    public class CreateAcademicYearDto
    {
        [Required]
        public string Year { get; set; } = string.Empty;
    }
}