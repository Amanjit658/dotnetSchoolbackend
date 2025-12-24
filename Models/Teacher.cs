namespace myFirstSchoolProject.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Subject { get; set; }
        public ApplicationUser User { get; set; }

    }
}
