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
        // GET api/casts - מחזיר את כל השחקנים מה-DB דרך Cast.Read()
        [HttpGet]
        public IActionResult GetCasts()
        {
            try
            {
                List<Cast> casts = Cast.Read();
                return Ok(casts);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error retrieving cast data from database.");
            }
        }
    }
}
