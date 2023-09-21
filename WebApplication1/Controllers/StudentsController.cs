using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApplication1.login;
using WebApplication1.Models;
using WebApplication1.Models.Functions;
using WebApplication1.Models.IdentityModel;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly SmallprojectContext _context;
        private readonly IConfiguration _configuration;
        private readonly IRoleId _roleId;

        public StudentsController(SmallprojectContext context,IConfiguration configuration,IRoleId roleId)
        {
            _context = context;
            _configuration = configuration;
            _roleId = roleId;
        }

        // GET: api/Students
        [HttpGet]
        [Authorize(Roles = "Admin,student")]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
          if (_context.Students == null)
          {
              return NotFound();
          }
                return await _context.Students.Include(x => x.Deptment).ToListAsync();
        }

        // GET: api/Students/5
        [HttpGet("getStudent")]
        [Authorize(Roles ="Admin,student")]
        public async Task<ActionResult<Student>> GetStudent()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

        if (identity != null)
            {
                Ident iden = _roleId.GetIden(identity);
                var user = await _context.Students.Include(x=> x.Deptment).FirstOrDefaultAsync(x=> x.Id == iden.Id);
                return user;
            }
            return null;
        }

        // PUT: api/Students/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("editStudent")]
        [Authorize(Roles ="Admin,student")]
        public async Task<IActionResult> PutStudent(Student student)
        {
             Console.WriteLine("Put");
            var identity = HttpContext.User.Identity as ClaimsIdentity;
                if(identity != null)
                {
                    Ident iden = _roleId.GetIden(identity);
                    Student user = await _context.Students.FirstOrDefaultAsync(x=>x.Id == iden.Id);
                    user.Name = student.Name;
                    user.RollNo = student.RollNo;
                    user.FatherName = student.FatherName;
                    user.MotherName = student.MotherName;
                    user.Address = student.Address;
                    user.Dob = student.Dob;
                    user.AdmissionNo = student.AdmissionNo;
                    user.DeptmentId = student.DeptmentId;
                    _context.SaveChangesAsync();
                    return Ok("Updated");

                }
                return BadRequest("Error");
        }

        // POST: api/Students
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Register")]
        public async Task<ActionResult<Student>> StudentRegister(Student student)
        {
          if (_context.Students == null)
          {
              return Problem("Entity set 'SmallprojectContext.Students'  is null.");
          }
            _context.Students.Add(student);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (StudentExists(student.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetStudent", new { id = student.Id }, student);
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> DeleteStudent(long id)
        {
            if (_context.Students == null)
            {
                return NotFound();
            }
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StudentExists(long id)
        {
            return (_context.Students?.Any(e => e.Id == id)).GetValueOrDefault(); 
        }


        [HttpPost("login")]
        public async Task<IActionResult> StudentLogin(LoginUser request)
        {
            var user = await AuthenticatUser(request);

            if (user != null)
            {
                var token = CreateToken(user.Id, "student");
                return Ok(token);
            }
            return NotFound("Student details not found");
        }

        private async Task<Student> AuthenticatUser(LoginUser request)
        {

            var curuser = await _context.Students.FirstOrDefaultAsync(o =>o.RollNo == request.RollNo && o.Password == request.Password);
            if (curuser != null)
            {
                return curuser;
            }
            return null;
        }

        private string CreateToken(long id,string Role)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,id.ToString()),
                new Claim(ClaimTypes.Role,Role)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration["Jwt:Key"]));
            var cred = new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials:cred
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

    }
}
