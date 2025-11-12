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
        public ActionResult<List<Cast>> GetAll()
        {
            try
            {
                return Cast.Read();
            }
            catch (Exception ex)
            {
                return Problem($"Server error: {ex.Message}");
            }
        }

        // POST api/casts - הוספת שחקן חדש
        [HttpPost]
        public ActionResult<Cast> Insert([FromBody] Cast cast)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                bool success = cast.Insert();
                if (!success)
                {
                    return Conflict("Cast with the same Id already exists.");
                }

                return cast; // מחזיר את השחקן שנוסף
            }
            catch (Exception ex)
            {
                return Problem($"Server error: {ex.Message}");
            }
        }
    }
}