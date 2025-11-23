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

        // מחזיר את כל ה-Cast מה-DB
        public List<Cast> GetAllCastFromDB()
        {
            List<Cast> lst = new List<Cast>();

            try
            {
                connection = Connect();

                // ✅ שם SP לפי ה-SQL החדש
                command = CreateCommandWithStoredProcedure("GetAllCast_sp", connection, null);

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Cast cast = new Cast();

                    cast.Id = Convert.ToInt32(reader["Id"]);
                    cast.Name = reader["Name"].ToString();

                    // NULL-safe
                    cast.Role = reader["Role"] == DBNull.Value ? null : reader["Role"].ToString();
                    cast.PhotoUrl = reader["PhotoUrl"] == DBNull.Value ? null : reader["PhotoUrl"].ToString();
                    cast.Country = reader["Country"] == DBNull.Value ? null : reader["Country"].ToString();

                    // NULL-safe Date
                    cast.DateOfBirth = reader["DateOfBirth"] == DBNull.Value
                        ? DateTime.MinValue
                        : Convert.ToDateTime(reader["DateOfBirth"]);

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
                    reader.Close();

                if (connection != null && connection.State != ConnectionState.Closed)
                    connection.Close();
            }

            return lst;
        }
    }
}