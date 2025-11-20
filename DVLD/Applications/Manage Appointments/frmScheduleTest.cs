using DVLD.Properties;
using DVLDBusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Applications
{
    public partial class frmScheduleTest : Form
    {
        public frmScheduleTest(clsLocalDrivingLicenseApplication LocalDrivingLicenseApplication
            , frmTestAppointments.enMode mode,clsApplication RetakeTestApplication = null)
        {
            InitializeComponent();
            _Mode = mode;
            _LocalDrivingLicenseApplication = LocalDrivingLicenseApplication;
            _RetakeTestApplication = RetakeTestApplication;
        }

        public frmScheduleTest( int AppointmentId)
        {
            InitializeComponent();
            _AppointmentId = AppointmentId;

        }

        clsTestAppointment _Appointment = null;

        int _AppointmentId;

        clsApplication _RetakeTestApplication;
        frmTestAppointments.enMode _Mode;

        private bool _PrepareIfEdit()
        {

            _Appointment = clsTestAppointment.Find(_AppointmentId);

            if (_Appointment == null)
            {
                return false;
            }

            _Mode = (frmTestAppointments.enMode)_Appointment.TestTypeID;

            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.Find(_Appointment.LocalDrivingLicenseApplicationID);

            _RetakeTestApplication = clsApplication.Find(_Appointment.RetakeTestApplicationID);

            _Application = clsApplication.Find(_LocalDrivingLicenseApplication.ApplicationId);

            lblId.Text = _Appointment.LocalDrivingLicenseApplicationID.ToString();

            return true;
        }

        private void _PrepareMode()
        {
            switch (_Mode)
            {
                case frmTestAppointments.enMode.VisionTestMode:
                    {
                        lblTitle.Text = "Vision Test Appointment";
                        pictureBox1.Image = Resources.Vision_512;
                        groupBox1.Text = "Vision Test";
                        break;
                    }
                case frmTestAppointments.enMode.WrittenTestMode:
                    {

                        lblTitle.Text = "Written Test Appointment";
                        pictureBox1.Image = Resources.Written_Test_512;
                        groupBox1.Text = "Written Test";
                        break;
                    }

                case frmTestAppointments.enMode.StreetTestMode:
                    {
                        lblTitle.Text = "Street Test Appointment";
                        pictureBox1.Image = Resources.Street_Test_32;
                        groupBox1.Text = "Practical Test";
                        break;
                    }
            }
        }

        clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;


        private void _PrepareRetakeTest()
        {
            decimal baseFees = Convert.ToDecimal(lblFees.Text); // رسوم الاختبار الأساسي
            decimal retakeFees = 0;

            if (_RetakeTestApplication != null)
            {
                retakeFees = clsApplicationType
                    .Find((int)clsApplicationType.enApplicationType.RetakeTest)
                    .Fees;

                lblRetakeFees.Text = retakeFees.ToString();
                _IsRetakeMode = true;
            }
            else
            {
                lblRetakeFees.Text = "0";
                _IsRetakeMode = false;
            }

            // حساب المجموع
            lblTotalFees.Text = (baseFees + retakeFees).ToString();
        }

        bool _IsRetakeMode;
        clsApplication _Application;

        private void _LoadInfo()
        {

            if (_PrepareIfEdit())
            {

            }
            else
            {
                _Application = clsApplication.Find(_LocalDrivingLicenseApplication.ApplicationId);
                _Appointment = new clsTestAppointment();
            }

            dateTimePicker1.MinDate = DateTime.Now;
            _PrepareMode();




            string LicenseClassName = clsLicenseClass.Find(_LocalDrivingLicenseApplication.LicenseClassId).ClassName;

            byte PassedTest = (byte)clsTest.GetPassedTestsCount(_LocalDrivingLicenseApplication.LocalDrivingLicenseAppID);


            lblTrial.Text = PassedTest.ToString();


            lblDrivingLicenseClass.Text = LicenseClassName;

            lblId.Text = _LocalDrivingLicenseApplication.LocalDrivingLicenseAppID.ToString();
            lblName.Text = clsPerson.Find(_Application.ApplicantPersonId).FullName;
            lblFees.Text =clsTestType.Find((int)_Mode).Fees.ToString();
            
            _PrepareRetakeTest();



        }


        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void frmScheduleTest_Load(object sender, EventArgs e)
        {
            if (clsTest.IsTestTaken(_AppointmentId))
            {
                MessageBox.Show("You Already Take This Test");
                button1.Enabled = false;
            }
            _LoadInfo();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            _Appointment.TestTypeID = (int)_Mode;
            _Appointment.LocalDrivingLicenseApplicationID = _LocalDrivingLicenseApplication.LocalDrivingLicenseAppID;
            _Appointment.AppointmentDate = dateTimePicker1.Value;
           
            
            decimal baseFees = clsTestType.Find((int)_Mode).Fees;
            decimal retakeFees = (_RetakeTestApplication != null)
                ? clsApplicationType.Find((int)clsApplicationType.enApplicationType.RetakeTest).Fees
                : 0;
            _Appointment.PaidFees = baseFees + retakeFees;
            
            _Appointment.CreatedByUserID = clsFormSettings.User.UserId;
            _Appointment.IsLocked = false;

            try
            {
                if (_RetakeTestApplication != null)
                    _Appointment.RetakeTestApplicationID = _RetakeTestApplication.ApplicationId;
                else
                    _Appointment.RetakeTestApplicationID = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            if (_Appointment.Save())
            {
                MessageBox.Show("Appointment Saved Successfully");
            }
            else
            {
                MessageBox.Show("Failed In Save This Appointment");
            }

        }

        private void lblFees_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
