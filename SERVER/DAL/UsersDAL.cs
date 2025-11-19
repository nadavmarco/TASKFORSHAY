using Microsoft.Data.SqlClient;
using TASKFORSHAY.Models;

namespace TASKFORSHAY.DAL
{
    public class UsersDAL : DBServices
    {
        private SqlDataReader reader;
        private SqlConnection connection;
        private SqlCommand command;


        //-- STORED PROCEDURE: RegisterUser
        public List<Users> GetAllCastFromDB()
        {
            try
            {
                connection = Connect();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            command = CreateCommandWithStoredProcedure("GetCast", connection, null);

            try
            {
                List<Users> lst = new List<Users>();
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    lst.Add(new Users()
                    {
                        //Id = int.Parse(reader["Id"].ToString()),
                        //Name = reader["Name"].ToString(),
                        //Role = reader["Role"].ToString(),
                        //DateOfBirth = DateTime.Parse(reader["DateOfBirth"].ToString()),
                        //PhotoUrl = reader["PhotoUrl"].ToString()
                    });
                }
                return lst;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }
    }
}
