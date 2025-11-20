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
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace DVLD.Applications
{
    public partial class frmTakeTest : Form
    {
        public frmTakeTest(int AppointmentId)
        {
            InitializeComponent();
            _AppointmentID = AppointmentId;
        }

        int _AppointmentID;

        clsTestAppointment _Appointment;


        clsApplication _RetakeTestApplication;
        frmTestAppointments.enMode _Mode;
        clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;
        clsApplication _Application;

        private void _PrepareMode()
        {
            switch (_Mode)
            {
                case frmTestAppointments.enMode.VisionTestMode:
                    {
                        lblTitle.Text = "Vision Test Appointment";
                        pictureBox1.Image = Resources.Vision_512;
                        break;
                    }
                case frmTestAppointments.enMode.WrittenTestMode:
                    {

                        lblTitle.Text = "Written Test Appointment";
                        pictureBox1.Image = Resources.Written_Test_512;
                        break;
                    }

                case frmTestAppointments.enMode.StreetTestMode:
                    {
                        lblTitle.Text = "Street Test Appointment";
                        pictureBox1.Image = Resources.Street_Test_32;
                        break;
                    }
            }
        }



        private void _PrepareRetakeTest()
        {


            if (_Application.ApplicationTypeId == (int)clsApplicationType.enApplicationType.RetakeTest)
            {
                lblRetakeFees.Text = clsApplication.Find((int)_Application.ApplicationTypeId).PaidFees.ToString();
                lblTotalFees.Text = (Convert.ToDecimal(lblRetakeFees.Text) + Convert.ToDecimal(lblFees.Text)).ToString();
                _IsRetakeMode = true;

            }

            else
            {
                lblTotalFees.Text = lblFees.Text;
                _IsRetakeMode = false;
            }
        }

        bool _IsRetakeMode;


        private void _LoadInfo()
        {
            _Appointment = clsTestAppointment.Find(_AppointmentID);

            _Mode = (frmTestAppointments.enMode)_Appointment.TestTypeID;

            _PrepareMode();

            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.Find(_Appointment.LocalDrivingLicenseApplicationID);

            _RetakeTestApplication = clsApplication.Find(_Appointment.RetakeTestApplicationID);

            _Application = clsApplication.Find(_LocalDrivingLicenseApplication.ApplicationId);

            lblId.Text = _LocalDrivingLicenseApplication.LocalDrivingLicenseAppID.ToString();
            lblDrivingLicenseClass.Text = clsLicenseClass.Find(_LocalDrivingLicenseApplication.LicenseClassId).ClassName.ToString();

            lblName.Text = clsPerson.Find(_Application.ApplicantPersonId).FullName;
            lblFees.Text = clsTestType.Find((int)_Mode).Fees.ToString();

            byte PassedTest = (byte)clsTest.GetPassedTestsCount(_LocalDrivingLicenseApplication.LocalDrivingLicenseAppID);

            lblTrial.Text = PassedTest.ToString();

            lblDate.Text = _Appointment.AppointmentDate.ToShortDateString();

            _PrepareRetakeTest();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (clsTest.IsTestTaken(_AppointmentID))
            {
                MessageBox.Show("You Already Take This Test");
                button1.Enabled = false;
            }

            clsTest test = new clsTest();

            test.TestAppointmentID = _AppointmentID;
            if (rbPass.Checked)
                test.TestResult = true;
            else 
                test.TestResult = false;

            test.CreatedByUserID = clsFormSettings.User.UserId;

            test.Notes = textBox1.Text;

            _Appointment.IsLocked = true;

            _Appointment.Save();

            if (test.Save())
            {
                MessageBox.Show("Test Result Saved Successfully");
                if (_Mode == frmTestAppointments.enMode.StreetTestMode && test.TestResult== true )
                {
                                        
                    clsDriver driver = new clsDriver();
                    driver.PersonID = _Application.ApplicantPersonId;
                    driver.CreatedUserID = clsFormSettings.User.UserId;
                    driver.CreatedDate = DateTime.Now;

                    if (driver.Save())
                    {
                        MessageBox.Show("Congratulations To Be Drivers");
                    }
                }
                this.Close();
            }
            else
                MessageBox.Show("Faild In Save This Test Result");


            

        }

       

        private void frmTakeTest_Load(object sender, EventArgs e)
        {
            if (clsTest.IsTestTaken(_AppointmentID))
            {
                MessageBox.Show("You Already Take This Test");
                button1.Enabled = false;
            }
            _LoadInfo();
        }
    }
}
