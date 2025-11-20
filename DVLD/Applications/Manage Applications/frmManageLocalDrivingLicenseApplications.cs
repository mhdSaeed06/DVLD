using DVLD.Applications;
using DVLD.Applications.Add_New_Local_Driving_License;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DVLD
{
    public partial class frmManageLocalDrivingLicenseApplications : Form
    {
        public frmManageLocalDrivingLicenseApplications()
        {
            InitializeComponent();
        }


        DataTable _dtLocalDrivingLicenseApp;
        private void _RefreshUserList()
        {
            _dtLocalDrivingLicenseApp = clsLocalDrivingLicenseApplication.GetAllLocalLicenseAppForList();
            dgvAllLoaclLicenseApps.DataSource = _dtLocalDrivingLicenseApp;
            lblNumOfRecords.Text = dgvAllLoaclLicenseApps.RowCount.ToString();
        }

        private void _FillComboBox()
        {
            cbFilter.Items.Add("None");

            for (int i = 0; i < _dtLocalDrivingLicenseApp.Columns.Count; i++)
            {
                cbFilter.Items.Add(_dtLocalDrivingLicenseApp.Columns[i].ColumnName);
            }
        }

        private void _PrepareContextMenueStrip(int LocalDrivigLicenseAppId)
        {

            contextMenuStrip1.Items["editApplicationToolStripMenuItem"].Enabled = true;
            contextMenuStrip1.Items["deleteApplicationToolStripMenuItem"].Enabled = true;
            contextMenuStrip1.Items["toolStripMenuItem6"].Enabled = true;
            contextMenuStrip1.Items["issueDrivingLicenseToolStripMenuItem"].Enabled = false;
            contextMenuStrip1.Items["showLicenseToolStripMenuItem"].Enabled = false;

            int PassedTest = clsTest.GetPassedTestsCount(LocalDrivigLicenseAppId);
            
            switch (PassedTest)
            {
                case 0:
                    {
                        toolStripMenuItem6.DropDownItems["shceduleEyeTestToolStripMenuItem"].Enabled = true;
                        toolStripMenuItem6.DropDownItems["scheduleWrittenTestToolStripMenuItem"].Enabled = false;
                        toolStripMenuItem6.DropDownItems["scheduleStreetTestToolStripMenuItem"].Enabled = false;
                        break;
                    }
                case 1:
                    {
                        toolStripMenuItem6.DropDownItems["shceduleEyeTestToolStripMenuItem"].Enabled = false;
                        toolStripMenuItem6.DropDownItems["scheduleWrittenTestToolStripMenuItem"].Enabled = true;
                        toolStripMenuItem6.DropDownItems["scheduleStreetTestToolStripMenuItem"].Enabled = false;

                        break;
                    }
                case 2:
                    {
                        toolStripMenuItem6.DropDownItems["shceduleEyeTestToolStripMenuItem"].Enabled = false;
                        toolStripMenuItem6.DropDownItems["scheduleWrittenTestToolStripMenuItem"].Enabled = false;
                        toolStripMenuItem6.DropDownItems["scheduleStreetTestToolStripMenuItem"].Enabled = true;
                        break;
                    }
                case 3:
                    {
                        toolStripMenuItem6.DropDownItems["scheduleStreetTestToolStripMenuItem"].Enabled = false;
                        toolStripMenuItem6.DropDownItems["scheduleWrittenTestToolStripMenuItem"].Enabled = false;
                        toolStripMenuItem6.DropDownItems["shceduleEyeTestToolStripMenuItem"].Enabled = false;
                       
                        contextMenuStrip1.Items["issueDrivingLicenseToolStripMenuItem"].Enabled = true;

                        break;
                    }


            }
        }

        private void frmManageLocalDrivingLicenseApplications_Load(object sender, EventArgs e)
        {
            _RefreshUserList();
            _FillComboBox();



        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            Form form = new frmAddNewLocalDrivingLicense();
            form.ShowDialog();
            _RefreshUserList();
        }

        private void txtbFilter_TextChanged(object sender, EventArgs e)
        {
            if (cbFilter.SelectedIndex == 0 || cbFilter.SelectedIndex == -1)
                return;

            DataView dataView = _dtLocalDrivingLicenseApp.DefaultView;

            string dataColumnName = cbFilter.Text;
            string searchValue = txtbFilter.Text.ToString();

            try
            {
                dataView.RowFilter = string.Format("CONVERT([{0}], 'System.String') LIKE '%{1}%'", dataColumnName, searchValue);
                dgvAllLoaclLicenseApps.DataSource = dataView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please enter correct string");

            }


        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void takeTestsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void eyeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmTestAppointments((int)dgvAllLoaclLicenseApps.CurrentRow.Cells[0].Value, frmTestAppointments.enMode.VisionTestMode);
            frm.ShowDialog();
        }

        private void shceduleEyeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmTestAppointments((int)dgvAllLoaclLicenseApps.CurrentRow.Cells[0].Value, frmTestAppointments.enMode.VisionTestMode);
            frm.ShowDialog();
            _RefreshUserList();
        }

        private void scheduleWrittenTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmTestAppointments((int)dgvAllLoaclLicenseApps.CurrentRow.Cells[0].Value, frmTestAppointments.enMode.WrittenTestMode);
            frm.ShowDialog();
            _RefreshUserList();
        }

        private void scheduleStreetTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmTestAppointments((int)dgvAllLoaclLicenseApps.CurrentRow.Cells[0].Value, frmTestAppointments.enMode.StreetTestMode);
            frm.ShowDialog();
            _RefreshUserList();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

            if (dgvAllLoaclLicenseApps.CurrentRow == null)
            {
                e.Cancel = true; // يلغي فتح القائمة إذا ما في صف محدد
                return;

            }
            object cellValue = dgvAllLoaclLicenseApps.CurrentRow.Cells[0].Value;

            if (cellValue == null || cellValue == DBNull.Value)
            {
                e.Cancel = true; // ما في ID نشتغل عليه
                return;
            }

            int localDrivingLicenseAppId = Convert.ToInt32(cellValue);

            clsLocalDrivingLicenseApplication localDrivingLicenseApplication = clsLocalDrivingLicenseApplication.Find(localDrivingLicenseAppId);
            clsApplication application = clsApplication.Find(localDrivingLicenseApplication.ApplicationId);

            _PrepareContextMenueStrip(localDrivingLicenseAppId);
            if (application != null)
            {
                clsDriver driver = clsDriver.FindByPersonID(application.ApplicantPersonId);

                if (driver != null)
                {
                    if (clsLicense.IsDriverHasLicenseInClass(driver.DriverID, localDrivingLicenseApplication.LicenseClassId))
                    {
                        contextMenuStrip1.Items["editApplicationToolStripMenuItem"].Enabled = false; 
                        contextMenuStrip1.Items["deleteApplicationToolStripMenuItem"].Enabled = false; 
                        contextMenuStrip1.Items["toolStripMenuItem6"].Enabled = false; 
                        contextMenuStrip1.Items["issueDrivingLicenseToolStripMenuItem"].Enabled = false; 
                        contextMenuStrip1.Items["showLicenseToolStripMenuItem"].Enabled = true; 

                    }
                }
            }
        }

        private void issueDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form form = new frmIssueDrivingLicense((int)dgvAllLoaclLicenseApps.CurrentRow.Cells[0].Value);
            form.ShowDialog();
            
        }

        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmShowLicenseInfo((int)dgvAllLoaclLicenseApps.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmLicenseHistory((int)dgvAllLoaclLicenseApps.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }

        private void deleteApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (clsLocalDrivingLicenseApplication.Delete((int)dgvAllLoaclLicenseApps.CurrentRow.Cells[0].Value))
                {
                    MessageBox.Show("Deleted Successfully");
                }
                else
                {
                    MessageBox.Show("Deleted Failed");

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show("You Cannot Delete This Application Becaus You Start In It");

            }
            _RefreshUserList();
        }
    }
}
