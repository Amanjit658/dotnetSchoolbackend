namespace myFirstSchoolProject.Models

{
    public class ExamSubject
{
    public int Id { get; set; }

    public int ExamId { get; set; }
    public Exam Exam { get; set; }

    public int SubjectId { get; set; }
    public Subject Subject { get; set; }

    public int MaxMarks { get; set; }
}
}