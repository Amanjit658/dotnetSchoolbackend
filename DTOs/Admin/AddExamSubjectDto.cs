using System.ComponentModel.DataAnnotations;

namespace myFirstSchoolProject.DTOs.Admin
{
    public class AddExamSubjectDto
{
    public int ExamId { get; set; }
    public int SubjectId { get; set; }
    public int MaxMarks { get; set; }
}
}