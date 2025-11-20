using DVLD.Applications.Manage_ApplicationTypes;
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
    public partial class frmManageApplicationsTypes : Form
    {
        public frmManageApplicationsTypes()
        {
            InitializeComponent();
        }


        private void _RefreshApplicationTypesList()
        {
            dgvApplicationTypesList.DataSource = clsApplicationType.GetAllApplicationTypes();
            lblApplicationTypesRecords.Text = dgvApplicationTypesList.RowCount.ToString();
        }
        private void frmManageApplicationsTypes_Load(object sender, EventArgs e)
        {
            _RefreshApplicationTypesList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form form = new frmEditApplicationTypes((int)dgvApplicationTypesList.CurrentRow.Cells[0].Value);
            form.ShowDialog();
            _RefreshApplicationTypesList();
        }
    }
}
