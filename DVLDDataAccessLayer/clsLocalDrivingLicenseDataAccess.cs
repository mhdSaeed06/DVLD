using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDDataAccessLayer
{
    public class clsLocalDrivingLicenseDataAccess
    {

        public static DataTable GetAllLocalDrivingLicenseList()
        {

            DataTable dtApplication = new DataTable();



            SqlConnection sqlConnection = new SqlConnection(clsDataAccessSettings.ConnectionString);



            string sqlQuery = "Select * From LocalDrivingLicenseApplications_View";




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

                            dtApplication.Load(reader);

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

                return dtApplication;





            }

        }
    


        public static DataTable GetAllLocalDrivingLicenseInfo()

        {

            DataTable dtApplication = new DataTable();



            SqlConnection sqlConnection = new SqlConnection(clsDataAccessSettings.ConnectionString);



            string sqlQuery = "Select * From LocalDrivingLicenseApplications";




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

                            dtApplication.Load(reader);

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

                return dtApplication;





            }

        }



        static public int AddNewLocalDrivingLicenseApp(int ApplicationId,int LicenseClassID)
        {
            int LocalDrivingLicenseAppId = -1;


            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"INSERT INTO [dbo].[LocalDrivingLicenseApplications]
           ([ApplicationId]
           ,[LicenseClassID])
     VALUES
           (@ApplicationId
           ,@LicenseClassID
            );
                             SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationId", ApplicationId);
            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
            


            try
            {
                connection.Open();

                object result = command.ExecuteScalar();


                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    ApplicationId = insertedID;
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


            return ApplicationId;
        }


        public static bool GetLocalDrivingLicenseApplicationsInfoById(int LocalDrivingLicenseApplicationId, ref int ApplicationId, ref int LicenseClassId)
        {

            bool isFound = false;

            SqlConnection sqlConnection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string sqlQuery = "\r\nSelect * From LocalDrivingLicenseApplications Where LocalDrivingLicenseApplicationId = @LocalDrivingLicenseApplicationId";


            SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection);


            try
            {
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@LocalDrivingLicenseApplicationId", LocalDrivingLicenseApplicationId);

                SqlDataReader reader = sqlCommand.ExecuteReader();
                using (reader)
                {
                    if (reader.Read())
                    {
                        isFound = true;

                        ApplicationId = (int)reader["ApplicationId"];
                        LicenseClassId = (int)reader["LicenseClassId"];
                        

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


        public static bool GetLocalDrivingLicenseApplicationsInfoByApplicationId(ref int LocalDrivingLicenseApplicationId, int ApplicationId, ref int LicenseClassId)
        {

            bool isFound = false;

            SqlConnection sqlConnection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string sqlQuery = "\r\nSelect * From LocalDrivingLicenseApplications Where ApplicationId = @ApplicationId";


            SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection);


            try
            {
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@ApplicationId", ApplicationId);

                SqlDataReader reader = sqlCommand.ExecuteReader();
                using (reader)
                {
                    if (reader.Read())
                    {
                        isFound = true;

                        LocalDrivingLicenseApplicationId = (int)reader["LocalDrivingLicenseApplicationId"];
                        LicenseClassId = (int)reader["LicenseClassId"];


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


        static public bool UpdateLocalDrivingLicenseApps(int LocalDrivingLicenseApplicationId,  int ApplicationId, int LicenseClassId)
        {
            int rowAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"UPDATE [dbo].[LocalDrivingLicenseApplications]
   SET [ApplicationId] = @ApplicationId
      ,[LicenseClassId] =@LicenseClassId
     
      
 WHERE LocalDrivingLicenseApplicationId = @LocalDrivingLicenseApplicationId; ";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationId", LocalDrivingLicenseApplicationId);
            command.Parameters.AddWithValue("@ApplicationId", ApplicationId);
            command.Parameters.AddWithValue("@LicenseClassId", LicenseClassId);


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


        static public bool Delete(int LocalDrivingLicenseAppId)
        {
            int rowAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string sqlQuery = @"Delete LocalDrivingLicenseApplications Where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseAppId";

            SqlCommand sqlCommand = new SqlCommand(sqlQuery, connection);

            sqlCommand.Parameters.AddWithValue("@LocalDrivingLicenseAppId", LocalDrivingLicenseAppId);

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


        static public bool IsLicenseClassForThisPersonExist(int PersonId,int LicenseClassId)
        {
            bool isFound = false;


            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"						
						 
SELECT      Found = 1
FROM            LocalDrivingLicenseApplications INNER JOIN
                         Applications ON LocalDrivingLicenseApplications.ApplicationID = Applications.ApplicationID
						 Where ApplicantPersonID = @PersonId and LicenseClassID = @LicenseClassId
						
	   
";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonId", PersonId);
            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassId);

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
