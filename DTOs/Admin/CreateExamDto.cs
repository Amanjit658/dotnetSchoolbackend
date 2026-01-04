using System.ComponentModel.DataAnnotations;

namespace myFirstSchoolProject.DTOs.Admin
{
    public class CreateExamDto
{
    public string Name { get; set; }
    public int AcademicYearId { get; set; }
}
}