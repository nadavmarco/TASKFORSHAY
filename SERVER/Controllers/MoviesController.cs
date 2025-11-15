using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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

        [HttpGet("byRating/{minRating}")]
        public IActionResult GetByRating([FromRoute] double minRating)
        {
            try
            {
                var movies = Movie.ReadByRating(minRating);

                if (!movies.Any())
                {
                    return NotFound($"No movies found with {minRating} or higher.");
                }
                return Ok(movies);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("duration")]
        public IActionResult GetByDuration([FromQuery] int maxDuration)
        {
            try
            {
                if (maxDuration <= 0)
                {
                    return BadRequest("maxDuration must be a positive number.");
                }

                var movies = Movie.ReadByDuration(maxDuration);

                if (!movies.Any())
                {
                    return NotFound($"No movies found with duration {maxDuration} minutes or less.");
                }

                return Ok(movies);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}