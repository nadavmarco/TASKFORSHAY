using Microsoft.AspNetCore.Mvc;
using System;
using TASKFORSHAY.Models;
using TASKFORSHAY.DAL;

namespace TASKFORSHAY.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private UsersDAL usersDal;

        public UsersController()
        {
            usersDal = new UsersDAL();
        }

        // POST api/users/register
        // רישום משתמש חדש
        [HttpPost("register")]
        public IActionResult Register([FromBody] Users user)
        {
            try
            {
                if (user == null)
                {
                    return BadRequest("User data is null.");
                }

                if (string.IsNullOrEmpty(user.UserName) ||
                    string.IsNullOrEmpty(user.Email) ||
                    string.IsNullOrEmpty(user.Password))
                {
                    return BadRequest("UserName, Email and Password are required.");
                }

                int rowsAffected = usersDal.RegisterUser(user);

                if (rowsAffected > 0)
                {
                    // נרשם בהצלחה
                    return Ok("User registered successfully.");
                }
                else
                {
                    // ה-SP לא הכניס שורה (נדיר, אבל נשמור מקרה קצה)
                    return Ok("User was not registered.");
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error while registering user.");
            }
        }
        // POST api/users/login
        // התחברות משתמש קיים
        [HttpPost("login")]
        public IActionResult Login([FromBody] Users loginData)
        {
            try
            {
                if (loginData == null ||
                    string.IsNullOrEmpty(loginData.Email) ||
                    string.IsNullOrEmpty(loginData.Password))
                {
                    return BadRequest("Email and Password are required.");
                }

                Users user = usersDal.Login(loginData.Email, loginData.Password);

                if (user == null)
                {
                    // אם המשתמש לא נמצא – נחזיר null
                    // הצד לקוח יתמודד עם זה בחלק ד'
                    return Ok(null);
                }

                // משתמש נמצא – מחזירים את פרטי המשתמש (ללא סיסמה)
                return Ok(user);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error while logging in.");
            }
        }
    }
}
