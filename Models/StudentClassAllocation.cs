namespace myFirstSchoolProject.Models
{
    public class StudentClassAllocation
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; }

        public int ClassId { get; set; }
        public Class Class { get; set; }
        public int AcademicYearId { get; set; }
        public AcademicYear AcademicYear { get; set; }

    }
}