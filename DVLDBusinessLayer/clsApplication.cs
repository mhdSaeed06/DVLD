using DVLDDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DVLDBusinessLayer.clsApplication;
using static DVLDBusinessLayer.clsPerson;

namespace DVLDBusinessLayer
{
    public class clsApplication
    {
        public int ApplicationId { get; private set; }

        public int ApplicantPersonId { get; set; }

        public int CreatedByUserId { get; set; }

        public DateTime ApplicationDate { get; set; }

        public DateTime LastStatusDate { get; set; }

        public int ApplicationTypeId { get; set; }

        public enum enApplicationStatus { New = 1 ,Cancelled = 2 ,Completed = 3 };

        public enApplicationStatus ApplicationStatus { get; set; }

        public decimal PaidFees { get; set; }


        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public clsApplication()
        {
            this.ApplicationId = -1;
            this.ApplicantPersonId = -1;
            this.CreatedByUserId = -1;
            this.ApplicationDate = DateTime.Now;
            this.LastStatusDate = DateTime.Now;
            this.PaidFees = 0;
            this.ApplicationStatus = enApplicationStatus.New;
            this.ApplicationTypeId = -1;


            Mode = enMode.AddNew;
        }


        private clsApplication(int applicationId , int PersonId, int CreatedByUserId, DateTime ApplicationDate,
            DateTime LastStatusDate, int ApplicationTypeId, enApplicationStatus applicationStatus, decimal PaidFees)
        {
            this.ApplicationId = applicationId;
            this.ApplicantPersonId = PersonId;
            this.CreatedByUserId = CreatedByUserId;
            this.ApplicationDate = ApplicationDate;
            this.LastStatusDate = LastStatusDate;
            this.PaidFees = PaidFees;
            this.ApplicationStatus = applicationStatus;
            this.ApplicationTypeId = ApplicationTypeId;

            Mode = enMode.Update;
        }

        private bool _AddNewApplication()
        {

          
            this.ApplicationId = clsApplicationDataAccess.AddNewApplication(this.ApplicantPersonId , this.CreatedByUserId,this.ApplicationDate
                , this.LastStatusDate, ApplicationTypeId,(byte)this.ApplicationStatus,this.PaidFees);


            return (this.ApplicationId != -1);

        }

        public static clsApplication Find(int ApplicationId)
        {
            int PersonId = -1, CreatedByUserId = -1;
            DateTime ApplicationDate = DateTime.Now;
            DateTime LastStatusDate = DateTime.Now;
            int ApplicationTypeId = -1;
            byte applicationStatus = 0;
            decimal PaidFees = 0;

            if (clsApplicationDataAccess.GetApplicationInfoById(ApplicationId,ref PersonId,ref CreatedByUserId,ref ApplicationDate,
                ref LastStatusDate,ref ApplicationTypeId,ref applicationStatus,ref PaidFees))
            {
                return new clsApplication(ApplicationId,PersonId,CreatedByUserId,ApplicationDate,LastStatusDate,
                    ApplicationTypeId,(enApplicationStatus)applicationStatus,PaidFees);
            }

            else
                return null;


        }

        private bool _UpdateApplicationInfo()
        {

            return clsApplicationDataAccess.UpdateApplication(this.ApplicationId,this.ApplicantPersonId,this.CreatedByUserId,this.ApplicationDate,
                this.LastStatusDate,this.ApplicationTypeId,(byte)this.ApplicationStatus,this.PaidFees);

        }

        

        public static bool DeleteApplication(int ApplicationId)
        {
            return clsApplicationDataAccess.DeleteApplication(ApplicationId);
        }

        public static bool IsApplicationExist(int ApplicationId)
        {
            return clsApplicationDataAccess.IsApplicationExist(ApplicationId);
        }

        public static string GetApplicationTypeTitleByAppID(int ApplicationId)
        {
            return clsApplicationDataAccess.GetApplicationTypeTitleByAppID(ApplicationId);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    {
                        if (_AddNewApplication())
                        {
                            Mode = enMode.Update;
                            return true;
                        }
                        else
                            return false;

                    }
                case enMode.Update:
                    {
                        return this._UpdateApplicationInfo();
                    }
                default:
                    return false;
            }

        }


    }
}
