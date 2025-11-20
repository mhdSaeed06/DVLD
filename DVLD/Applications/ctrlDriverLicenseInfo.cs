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
    public partial class ctrlDriverLicenseInfo : UserControl
    {
        public ctrlDriverLicenseInfo()
        {
            InitializeComponent();
        }

        int _LocalDrivingLicenseId;

        public void LoadInfo(int LocalDrivingLicenseAppId)
        {
            _LocalDrivingLicenseId = LocalDrivingLicenseAppId;

            clsLocalDrivingLicenseApplication localDrivingLicenseApplication = clsLocalDrivingLicenseApplication.Find(_LocalDrivingLicenseId);
            lblClass.Text = clsLicenseClass.Find(localDrivingLicenseApplication.LicenseClassId).ClassName.ToString();
            clsApplication application = clsApplication.Find(localDrivingLicenseApplication.ApplicationId);

            clsPerson person = clsPerson.Find(application.ApplicantPersonId);
            clsDriver driver = clsDriver.FindByPersonID(person.PersonId);
            clsLicense license = clsLicense.FindByApplicationId(application.ApplicationId);

            lblLicenseId.Text = license.LicenseId.ToString(); 
            lblName.Text = person.FullName;
            lblNationalNo.Text = person.NationalNumber;
            lblGendor.Text = (person.gender == clsPerson.enGender.Male) ? "Male" : "Female";
            lblIssueDate.Text = license.IssueDate.ToShortDateString();
            clsLicense.enIssueReason issueReason =  license.IssueReason;
            lblIssueReason.Text = issueReason.ToString();

            if (person.ImagePath != "")
                pictureBox1.ImageLocation = person.ImagePath;

            lblIsActive.Text = (license.IsActive) ? "Active" : "Not Active";
            lblDateOfBirth.Text = person.DateOfBirth.ToShortDateString();
            lblFees.Text = license.PaidFees.ToString();
            lblExpirationDate.Text = license.ExpirationDate.ToShortDateString();
            clsDetainedLicense detainedLicense = clsDetainedLicense.Find(license.LicenseId);
            lblIsDetained.Text = ( detainedLicense == null) ? "No" : "Yes";  
            lblNotes.Text = license.Notes;


        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void ctrlDriverLicenseInfo_Load(object sender, EventArgs e)
        {

        }
    }
}
