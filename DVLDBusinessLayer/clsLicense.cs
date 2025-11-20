using DVLDDataAccessLayer;
using System;
using System.Data;

namespace DVLDBusinessLayer
{
    public class clsLicense
    {
        public int LicenseId { get; private set; }
        public int ApplicationId { get; set; }
        public int DriverId { get; set; }
        public int LicenseClassId { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Notes { get; set; }
        public decimal PaidFees { get; set; }
        public bool IsActive { get; set; }

        public enum enIssueReason : byte
        {
            FirstTime = 1,
            Renew = 2,
            ReplacementForDamaged = 3,
            ReplacementForLost = 4
        };

        public enIssueReason IssueReason { get; set; }
        public int CreatedByUserId { get; set; }


        public enum enMode { AddNew = 0, Update = 1 }
        public enMode Mode = enMode.AddNew;


        public clsLicense()
        {
            this.LicenseId = -1;
            this.ApplicationId = -1;
            this.DriverId = -1;
            this.LicenseClassId = -1;
            this.IssueDate = DateTime.Now;
            this.ExpirationDate = DateTime.Now;
            this.Notes = string.Empty;
            this.PaidFees = 0;
            this.IsActive = true;
            this.IssueReason = enIssueReason.FirstTime;
            this.CreatedByUserId = -1;

            Mode = enMode.AddNew;
        }

        private clsLicense(int licenseId, int applicationId, int driverId, int licenseClassId,
            DateTime issueDate, DateTime expirationDate, string notes, decimal paidFees,
            bool isActive, enIssueReason issueReason, int createdByUserId)
        {
            this.LicenseId = licenseId;
            this.ApplicationId = applicationId;
            this.DriverId = driverId;
            this.LicenseClassId = licenseClassId;
            this.IssueDate = issueDate;
            this.ExpirationDate = expirationDate;
            this.Notes = notes;
            this.PaidFees = paidFees;
            this.IsActive = isActive;
            this.IssueReason = issueReason;
            this.CreatedByUserId = createdByUserId;

            Mode = enMode.Update;
        }

        private bool _AddNew()
        {
            this.LicenseId = clsLicenseDataAccess.AddNewLicense(
                this.ApplicationId, this.DriverId, this.LicenseClassId,
                this.IssueDate, this.ExpirationDate, this.Notes,
                this.PaidFees, this.IsActive, (byte)this.IssueReason, this.CreatedByUserId);

            return (this.LicenseId != -1);
        }

        private bool _Update()
        {
            return clsLicenseDataAccess.UpdateLicense(
                this.LicenseId, this.ApplicationId, this.DriverId, this.LicenseClassId,
                this.IssueDate, this.ExpirationDate, this.Notes, this.PaidFees,
                this.IsActive, (byte)this.IssueReason, this.CreatedByUserId);
        }

        public static clsLicense Find(int LicenseId)
        {
            int ApplicationId = -1, DriverId = -1, LicenseClassId = -1, CreatedByUserId = -1;
            DateTime IssueDate = DateTime.Now, ExpirationDate = DateTime.Now;
            string Notes = string.Empty;
            decimal PaidFees = 0;
            bool IsActive = false;
            int IssueReason = 1;

            if (clsLicenseDataAccess.GetLicenseById(LicenseId, ref ApplicationId, ref DriverId,
                ref LicenseClassId, ref IssueDate, ref ExpirationDate, ref Notes,
                ref PaidFees, ref IsActive, ref IssueReason, ref CreatedByUserId))
            {
                return new clsLicense(LicenseId, ApplicationId, DriverId, LicenseClassId,
                    IssueDate, ExpirationDate, Notes, PaidFees,
                    IsActive, (enIssueReason)IssueReason, CreatedByUserId);
            }
            else
                return null;
        }

        public static clsLicense FindByApplicationId(int ApplicationID)
        {
            int LicenseID = -1, DriverId = -1, LicenseClassId = -1, CreatedByUserId = -1;
            DateTime IssueDate = DateTime.Now, ExpirationDate = DateTime.Now;
            string Notes = string.Empty;
            decimal PaidFees = 0;
            bool IsActive = false;
            int IssueReason = 1;

            if (clsLicenseDataAccess.GetLicenseByApplicationID(ref LicenseID, ApplicationID, ref DriverId,
                ref LicenseClassId, ref IssueDate, ref ExpirationDate, ref Notes,
                ref PaidFees, ref IsActive, ref IssueReason, ref CreatedByUserId))
            {
                return new clsLicense(LicenseID, ApplicationID, DriverId, LicenseClassId,
                    IssueDate, ExpirationDate, Notes, PaidFees,
                    IsActive, (enIssueReason)IssueReason, CreatedByUserId);
            }
            else
                return null;
        }


        public static bool Delete(int LicenseId)
        {
            return clsLicenseDataAccess.DeleteLicense(LicenseId);
        }

        public static bool IsLicenseExist(int LicenseId)
        {
            return clsLicenseDataAccess.IsLicenseExist(LicenseId);
        }

        public static DataTable GetAllLicenses()
        {
            return clsLicenseDataAccess.GetAllLicenses();
        }

        public static DataTable GetAllLicenses(int DriverId)
        {
            return clsLicenseDataAccess.GetAllLicenses(DriverId);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNew())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;

                case enMode.Update:
                    return _Update();

                default:
                    return false;
            }
        }



        public static bool IsDriverHasLicenseInClass(int driverId, int licenseClassId)
        {
            return clsLicenseDataAccess.IsDriverHasLicenseInClass(driverId, licenseClassId);
        }
    }
}
