using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace TASKFORSHAY.Models
{
    public class Cast
    {
        public static List<Cast> CastsList = new List<Cast>();

       
        public int Id { get; set; }


        public string Name { get; set; }


        public string Role { get; set; }

        public DateTime DateOfBirth { get; set; }

   
        public string Country { get; set; }

        // Insert() - הוספת שחקן חדש
        public bool Insert()
        {
            foreach (var cast in CastsList)
            {
                if (cast.Id == this.Id)
                {
                    return false; // שחקן עם אותו Id כבר קיים
                }
            }

            CastsList.Add(this);
            return true; // נוסף בהצלחה
        }

        // Read() - מחזיר את כל השחקנים
        public static List<Cast> Read()
        {
            return CastsList;
        }
    }
}