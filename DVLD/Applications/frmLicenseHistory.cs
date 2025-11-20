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
    public partial class frmLicenseHistory : Form
    {
        public frmLicenseHistory(int LocalDrivingLicenseApplicationId)
        {
            InitializeComponent();
            _LocalDrivingLicenseApplicationId = LocalDrivingLicenseApplicationId;
        }

        int _LocalDrivingLicenseApplicationId;

        DataTable _dtLocalLicense;

        DataTable _dtInternationalLicense;

        private void _LoadInfo()
        {
            clsLocalDrivingLicenseApplication localDrivingLicenseApplication = clsLocalDrivingLicenseApplication.Find(_LocalDrivingLicenseApplicationId);

            clsApplication application = clsApplication.Find(localDrivingLicenseApplication.ApplicationId);

            clsPerson person = clsPerson.Find(application.ApplicantPersonId);
            clsDriver driver = clsDriver.FindByPersonID(person.PersonId);
            if (driver == null)
            {
                MessageBox.Show("No driver in this Application");
                return;
            }

            clsLicense license = clsLicense.FindByApplicationId(application.ApplicationId);
            ctrlPersonInformation1.LoadPerson(person.PersonId);

            _dtLocalLicense = clsLicense.GetAllLicenses(driver.DriverID);
            dgvAllLocalLicense.DataSource = _dtLocalLicense;
            lblNumOfLocalLicenses.Text = dgvAllLocalLicense.RowCount.ToString();

            _dtInternationalLicense = clsInternationalLicense.GetAllInternationalLicenses(driver.DriverID);
            dgvAllInternationalLicenses.DataSource = _dtInternationalLicense;
            lblNumOfInternationalLicense.Text = dgvAllInternationalLicenses.RowCount.ToString();
            

        }


        private void frmLicenseHistory_Load(object sender, EventArgs e)
        {
            _LoadInfo();
        }

        private void ctrlPersonInformation1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }
    }
}
