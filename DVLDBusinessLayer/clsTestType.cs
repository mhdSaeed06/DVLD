using DVLDDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBusinessLayer
{
    public class clsTestType
    {
        public int ApplicationTypeId { get; private set; }

        public string ApplicationTitle { get; set; }
        public string ApplicationDescription { get; set; }

        public decimal Fees { get; set; }


        private clsTestType(int applicationTypeId, string Title,string Description, decimal Fees)
        {
            this.ApplicationTypeId = applicationTypeId;
            this.ApplicationTitle = Title;
            this.Fees = Fees;
            this.ApplicationDescription = Description;
        }

        public static DataTable GetAllTestsTypes()
        {
            return clsTestTypeDataAccees.GetAllTestTypes();
        }

        public static bool UpdateTestType(int TestTypeId, double Fees,string Description, string Title)
        {
            return clsTestTypeDataAccees.UpdateTestType(TestTypeId,Title,Description,Fees);
        }


        public static clsTestType Find(int TestTypeId)
        {
            string Title = "";
            string Description = "";
            decimal Fees = 0;

            if (clsTestTypeDataAccees.GetTestTypeInfoById(TestTypeId, ref Title,ref Description, ref Fees))
            {
                return new clsTestType(TestTypeId, Title,Description, Fees);
            }
            else
                return null;

        }
    }
}
