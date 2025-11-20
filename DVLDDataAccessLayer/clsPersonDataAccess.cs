using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace DVLDDataAccessLayer
{
    public class clsPersonDataAccess
    {
        public static DataTable GetAllPeopleForList()

        {

            DataTable dtPeople = new DataTable();



            SqlConnection sqlConnection = new SqlConnection(clsDataAccessSettings.ConnectionString);



            string sqlQuery2 = "Select * From People";
            string sqlQuery = @"
SELECT        People.PersonID,People.NationalNo, People.FirstName, People.SecondName, People.ThirdName, People.LastName, Countries.CountryName, People.DateOfBirth, 
case People.Gendor
when 0 then 'Male'
when 1 then 'Female'
end as Gender, People.Address, People.Phone, People.Email
FROM            People INNER JOIN
                         Countries ON People.NationalityCountryID = Countries.CountryID";



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


        public static DataTable GetAllPeopleInfo()

        {

            DataTable dtPeople = new DataTable();



            SqlConnection sqlConnection = new SqlConnection(clsDataAccessSettings.ConnectionString);



            string sqlQuery = "Select * From People";
            


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

        static public int AddNewPerson(string FirstName, string SecondName, string ThirdName, string LastName,
             string NationalNo,string Email, string Phone, string Address, DateTime DateOfBirth, int CountryID, string ImagePath,byte Gender)
        {
            int PersonId = -1;


            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"INSERT INTO [dbo].[People]
           ([NationalNo]
           ,[FirstName]
           ,[SecondName]
           ,[ThirdName]
           ,[LastName]
           ,[DateOfBirth]
           ,[Gendor]
           ,[Address]
           ,[Phone]
           ,[Email]
           ,[NationalityCountryID]
           ,[ImagePath])
     VALUES
           (@NationalNo
           ,@FirstName
           ,@SecondName
           ,@ThirdName
           ,@LastName
           ,@DateOfBirth
           ,@Gendor
           ,@Address
           ,@Phone
           ,@Email
           ,@NationalityCountryID
           ,@ImagePath);
                             SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@NationalNo", NationalNo);
            command.Parameters.AddWithValue("@FirstName", FirstName);
            command.Parameters.AddWithValue("@SecondName", SecondName);
            command.Parameters.AddWithValue("@ThirdName", ThirdName);
            command.Parameters.AddWithValue("@LastName", LastName);
            command.Parameters.AddWithValue("@Gendor", Gender);
            command.Parameters.AddWithValue("@Email", Email);
            command.Parameters.AddWithValue("@Phone", Phone);
            command.Parameters.AddWithValue("@Address", Address);
            command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            command.Parameters.AddWithValue("@NationalityCountryID", CountryID);

            if (ImagePath != "" && ImagePath != null)
                command.Parameters.AddWithValue("@ImagePath", ImagePath);
            else
                command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();


                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    PersonId = insertedID;
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


            return PersonId;
        }




        public static bool GetPersonInfoById(int PersonId,ref string FirstName, ref string NationalNo, ref string SecondName, ref string ThirdName, ref string LastName,
           ref string Email, ref string Phone, ref string Address, ref DateTime DateOfBirth, ref int CountryID, ref string ImagePath, ref byte Gender)
        {

            bool isFound = false;

            SqlConnection sqlConnection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string sqlQuery = "\r\nSelect * From People Where PersonId = @PersonId";


            SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection);


                try
                {
                    sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@PersonId", PersonId);

                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    using (reader)
                    {
                        if (reader.Read())
                        {
                            isFound = true;

                            FirstName = (string)reader["FirstName"];
                            NationalNo = (string)reader["NationalNo"];
                            SecondName = (string)reader["SecondName"];
                            ThirdName = (string)reader["ThirdName"];
                            LastName = (string)reader["LastName"];
                            Email = (string)reader["Email"];
                            Phone = (string)reader["Phone"];
                            Address = (string)reader["Address"];
                            Gender = (byte)reader["Gendor"];
                            DateOfBirth = (DateTime)reader["DateOfBirth"];
                            CountryID = (int)reader["NationalityCountryID"];

                            if (reader["ImagePath"] != DBNull.Value)
                            {
                                ImagePath = (string)reader["ImagePath"];
                            }
                            else
                            {
                                ImagePath = "";
                            }



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



        static public bool UpdatePerson(int PersonId, string FirstName, string SecondName, string ThirdName, string LastName,
             string NationalNo, string Email, string Phone, string Address, DateTime DateOfBirth, int CountryID, string ImagePath, byte Gender)
        {
            int rowAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"UPDATE [dbo].[People]
   SET [NationalNo] =@NationalNo
      ,[FirstName] = @FirstName
      ,[SecondName] =@SecondName
      ,[ThirdName] = @ThirdName
      ,[LastName] =  @LastName
      ,[DateOfBirth] = @DateOfBirth
      ,[Gendor] = @Gendor
      ,[Address] = @Address
      ,[Phone] = @Phone
      ,[Email] = @Email
      ,[NationalityCountryId] = @NationalityCountryId
      ,[ImagePath] = @ImagePath
 WHERE PersonId = @PersonId; ";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonId", PersonId);

            command.Parameters.AddWithValue("@NationalNo", NationalNo);
            command.Parameters.AddWithValue("@FirstName", FirstName);
            command.Parameters.AddWithValue("@SecondName", SecondName);
            command.Parameters.AddWithValue("@ThirdName", ThirdName);
            command.Parameters.AddWithValue("@LastName", LastName);
            command.Parameters.AddWithValue("@Gendor", Gender);
            command.Parameters.AddWithValue("@Email", Email);
            command.Parameters.AddWithValue("@Phone", Phone);
            command.Parameters.AddWithValue("@Address", Address);
            command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            command.Parameters.AddWithValue("@NationalityCountryID", CountryID);

            if (ImagePath != "" && ImagePath != null)
                command.Parameters.AddWithValue("@ImagePath", ImagePath);
            else
                command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);

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



        static public bool DeletePerson(int PersonId)
        {
            int rowAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string sqlQuery = @"Delete People Where PersonId = @PersonId";

            SqlCommand sqlCommand = new SqlCommand(sqlQuery, connection);

            sqlCommand.Parameters.AddWithValue("@PersonId", PersonId);

            try
            {
                connection.Open();
                
                rowAffected = sqlCommand.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                return false;
            }
            finally
            {
                connection.Dispose();
            }

            return (rowAffected > 0);
        }


        static public bool IsPersonExist(int PersonId)
        {
            bool isFound = false;


            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT Found=1 FROM People WHERE PersonId = @PersonId";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonId", PersonId);

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
