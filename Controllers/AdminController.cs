using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using myFirstSchoolProject.Data;
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
            [FromBody] CreateTeacherDto model)   // 🔥 FIX
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
            [FromBody] CreateStudentDto model)   // 🔥 FIX
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

        // Temporary diagnostic endpoint to verify routing without auth
        [HttpGet("ping")]
        [AllowAnonymous]
        public IActionResult Ping()
        {
            return Ok("pong");
        }
    }


}

