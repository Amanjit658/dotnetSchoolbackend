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
       [Authorize(Roles = "Student", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public StudentController(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
      [HttpGet("myattendance")]
        public async Task<IActionResult> GetMyAttendance()
        {
            var studentId = GetLoggedInStudentId(); // helper method

            var attendanceRecords = await _context.StudentAttendances
                .Where(sa => sa.StudentId == studentId)
                .Include(sa => sa.Subject)
                .Include(sa => sa.Class)
                .ToListAsync();

            var result = attendanceRecords.Select(ar => new
            {
                ar.Date,
                ClassName = ar.Class.Name,
                SubjectName = ar.Subject.Name,
                ar.IsPresent
            });

            return Ok(result);
        }

        [HttpGet("result/{examId}")]
        public IActionResult ViewResult(int examId)
        {
            var studentId = GetLoggedInStudentId();

            var result = _context.StudentExamResults
                .Where(x => x.StudentId == studentId &&
                            x.ExamSubject.ExamId == examId)
                .Include(x => x.ExamSubject)
                .ThenInclude(es => es.Subject)
                .Select(x => new
                {
                    x.ExamSubject.Subject.Name,
                    x.MarksObtained,
                    x.ExamSubject.MaxMarks
                })
                .ToList();

            return Ok(result);
        }











    private int GetLoggedInStudentId()
            {
                var userId = _userManager.GetUserId(User);
                var student = _context.Students.FirstOrDefault(s => s.UserId == userId);
                if (student == null)
                    throw new Exception("Student not found");
                return student.Id;
            }
     
    }
}