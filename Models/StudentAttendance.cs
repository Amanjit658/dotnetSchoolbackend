namespace myFirstSchoolProject.Models

{
    public class StudentAttendance
{
    public int Id { get; set; }

    public int StudentId { get; set; }
    public Student Student { get; set; }

    public int ClassId { get; set; }
    public Class Class { get; set; }

    public int SubjectId { get; set; }
    public Subject Subject { get; set; }

    public int TeacherId { get; set; }
    public Teacher Teacher { get; set; }

    public DateTime Date { get; set; }

    public bool IsPresent { get; set; }
}
}