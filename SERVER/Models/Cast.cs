using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


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

        public bool Insert()
        {

            foreach (var cast in CastsList)
            {
                if (cast.Id == this.Id)
                {
                    return false;
                }
            }
            CastsList.Add(this);
            return true;
        }

        public static List<Cast> Read()
        {
            return CastsList;
        }
    }
}