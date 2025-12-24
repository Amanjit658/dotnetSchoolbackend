namespace myFirstSchoolProject.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int Class { get; set; }
        public ApplicationUser User { get; set; }

    }
}
