using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using TASKFORSHAY.Models;

namespace TASKFORSHAY.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        // GET api/movies – מביא את כל הסרטים מה-DB דרך Movie.Read()
        [HttpGet]
        public IActionResult GetMovies()
        {
            try
            {
                List<Movie> movies = Movie.Read();
                return Ok(movies);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error while getting movies.");
            }
        }

        // POST api/movies – מוסיף סרט חדש ל-DB דרך movie.Insert()
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
                    return Ok("Movie was not added.");
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error while adding movie.");
            }
        }

        // GET api/movies/byRating/{minRating} – סינון לפי דירוג, על בסיס נתוני DB
        [HttpGet("byRating/{minRating}")]
        public IActionResult GetByRating([FromRoute] double minRating)
        {
            try
            {
                List<Movie> movies = Movie.ReadByRating(minRating);

                if (!movies.Any())
                {
                    return NotFound($"No movies found with rating {minRating} or higher.");
                }

                return Ok(movies);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error while filtering movies by rating.");
            }
        }

        // GET api/movies/duration?maxDuration=120 – סינון לפי משך, על בסיס נתוני DB
        [HttpGet("duration")]
        public IActionResult GetByDuration([FromQuery] int maxDuration)
        {
            try
            {
                if (maxDuration <= 0)
                {
                    return BadRequest("maxDuration must be a positive number.");
                }

                List<Movie> movies = Movie.ReadByDuration(maxDuration);

                if (!movies.Any())
                {
                    return NotFound($"No movies found with duration {maxDuration} minutes or less.");
                }

                return Ok(movies);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error while filtering movies by duration.");
            }
        }
    }
}
