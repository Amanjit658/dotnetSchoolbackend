namespace myFirstSchoolProject.Models

{
    public class StudentExamResult
{
    public int Id { get; set; }

    public int ExamSubjectId { get; set; }
    public ExamSubject ExamSubject { get; set; }

    public int StudentId { get; set; }
    public Student Student { get; set; }

    public int MarksObtained { get; set; }
}

}