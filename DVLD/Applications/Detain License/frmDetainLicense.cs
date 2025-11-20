using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLDBusinessLayer;
namespace DVLD.Applications.Detain_License
{
    public partial class frmDetainLicense : Form
    {
        public frmDetainLicense()
        {
            InitializeComponent();
        }


        int _LicenseId;
        clsLicense _License;
        clsApplication _Application;
        clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;


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

            lblDetainDate.Text = DateTime.Now.ToShortDateString();
            lblUserID.Text = clsFormSettings.User.UserId.ToString();

            llLicenseHistory.Enabled = true;
            llLicenseInfo.Enabled = true;
            btnRetake.Enabled = true;

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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();   
        }

        private void btnRetake_Click(object sender, EventArgs e)
        {
            clsDetainedLicense detainedLicense1 = clsDetainedLicense.Find(_License.LicenseId);
            if (detainedLicense1 != null && !detainedLicense1.IsReleased)
            {
                MessageBox.Show("License Already Detained","Failed",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }


            if (!clsLicense.Find(_License.LicenseId).IsActive)
            {
                MessageBox.Show("This License Is Not Active ", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!textBox2.Text.All(char.IsDigit) || string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Please enter Digit", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            clsDetainedLicense detainedLicense = new clsDetainedLicense();
            detainedLicense.LicenseID = _License.LicenseId;
            detainedLicense.DetainDate = DateTime.Now;
            detainedLicense.FineFees = Convert.ToDecimal(textBox2.Text);
            detainedLicense.CreatedByUserID = clsFormSettings.User.UserId;
            
            if (detainedLicense.Save())
            {
                MessageBox.Show("Detained Successfully");

            }
            else
            {
                MessageBox.Show("Failed In Detain This License");
            }


        }

        private void textBox2_Validated(object sender, EventArgs e)
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

        private void frmDetainLicense_Load(object sender, EventArgs e)
        {

        }
    }
}
