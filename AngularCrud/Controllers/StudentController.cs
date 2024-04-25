using AngularCrud.data;
using AngularCrud.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularCrud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly StudentDbContext _studentDbContext;

        public StudentController(StudentDbContext studentDbContext)
        {
            _studentDbContext = studentDbContext;
        }

        [HttpGet]
        [Route("GetStudent")]
        public async Task<IEnumerable<Student>> GetStudents()
        {
            return await _studentDbContext.Student.ToListAsync();
        }

        [HttpPost]
        [Route("AddStudent")]
        public async Task<ActionResult<Student>> AddStudent(Student objStudent)
        {
            _studentDbContext.Student.Add(objStudent);
            await _studentDbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetStudents), new { id = objStudent.id }, objStudent);
        }
        [HttpPatch("UpdateStudent/{id}")]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] Student objStudent)
        {
            if (id != objStudent.id)
            {
                return BadRequest("ID mismatch");
            }

            var existingStudent = await _studentDbContext.Student.FindAsync(id);
            if (existingStudent == null)
            {
                return NotFound("Student not found");
            }

            _studentDbContext.Entry(existingStudent).CurrentValues.SetValues(objStudent);

            try
            {
                await _studentDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_studentDbContext.Student.Any(s => s.id == id))
                {
                    return NotFound("Student not found");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete]
        [Route("DeleteStudent/{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _studentDbContext.Student.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _studentDbContext.Student.Remove(student);
            await _studentDbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
