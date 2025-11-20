using DVLD.Applications;
using DVLD.Applications.Add_New_Local_Driving_License;
using DVLD.Applications.Detain_License;
using DVLD.Applications.Manage_Applications;
using DVLD.Applications.Manage_Test_Type;
using DVLD.Drivers;
using DVLD.User_Management;
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
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }



     

        private void peopleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form form = new frmPeople ();
            form.ShowDialog();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void frmMain_Load(object sender, EventArgs e)
        {

        }

        private void userToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmUserList();
            frm.ShowDialog();
        }

        private void showUserInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmShowUserInfo(clsFormSettings.User.UserId);
            frm.ShowDialog();
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
           Form frm = new frmChangeUserPassword(clsFormSettings.User.UserId);
            frm.ShowDialog();
        }

        private void signOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmLogin(true);
            frm.ShowDialog();
        }

        private void drivingLicenseServicToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void manageApplicationsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form form = new frmManageApplicationsTypes();
            form.ShowDialog();

        }

        private void manageTestTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form form = new frmManageTestsTypes();
            form.ShowDialog();
        }

        private void renewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            

        }

        private void localToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmAddNewLocalDrivingLicense();
            frm.ShowDialog();
        }

        private void manageApplicationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form form = new frmManageApplicationsTypes();
            form.ShowDialog();
        }

        private void manageApplicationsToolStripMenuItem_Click_1(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form form = new frmAddNewInternationalLicense();
            form.ShowDialog();
        }

        private void localDrivingLicenseApplicationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmManageLocalDrivingLicenseApplications(); 
            frm.ShowDialog();   
        }

        private void driversToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmShowDriversList();
            frm.ShowDialog();
        }

        private void internationalDrivingLicenseApplicationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmManageInternationalLicenses();
            frm.ShowDialog();
        }

        private void renToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmRenewLicense();
            frm.ShowDialog();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }

        private void replacementForLostOrDamageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form form = new frmReplacementForLostOrDamaged();
            form.ShowDialog();
        }

        private void releaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmReleaseLicense();
            frm.ShowDialog();
        }

        private void detainLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmDetainLicense();
            frm.ShowDialog();
        }

        private void manageDetainedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmManageDetainedLicenses();
            frm.ShowDialog();
        }
    }
}
