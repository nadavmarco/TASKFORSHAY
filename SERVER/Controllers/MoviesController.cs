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
        // GET api/movies - מחזיר את כל הסרטים או מסנן לפי משך (Query String)
        [HttpGet]
        public ActionResult<List<Movie>> GetAll([FromQuery] int? maxDuration)
        {
            try
            {
                if (maxDuration.HasValue)
                {
                    return Movie.ReadByDuration(maxDuration.Value);
                }

                return Movie.Read();
            }
            catch (Exception ex)
            {
                return Problem($"Server error: {ex.Message}");
            }
        }

        // GET api/movies/rating/7.5 - מסנן לפי דירוג מינימלי
        [HttpGet("rating/{minRating:double}")]
        public ActionResult<List<Movie>> GetByRating(double minRating)
        {
            try
            {
                return Movie.ReadByRating(minRating);
            }
            catch (Exception ex)
            {
                return Problem($"Server error: {ex.Message}");
            }
        }

        // POST api/movies - הוספת סרט חדש
        [HttpPost]
        public ActionResult<Movie> Insert([FromBody] Movie movie)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                bool success = movie.Insert();
                if (!success)
                {
                    return Conflict("Movie with the same Id already exists.");
                }

                return movie; // מחזיר את הסרט שנוסף
            }
            catch (Exception ex)
            {
                return Problem($"Server error: {ex.Message}");
            }
        }
    }
}