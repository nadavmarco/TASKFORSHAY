using System;
using System.Data;
using Microsoft.Data.SqlClient;
using TASKFORSHAY.Models;

namespace TASKFORSHAY.DAL
{
    public class UsersDAL : DBServices
    {
        private SqlConnection connection;
        private SqlCommand command;
        private SqlDataReader reader;

        // רישום משתמש חדש - קריאה ל-SP: sp_RegisterUser
        // מחזיר את מספר השורות שהושפעו (1 אם נרשם בהצלחה, 0 אם לא)
        public int RegisterUser(Users user)
        {
            int rowsAffected = 0;

            try
            {
                connection = Connect();

                Dictionary<string, object> paramDic = new Dictionary<string, object>();
                paramDic.Add("@UserName", user.UserName);
                paramDic.Add("@Email", user.Email);
                paramDic.Add("@Password", user.Password);

                command = CreateCommandWithStoredProcedure("RegisterUser_sp", connection, paramDic);

                rowsAffected = command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                // כאן אפשר לזהות שגיאה של אימייל קיים (אם ה-SP מרים RAISERROR)
                throw;
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

        // התחברות משתמש - קריאה ל-SP: LoginUser_sp
        // מחזיר אובייקט Users (ללא סיסמה) אם נמצאה התאמה, אחרת null
        public Users Login(string email, string password)
        {
            Users user = null;

            try
            {
                connection = Connect();

                Dictionary<string, object> paramDic = new Dictionary<string, object>();
                paramDic.Add("@Email", email);
                paramDic.Add("@Password", password);

                command = CreateCommandWithStoredProcedure("LoginUser_sp", connection, paramDic);

                reader = command.ExecuteReader();

                if (reader.Read())
                {
                    user = new Users();
                    user.Id = Convert.ToInt32(reader["Id"]);
                    user.UserName = reader["UserName"].ToString();
                    user.Email = reader["Email"].ToString();
                    // שים לב: הסיסמה לא חוזרת מה-SP וזה מעולה.
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

            return user;
        }
    }
}
