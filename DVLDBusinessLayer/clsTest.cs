using DVLDDataAccessLayer;
using System;
using System.Data;

namespace DVLDBusinessLayer
{
    public class clsTest
    {
        public int TestID { get; private set; }
        public int TestAppointmentID { get; set; }
        public int CreatedByUserID { get; set; }
        public bool TestResult { get; set; }
        public string Notes { get; set; }

        public enum enMode { AddNew = 0, Update = 1 }
        public enMode Mode = enMode.AddNew;

        public enum enTestType : int { Vision = 1 ,Written = 2 , Practical = 3 }


        public clsTest()
        {
            this.TestID = -1;
            this.TestAppointmentID = -1;
            this.CreatedByUserID = -1;
            this.TestResult = false;
            this.Notes = null;

            Mode = enMode.AddNew;
        }

        private clsTest(int TestID, int TestAppointmentID, int CreatedByUserID, bool TestResult, string Notes)
        {
            this.TestID = TestID;
            this.TestAppointmentID = TestAppointmentID;
            this.CreatedByUserID = CreatedByUserID;
            this.TestResult = TestResult;
            this.Notes = Notes;

            Mode = enMode.Update;
        }

        private bool _AddNew()
        {
            this.TestID = clsTestDataAccess.AddNewTest(this.TestAppointmentID, this.CreatedByUserID, this.TestResult, this.Notes);
            return (this.TestID != -1);
        }

        private bool _Update()
        {
            return clsTestDataAccess.UpdateTest(this.TestID, this.TestAppointmentID, this.CreatedByUserID, this.TestResult, this.Notes);
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
                    return false;

                case enMode.Update:
                    return _Update();
            }
            return false;
        }

        public static clsTest Find(int TestID)
        {
            int TestAppointmentID = -1, CreatedByUserID = -1;
            bool TestResult = false;
            string Notes = null;

            if (clsTestDataAccess.GetTestInfoById(TestID, ref TestAppointmentID, ref CreatedByUserID, ref TestResult, ref Notes))
            {
                return new clsTest(TestID, TestAppointmentID, CreatedByUserID, TestResult, Notes);
            }
            return null;
        }

        public static bool Delete(int TestID)
        {
            return clsTestDataAccess.DeleteTest(TestID);
        }

        public static DataTable GetAll()
        {
            return clsTestDataAccess.GetAllTests();
        }

        public static int GetPassedTestsCount(int LocalDrivingLicenseApplicationID)
        {
            return clsTestDataAccess.GetPassedTestsCount(LocalDrivingLicenseApplicationID);
        }

        public static bool IsTestTaken(int appointmentId)
        {
            return clsTestDataAccess.IsTestExistByAppointmentId(appointmentId);
        }

    }
}
