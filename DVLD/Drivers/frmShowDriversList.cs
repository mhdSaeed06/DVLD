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

namespace DVLD.Drivers
{
    public partial class frmShowDriversList : Form
    {
        public frmShowDriversList()
        {
            InitializeComponent();
        }

        DataTable _dtDrivers;
        private void _RefreshDriverList()
        {
            _dtDrivers = clsDriver.GetAllDrivers();
            dgvAllDrivers.DataSource = _dtDrivers;
            lblNumOfRecords.Text = dgvAllDrivers.RowCount.ToString();

        }
        private void _FillComboBox()
        {
            cbFilter.Items.Add("None");

            for (int i = 0; i < _dtDrivers.Columns.Count; i++)
            {
                cbFilter.Items.Add(_dtDrivers.Columns[i].ColumnName);
            }


        }


        private void frmShowDriversList_Load(object sender, EventArgs e)
        {
            _RefreshDriverList();
            _FillComboBox();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtbFilter_TextChanged(object sender, EventArgs e)
        {
            DataView dvUser = _dtDrivers.DefaultView;

            if (!string.IsNullOrWhiteSpace(txtbFilter.Text))
            {
                string dataColumnName = cbFilter.Text;
                string searchValue = txtbFilter.Text;

                try
                {
                    dvUser.RowFilter =
                        string.Format("CONVERT([{0}], 'System.String') LIKE '%{1}%'", dataColumnName, searchValue);

                    dgvAllDrivers.DataSource = dvUser;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            if (string.IsNullOrWhiteSpace(txtbFilter.Text))
            {
                _dtDrivers.DefaultView.RowFilter = "";
                dgvAllDrivers.DataSource = _dtDrivers;
            }
        }
    }
}
