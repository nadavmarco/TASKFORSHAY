using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using TASKFORSHAY.Models;

namespace TASKFORSHAY.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CastsController : ControllerBase
    {
        // GET api/casts - מחזיר את כל השחקנים
        [HttpGet]
        public IActionResult GetCasts()
        {
            var casts = Cast.Read();
            return Ok(casts);
        }

        // POST api/casts - הוספת שחקן חדש
        [HttpPost]
         public IActionResult actionResult([FromBody] Cast cast)
        {
            try
            {
                if (cast == null)
                {
                    return BadRequest("Cast data is null.");
                }

                bool isInserted = cast.Insert();

                if (isInserted)
                {
                    return Ok("Cast added successfully.");
                }
                else
                {
                    return Conflict("A cast with the same CastId already exists.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}