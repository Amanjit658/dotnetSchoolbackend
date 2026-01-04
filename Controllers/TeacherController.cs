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
       [Authorize(Roles = "Teacher", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public TeacherController(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        [HttpPost("mark")]
        public async Task<IActionResult> MarkAttendance(MarkAttendanceDto dto)
        {
            var teacherId = GetLoggedInTeacherId(); // helper method

            foreach (var item in dto.Students)
            {
                bool alreadyMarked = _context.StudentAttendances.Any(x =>
                    x.StudentId == item.StudentId &&
                    x.SubjectId == dto.SubjectId &&
                    x.Date.Date == dto.Date.Date);

                if (alreadyMarked)
                    continue;

                _context.StudentAttendances.Add(new StudentAttendance
                {
                    StudentId = item.StudentId,
                    ClassId = dto.ClassId,
                    SubjectId = dto.SubjectId,
                    TeacherId = teacherId,
                    Date = dto.Date,
                    IsPresent = item.IsPresent
                });
            }

            await _context.SaveChangesAsync();
            return Ok("Attendance marked successfully");
        }
        private int GetLoggedInTeacherId()
        {
            var userId = _userManager.GetUserId(User);
            var teacher = _context.Teachers
                .FirstOrDefault(t => t.UserId == userId);

            if (teacher == null)
                throw new Exception("Teacher not found");

            return teacher.Id;
        }

        [HttpPost("marks")]
        public async Task<IActionResult> EnterMarks(EnterMarksDto dto)
        {
            foreach (var s in dto.Students)
            {
                _context.StudentExamResults.Add(new StudentExamResult
                {
                    ExamSubjectId = dto.ExamSubjectId,
                    StudentId = s.StudentId,
                    MarksObtained = s.MarksObtained
                });
            }

            await _context.SaveChangesAsync();
            return Ok("Marks saved");
        }

    }
}