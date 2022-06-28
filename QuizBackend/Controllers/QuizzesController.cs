using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizBackend.Data;
using QuizBackend.Models;

namespace QuizBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QuizzesController : ControllerBase
    {
        private readonly QuizContext _context;
        private readonly IConfiguration _configuration;

        public QuizzesController(QuizContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/Quizzes/all
        [HttpGet("all")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Quiz>>> GetAllQuiz()
        {
            if (_context.Quiz == null)
            {
                return NotFound();
            }
            try
            {
                // Return quizzes that belongs authenticated user
                return await _context.Quiz.ToListAsync();

            }
            catch ( Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Quizzes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Quiz>>> GetQuizzes()
        {
          if (_context.Quiz == null)
          {
              return NotFound();
          }
            // Get userid from Jwt from HttpRequest
            var userId = HttpContext.User.Claims.First().Value;

            // Return quizzes that belongs authenticated user
            return await _context.Quiz.Where(p=>p.UserId==new Guid(userId)).ToListAsync();
        }

        // GET: api/Quizzes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Quiz>> GetQuiz(int id)
        {
            if (_context.Quiz == null)
            {
                return NotFound();
            }

            // Get userid from Jwt from HttpRequest
            var userId = HttpContext.User.Claims.First().Value;

            // Find quiz
            var quiz = await _context.Quiz.FindAsync(id);

            // And check if it belongs user
            if (quiz == null || quiz.UserId != new Guid(userId))
            {
                return NotFound();
            }

            return quiz;
        }

        // PUT: api/Quizzes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuiz(int id, Quiz quiz)
        {
            if (id != quiz.Id)
            {
                return BadRequest();
            }

            if (_context.Quiz == null)
            {
                return NotFound();
            }

            // Get userid from Jwt from HttpRequest
            var userId = HttpContext.User.Claims.First().Value;

            // Check if quiz belongs authenticated user
            var quiz1 = await _context.Quiz.FindAsync(id);
            if ( quiz1 == null || quiz1.UserId != new Guid(userId))
                return NotFound();

            _context.Entry(quiz).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuizExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Quizzes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Quiz>> PostQuiz(Quiz quiz)
        {
          if (_context.Quiz == null)
          {
              return Problem("Entity set 'QuizContext.Quiz'  is null.");
          }

            // Get userid from Jwt from HttpRequest
            var userId = HttpContext.User.Claims.First().Value;

            // Bind UserId to quiz
            quiz.UserId = new Guid(userId);

            // Create quiz
            _context.Quiz.Add(quiz);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetQuiz", new { id = quiz.Id }, quiz);
        }

        // DELETE: api/Quizzes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuiz(int id)
        {
            if (_context.Quiz == null)
            {
                return NotFound();
            }

            // Get userid from Jwt from HttpRequest
            var userId = HttpContext.User.Claims.First().Value;

            var quiz = await _context.Quiz.FindAsync(id);
            if (quiz == null || quiz.UserId != new Guid(userId))
            {
                return NotFound();
            }

            _context.Quiz.Remove(quiz);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool QuizExists(int id)
        {
            return (_context.Quiz?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
