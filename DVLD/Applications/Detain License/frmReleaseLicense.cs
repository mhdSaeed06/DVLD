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

namespace DVLD.Applications.Detain_License
{
    public partial class frmReleaseLicense : Form
    {
        public frmReleaseLicense(int LicenseId)
        {
            InitializeComponent();
            _LicenseId = LicenseId;
        }

        public frmReleaseLicense()
        {
            InitializeComponent();
            groupBox1.Enabled = true;
        }


        int _LicenseId;

        clsLicense _License;
        clsApplication _Application;
        clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;


        decimal _AppFees;



        private void frmReleaseLicense_Load(object sender, EventArgs e)
        {
            if (_LicenseId != 0)
                _LoadInfo();

        }
        decimal _FineFees;

        private void _LoadInfo()
        {

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
            _AppFees = clsApplicationType.Find((int)clsApplicationType.enApplicationType.Release).Fees;
            lblAppFees.Text = _AppFees.ToString();
            _FineFees = clsDetainedLicense.Find(_LicenseId).FineFees;
            lblFineFees.Text = _FineFees.ToString();

            lblTotalFees.Text = (_FineFees + _AppFees).ToString();


            llLicenseHistory.Enabled = true;
            llLicenseInfo.Enabled = true;
            btnRelease.Enabled = true;
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

            _AppFees = clsApplicationType.Find((int)clsApplicationType.enApplicationType.Release).Fees;
            lblAppFees.Text = _AppFees.ToString();
            _FineFees = clsDetainedLicense.Find(_LicenseId).FineFees;
            lblFineFees.Text = _FineFees.ToString();

            lblTotalFees.Text = (_FineFees + _AppFees).ToString();

            llLicenseHistory.Enabled = true;
            llLicenseInfo.Enabled = true;
            btnRelease.Enabled = true;

        }

        private void textBox1_Validated(object sender, EventArgs e)
        {

        }

        private void btnRetake_Click(object sender, EventArgs e)
        {
            clsDetainedLicense detainedLicense = clsDetainedLicense.Find(_LicenseId);

            if (detainedLicense == null)
            {
                MessageBox.Show("Your License Is Already Released");
                return;

            }


            if (detainedLicense.IsReleased)
                MessageBox.Show("Your License Is Already Released");

            clsApplication application = new clsApplication();
            application.ApplicantPersonId = _Application.ApplicantPersonId;
            application.ApplicationDate = DateTime.Now;
            application.ApplicationTypeId = (int)clsApplicationType.enApplicationType.Release;
            application.ApplicationStatus = clsApplication.enApplicationStatus.New;
            application.LastStatusDate = DateTime.Now;
            application.PaidFees = clsApplicationType.Find((int)clsApplicationType.enApplicationType.Release).Fees;
            application.CreatedByUserId = _Application.CreatedByUserId;

            application.Save();

            detainedLicense.ReleasedByUserID = clsFormSettings.User.UserId;
            detainedLicense.IsReleased = true;
            detainedLicense.ReleaseDate = DateTime.Now;
            detainedLicense.ReleaseApplicationID = application.ApplicationId;

            if (detainedLicense.Save())
                MessageBox.Show("Released Successfully", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("Released Failed", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);



        }
    }
}
