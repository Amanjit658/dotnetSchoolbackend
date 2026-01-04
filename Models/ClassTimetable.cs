namespace myFirstSchoolProject.Models
{
    public class ClassTimetable
    {
        public int Id { get; set; }

        public int ClassId { get; set; }
        public Class Class { get; set; }

        public int SubjectId { get; set; }
        public Subject Subject { get; set; }

        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        public DayOfWeek Day { get; set; }   // Monday – Friday

        public int PeriodNumber { get; set; }  // 1–8 
    }
}