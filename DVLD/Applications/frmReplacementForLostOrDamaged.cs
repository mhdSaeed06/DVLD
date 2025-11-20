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
    public partial class frmReplacementForLostOrDamaged : Form
    {
        public frmReplacementForLostOrDamaged()
        {
            InitializeComponent();
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

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            lblTitle.Text = "Replacement For Damaged License";
            _Replacement = enReplacement.Damaged;

            _AppFees = clsApplicationType.Find
        ((int)clsApplicationType.enApplicationType.ReplacementForDamage).Fees;
            lblAppFees.Text = _AppFees.ToString();
            _ApplicationType = clsApplicationType.enApplicationType.ReplacementForDamage;

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        clsApplicationType.enApplicationType _ApplicationType;

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            lblTitle.Text = "Lost For Damaged License";
            _Replacement = enReplacement.Lost;

            _AppFees = clsApplicationType.Find
        ((int)clsApplicationType.enApplicationType.ReplacementForLost).Fees;
            _ApplicationType = clsApplicationType.enApplicationType.ReplacementForLost;
            lblAppFees.Text = _AppFees.ToString();

        }
        int _LicenseId;
        clsLicense _License;
        clsApplication _Application;
        clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;

        enum enReplacement { Lost = 1 , Damaged = 2 }
        enReplacement _Replacement;

        decimal _AppFees;
        private void button1_Click(object sender, EventArgs e)
        {
            if (!textBox1.Text.All(char.IsDigit))
            {
                MessageBox.Show("Please enter Digit");
                return;
            }

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

            
            lblAppFees.Text = _AppFees.ToString();
            
            lblAppDate.Text = DateTime.Now.ToShortDateString();
            lblOldLicenseId.Text = _License.LicenseId.ToString();
            lblUserID.Text = clsFormSettings.User.UserId.ToString();

            llLicenseHistory.Enabled = true;
            llLicenseInfo.Enabled = true;
            btnRetake.Enabled = true;


        }

        private void frmReplacementForLostOrDamaged_Load(object sender, EventArgs e)
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

            if (!_License.IsActive)
            {

                MessageBox.Show("Your License Is Not Active",
                    "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }
            clsApplication application = new clsApplication();
            application.ApplicantPersonId = _Application.ApplicantPersonId;
            application.ApplicationDate = DateTime.Now;
            
            if (_ApplicationType == clsApplicationType.enApplicationType.ReplacementForDamage)
                application.ApplicationTypeId = (int)clsApplicationType.enApplicationType.ReplacementForDamage;
            else
                application.ApplicationTypeId = (int)clsApplicationType.enApplicationType.ReplacementForLost;
                       
            
            application.ApplicationStatus = clsApplication.enApplicationStatus.New;
            application.LastStatusDate = DateTime.Now;
            application.PaidFees = _AppFees;
            application.CreatedByUserId = clsFormSettings.User.UserId;

            application.Save();

            clsLicense license = new clsLicense();
            license.ApplicationId = application.ApplicationId;
            license.DriverId = _License.DriverId;
            license.LicenseClassId = _License.LicenseClassId;
            license.IssueDate = DateTime.Now;
            license.ExpirationDate = DateTime.Now.AddYears(clsLicenseClass.Find(_License.LicenseClassId).DefaultValidityLength);
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
                MessageBox.Show("License Saved Successfully with Id : " + license.LicenseId, "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Failed I Save");

            }


        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
