using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using TASKFORSHAY.DAL;

namespace TASKFORSHAY.Models
{
    public class Movie
    {
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

        // Insert - הוספת סרט חדש למסד הנתונים דרך DAL
        public bool Insert()
        {
            try
            {
                MovieDAL dal = new MovieDAL();
                int rowsAffected = dal.InsertMovieToDB(this);
                return rowsAffected > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Read - מחזיר את כל הסרטים מה-DB דרך DAL
        public static List<Movie> Read()
        {
            try
            {
                MovieDAL dal = new MovieDAL();
                List<Movie> movies = dal.GetAllMoviesFromDB();
                return movies;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // ReadByRating - סינון לפי דירוג, על בסיס נתונים מה-DB
        public static List<Movie> ReadByRating(double minRating)
        {
            try
            {
                MovieDAL dal = new MovieDAL();
                List<Movie> movies = dal.GetAllMoviesFromDB();

                List<Movie> filtered =
                    movies.Where(m => m.Rating >= minRating).ToList();

                return filtered;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // ReadByDuration - סינון לפי משך, על בסיס נתונים מה-DB
        public static List<Movie> ReadByDuration(int maxDuration)
        {
            try
            {
                MovieDAL dal = new MovieDAL();
                List<Movie> movies = dal.GetAllMoviesFromDB();

                List<Movie> filtered =
                    movies.Where(m => m.Duration <= maxDuration).ToList();

                return filtered;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
