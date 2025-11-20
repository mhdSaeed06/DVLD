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

namespace DVLD.Applications
{
    public partial class frmRenewLicense : Form
    {
        public frmRenewLicense()
        {
            InitializeComponent();
        }

        private void frmRenewLicense_Load(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_Validated(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                textBox1.Focus();
                button1.Enabled = false;
                errorProvider1.SetError(textBox1, "This field cannot be empty");
            }
            else if (!textBox1.Text.All(char.IsDigit))
            {
                textBox1.Focus();
                button1.Enabled = false;
                errorProvider1.SetError(textBox1, "Only numbers are allowed");
            }
            else
            {
                button1.Enabled = true;
                errorProvider1.SetError(textBox1, "");

            }
        }


        int _LicenseId;
        clsLicense _License;
        clsApplication _Application;
        clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;

        private void button1_Click(object sender, EventArgs e)
        {
            _LicenseId = Convert.ToInt32(textBox1.Text);
            _License = clsLicense.Find(_LicenseId);

            if (_License == null)
            {
                MessageBox.Show("No License In This Id");
                return;
            }

            _Application = clsApplication.Find(_License.ApplicationId);
            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByApplicationId(_License.ApplicationId);

            ctrlDriverLicenseInfo1.LoadInfo(_LocalDrivingLicenseApplication.LocalDrivingLicenseAppID);
            
            lblAppDate.Text = DateTime.Now.ToShortDateString();
            lblIssueDate.Text = DateTime.Now.ToShortDateString();
            _AppFees = clsApplicationType.Find
                    ((int)clsApplicationType.enApplicationType.RenewDrivingLicense).Fees;
            lblAppFees.Text = _AppFees.ToString();
                _LicenseFees = clsLicenseClass.Find(_License.LicenseClassId).ClassFees; 
            lblLicenseFees.Text = _LicenseFees.ToString();
            lblTotalFees.Text = (_AppFees + _LicenseFees).ToString();
            lblExpirationDate.Text = DateTime.Now.AddYears(clsLicenseClass.Find(_License.LicenseClassId).DefaultValidityLength).ToLongDateString();


            llLicenseHistory.Enabled = true;
            llLicenseInfo.Enabled = true;
            btnRetake.Enabled = true;

        }
        decimal _LicenseFees;
        decimal _AppFees;
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void llLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form form = new frmShowLicenseInfo(_LocalDrivingLicenseApplication.LocalDrivingLicenseAppID);
            form.ShowDialog();
        }

        private void llLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form form = new frmLicenseHistory(_LocalDrivingLicenseApplication.LocalDrivingLicenseAppID);
            form.ShowDialog();
        }

        private void btnRetake_Click(object sender, EventArgs e)    
        {
            if (_License.ExpirationDate > DateTime.Now)
            {
                MessageBox.Show("Selecte License is not yet expiared it whil expire in " + _License.ExpirationDate,
                    "Not Allowed",MessageBoxButtons.OK,MessageBoxIcon.Error);

                return;
            }

            if(!_License.IsActive)
            {

                MessageBox.Show("Your License Is Not Active",
                    "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }
            clsApplication application = new clsApplication();
            application.ApplicantPersonId = _Application.ApplicantPersonId;
            application.ApplicationDate = DateTime.Now;
            application.ApplicationTypeId = clsApplicationType.Find
                    ((int)clsApplicationType.enApplicationType.RenewDrivingLicense).ApplicationTypeId;

            application.ApplicationStatus = clsApplication.enApplicationStatus.New;
            application.LastStatusDate = DateTime.Now;
            application.PaidFees = _LicenseFees + _AppFees;
            application.CreatedByUserId = clsFormSettings.User.UserId;

            application.Save();

            clsLicense license = new clsLicense();
            license.ApplicationId = application.ApplicationId;
            license.DriverId = _License.DriverId;
            license.LicenseClassId = _License.LicenseClassId;
            license.IssueDate = DateTime.Now;
            license.ExpirationDate = DateTime.Now.AddYears(clsLicenseClass.Find(_License.LicenseClassId).DefaultValidityLength);
            license.Notes = textBox2.Text;
            license.PaidFees = clsLicenseClass.Find(_License.LicenseClassId).ClassFees;
            license.IsActive = true;
            _License.IsActive = false;
            license.IssueReason = clsLicense.enIssueReason.Renew;
            license.CreatedByUserId = clsFormSettings.User.UserId;

            clsLocalDrivingLicenseApplication localDrivingLicenseApplication = new clsLocalDrivingLicenseApplication();
            localDrivingLicenseApplication.ApplicationId = application.ApplicationId;
            localDrivingLicenseApplication.LicenseClassId = _License.LicenseClassId;

            localDrivingLicenseApplication.Save();

            _License.Save();
            if (license.Save())
            {
                MessageBox.Show("License Saved Successfully with Id : " + license.LicenseId,"Saved",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }    
            else
            {
                MessageBox.Show("Failed I Save");

            }


            lblAppId.Text = application.ApplicationId.ToString();
            lblRenwedLicenseId.Text = license.LicenseId.ToString();

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Validated(object sender, EventArgs e)
        {
           
        }

        private void ctrlDriverLicenseInfo1_Load(object sender, EventArgs e)
        {

        }
    }
}
