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

namespace DVLD.Applications.Manage_Applications
{
    public partial class frmManageInternationalLicenses : Form
    {
        public frmManageInternationalLicenses()
        {
            InitializeComponent();
        }


        DataTable _dtInternationalLicense;

        private void _RefreshList()
        {
            _dtInternationalLicense = clsInternationalLicense.GetAllInternationalLicenses();
            dgvAllLoaclLicenseApps.DataSource = _dtInternationalLicense;
            lblNumOfRecords.Text = dgvAllLoaclLicenseApps.RowCount.ToString();

        }

        private void _FillComboBox()
        {
            cbFilter.Items.Add("None");

            for (int i = 0; i < _dtInternationalLicense.Columns.Count; i++)
            {
                cbFilter.Items.Add(_dtInternationalLicense.Columns[i].ColumnName);
            }
        }

        private void frmManageInternationalLicenses_Load(object sender, EventArgs e)
        {
            _RefreshList();
            _FillComboBox();
        }

        private void txtbFilter_TextChanged(object sender, EventArgs e)
        {
            if (cbFilter.SelectedIndex == 0 || cbFilter.SelectedIndex == -1)
                return;

            DataView dataView = _dtInternationalLicense.DefaultView;

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

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            Form frm = new frmAddNewInternationalLicense();
            frm.ShowDialog();
            _RefreshList();

        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsLocalDrivingLicenseApplication localDrivingLicenseApplication =
                    clsLocalDrivingLicenseApplication.FindByApplicationId((int)dgvAllLoaclLicenseApps.CurrentRow.Cells[1].Value);
             Form frm = new frmLicenseHistory(localDrivingLicenseApplication.LocalDrivingLicenseAppID);
             frm.ShowDialog();
           
        }

        private void showLicenseDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmShowInternationalLicenseInfo((int)dgvAllLoaclLicenseApps.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }

        private void showPersonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int personId = clsDriver.Find((int)dgvAllLoaclLicenseApps.CurrentRow.Cells[2].Value).PersonID;
            if (personId == 0 )
            {
                MessageBox.Show("Sorry Ya Torry");
                return;
            }
            Form frm = new frmPersonInfo(personId);
            frm.ShowDialog();
        }
    }
}
