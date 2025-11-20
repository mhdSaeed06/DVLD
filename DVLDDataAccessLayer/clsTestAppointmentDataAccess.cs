using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLDDataAccessLayer
{
    public class clsTestAppointmentDataAccess
    {
        public static int AddNewTestAppointment(int TestTypeID, int LocalDrivingLicenseApplicationID,
            int CreatedByUserID, DateTime AppointmentDate, decimal PaidFees, bool IsLocked, int RetakeTestApplicationID)
        {
            int TestAppointmentID = -1;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"
INSERT INTO TestAppointments
(TestTypeID, LocalDrivingLicenseApplicationID, CreatedByUserID, AppointmentDate, PaidFees, IsLocked, RetakeTestApplicationID)
VALUES (@TestTypeID, @LocalDrivingLicenseApplicationID, @CreatedByUserID, @AppointmentDate, @PaidFees, @IsLocked, @RetakeTestApplicationID);
SELECT SCOPE_IDENTITY();";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
                command.Parameters.AddWithValue("@PaidFees", PaidFees);
                command.Parameters.AddWithValue("@IsLocked", IsLocked);
                command.Parameters.AddWithValue("@RetakeTestApplicationID",
                    (RetakeTestApplicationID == -1) ? DBNull.Value : (object)RetakeTestApplicationID );

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        TestAppointmentID = insertedID;
                }
                catch { }
            }

            return TestAppointmentID;
        }

        public static bool GetTestAppointmentInfoById(int TestAppointmentID,
            ref int TestTypeID, ref int LocalDrivingLicenseApplicationID, ref int CreatedByUserID,
            ref DateTime AppointmentDate, ref decimal PaidFees, ref bool IsLocked, ref int RetakeTestApplicationID)
        {
            bool isFound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "SELECT * FROM TestAppointments WHERE TestAppointmentID = @TestAppointmentID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        isFound = true;

                        TestTypeID = (int)reader["TestTypeID"];
                        LocalDrivingLicenseApplicationID = (int)reader["LocalDrivingLicenseApplicationID"];
                        CreatedByUserID = (int)reader["CreatedByUserID"];
                        AppointmentDate = (DateTime)reader["AppointmentDate"];
                        PaidFees = (decimal)reader["PaidFees"];
                        IsLocked = (bool)reader["IsLocked"];

                        if (reader["RetakeTestApplicationID"] != DBNull.Value)
                        {
                            RetakeTestApplicationID = (int)reader["RetakeTestApplicationID"];
                        }
                        else
                        {
                            RetakeTestApplicationID = -1;
                        }
                    }

                    reader.Close();
                }
                catch { }
            }

            return isFound;
        }

        public static bool UpdateTestAppointment(int TestAppointmentID, int TestTypeID,
            int LocalDrivingLicenseApplicationID, int CreatedByUserID, DateTime AppointmentDate,
            decimal PaidFees, bool IsLocked, int RetakeTestApplicationID)
        {
            int rowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"
UPDATE TestAppointments
SET TestTypeID = @TestTypeID,
    LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID,
    CreatedByUserID = @CreatedByUserID,
    AppointmentDate = @AppointmentDate,
    PaidFees = @PaidFees,
    IsLocked = @IsLocked,
    RetakeTestApplicationID = @RetakeTestApplicationID
WHERE TestAppointmentID = @TestAppointmentID";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
                command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
                command.Parameters.AddWithValue("@PaidFees", PaidFees);
                command.Parameters.AddWithValue("@IsLocked", IsLocked);
                command.Parameters.AddWithValue("@RetakeTestApplicationID",
                    (RetakeTestApplicationID == -1) ? DBNull.Value : (object)RetakeTestApplicationID);

                try
                {
                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                }
                catch { }
            }

            return (rowsAffected > 0);
        }

        public static bool DeleteTestAppointment(int TestAppointmentID)
        {
            int rowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "DELETE FROM TestAppointments WHERE TestAppointmentID = @TestAppointmentID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

                try
                {
                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                }
                catch { }
            }

            return (rowsAffected > 0);
        }

        public static DataTable GetAllTestAppointments()
        {
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "SELECT * FROM TestAppointments";
                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    dt.Load(reader);
                }
                catch { }
            }

            return dt;
        }



        public static DataTable GetTestAppointmentsById(int LocalDrivingLicenseApplicationID,int TestTypeID)
        {
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"Select TA.TestAppointmentID As AppointmentId ,
                        TA.AppointmentDate,TA.PaidFees,TA.IsLocked From TestAppointments TA 
                                    Where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID and TestTypeID = @TestTypeID";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    dt.Load(reader);
                }
                catch { }
            }

            return dt;
        }
    }
}
