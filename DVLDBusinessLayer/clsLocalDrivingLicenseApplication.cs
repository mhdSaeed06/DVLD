using DVLDDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DVLDBusinessLayer.clsApplication;

namespace DVLDBusinessLayer
{
    public class clsLocalDrivingLicenseApplication
    {
        public int LocalDrivingLicenseAppID { get; set; }

        public int ApplicationId { get; set; }

        public int LicenseClassId { get; set; }



        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public clsLocalDrivingLicenseApplication()
        {
            this.LocalDrivingLicenseAppID = -1;
            this.ApplicationId = -1;
            this.LicenseClassId = -1;

            Mode = enMode.AddNew;
        }


        private clsLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationId,int ApplicationId,int LicenseClassId)
        {
            this.LocalDrivingLicenseAppID = LocalDrivingLicenseApplicationId;
            this.ApplicationId = ApplicationId;
            this.LicenseClassId = LicenseClassId;

            Mode = enMode.Update;
        }

        private bool _AddNewLocalDrivingLicenseApplication()
        {
            this.LocalDrivingLicenseAppID = clsLocalDrivingLicenseDataAccess.AddNewLocalDrivingLicenseApp(this.ApplicationId,this.LicenseClassId);


            return (this.LocalDrivingLicenseAppID != -1);

        }

        public static bool IsLicenseClassForThisPersonExist(int PersonId,int LicenseClassId)
        {
            return clsLocalDrivingLicenseDataAccess.IsLicenseClassForThisPersonExist(PersonId, LicenseClassId);
        }


        public static clsLocalDrivingLicenseApplication Find(int LocalDrivingLicenseAppID)
        {
            int ApplicationId = 0;
            int LicenseClassId = 0;

            if (clsLocalDrivingLicenseDataAccess.GetLocalDrivingLicenseApplicationsInfoById(LocalDrivingLicenseAppID,ref ApplicationId,ref LicenseClassId))
            {
                return new clsLocalDrivingLicenseApplication(LocalDrivingLicenseAppID,ApplicationId,LicenseClassId);
            }

            else
                return null;


        }

        public static clsLocalDrivingLicenseApplication FindByApplicationId(int ApplicationId)
        {
            int LocalDrivingLicenseID = 0;
            int LicenseClassId = 0;

            if (clsLocalDrivingLicenseDataAccess.GetLocalDrivingLicenseApplicationsInfoByApplicationId(ref LocalDrivingLicenseID,ApplicationId,ref LicenseClassId))
            {
                return new clsLocalDrivingLicenseApplication(LocalDrivingLicenseID, LocalDrivingLicenseID, LicenseClassId);
            }

            else
                return null;


        }

        private bool _UpdateInfo()
        {

            return clsLocalDrivingLicenseDataAccess.UpdateLocalDrivingLicenseApps(this.LocalDrivingLicenseAppID,
                this.ApplicationId,this.LicenseClassId);

        }


        public static DataTable GetAllLocalLicenseAppForList()
        {
            return clsLocalDrivingLicenseDataAccess.GetAllLocalDrivingLicenseList();
        }


        public static DataTable GetAllLocalLicenseAppInfo()
        {
            return clsLocalDrivingLicenseDataAccess.GetAllLocalDrivingLicenseInfo();
        }


        public static bool Delete(int LocalDrivingLicenseAppID)
        {
            return clsLocalDrivingLicenseDataAccess.Delete(LocalDrivingLicenseAppID);
        }


        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    {
                        if (_AddNewLocalDrivingLicenseApplication())
                        {
                            Mode = enMode.Update;
                            return true;
                        }
                        else
                            return false;

                    }
                case enMode.Update:
                    {
                        return this._UpdateInfo();
                    }
                default:
                    return false;
            }

        }


    }
}
