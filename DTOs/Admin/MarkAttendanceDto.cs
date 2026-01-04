using System.ComponentModel.DataAnnotations;
namespace myFirstSchoolProject.DTOs.Admin
{
    public class MarkAttendanceDto
{
    public int ClassId { get; set; }
    public int SubjectId { get; set; }
    public DateTime Date { get; set; }

    public List<StudentAttendanceItem> Students { get; set; }
}

public class StudentAttendanceItem
{
    public int StudentId { get; set; }
    public bool IsPresent { get; set; }
}
}