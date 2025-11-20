using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDDataAccessLayer
{
    public class clsLicenseClassDataAccess
    {
        public static DataTable GetAllApplicationTypes()

        {

            DataTable dtLicenseClass = new DataTable();



            SqlConnection sqlConnection = new SqlConnection(clsDataAccessSettings.ConnectionString);



            string sqlQuery = "Select * From LicenseClasses";



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

                            dtLicenseClass.Load(reader);

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

                return dtLicenseClass;





            }

        }

        public static bool GetLicenseClassByID(int licenseClassID, ref string className,
            ref string classDescription, ref byte minimumAllowedAge,
            ref byte defaultValidityLength, ref decimal classFees)
        {
            bool isFound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"SELECT LicenseClassID, ClassName, ClassDescription, 
                                        MinimumAllowedAge, DefaultValidityLength, ClassFees
                                 FROM LicenseClasses
                                 WHERE LicenseClassID = @LicenseClassID";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@LicenseClassID", licenseClassID);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        isFound = true;

                        className = (string)reader["ClassName"];
                        classDescription = (string)reader["ClassDescription"];
                        minimumAllowedAge = (byte)reader["MinimumAllowedAge"];
                        defaultValidityLength = (byte)reader["DefaultValidityLength"];
                        classFees = (decimal)reader["ClassFees"];
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    // Log error if needed
                    isFound = false;
                }
            }

            return isFound;
        }



    }
}
