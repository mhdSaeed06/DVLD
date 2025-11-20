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
    public partial class frmIssueDrivingLicense : Form
    {
        public frmIssueDrivingLicense(int LocalDrivingLicenseId)
        {
            InitializeComponent();
            _LocalDrivingLicenseId = LocalDrivingLicenseId;
        }

        int _LocalDrivingLicenseId;

        private void label1_Click(object sender, EventArgs e)
        {

        }
        clsApplication _Application;

        clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;
        private void frmIssueDrivingLicense_Load(object sender, EventArgs e)
        {
           _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.Find(_LocalDrivingLicenseId);
           
            if (_LocalDrivingLicenseApplication == null)
            {
                MessageBox.Show("No Application In This Id");
                return;
            }    

            _Application = clsApplication.Find(_LocalDrivingLicenseApplication.ApplicationId);
            ctrlApplicationInfo1.LoadInfo(_Application);


            ctrlDrivingLicenseApplnfo1.LoadInfo(_LocalDrivingLicenseApplication.LocalDrivingLicenseAppID);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            clsDriver driver = clsDriver.FindByPersonID(_Application.ApplicantPersonId);
            
            if (clsLicense.IsDriverHasLicenseInClass(driver.DriverID,_LocalDrivingLicenseApplication.LicenseClassId))
            {
                MessageBox.Show("This Driver has Already this License Class");
                return;

            }

            
                clsLicense license = new clsLicense();
            license.ApplicationId = _LocalDrivingLicenseApplication.ApplicationId;

            try { license.DriverId = driver.DriverID; }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            license.LicenseClassId = _LocalDrivingLicenseApplication.LicenseClassId;
            license.IssueDate = DateTime.Now;
            license.ExpirationDate = DateTime.Now.AddYears(clsLicenseClass.Find(_LocalDrivingLicenseApplication.LicenseClassId).DefaultValidityLength);
            license.Notes = txtbNotes.Text;
            license.PaidFees = _Application.PaidFees;
            license.IsActive = true;
            license.CreatedByUserId = clsFormSettings.User.UserId;
            license.IssueReason = clsLicense.enIssueReason.FirstTime;

            if (license.Save())
            {
                MessageBox.Show("License Issued Successfully With License ID " + license.LicenseId,"Succeeded",MessageBoxButtons.OK,MessageBoxIcon.Information);
                _Application.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
                _Application.LastStatusDate = DateTime.Now;
                _Application.Save();
            }
            else
            {
                MessageBox.Show("License Issued Failed","Failed",MessageBoxButtons.OK,MessageBoxIcon.Error);
                
            }
        }
    }
}
