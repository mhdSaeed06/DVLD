using DVLD.Properties;
using DVLDBusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Applications
{
    public partial class frmTestAppointments : Form
    {
        public frmTestAppointments(int LocalDrivingLicenseApplicationId,enMode Mode )
        {
            InitializeComponent();
            _LocalDrivingLicenseApplicationId = LocalDrivingLicenseApplicationId;
            _Mode = Mode;
        }

        clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;
        int _LocalDrivingLicenseApplicationId;


        DataTable dtLocalDrivingLicenseApplication;


        public enum enMode { VisionTestMode = 1,WrittenTestMode = 2 , StreetTestMode  = 3 }

        enMode _Mode = enMode.VisionTestMode;


        private void _PrepareMode()
        {
            switch (_Mode)
            {
                case enMode.VisionTestMode:
                    {
                        lblTitle.Text = "Vision Test Appointment";
                        pictureBox1.Image = Resources.Vision_512;
                    break;
                    }
                case enMode.WrittenTestMode:
                    {

                        lblTitle.Text = "Written Test Appointment";
                        pictureBox1.Image = Resources.Written_Test_512;
                        break;
                    }

                case enMode.StreetTestMode:
                    {
                        lblTitle.Text = "Street Test Appointment";
                        pictureBox1.Image = Resources.Street_Test_32;
                        break;
                    }
            }
        }

        clsApplication _Application;
        private void _LoadInfo()
        {

            _PrepareMode();

            _LocalDrivingLicenseApplication =
               clsLocalDrivingLicenseApplication.Find(_LocalDrivingLicenseApplicationId);

          
            ctrlDrivingLicenseApplnfo1.LoadInfo(_LocalDrivingLicenseApplicationId);


            _Application  = clsApplication.Find(_LocalDrivingLicenseApplication.ApplicationId);
            ctrlApplicationInfo1.LoadInfo(_Application);

            _RefreshApoointmentsList();

        }

        private void _RefreshApoointmentsList()
        {
            dgvAppointments.DataSource = clsTestAppointment.GetBy(_LocalDrivingLicenseApplicationId, (int)_Mode);
            lblNumOfRecords.Text = dgvAppointments.RowCount.ToString();

        }

        private void frmTestAppointments_Load(object sender, EventArgs e)
        {
            _LoadInfo();
        }

        private void ctrlDrivingLicenseApplnfo1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dgvAppointments.RowCount == 0)
            {
                Form frm = new frmScheduleTest(_LocalDrivingLicenseApplication, _Mode);
                frm.ShowDialog();
                _RefreshApoointmentsList();

                return;
            }

            bool isLocked = false;

            foreach (DataGridViewRow row in dgvAppointments.Rows)
            {
                if (row.Cells["IsLocked"].Value != null)
                {
                    isLocked = (bool)row.Cells["IsLocked"].Value;
                }
                if (!isLocked)
                    break;
                
                
            }

            if (!isLocked)
            {
                MessageBox.Show("You have already an Active Appointment");
                return;
            }


            if (clsTest.GetPassedTestsCount(_LocalDrivingLicenseApplicationId) >= (int)_Mode && isLocked)
            {
                MessageBox.Show("You have already Passed In this Test");
                return;

            }


            if (isLocked)
            {
                clsApplication application;

                if (dgvAppointments.RowCount > 0)
                {
                    application = new clsApplication();
                    application.ApplicantPersonId = _Application.ApplicantPersonId;
                    application.ApplicationDate = DateTime.Now;
                    application.ApplicationTypeId = (int)clsApplicationType.enApplicationType.RetakeTest;
                    application.ApplicationStatus = clsApplication.enApplicationStatus.New;
                    application.LastStatusDate = DateTime.Now;
                    application.PaidFees = clsApplicationType.Find((int)clsApplicationType.enApplicationType.RetakeTest).Fees;
                    application.CreatedByUserId = clsFormSettings.User.UserId;

                    application.Save();

                    clsTestAppointment appointment = clsTestAppointment.Find((int)dgvAppointments.CurrentRow.Cells[0].Value);
                    appointment.RetakeTestApplicationID = application.ApplicationId;


                    Form frm1 = new frmScheduleTest(_LocalDrivingLicenseApplication, _Mode,application);
                    //Form frm1 = new frmScheduleTest(appointment.TestAppointmentID);

                    frm1.ShowDialog();
                    _RefreshApoointmentsList();

                }

            }

            else
            {
            Form frm = new frmScheduleTest(_LocalDrivingLicenseApplication,_Mode);
            frm.ShowDialog();
                _RefreshApoointmentsList();

            }



        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form form = new frmScheduleTest((int)dgvAppointments.CurrentRow.Cells[0].Value);
            form.ShowDialog();
            _RefreshApoointmentsList();

        }

        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form form = new frmTakeTest((int)dgvAppointments.CurrentRow.Cells[0].Value);
            form.ShowDialog();
            _RefreshApoointmentsList();

        }
    }
}
