using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLDDataAccessLayer
{
    public class clsDriverDataAccess
    {
        public static bool GetDriverInfoByID(int driverID, ref int personID,
                                             ref int createdUserID,
                                             ref DateTime createdDate)
        {
            bool isFound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"SELECT PersonID, CreatedByUserID, CreatedDate
                                 FROM Drivers
                                 WHERE DriverID = @DriverID";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@DriverID", driverID);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    personID = (int)reader["PersonID"];
                    createdUserID = (int)reader["CreatedByUserID"];
                    createdDate = (DateTime)reader["CreatedDate"];
                }

                reader.Close();
            }

            return isFound;
        }


        public static bool GetDriverInfoByPersonIdID(ref int driverID,int personID,
                                            ref int createdUserID,
                                            ref DateTime createdDate)
        {
            bool isFound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"SELECT driverID, CreatedByUserID, CreatedDate
                                 FROM Drivers
                                 WHERE personID = @personID";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@personID", personID);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    driverID = (int)reader["driverID"];
                    createdUserID = (int)reader["CreatedByUserID"];
                    createdDate = (DateTime)reader["CreatedDate"];
                }

                reader.Close();
            }

            return isFound;
        }


        public static int AddNewDriver(int personID, int createdUserID, DateTime createdDate)
        {
            int driverID = -1;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"INSERT INTO Drivers (PersonID, CreatedByUserID, CreatedDate)
                                 VALUES (@PersonID, @CreatedByUserID, @CreatedDate);
                                 SELECT SCOPE_IDENTITY();";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@PersonID", personID);
                command.Parameters.AddWithValue("@CreatedByUserID", createdUserID);
                command.Parameters.AddWithValue("@CreatedDate", createdDate);

                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int id))
                    driverID = id;
            }

            return driverID;
        }

        public static bool UpdateDriver(int driverID, int personID, int createdUserID, DateTime createdDate)
        {
            int rowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"UPDATE Drivers
                                 SET PersonID = @PersonID,
                                     CreatedUserID = @CreatedByUserID,
                                     CreatedDate = @CreatedDate
                                 WHERE DriverID = @DriverID";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@DriverID", driverID);
                command.Parameters.AddWithValue("@PersonID", personID);
                command.Parameters.AddWithValue("@CreatedByUserID", createdUserID);
                command.Parameters.AddWithValue("@CreatedDate", createdDate);

                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }

            return (rowsAffected > 0);
        }

        public static bool DeleteDriver(int driverID)
        {
            int rowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"DELETE FROM Drivers WHERE DriverID = @DriverID";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@DriverID", driverID);

                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }

            return (rowsAffected > 0);
        }

        public static DataTable GetAllDrivers()
        {
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"
SELECT        Drivers.DriverID, People.NationalNo, People.FirstName + ' ' + People.SecondName + ' ' + People.ThirdName
 +' ' + People.LastName As FullName, Users.UserName
FROM            Drivers INNER JOIN
                         People ON Drivers.PersonID = People.PersonID INNER JOIN
                         Users ON Drivers.CreatedByUserID = Users.UserID 
						 ";
                SqlCommand command = new SqlCommand(query, connection);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);
            }

            return dt;
        }
    }
}
