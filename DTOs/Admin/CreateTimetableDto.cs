using System.ComponentModel.DataAnnotations;
namespace myFirstSchoolProject.DTOs.Admin
{
    public class CreateTimetableDto
    {
        [Required]
        public int ClassId { get; set; }
        [Required]
        public int SubjectId { get; set; }
        [Required]
        public int TeacherId { get; set; }
        [Required]
        public DayOfWeek Day { get; set; }
        [Required]
        public int PeriodNumber { get; set; }
    }
}