using DVLDDataAccessLayer;
using System;
using System.Data;

namespace DVLDBusinessLayer
{
    public class clsDriver
    {
        public int DriverID { get; private set; }
        public int PersonID { get; set; }
        public int CreatedUserID { get; set; }
        public DateTime CreatedDate { get; set; }

        public clsDriver()
        {
            this.DriverID = -1;
            this.PersonID = -1;
            this.CreatedUserID = -1;
            this.CreatedDate = DateTime.Now;
        }

        private clsDriver(int driverID, int personID, int createdUserID, DateTime createdDate)
        {
            this.DriverID = driverID;
            this.PersonID = personID;
            this.CreatedUserID = createdUserID;
            this.CreatedDate = createdDate;
        }

        public static clsDriver Find(int driverID)
        {
            int personID = -1, createdUserID = -1;
            DateTime createdDate = DateTime.Now;

            if (clsDriverDataAccess.GetDriverInfoByID(driverID, ref personID, ref createdUserID, ref createdDate))
            {
                return new clsDriver(driverID, personID, createdUserID, createdDate);
            }
            else
                return null;
        }

        public static clsDriver FindByPersonID(int PersonID)
        {
            int DriverID = -1, createdUserID = -1;
            DateTime createdDate = DateTime.Now;

            if (clsDriverDataAccess.GetDriverInfoByPersonIdID(ref DriverID,PersonID, ref createdUserID, ref createdDate))
            {
                return new clsDriver(DriverID, PersonID, createdUserID, createdDate);
            }
            else
                return null;
        }

        public bool Save()
        {
            if (this.DriverID == -1)
            {
                this.DriverID = clsDriverDataAccess.AddNewDriver(this.PersonID, this.CreatedUserID, this.CreatedDate);
                return (this.DriverID != -1);
            }
            else
            {
                return clsDriverDataAccess.UpdateDriver(this.DriverID, this.PersonID, this.CreatedUserID, this.CreatedDate);
            }
        }

        public static bool Delete(int driverID)
        {
            return clsDriverDataAccess.DeleteDriver(driverID);
        }

        public static DataTable GetAllDrivers()
        {
            return clsDriverDataAccess.GetAllDrivers();
        }
    }
}
