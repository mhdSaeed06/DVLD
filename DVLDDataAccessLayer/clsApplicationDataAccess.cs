    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using static System.Net.Mime.MediaTypeNames;

    namespace DVLDDataAccessLayer
    {
        public class clsApplicationDataAccess
        {
            public static DataTable GetAllApplicationsInfo()

            {

                DataTable dtApplication = new DataTable();



                SqlConnection sqlConnection = new SqlConnection(clsDataAccessSettings.ConnectionString);



                string sqlQuery = "Select * From Applications";
            



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



            static public int AddNewApplication(int PersonId,int CreatedByUserId,DateTime ApplicationDate,
                DateTime LastStatusDate,int ApplicationTypeId,byte ApplicationStatus,decimal PaidFees)
            {
                int ApplicationId = -1;


                SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

                string query = @"INSERT INTO [dbo].[Applications]
               ([ApplicantPersonID]
               ,[CreatedByUserId]
               ,[ApplicationDate]
               ,[LastStatusDate]
               ,[ApplicationTypeID]
               ,[ApplicationStatus]
               ,[PaidFees])
         VALUES
               (@PersonId
               ,@CreatedByUserId
               ,@ApplicationDate
               ,@LastStatusDate
               ,@ApplicationTypeId
               ,@ApplicationStatus
               ,@PaidFees );
                                 SELECT SCOPE_IDENTITY();";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@PersonId", PersonId);
                command.Parameters.AddWithValue("@CreatedByUserId", CreatedByUserId);
                command.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
                command.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
                command.Parameters.AddWithValue("@ApplicationTypeId", ApplicationTypeId);
                command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
                command.Parameters.AddWithValue("@PaidFees", PaidFees);

           
           
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

        
            public static bool GetApplicationInfoById(int ApplicationId,ref int PersonId, ref int CreatedByUserId, ref DateTime ApplicationDate,
               ref DateTime LastStatusDate,ref int ApplicationTypeId,ref byte ApplicationStatus, ref decimal PaidFees)
            {

                bool isFound = false;

                SqlConnection sqlConnection = new SqlConnection(clsDataAccessSettings.ConnectionString);

                string sqlQuery = "\r\nSelect * From Applications Where ApplicationID = @ApplicationID";


                SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection);


                try
                {
                    sqlConnection.Open();
                    sqlCommand.Parameters.AddWithValue("@ApplicationID", ApplicationId);

                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    using (reader)
                    {
                        if (reader.Read())
                        {
                            isFound = true;

                            PersonId = (int)reader["ApplicantPersonID"];
                            ApplicationDate = (DateTime)reader["ApplicationDate"];
                            ApplicationTypeId = (int)reader["ApplicationTypeID"];
                            ApplicationStatus = (byte)reader["ApplicationStatus"];
                            LastStatusDate = (DateTime)reader["LastStatusDate"];
                            PaidFees = (decimal)reader["PaidFees"];
                            CreatedByUserId = (int)reader["CreatedByUserId"];
                        

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



            static public bool UpdateApplication(int ApplicationId,  int PersonId,  int CreatedByUserId,  DateTime ApplicationDate,
                DateTime LastStatusDate,  int ApplicationTypeId, byte ApplicationStatus,  decimal PaidFees)
            {
                int rowAffected = 0;

                SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

                string query = @"UPDATE [dbo].[Applications]
       SET [ApplicantPersonID] = @PersonId
          ,[CreatedByUserId] =@CreatedByUserId
          ,[ApplicationDate] = @ApplicationDate
          ,[ApplicationTypeId] =  @ApplicationTypeId
          ,[LastStatusDate] = @LastStatusDate
          ,[ApplicationStatus] = @ApplicationStatus
          ,[PaidFees] = @PaidFees
      
     WHERE ApplicationId = @ApplicationId; ";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@PersonId", PersonId);
                command.Parameters.AddWithValue("@ApplicationId", ApplicationId);
                command.Parameters.AddWithValue("@CreatedByUserId", CreatedByUserId);
                command.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
                command.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
                command.Parameters.AddWithValue("@ApplicationTypeId", ApplicationTypeId);
                command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
                command.Parameters.AddWithValue("@PaidFees", PaidFees);

          
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



            static public bool DeleteApplication(int ApplicationId)
            {
                int rowAffected = 0;

                SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

                string sqlQuery = @"Delete Applications Where ApplicationId = @ApplicationId";

                SqlCommand sqlCommand = new SqlCommand(sqlQuery, connection);

                sqlCommand.Parameters.AddWithValue("@ApplicationId", ApplicationId);

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


            static public bool IsApplicationExist(int ApplicationID)
            {
                bool isFound = false;


                SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

                string query = "SELECT Found=1 FROM Applications WHERE ApplicationId = @ApplicationId";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@ApplicationId", ApplicationID);

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


            public static string GetApplicationTypeTitleByAppID(int ApplicationID)
            {
                string ApplicationTitle = "";

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"
    SELECT ApplicationTypes.ApplicationTypeTitle From Applications join ApplicationTypes on Applications.ApplicationTypeID = ApplicationTypes.ApplicationTypeID
    Where Applications.ApplicationID = @ApplicationID";


                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

                    try
                    {
                        connection.Open();
                        ApplicationTitle = (string)command.ExecuteScalar();
                    }
                    catch { }
                }

                return ApplicationTitle;
            }

        }
    }
