using DVLDDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBusinessLayer
{
    public class clsLicenseClass
    {
        public static DataTable GetAllLicenseClassInfo()
        {
            return clsLicenseClassDataAccess.GetAllApplicationTypes();
        }


        public int LicenseClassID { get; set; }
        public string ClassName { get; set; }
        public string ClassDescription { get; set; }
        public byte MinimumAllowedAge { get; set; }
        public byte DefaultValidityLength { get; set; }
        public decimal ClassFees { get; set; }


        private clsLicenseClass(int licenseClassID, string className, string classDescription,
                               byte minimumAllowedAge, byte defaultValidityLength, decimal classFees)
        {
            LicenseClassID = licenseClassID;
            ClassName = className;
            ClassDescription = classDescription;
            MinimumAllowedAge = minimumAllowedAge;
            DefaultValidityLength = defaultValidityLength;
            ClassFees = classFees;
        }

        public static clsLicenseClass Find(int licenseClassID)
        {
            int id = -1;
            string className = "";
            string classDescription = "";
            byte minAge = 0;
            byte validityLength = 0;
            decimal fees = 0;

            if (clsLicenseClassDataAccess.GetLicenseClassByID(
                licenseClassID, ref className, ref classDescription,
                ref minAge, ref validityLength, ref fees))
            {
                return new clsLicenseClass(licenseClassID, className, classDescription, minAge, validityLength, fees);
            }
            else
            {
                return null;
            }


          
        }


        
    }
}
