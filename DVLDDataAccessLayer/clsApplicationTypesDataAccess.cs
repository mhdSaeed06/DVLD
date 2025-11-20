using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDDataAccessLayer
{
    public class clsApplicationTypesDataAccess
    {
        public static DataTable GetAllApplicationTypes()

        {

            DataTable dtPeople = new DataTable();



            SqlConnection sqlConnection = new SqlConnection(clsDataAccessSettings.ConnectionString);



            string sqlQuery = "Select * From ApplicationTypes";



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

        static public bool UpdateApplicationTypes(int ApplicationTypeID, string Title, decimal Fees)
        {
            int rowAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"UPDATE [dbo].[ApplicationTypes]
   SET [ApplicationTypeTitle] = @ApplicationTypeTitle
      ,[ApplicationFees] = @ApplicationFees
 WHERE ApplicationTypeID = @ApplicationTypeID ";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ApplicationTypeTitle", Title);

            command.Parameters.AddWithValue("@ApplicationFees", Fees);
            command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
            
         
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

        public static bool GetApplicationTypeById(int ApplicationTypeID,ref string Title,ref decimal Fees)
        {

            bool isFound = false;

            SqlConnection sqlConnection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string sqlQuery = "\r\nSelect * From ApplicationTypes Where ApplicationTypeID = @ApplicationTypeID";


            SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection);


            try
            {
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);

                SqlDataReader reader = sqlCommand.ExecuteReader();
                using (reader)
                {
                    if (reader.Read())
                    {
                        isFound = true;

                        Title = (string)reader["ApplicationTypeTitle"];
                        Fees = Convert.ToDecimal(reader["ApplicationFees"]);
                        

                     
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


        public static double GetApplicationFeesById(int applicationTypeId)
        {
            double fees = 0;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "SELECT ApplicationFees FROM ApplicationTypes WHERE ApplicationTypeID = @ApplicationTypeID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ApplicationTypeID", applicationTypeId);

                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                    fees = Convert.ToDouble(result);
            }

            return fees;
        }

    }
}
