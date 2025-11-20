using DVLDDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBusinessLayer
{
    public class clsApplicationType
    {
        public int ApplicationTypeId { get; private set; }

        public string ApplicationTitle { get; set; }

        public decimal Fees { get; set; }

        public enum enApplicationType{ NewLocalDrivingLicense = 1 , RenewDrivingLicense = 2 , ReplacementForLost = 3 , ReplacementForDamage = 4 , Release = 5 , NewInternatinal = 6 , RetakeTest = 7  }

        private clsApplicationType(int applicationTypeId,string Title,decimal Fees)
        {
            this.ApplicationTypeId = applicationTypeId;
            this.ApplicationTitle = Title;
            this.Fees = Fees;
        }
        public static DataTable GetAllApplicationTypes()
        {
            return clsApplicationTypesDataAccess.GetAllApplicationTypes();
        }

        public static bool UpdateApplicationType(int ApplicationsTypeId,decimal Fees,string Title)
        {
            return clsApplicationTypesDataAccess.UpdateApplicationTypes(ApplicationsTypeId, Title, Fees);
        }

        public static clsApplicationType Find(int ApplicationsTypeId)
        {
            string Title = "";
            decimal Fees = 0;

            if (clsApplicationTypesDataAccess.GetApplicationTypeById(ApplicationsTypeId, ref Title, ref Fees))
            {
                return new clsApplicationType(ApplicationsTypeId,Title,Fees);
            }
            else
                return null;

        }


        public static double GetApplicationFeesById(int ApplicationsTypeId)
        {
            return clsApplicationTypesDataAccess.GetApplicationFeesById(ApplicationsTypeId);
        }

    }
}
