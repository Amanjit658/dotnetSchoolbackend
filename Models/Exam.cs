namespace myFirstSchoolProject.Models

{
    public class Exam
{
    public int Id { get; set; }
    public string Name { get; set; }        // Midterm, Final
    public int AcademicYearId { get; set; }
    public AcademicYear AcademicYear { get; set; }
}
}