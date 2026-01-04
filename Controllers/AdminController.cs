using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using myFirstSchoolProject.Data;
using Microsoft.EntityFrameworkCore;
using myFirstSchoolProject.DTOs.Admin;
using myFirstSchoolProject.Models;

namespace myFirstSchoolProject.Controllers
{
    [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public AdminController(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpPost("create-teacher")]
        public async Task<IActionResult> CreateTeacher(
            [FromBody] CreateTeacherDto model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await _userManager.AddToRoleAsync(user, "Teacher");

            var teacher = new Teacher
            {
                UserId = user.Id,
                Subject = model.Subject
            };

            _context.Teachers.Add(teacher);
            await _context.SaveChangesAsync();

            return Ok("Teacher created successfully");
        }

        [HttpPost("create-student")]
        public async Task<IActionResult> CreateStudent(
            [FromBody] CreateStudentDto model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await _userManager.AddToRoleAsync(user, "Student");

            var student = new Student
            {
                UserId = user.Id,
                Class = model.Class
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return Ok("Student created successfully");
        }

        [HttpGet("TeachersList")]
        public async Task<IActionResult> GetTeachersList()
        {
            var teachers = await _context.Teachers
                .Include(t => t.User)
                .Select(t => new
                {
                    t.Id,
                    t.UserId,
                    t.Subject,
                    User = new
                    {
                        t.User.Id,
                        t.User.UserName,
                        t.User.Email,
                        t.User.FullName
                    }
                })
                .ToListAsync();

            return Ok(teachers);
        }

        [HttpGet("studentsList")]
        public async Task<IActionResult> GetStudentsList()
        {
            var students = await _context.Students
                .Include(s => s.User)
                .Select(s => new
                {
                    s.Id,
                    s.UserId,
                    s.Class,
                    User = new
                    {
                        s.User.Id,
                        s.User.UserName,
                        s.User.Email,
                        s.User.FullName
                    }
                })
                .ToListAsync();

            return Ok(students);
        }

        [HttpGet("teacher/{id}")]
        public async Task<IActionResult> GetTeacherProfile([FromRoute] int id)
        {
            var teacher = await _context.Teachers
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (teacher == null)
                return NotFound("Teacher not found");

            var result = new
            {
                teacher.Id,
                teacher.UserId,
                Subject = teacher.Subject,
                User = new
                {
                    teacher.User?.Id,
                    teacher.User?.UserName,
                    teacher.User?.Email,
                    teacher.User?.FullName
                }
            };

            return Ok(result);
        }

        [HttpGet("student/{id}")]
        public async Task<IActionResult> GetStudentProfile([FromRoute] int id)
        {
            var student = await _context.Students
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null)
                return NotFound("Student not found");

            var result = new
            {
                student.Id,
                student.UserId,
                student.Class,
                User = new
                {
                    student.User?.Id,
                    student.User?.UserName,
                    student.User?.Email,
                    student.User?.FullName
                }
            };

            return Ok(result);
        }

        [HttpDelete("delete-teacher/{id}")]
        public async Task<IActionResult> DeleteTeacher([FromRoute] int id)
        {
            var teacher = await _context.Teachers
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (teacher == null)
                return NotFound("Teacher not found");

            var user = teacher.User;

            _context.Teachers.Remove(teacher);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
            await _context.SaveChangesAsync();

            return Ok("Teacher deleted successfully");
        }

        [HttpDelete("delete-student/{id}")]
        public async Task<IActionResult> DeleteStudent([FromRoute] int id)
        {
            var student = await _context.Students
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.Id == id);
            Console.WriteLine("here", student);
            if (student == null)
                return NotFound("Student not found");

            var user = student.User;

            _context.Students.Remove(student);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
            await _context.SaveChangesAsync();

            return Ok("Student deleted successfully");
        }

        [HttpPut("update-teacher/{id}")]
        public async Task<IActionResult> UpdateTeacher(
            [FromRoute] int id,
            [FromBody] UpdateTeacherDto model)
        {
            var teacher = await _context.Teachers
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (teacher == null)
                return NotFound("Teacher not found");

            teacher.Subject = model.Subject ?? teacher.Subject;
            if (teacher.User != null)
            {
                teacher.User.FullName = model.FullName ?? teacher.User.FullName;
                teacher.User.Email = model.Email ?? teacher.User.Email;
                teacher.User.UserName = model.Email ?? teacher.User.UserName;
            }

            await _context.SaveChangesAsync();

            return Ok("Teacher updated successfully");
        }

        [HttpPut("update-student/{id}")]

        public async Task<IActionResult> UpdateStudent(
            [FromRoute] int id,

            [FromBody] UpdateStudentDto model)
        {
            var student = await _context.Students
                .Include(s => s.User).FirstOrDefaultAsync(s => s.Id == id);

            if (student == null)
                return NotFound("Student not found");

            student.Class = model.Class ?? student.Class;
            if (student.User != null)
            {
                student.User.FullName = model.FullName ?? student.User.FullName;
                student.User.Email = model.Email ?? student.User.Email;
                student.User.UserName = model.Email ?? student.User.UserName;
            }

            await _context.SaveChangesAsync();

            return Ok("Student updated successfully");
        }

        [HttpPost("add-class")]
        public async Task<IActionResult> AddClass([FromBody] AddClasssDto model)
        {
            var newClass = new Class
            {
                Name = model.Name
            };

            _context.Classes.Add(newClass);
            await _context.SaveChangesAsync();

            return Ok("Class added successfully");
        }

        [HttpPost("add-subject")]
        public async Task<IActionResult> AddSubject([FromBody] AddSubjectDto model)
        {
            var newSubject = new Subject
            {
                Name = model.Name
            };

            _context.Subjects.Add(newSubject);
            await _context.SaveChangesAsync();

            return Ok("Subject added successfully");
        }

        [HttpPost("create-academic-year")]
        public async Task<IActionResult> CreateAcademicYear([FromBody] CreateAcademicYearDto model)
        {
            var newAcademicYear = new AcademicYear
            {
                Year = model.Year
            };

            _context.AcademicYears.Add(newAcademicYear);
            await _context.SaveChangesAsync();

            return Ok("Academic year created successfully");
        }

        [HttpGet("academic-years")]

        public async Task<IActionResult> GetAcademicYears()
        {
            var years = await _context.AcademicYears
                .Select(ay => new
                {
                    ay.Id,
                    ay.Year
                })
                .ToListAsync();

            return Ok(years);
        }

        // Assign subject to class
        [HttpPost("assign")]

        public async Task<IActionResult> AssignSubjectToClass(AssignSubjectToClassDto dto)
        {
            var exists = await _context.ClassSubjects
                          .AnyAsync(cs =>
                              cs.ClassId == dto.ClassId &&
                              cs.SubjectId == dto.SubjectId);
            if (exists)
                return BadRequest("Subject already assigned to this class");
            var mapping = new ClassSubject
            {
                ClassId = dto.ClassId,
                SubjectId = dto.SubjectId
            };
            _context.ClassSubjects.Add(mapping);
            await _context.SaveChangesAsync();
            return Ok("Subject assigned to class");

        }

        // Get subjects of a class
        [HttpGet("class-subjects/{classId}")]
        public async Task<IActionResult> GetSubjectsOfClass([FromRoute] int classId)
        {
            var subjects = await _context.ClassSubjects
                .Where(cs => cs.ClassId == classId)
                .Include(cs => cs.Subject)
                .Select(cs => new
                {
                    cs.Subject.Id,
                    cs.Subject.Name
                })
                .ToListAsync();

            // Print subjects as JSON so console shows readable values when the API is hit
            try
            {
                var subjectsJson = System.Text.Json.JsonSerializer.Serialize(subjects);
                Console.WriteLine($"[GetSubjectsOfClass] classId={classId} subjects={subjectsJson}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[GetSubjectsOfClass] classId={classId} - Error serializing subjects: {ex.Message}");
            }

            return Ok(subjects);
        }

        [HttpPost("assign-teacher")]
        public async Task<IActionResult> AssignTeacherToClassSubject([FromBody] AssignTeacherDto dto)
        {
            var exists = await _context.TeacherClassSubjects
                          .AnyAsync(tcs =>
                              tcs.TeacherId == dto.TeacherId &&
                              tcs.ClassId == dto.ClassId &&
                              tcs.SubjectId == dto.SubjectId &&
                              tcs.AcademicYearId == dto.AcademicYearId);
            if (exists)
                return BadRequest("Teacher already assigned to this class and subject");
            var mapping = new TeacherClassSubject
            {
                TeacherId = dto.TeacherId,
                ClassId = dto.ClassId,
                SubjectId = dto.SubjectId,
                AcademicYearId = dto.AcademicYearId

            };
            _context.TeacherClassSubjects.Add(mapping);
            await _context.SaveChangesAsync();
            return Ok("Teacher assigned to class and subject");

        }

        [HttpGet("class/{classId}/teachers")]
        public IActionResult GetTeachersByClass(int classId, int academicYearId)
        {
            var data = _context.TeacherClassSubjects
                .Where(x => x.ClassId == classId && x.AcademicYearId == academicYearId)
                .Include(x => x.Teacher)
                .Include(x => x.Subject)
                .Select(x => new
                {
                    TeacherId = x.Teacher.Id,
                    TeacherName = x.Teacher.User.FullName,
                    Subject = x.Subject.Name
                })
                .ToList();

            return Ok(data);
        }

        [HttpGet("teacher/{teacherId}/classes")]
        public IActionResult GetClassesByTeacher(int teacherId)
        {
            var data = _context.TeacherClassSubjects
                .Where(x => x.TeacherId == teacherId)
                .Include(x => x.Class)
                .Include(x => x.Subject)
                .Select(x => new
                {
                    ClassName = x.Class.Name,
                    Subject = x.Subject.Name
                })
                .ToList();

            return Ok(data);
        }

        [HttpPost("timetable")]

        public async Task<IActionResult> CreateClassTimetable([FromBody] CreateTimetableDto dto)
        {
            // Rule 1: class conflict
            bool classConflict = _context.ClassTimetables.Any(x =>
                x.ClassId == dto.ClassId &&
                x.Day == dto.Day &&
                x.PeriodNumber == dto.PeriodNumber);

            if (classConflict)
                return BadRequest("Class already has a subject at this time");

            // Rule 2: teacher conflict
            bool teacherConflict = _context.ClassTimetables.Any(x =>
                x.TeacherId == dto.TeacherId &&
                x.Day == dto.Day &&
                x.PeriodNumber == dto.PeriodNumber);
            if (teacherConflict)
                return BadRequest("Teacher already has a subject at this time");

            var timetableEntry = new ClassTimetable
            {
                ClassId = dto.ClassId,
                SubjectId = dto.SubjectId,
                TeacherId = dto.TeacherId,
                Day = dto.Day,
                PeriodNumber = dto.PeriodNumber
            };

            _context.ClassTimetables.Add(timetableEntry);
            await _context.SaveChangesAsync();

            return Ok("Class timetable entry created successfully");
        }

        [HttpGet("timetable/class/{classId}")]

        public async Task<IActionResult> GetClassTimetable([FromRoute] int classId)
        {
            var timetable = await _context.ClassTimetables
                .Where(ct => ct.ClassId == classId)
                .Include(ct => ct.Subject)
                .Include(ct => ct.Teacher)
                .ThenInclude(t => t.User)
                .Select(ct => new
                {
                    ct.Id,
                    Subject = ct.Subject.Name,
                    Teacher = ct.Teacher.User.FullName,
                    ct.Day,
                    ct.PeriodNumber
                })
                .ToListAsync();

            return Ok(timetable);
        }

        [HttpPost("StudentClassAllocations")]

        public async Task<IActionResult> AllocateStudentToClass([FromBody] AllocateStudentDto dto)
        {
            bool alreadyAllocated = _context.StudentClassAllocations.Any(x =>
              x.StudentId == dto.StudentId && x.AcademicYearId == dto.AcademicYearId);

            if (alreadyAllocated)
                return BadRequest("Student already allocated in this academic year");

            var allocation = new StudentClassAllocation
            {
                StudentId = dto.StudentId,
                ClassId = dto.ClassId,
                AcademicYearId = dto.AcademicYearId
            };

            _context.StudentClassAllocations.Add(allocation);
            await _context.SaveChangesAsync();

            return Ok("Student allocated to class successfully");
        }

        [HttpGet("class/{classId}/students")]
        public IActionResult GetStudentsByClass(int classId, int academicYearId)
        {
            var students = _context.StudentClassAllocations
                .Where(x => x.ClassId == classId && x.AcademicYearId == academicYearId)
                .Include(x => x.Student)
                .ThenInclude(s => s.User)
                .Select(x => new
                {
                    x.Student.Id,
                    x.Student.User.FullName,
                    x.Student.User.Email
                })
                .ToList();

            return Ok(students);
        }

        [HttpGet("class/{classId}")]
        public IActionResult GetClassAttendance(int classId, DateTime date)
        {
            var attendanceRecords = _context.StudentAttendances
                .Where(sa => sa.ClassId == classId && sa.Date.Date == date.Date)
                .Include(sa => sa.Student)
                .ThenInclude(s => s.User)
                .Select(sa => new
                {
                    sa.Student.Id,
                    sa.Student.User.FullName,
                    sa.IsPresent
                })
                .ToList();

            return Ok(attendanceRecords);
        }
 
        [HttpPost("exam")]
        public async Task<IActionResult> CreateExam(CreateExamDto dto)
        {
            var exam = new Exam
            {
                Name = dto.Name,
                AcademicYearId = dto.AcademicYearId
            };

            _context.Exams.Add(exam);
            await _context.SaveChangesAsync();

            return Ok("Exam created");
        }

        [HttpPost("exam/subject")]
        public async Task<IActionResult> AddSubjectToExam(AddExamSubjectDto dto)
        {
            var examSubject = new ExamSubject
            {
                ExamId = dto.ExamId,
                SubjectId = dto.SubjectId,
                MaxMarks = dto.MaxMarks
            };

            _context.ExamSubjects.Add(examSubject);
            await _context.SaveChangesAsync();

            return Ok("Subject added to exam");
        }


    }
}
