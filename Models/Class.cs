using System.ComponentModel.DataAnnotations;

namespace myFirstSchoolProject.Models
{
    public class Class
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
    }
}
