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
    public partial class ctrlInternationalLicenseInfo : UserControl
    {
        public ctrlInternationalLicenseInfo()
        {
            InitializeComponent();

        }
        int _InternationalLicenseId;
        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
        clsInternationalLicense _InternationalLicense;

        clsDriver _Driver;
        clsPerson _Person;

        public void LoadInfo(int InternationalLicenseId)
        {
            _InternationalLicenseId = InternationalLicenseId;
            _InternationalLicense = clsInternationalLicense.Find(_InternationalLicenseId);
            lblLicenseId.Text = _InternationalLicenseId.ToString();

            _Driver = clsDriver.Find(_InternationalLicense.DriverID);
            _Person = clsPerson.Find(_Driver.PersonID);
            lblName.Text = _Person.FullName;
            lblNationalNo.Text = _Person.NationalNumber.ToString();
            clsPerson.enGender gender = _Person.gender;
            lblGendor.Text = gender.ToString();
            lblIssueDate.Text = _InternationalLicense.IssueDate.ToShortDateString();
            lblApplicationd.Text = _InternationalLicense.ApplicationID.ToString();
            lblIsActive.Text = _InternationalLicense.IsActive.ToString();
            lblDateOfBirth.Text = _Person.DateOfBirth.ToShortDateString();
            lblDriverId.Text = _Driver.DriverID.ToString();
            lblExpirationDate.Text = _InternationalLicense.ExpirationDate.ToShortDateString();
                

        }

        private void ctrlInternationalLicenseInfo_Load(object sender, EventArgs e)
        {
        }


    }
}
