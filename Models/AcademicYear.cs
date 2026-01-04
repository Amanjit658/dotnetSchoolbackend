using System.ComponentModel.DataAnnotations;

namespace myFirstSchoolProject.Models
{
    public class AcademicYear
    {
        public int Id { get; set; }

        [Required]
        public string Year { get; set; } = string.Empty;
        // Example: 2024-2025

        public bool IsActive { get; set; } = true;
    }
}
