using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TASKFORSHAY.Models
{
    public class Movie
    {
        public static List<Movie> MoviesList = new List<Movie>();


        public int Id { get; set; }


        public string Title { get; set; }


        public double Rating { get; set; }

  
        public double Income { get; set; }

    
        public int ReleaseYear { get; set; }

 
        public int Duration { get; set; }


        public string Language { get; set; }


        public string Description { get; set; }

 
        public string Genre { get; set; }

  
        public string PhotoUrl { get; set; }

        // Insert() - הוספת סרט חדש אם לא קיים עם אותו Id
        public bool Insert()
        {
            foreach (var movie in MoviesList)
            {
                if (movie.Id == this.Id)
                {
                    return false; // כבר קיים סרט עם אותו מזהה
                }
            }
            MoviesList.Add(this);
            return true; // נוסף בהצלחה
        }

        // Read() - החזרת כל הסרטים
        public static List<Movie> Read()
        {
            return MoviesList;
        }

        // ReadByRating() - מחזיר סרטים עם דירוג מינימלי
        public static List<Movie> ReadByRating(double minRating)
        {
            List<Movie> filtered = new List<Movie>();

            foreach (var movie in MoviesList)
            {
                if (movie.Rating >= minRating)
                {
                    filtered.Add(movie);
                }
            }

            return filtered;
        }

        // ReadByDuration() - מחזיר סרטים שמשך הזמן שלהם קטן או שווה
        public static List<Movie> ReadByDuration(int maxDuration)
        {
            List<Movie> filtered = new List<Movie>();

            foreach (var movie in MoviesList)
            {
                if (movie.Duration <= maxDuration)
                {
                    filtered.Add(movie);
                }
            }

            return filtered;
        }    }
}