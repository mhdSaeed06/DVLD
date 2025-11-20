using System;
using System.Data;
using DVLDDataAccessLayer;
public class clsDetainedLicense
{
    public int DetainID { get; set; }
    public int LicenseID { get; set; }
    public DateTime DetainDate { get; set; }
    public decimal FineFees { get; set; }
    public int CreatedByUserID { get; set; }
    public bool IsReleased { get; set; }
    public DateTime ReleaseDate { get; set; }
    public int ReleasedByUserID { get; set; }
    public int ReleaseApplicationID { get; set; }

    public clsDetainedLicense()
    {
        DetainID = -1;
        LicenseID = -1;
        DetainDate = DateTime.MinValue;
        FineFees = -1;
        CreatedByUserID = -1;
        IsReleased = false;
        ReleaseDate = DateTime.MinValue;
        ReleasedByUserID = -1;
        ReleaseApplicationID = -1;
    }

    private clsDetainedLicense(int detainID, int licenseID, DateTime detainDate, decimal fineFees,
        int createdByUserID, bool isReleased, DateTime releaseDate,
        int releasedByUserID, int releaseApplicationID)
    {
        DetainID = detainID;
        LicenseID = licenseID;
        DetainDate = detainDate;
        FineFees = fineFees;
        CreatedByUserID = createdByUserID;
        IsReleased = isReleased;
        ReleaseDate = releaseDate;
        ReleasedByUserID = releasedByUserID;
        ReleaseApplicationID = releaseApplicationID;
    }

    public static clsDetainedLicense Find(int LicenseId,bool IfReleased = false)
    {
        int DetainID = -1;
        DateTime DetainDate = DateTime.MinValue;
        decimal FineFees = -1;
        int CreatedByUserID = -1;
        bool IsReleased = false;
        DateTime ReleaseDate = DateTime.MinValue;
        int ReleasedByUserID = -1;
        int ReleaseApplicationID = -1;

        bool isFound = clsDetainedLicenseDataAccess.GetDetainedLicenseInfoByID(ref DetainID,
            LicenseId, ref DetainDate, ref FineFees, ref CreatedByUserID,
            ref IsReleased, ref ReleaseDate, ref ReleasedByUserID, ref ReleaseApplicationID,IfReleased);

        if (isFound)
        {
            return new clsDetainedLicense(DetainID, LicenseId, DetainDate, FineFees,
                CreatedByUserID, IsReleased, ReleaseDate, ReleasedByUserID, ReleaseApplicationID);
        }
        else
            return null;
    }

    public bool Save()
    {
        if (DetainID == -1) // Insert
        {
            int newID = clsDetainedLicenseDataAccess.AddNewDetainedLicense(
                LicenseID, DetainDate, FineFees, CreatedByUserID, IsReleased,
                ReleaseDate, ReleasedByUserID, ReleaseApplicationID);

            if (newID != -1)
            {
                this.DetainID = newID;
                return true;
            }
            return false;
        }
        else // Update
        {
            return clsDetainedLicenseDataAccess.UpdateDetainedLicense(
                DetainID, LicenseID, DetainDate, FineFees, CreatedByUserID, IsReleased,
                ReleaseDate, ReleasedByUserID, ReleaseApplicationID);
        }
    }

    public static bool Delete(int DetainID)
    {
        return clsDetainedLicenseDataAccess.DeleteDetainedLicense(DetainID);
    }

    public static DataTable GetAll()
    {
        return clsDetainedLicenseDataAccess.GetAllDetainedLicenses();
    }
}
