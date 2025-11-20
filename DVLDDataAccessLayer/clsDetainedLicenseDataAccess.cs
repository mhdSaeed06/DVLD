using DVLDDataAccessLayer;
using System;
using System.Data;
using System.Data.SqlClient;

public static class clsDetainedLicenseDataAccess
{
    public static bool GetDetainedLicenseInfoByID(ref int DetainID,
         int LicenseID, ref DateTime DetainDate, ref decimal FineFees,
        ref int CreatedByUserID, ref bool IsReleased, ref DateTime ReleaseDate,
        ref int ReleasedByUserID, ref int ReleaseApplicationID,bool Released = false)
    {
        bool isFound = false;

        using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
        {
            string query = @"SELECT * FROM DetainedLicenses WHERE LicenseID = @LicenseID and IsReleased = @IsReleased";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LicenseID", LicenseID);
            command.Parameters.AddWithValue("@IsReleased", Released);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;
                    DetainID = (int)reader["DetainID"];
                    DetainDate = (DateTime)reader["DetainDate"];
                    FineFees = Convert.ToDecimal(reader["FineFees"]);
                    CreatedByUserID = (int)reader["CreatedByUserID"];
                    IsReleased = (bool)reader["IsReleased"];

                    ReleaseDate = reader["ReleaseDate"] != DBNull.Value ? (DateTime)reader["ReleaseDate"] : DateTime.MinValue;
                    ReleasedByUserID = reader["ReleasedByUserID"] != DBNull.Value ? (int)reader["ReleasedByUserID"] : -1;
                    ReleaseApplicationID = reader["ReleaseApplicationID"] != DBNull.Value ? (int)reader["ReleaseApplicationID"] : -1;
                }
                reader.Close();
            }
            catch { isFound = false; }
        }
        return isFound;
    }

    public static int AddNewDetainedLicense(int LicenseID, DateTime DetainDate, decimal FineFees,
        int CreatedByUserID, bool IsReleased, DateTime ReleaseDate,
        int ReleasedByUserID, int ReleaseApplicationID)
    {
        int newID = -1;

        using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
        {
            string query = @"
                INSERT INTO DetainedLicenses
                (LicenseID, DetainDate, FineFees, CreatedByUserID, IsReleased, ReleaseDate, ReleasedByUserID, ReleaseApplicationID)
                VALUES (@LicenseID, @DetainDate, @FineFees, @CreatedByUserID, @IsReleased, @ReleaseDate, @ReleasedByUserID, @ReleaseApplicationID);
                SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseID", LicenseID);
            command.Parameters.AddWithValue("@DetainDate", DetainDate);
            command.Parameters.AddWithValue("@FineFees", FineFees);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            command.Parameters.AddWithValue("@IsReleased", IsReleased);
            command.Parameters.AddWithValue("@ReleaseDate", ReleaseDate == DateTime.MinValue ? (object)DBNull.Value : ReleaseDate);
            command.Parameters.AddWithValue("@ReleasedByUserID", ReleasedByUserID == -1 ? (object)DBNull.Value : ReleasedByUserID);
            command.Parameters.AddWithValue("@ReleaseApplicationID", ReleaseApplicationID == -1 ? (object)DBNull.Value : ReleaseApplicationID);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null)
                    newID = Convert.ToInt32(result);
            }
            catch { newID = -1; }
        }

        return newID;
    }

    public static bool UpdateDetainedLicense(int DetainID, int LicenseID, DateTime DetainDate,
        decimal FineFees, int CreatedByUserID, bool IsReleased, DateTime ReleaseDate,
        int ReleasedByUserID, int ReleaseApplicationID)
    {
        int rowsAffected = 0;

        using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
        {
            string query = @"
                UPDATE DetainedLicenses
                SET LicenseID=@LicenseID, DetainDate=@DetainDate, FineFees=@FineFees,
                    CreatedByUserID=@CreatedByUserID, IsReleased=@IsReleased,
                    ReleaseDate=@ReleaseDate, ReleasedByUserID=@ReleasedByUserID,
                    ReleaseApplicationID=@ReleaseApplicationID
                WHERE DetainID=@DetainID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@DetainID", DetainID);
            command.Parameters.AddWithValue("@LicenseID", LicenseID);
            command.Parameters.AddWithValue("@DetainDate", DetainDate);
            command.Parameters.AddWithValue("@FineFees", FineFees);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            command.Parameters.AddWithValue("@IsReleased", IsReleased);
            command.Parameters.AddWithValue("@ReleaseDate", ReleaseDate == DateTime.MinValue ? (object)DBNull.Value : ReleaseDate);
            command.Parameters.AddWithValue("@ReleasedByUserID", ReleasedByUserID == -1 ? (object)DBNull.Value : ReleasedByUserID);
            command.Parameters.AddWithValue("@ReleaseApplicationID", ReleaseApplicationID == -1 ? (object)DBNull.Value : ReleaseApplicationID);

            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
            catch { rowsAffected = 0; }
        }

        return rowsAffected > 0;
    }

    public static bool DeleteDetainedLicense(int DetainID)
    {
        int rowsAffected = 0;

        using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
        {
            string query = "DELETE FROM DetainedLicenses WHERE DetainID = @DetainID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@DetainID", DetainID);

            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
            catch { rowsAffected = 0; }
        }
        return rowsAffected > 0;
    }

    public static DataTable GetAllDetainedLicenses()
    {
        DataTable dt = new DataTable();

        using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
        {
            string query = "Select * From DetainedLicenses_View";
            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                dt.Load(reader);
                reader.Close();
            }
            catch { dt = null; }
        }
        return dt;
    }
}
