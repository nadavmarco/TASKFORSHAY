using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using TASKFORSHAY.Models;

namespace TASKFORSHAY.DAL
{
    public class MovieDAL : DBServices
    {
        private SqlDataReader reader;
        private SqlConnection connection;
        private SqlCommand command;

        // מחזיר את כל הסרטים מה-DB
        // חשוב: שם ה-SP צריך להיות בדיוק כמו במסד (לדוגמה: sp_GetAllMovies)
        public List<Movie> GetAllMoviesFromDB()
        {
            List<Movie> movies = new List<Movie>();

            try
            {
                connection = Connect();

                // עדכן את שם ה-SP אם אצלך הוא שונה
               command = CreateCommandWithStoredProcedure("sp_GetAllMovies", connection, null);

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Movie movie = new Movie();

                    movie.Id = Convert.ToInt32(reader["Id"]);
                    movie.Title = reader["Title"].ToString();
                    movie.Rating = Convert.ToDouble(reader["Rating"]);
                    movie.Income = Convert.ToDouble(reader["Income"]);
                    movie.ReleaseYear = Convert.ToInt32(reader["ReleaseYear"]);
                    movie.Duration = Convert.ToInt32(reader["Duration"]);
                    movie.Language = reader["Language"].ToString();
                    movie.Description = reader["Description"].ToString();
                    movie.Genre = reader["Genre"].ToString();
                    movie.PhotoUrl = reader["PhotoUrl"].ToString();

                    movies.Add(movie);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                {
                    reader.Close();
                }

                if (connection != null && connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }

            return movies;
        }

        // הוספת סרט חדש למסד הנתונים
        // חשוב: צריך SP מתאים במסד (לדוגמה: sp_InsertMovie)
        public int InsertMovieToDB(Movie movie)
        {
            int rowsAffected = 0;

            try
            {
                connection = Connect();

                Dictionary<string, object> paramDic = new Dictionary<string, object>();

                // שמות הפרמטרים צריכים להיות זהים לשמות ב-SP שלך
                paramDic.Add("@Title", movie.Title);
                paramDic.Add("@Rating", movie.Rating);
                paramDic.Add("@Income", movie.Income);
                paramDic.Add("@ReleaseYear", movie.ReleaseYear);
                paramDic.Add("@Duration", movie.Duration);
                paramDic.Add("@Language", movie.Language);
                paramDic.Add("@Description", movie.Description);
                paramDic.Add("@Genre", movie.Genre);
                paramDic.Add("@PhotoUrl", movie.PhotoUrl);

                // עדכן אם שם ה-SP אצלך שונה
                command = CreateCommandWithStoredProcedure("InsertMovie_sp", connection, paramDic);

                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (connection != null && connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }

            return rowsAffected;
        }
    }
}
