using System.ComponentModel.DataAnnotations;

namespace myFirstSchoolProject.DTOs.Admin
{
    public class AssignSubjectToClassDto
    {
        [Required]
        public int ClassId { get; set; }

        [Required]
        public int SubjectId { get; set; }
    }
}
