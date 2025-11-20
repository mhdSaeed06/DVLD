using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDDataAccessLayer
{
    public class clsTestTypeDataAccees
    {
        public static DataTable GetAllTestTypes()

        {

            DataTable dtPeople = new DataTable();



            SqlConnection sqlConnection = new SqlConnection(clsDataAccessSettings.ConnectionString);



            string sqlQuery = "Select * From TestTypes";



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

        static public bool UpdateTestType(int TestTypeID, string Title,string Description, double Fees)
        {
            int rowAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"UPDATE [dbo].[TestTypes]
   SET [TestTypeTitle] = @TestTypeTitle
      ,[TestTypeDescription] = @TestTypeDescription
        , [TestTypeFees] = @TestTypeFees
 WHERE TestTypeID = @TestTypeID ";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestTypeTitle", Title);
            command.Parameters.AddWithValue("@TestTypeDescription", Description);

            command.Parameters.AddWithValue("@TestTypeFees", Fees);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);


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

        public static bool GetTestTypeInfoById(int TestTypeID, ref string Title
            , ref string Description, ref decimal Fees)
        {

            bool isFound = false;

            SqlConnection sqlConnection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string sqlQuery = "\r\nSelect * From TestTypes Where TestTypeID = @TestTypeID";


            SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection);


            try
            {
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                SqlDataReader reader = sqlCommand.ExecuteReader();
                using (reader)
                {
                    if (reader.Read())
                    {
                        isFound = true;

                        Title = (string)reader["TestTypeTitle"];
                        Description = (string)reader["TestTypeDescription"];
                        Fees = Convert.ToDecimal(reader["TestTypeFees"]);



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

    }
}
