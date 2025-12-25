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
    }
}

