using System.ComponentModel.DataAnnotations;
namespace myFirstSchoolProject.DTOs.Admin
{
    public class EnterMarksDto
{
    public int ExamSubjectId { get; set; }
    public List<StudentMarksDto> Students { get; set; }
}

public class StudentMarksDto
{
    public int StudentId { get; set; }
    public int MarksObtained { get; set; }
}

}