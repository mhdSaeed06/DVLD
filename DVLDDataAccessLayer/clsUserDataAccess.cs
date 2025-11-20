using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDDataAccessLayer
{
    public class clsUserDataAccess
    {
        public static DataTable GetAllUsers()

        {

            DataTable dtPeople = new DataTable();



            SqlConnection sqlConnection = new SqlConnection(clsDataAccessSettings.ConnectionString);



            string sqlQuery = @"
	SELECT        Users.UserID, Users.PersonID,
	People.FirstName + ' ' + People.SecondName+ ' '  + People.ThirdName + ' ' + People.LastName As FullName
	
	, Users.UserName, Users.IsActive
FROM            People INNER JOIN
                         Users ON People.PersonID = Users.PersonID
";
            
            SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection);

            using (sqlConnection)

            {



                try

                {

                    sqlConnection.Open();

                    using (SqlDataReader reader = sqlCommand.ExecuteReader())

                    {

                        if (reader.HasRows)

                        {

                            dtPeople.Load(reader);

                        }



                        reader.Close();

                    }





                }

                catch (Exception ex)

                {



                }

                finally

                {

                    sqlConnection.Close();



                }

                return dtPeople;





            }

        }

        public static DataTable GetAllUsersInfo()

        {

            DataTable dtPeople = new DataTable();



            SqlConnection sqlConnection = new SqlConnection(clsDataAccessSettings.ConnectionString);



            string sqlQuery = @"
	SELECT       * From Users;
";

            SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection);

            using (sqlConnection)

            {



                try

                {

                    sqlConnection.Open();

                    using (SqlDataReader reader = sqlCommand.ExecuteReader())

                    {

                        if (reader.HasRows)

                        {

                            dtPeople.Load(reader);

                        }



                        reader.Close();

                    }





                }

                catch (Exception ex)

                {



                }

                finally

                {

                    sqlConnection.Close();



                }

                return dtPeople;





            }

        }


        static public int AddNewUser(string UserName, int PersonId, string Password
             , bool IsActive = true)
        {
            int UserID = -1;


            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"INSERT INTO [dbo].[Users]
           ([UserName]
           ,[PersonId]
           ,[Password]
           ,[IsActive] )
     VALUES
           (@UserName
           ,@PersonId
           ,@Password
           ,@IsActive);
                             SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserName", UserName);
            command.Parameters.AddWithValue("@PersonId", PersonId);
            command.Parameters.AddWithValue("@Password", Password);
            command.Parameters.AddWithValue("@IsActive", IsActive);
           
            try
            {
                connection.Open();

                object result = command.ExecuteScalar();


                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    UserID = insertedID;
                }
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);

            }

            finally
            {
                connection.Close();
            }


            return UserID;
        }




        public static bool GetUserInfoById(int UserId,ref string UserName, ref int PersonId, ref string Password
             , ref bool IsActive)
        {

            bool isFound = false;

            SqlConnection sqlConnection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string sqlQuery = "\r\nSelect * From Users Where UserId = @UserId";


            SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection);


            try
            {
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@UserId", UserId);

                SqlDataReader reader = sqlCommand.ExecuteReader();
                using (reader)
                {
                    if (reader.Read())
                    {
                        isFound = true;

                        UserName = (string)reader["UserName"];
                        PersonId = (int)reader["PersonId"];
                        Password = (string)reader["Password"];
                        IsActive = (bool)reader["IsActive"];
                       

                    


                    }

                }
                reader.Dispose();
            }
            catch (Exception ex)
            {

            }
            finally { sqlConnection.Close(); }


            return isFound;

        }



        static public bool UpdateUser(int UserId, string UserName, int PersonId, string Password
             , bool IsActive)
        {
            int rowAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"UPDATE [dbo].[Users]
   SET [UserName] =@UserName
      ,[PersonId] = @PersonId
      ,[Password] =@Password
      ,[IsActive] = @IsActive
 WHERE UserId = @UserId; ";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserId", UserId);

            command.Parameters.AddWithValue("@UserName", UserName);
            command.Parameters.AddWithValue("@PersonId", PersonId);
            command.Parameters.AddWithValue("@Password", Password);
            command.Parameters.AddWithValue("@IsActive", IsActive);


            try
            {
                connection.Open();

                rowAffected = command.ExecuteNonQuery();
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                return false;

            }

            finally
            {
                connection.Close();
            }


            return (rowAffected > 0);
        }



        static public bool DeleteUser(int UserId)
        {
            int rowAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string sqlQuery = @"Delete Users Where UserId = @UserId";

            SqlCommand sqlCommand = new SqlCommand(sqlQuery, connection);

            sqlCommand.Parameters.AddWithValue("@UserId", UserId);

            try
            {
                connection.Open();

                rowAffected = sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                connection.Dispose();
            }

            return (rowAffected > 0);
        }


        static public bool IsUserExist(int UserId)
        {
            bool isFound = false;


            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT Found=1 FROM Users WHERE UserId = @UserId";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserId", UserId);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                isFound = reader.HasRows;

                reader.Close();
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

    }
}
