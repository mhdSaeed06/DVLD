using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLDDataAccessLayer
{
    public class clsLicenseDataAccess
    {
        public static DataTable GetAllLicenses()
        {
            DataTable dtLicenses = new DataTable();

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "SELECT * FROM Licenses";

                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            dtLicenses.Load(reader);
                    }
                }
                catch (Exception ex)
                {
                    // log exception
                }
            }

            return dtLicenses;
        }


        public static DataTable GetAllLicenses(int DriverId)
        {
            DataTable dtLicenses = new DataTable();

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"
SELECT        Licenses.LicenseID, Licenses.ApplicationID, LicenseClasses.ClassName, Licenses.IssueDate, Licenses.ExpirationDate, Licenses.IsActive
FROM            Licenses INNER JOIN
                         LicenseClasses ON Licenses.LicenseClassId = LicenseClasses.LicenseClassID Where DriverId = @DriverId";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@DriverId", DriverId);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            dtLicenses.Load(reader);
                    }
                }
                catch (Exception ex)
                {
                    // log exception
                }
            }

            return dtLicenses;
        }

        public static int AddNewLicense(int ApplicationId, int DriverId, int LicenseClassId,
            DateTime IssueDate, DateTime ExpirationDate, string Notes,
            decimal PaidFees, bool IsActive, int IssueReason, int CreatedByUserId)
        {
            int LicenseId = -1;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"
INSERT INTO [dbo].[Licenses]
       ([ApplicationId]
       ,[DriverId]
       ,[LicenseClassId]
       ,[IssueDate]
       ,[ExpirationDate]
       ,[Notes]
       ,[PaidFees]
       ,[IsActive]
       ,[IssueReason]
       ,[CreatedByUserId])
 VALUES
       (@ApplicationId
       ,@DriverId
       ,@LicenseClassId
       ,@IssueDate
       ,@ExpirationDate
       ,@Notes
       ,@PaidFees
       ,@IsActive
       ,@IssueReason
       ,@CreatedByUserId);
SELECT SCOPE_IDENTITY();";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@ApplicationId", ApplicationId);
                command.Parameters.AddWithValue("@DriverId", DriverId);
                command.Parameters.AddWithValue("@LicenseClassId", LicenseClassId);
                command.Parameters.AddWithValue("@IssueDate", IssueDate);
                command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
                if (string.IsNullOrEmpty(Notes))
                    command.Parameters.AddWithValue("@Notes", DBNull.Value);
                else
                    command.Parameters.AddWithValue("@Notes", Notes); command.Parameters.AddWithValue("@PaidFees", PaidFees);
                command.Parameters.AddWithValue("@IsActive", IsActive);
                command.Parameters.AddWithValue("@IssueReason", IssueReason);
                command.Parameters.AddWithValue("@CreatedByUserId", CreatedByUserId);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int insertedId))
                        LicenseId = insertedId;
                }
                catch (Exception ex)
                {
                    // log exception
                }
            }

            return LicenseId;
        }


        public static bool GetLicenseById(int LicenseId, ref int ApplicationId, ref int DriverId,
            ref int LicenseClassId, ref DateTime IssueDate, ref DateTime ExpirationDate,
            ref string Notes, ref decimal PaidFees, ref bool IsActive,
            ref int IssueReason, ref int CreatedByUserId)
        {
            bool isFound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "SELECT * FROM Licenses WHERE LicenseId = @LicenseId";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@LicenseId", LicenseId);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    using (reader)
                    {
                        if (reader.Read())
                        {
                            isFound = true;

                            ApplicationId = (int)reader["ApplicationId"];
                            DriverId = (int)reader["DriverId"];
                            LicenseClassId = (int)reader["LicenseClassId"];
                            IssueDate = (DateTime)reader["IssueDate"];
                            ExpirationDate = (DateTime)reader["ExpirationDate"];
                            Notes = reader["Notes"] == DBNull.Value ? null : (string)reader["Notes"];
                            PaidFees = (decimal)reader["PaidFees"];
                            IsActive = (bool)reader["IsActive"];
                            IssueReason = (byte)reader["IssueReason"];
                            CreatedByUserId = (int)reader["CreatedByUserId"];
                        }
                    }
                }
                catch (Exception ex)
                {
                    // log exception
                }
            }

            return isFound;
        }


        public static bool GetLicenseByApplicationID(ref int LicenseId, int ApplicationId, ref int DriverId,
           ref int LicenseClassId, ref DateTime IssueDate, ref DateTime ExpirationDate,
           ref string Notes, ref decimal PaidFees, ref bool IsActive,
           ref int IssueReason, ref int CreatedByUserId)
        {
            bool isFound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "SELECT * FROM Licenses WHERE ApplicationId = @ApplicationId";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ApplicationId", ApplicationId);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    using (reader)
                    {
                        if (reader.Read())
                        {
                            isFound = true;

                            LicenseId = (int)reader["LicenseId"];
                            DriverId = (int)reader["DriverId"];
                            LicenseClassId = (int)reader["LicenseClassId"];
                            IssueDate = (DateTime)reader["IssueDate"];
                            ExpirationDate = (DateTime)reader["ExpirationDate"];
                            Notes = reader["Notes"] == DBNull.Value ? null : (string)reader["Notes"];
                            PaidFees = (decimal)reader["PaidFees"];
                            IsActive = (bool)reader["IsActive"];
                            IssueReason = (byte)reader["IssueReason"];
                            CreatedByUserId = (int)reader["CreatedByUserId"];
                        }
                    }
                }
                catch (Exception ex)
                {
                    // log exception
                }
            }

            return isFound;
        }


        public static bool UpdateLicense(int LicenseId, int ApplicationId, int DriverId,
            int LicenseClassId, DateTime IssueDate, DateTime ExpirationDate,
            string Notes, decimal PaidFees, bool IsActive,
            int IssueReason, int CreatedByUserId)
        {
            int rowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"
UPDATE [dbo].[Licenses]
   SET [ApplicationId] = @ApplicationId
      ,[DriverId] = @DriverId
      ,[LicenseClassId] = @LicenseClassId
      ,[IssueDate] = @IssueDate
      ,[ExpirationDate] = @ExpirationDate
      ,[Notes] = @Notes
      ,[PaidFees] = @PaidFees
      ,[IsActive] = @IsActive
      ,[IssueReason] = @IssueReason
      ,[CreatedByUserId] = @CreatedByUserId
 WHERE LicenseId = @LicenseId;";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@LicenseId", LicenseId);
                command.Parameters.AddWithValue("@ApplicationId", ApplicationId);
                command.Parameters.AddWithValue("@DriverId", DriverId);
                command.Parameters.AddWithValue("@LicenseClassId", LicenseClassId);
                command.Parameters.AddWithValue("@IssueDate", IssueDate);
                command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
                if (string.IsNullOrEmpty(Notes))
                    command.Parameters.AddWithValue("@Notes", DBNull.Value);
                else
                    command.Parameters.AddWithValue("@Notes", Notes);
                command.Parameters.AddWithValue("@PaidFees", PaidFees);
                command.Parameters.AddWithValue("@IsActive", IsActive);
                command.Parameters.AddWithValue("@IssueReason", IssueReason);
                command.Parameters.AddWithValue("@CreatedByUserId", CreatedByUserId);

                try
                {
                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

            return (rowsAffected > 0);
        }


        public static bool DeleteLicense(int LicenseId)
        {
            int rowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "DELETE FROM Licenses WHERE LicenseId = @LicenseId";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@LicenseId", LicenseId);

                try
                {
                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

            return (rowsAffected > 0);
        }


        public static bool IsLicenseExist(int LicenseId)
        {
            bool isFound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "SELECT Found=1 FROM Licenses WHERE LicenseId = @LicenseId";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@LicenseId", LicenseId);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    isFound = reader.HasRows;
                    reader.Close();
                }
                catch (Exception ex)
                {
                    isFound = false;
                }
            }

            return isFound;
        }


        public static bool IsDriverHasLicenseInClass(int driverId, int licenseClassId)
        {
            bool exists = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"SELECT TOP 1 1 
                         FROM Licenses
                         WHERE DriverId = @DriverId 
                           AND LicenseClassId = @LicenseClassId
                           AND IsActive = 1";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@DriverId", driverId);
                command.Parameters.AddWithValue("@LicenseClassId", licenseClassId);

                connection.Open();

                object result = command.ExecuteScalar();
                exists = (result != null);
            }

            return exists;
        }

    }
}
