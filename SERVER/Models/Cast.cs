using System;
using System.Collections.Generic;
using TASKFORSHAY.DAL;

namespace TASKFORSHAY.Models
{
    public class Cast
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Role { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string PhotoUrl { get; set; }

        public string Country { get; set; }

        // Insert() - מיועד להוספה למסד הנתונים (כשתוסיף SP)
        // כרגע נשאיר את המתודה מוכנה אבל בלי מימוש
        public bool Insert()
        {
            try
            {
                // אם בעתיד תיצור SP ל־InsertCast, זה יהיה כאן:
                // CastDAL dal = new CastDAL();
                // int rows = dal.InsertCastToDB(this);
                // return rows > 0;

                // כרגע אין הוספה ל-DB, ולכן נחזיר false
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Read() - החזרת כל ה-Cast מהדאטאבייס דרך DAL
        public static List<Cast> Read()
        {
            try
            {
                CastDAL dal = new CastDAL();
                return dal.GetAllCastFromDB();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
