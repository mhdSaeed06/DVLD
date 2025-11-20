using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDDataAccessLayer
{
    public class clsCountryDataAccess
    {

        public static DataTable GetAllCountries()

        {

            DataTable dtCountries = new DataTable();



            SqlConnection sqlConnection = new SqlConnection(clsDataAccessSettings.ConnectionString);



            string sqlQuery = "Select * From Countries";



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

                            dtCountries.Load(reader);

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

                return dtCountries;





            }

        }

    }
}
