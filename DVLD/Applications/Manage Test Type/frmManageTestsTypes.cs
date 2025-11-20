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

namespace DVLD.Applications.Manage_Test_Type
{
    public partial class frmManageTestsTypes : Form
    {
        public frmManageTestsTypes()
        {
            InitializeComponent();
        }


        private void _RefreshTestTypesList()
        {
            dgvTestTypesList.DataSource = clsTestType.GetAllTestsTypes();
            lblTestTypesRecords.Text = dgvTestTypesList.RowCount.ToString();
        }
        private void frmManageTestsTypes_Load(object sender, EventArgs e)
        {
            _RefreshTestTypesList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void editTestTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form form = new frmEditTestType((int)dgvTestTypesList.CurrentRow.Cells[0].Value);
            form.ShowDialog();
            _RefreshTestTypesList();
        }
    }
}
