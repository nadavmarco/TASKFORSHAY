using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using TASKFORSHAY.Models;

namespace TASKFORSHAY.DAL
{
    public class CastDAL : DBServices
    {
        private SqlDataReader reader;
        private SqlConnection connection;
        private SqlCommand command;

        // מחזיר את כל ה-Cast מה-DB בלבד
        // תוודא שה-SP שלך נקרא בדיוק: sp_GetAllCast
        public List<Cast> GetAllCastFromDB()
        {
            List<Cast> lst = new List<Cast>();

            try
            {
                connection = Connect();
                command = CreateCommandWithStoredProcedure("sp_GetAllCast", connection, null);

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Cast cast = new Cast();

                    cast.Id = Convert.ToInt32(reader["Id"]);
                    cast.Name = reader["Name"].ToString();
                    cast.Role = reader["Role"].ToString();
                    cast.DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]);
                    cast.PhotoUrl = reader["PhotoUrl"].ToString();

                    // אם תוסיף בטבלה Country תרצה גם לקרוא אותה כאן
                    // cast.Country = reader["Country"].ToString();

                    lst.Add(cast);
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

            return lst;
        }
    }
}
