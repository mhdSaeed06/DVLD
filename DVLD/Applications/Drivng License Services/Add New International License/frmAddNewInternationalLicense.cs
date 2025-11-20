using DVLD.Applications;
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

namespace DVLD
{
    public partial class frmAddNewInternationalLicense : Form
    {
        public frmAddNewInternationalLicense()
        {
            InitializeComponent();
        }

        private void frmAddNewInternationalLicense_Load(object sender, EventArgs e)
        {

        }

        int _LicenseId;
        clsApplication _Application;
        clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;
        clsLicense _License;

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

            ctrlApplicationInfo1.LoadInfo(_Application);
            ctrlDriverLicenseInfo1.LoadInfo(_LocalDrivingLicenseApplication.LocalDrivingLicenseAppID);

            if (clsInternationalLicense.IsInternationalLicenseExistAndActive(_License.LicenseId))
            {
                MessageBox.Show("This Driver ALready Has International License");
                _InternationalLicense = clsInternationalLicense.FindByLocalLicense(_LicenseId);
                btnIssue.Enabled = false;
            }
            else
            {
                btnIssue.Enabled = true;
            }

        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {

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
        clsInternationalLicense _InternationalLicense;
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (_License == null)
                return;
            if (clsInternationalLicense.IsInternationalLicenseExistAndActive(_License.LicenseId))
            {
                MessageBox.Show("You already have this License");
                return;
            }    
             _InternationalLicense = new clsInternationalLicense();
            _InternationalLicense.ApplicationID = _Application.ApplicationId;
            _InternationalLicense.IssuedUsingLocalLicenseID = _License.LicenseId;
            clsDriver driver = clsDriver.Find(_License.DriverId);
            _InternationalLicense.IssueDate = DateTime.Now;
            _InternationalLicense.ExpirationDate = DateTime.Now.AddYears(clsLicenseClass.Find(_LocalDrivingLicenseApplication.LicenseClassId).DefaultValidityLength);
            _InternationalLicense.IsActive = true;
            _InternationalLicense.CreatedByUserID = clsFormSettings.User.UserId;
            _InternationalLicense.DriverID = driver.DriverID;


                if (_InternationalLicense.Save())
            {
                MessageBox.Show("Saved Successfuly With ID " + _InternationalLicense.InternationalLicenseID);

            }
            else
            {
                MessageBox.Show("Failed In Save");
            }


        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (_LocalDrivingLicenseApplication == null)
            {
                MessageBox.Show("Select License");
                return;
            }
            Form frm = new frmLicenseHistory(_LocalDrivingLicenseApplication.LocalDrivingLicenseAppID);
            frm.ShowDialog();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form form = new frmShowInternationalLicenseInfo(_InternationalLicense.InternationalLicenseID);
            form.ShowDialog();
        }
    }
}
