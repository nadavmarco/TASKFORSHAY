using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TASKFORSHAY.Models;

namespace TASKFORSHAY.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetMovies()
        {
            try
            {
                var movies = Movie.Read();
                return Ok(movies);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult actionResult([FromBody] Movie movie)
        {
            try
            {
                if (movie == null)
                {
                    return BadRequest("Movie data is null.");
                }
                bool isInserted = movie.Insert();
                if (isInserted)
                {
                    return Ok("Movie added successfully.");
                }
                else
                {
                    return Conflict("A movie with the same Id already exists.");
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}