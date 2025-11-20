using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLDDataAccessLayer
{
    public class clsTestDataAccess
    {
        public static int AddNewTest(int TestAppointmentID, int CreatedByUserID, bool TestResult, string Notes)
        {
            int TestID = -1;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"
INSERT INTO Tests (TestAppointmentID, CreatedByUserID, TestResult, Notes)
VALUES (@TestAppointmentID, @CreatedByUserID, @TestResult, @Notes);
SELECT SCOPE_IDENTITY();";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
                command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                command.Parameters.AddWithValue("@TestResult", TestResult);
                if (string.IsNullOrEmpty(Notes))
                    command.Parameters.AddWithValue("@Notes", DBNull.Value);
                else
                    command.Parameters.AddWithValue("@Notes", Notes);
                
                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        TestID = insertedID;
                }
                catch { }
            }

            return TestID;
        }

        public static bool GetTestInfoById(int TestID,
            ref int TestAppointmentID, ref int CreatedByUserID, ref bool TestResult, ref string Notes)
        {
            bool isFound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "SELECT * FROM Tests WHERE TestID = @TestID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@TestID", TestID);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        isFound = true;

                        TestAppointmentID = (int)reader["TestAppointmentID"];
                        CreatedByUserID = (int)reader["CreatedByUserID"];
                        TestResult = (bool)reader["TestResult"];
                        Notes = reader["Notes"] != DBNull.Value ? (string)reader["Notes"] : null;
                    }

                    reader.Close();
                }
                catch { }
            }

            return isFound;
        }

        public static bool UpdateTest(int TestID, int TestAppointmentID, int CreatedByUserID,
            bool TestResult, string Notes)
        {
            int rowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"
UPDATE Tests
SET TestAppointmentID = @TestAppointmentID,
    CreatedByUserID = @CreatedByUserID,
    TestResult = @TestResult,
    Notes = @Notes
WHERE TestID = @TestID";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@TestID", TestID);
                command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
                command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                command.Parameters.AddWithValue("@TestResult", TestResult);
                if (string.IsNullOrEmpty(Notes))
                    command.Parameters.AddWithValue("@Notes", DBNull.Value);
                else
                    command.Parameters.AddWithValue("@Notes", Notes);
                
                
                try
                {
                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                }
                catch { }
            }

            return (rowsAffected > 0);
        }

        public static bool DeleteTest(int TestID)
        {
            int rowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "DELETE FROM Tests WHERE TestID = @TestID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@TestID", TestID);

                try
                {
                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                }
                catch { }
            }

            return (rowsAffected > 0);
        }

        public static DataTable GetAllTests()
        {
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "SELECT * FROM Tests";
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

        // مثال: كم اختبار نجح فيه الشخص بناءً على الـ LocalDrivingLicenseAppID
        public static int GetPassedTestsCount(int LocalDrivingLicenseApplicationID)
        {
            int count = 0;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"
SELECT COUNT(*) FROM Tests T
JOIN TestAppointments TA ON T.TestAppointmentID = TA.TestAppointmentID
WHERE TA.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID AND T.TestResult = 1";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

                try
                {
                    connection.Open();
                    count = (int)command.ExecuteScalar();
                }
                catch { }
            }
                
            return count;
        }

        public static bool IsTestExistByAppointmentId(int appointmentId)
        {
            bool isExist = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "SELECT Found = 1 FROM Tests WHERE TestAppointmentID = @AppointmentID";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@AppointmentID", appointmentId);

                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null)
                    isExist = true;
            }

            return isExist;
        }

    }
}
