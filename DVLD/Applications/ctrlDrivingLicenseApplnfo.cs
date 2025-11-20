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
    public partial class ctrlDrivingLicenseApplnfo : UserControl
    {
        public ctrlDrivingLicenseApplnfo()
        {
            InitializeComponent();
        }

        public void LoadInfo(int LocalDrivingLicenseApplicationId)
        {
            clsLocalDrivingLicenseApplication localDrivingLicenseApplication = clsLocalDrivingLicenseApplication.Find(LocalDrivingLicenseApplicationId);
            string LicenseClassName = clsLicenseClass.Find(localDrivingLicenseApplication.LicenseClassId).ClassName;

            byte PassedTest = (byte)clsTest.GetPassedTestsCount(LocalDrivingLicenseApplicationId);

            lblId.Text = LocalDrivingLicenseApplicationId.ToString();
            lblLicenseClass.Text = LicenseClassName.ToString();
            lblPassedTest.Text = PassedTest.ToString();

        }

        private void gbDrivingAppInfo_Enter(object sender, EventArgs e)
        {

        }
    }
}
