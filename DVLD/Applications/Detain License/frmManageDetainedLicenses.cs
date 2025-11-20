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

namespace DVLD.Applications.Detain_License
{
    public partial class frmManageDetainedLicenses : Form
    {
        public frmManageDetainedLicenses()
        {
            InitializeComponent();
        }

        private void _FillComboBox()
        {
            cbFilter.Items.Add("None");

            for (int i = 0; i < _dtDetainedLicense.Columns.Count; i++)
            {
                cbFilter.Items.Add(_dtDetainedLicense.Columns[i].ColumnName);
            }
        }

        DataTable _dtDetainedLicense;

        private void _RefreshList()
        {
            _dtDetainedLicense = clsDetainedLicense.GetAll();
            dgvAllDetainedLicenses.DataSource = _dtDetainedLicense;
            lblNumOfRecords.Text = dgvAllDetainedLicenses.RowCount.ToString();

        }

        private void txtbFilter_TextChanged(object sender, EventArgs e)
        {
            if (cbFilter.SelectedIndex == 0 || cbFilter.SelectedIndex == -1)
                return;

            DataView dataView = _dtDetainedLicense.DefaultView;

            string dataColumnName = cbFilter.Text;
            string searchValue = txtbFilter.Text.ToString();

            try
            {
                dataView.RowFilter = string.Format("CONVERT([{0}], 'System.String') LIKE '%{1}%'", dataColumnName, searchValue);
                dgvAllDetainedLicenses.DataSource = dataView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please enter correct string");

            }
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            Form frm = new frmDetainLicense();
            frm.ShowDialog();
            _RefreshList();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnReleaseLicense_Click(object sender, EventArgs e)
        {
            Form form = new frmReleaseLicense();
            form.ShowDialog();
            _RefreshList();

        }

        private void releaseLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmReleaseLicense((int)dgvAllDetainedLicenses.CurrentRow.Cells[1].Value);
            frm.ShowDialog();
        }

        private void frmManageDetainedLicenses_Load(object sender, EventArgs e)
        {
            _RefreshList();
            _FillComboBox();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if ((bool)dgvAllDetainedLicenses.CurrentRow.Cells["IsReleased"].Value == true)
            {
                contextMenuStrip1.Items["releaseLicenseToolStripMenuItem"].Enabled = false;
            }
        }
    }
}
