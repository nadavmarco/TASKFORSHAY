using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public bool Insert()
        {

            foreach (var movie in MoviesList)
            {
                if (movie.Id == this.Id)
                {
                    return false; // Movie with the same Id already exists
                }
            }
            MoviesList.Add(this);
            return true; // Movie added successfully
        }

        public static List<Movie> Read()
        {
            return MoviesList;
        }
    }
}