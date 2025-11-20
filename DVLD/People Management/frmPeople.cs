using DVLDBusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    public partial class frmPeople : Form
    {
        public frmPeople()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        DataTable _dtPeople;
       
        private void _RefreshPeopleList()
        {
            _dtPeople = clsPerson.GetAllPeopleForList();
            dgvAllPeople.DataSource = _dtPeople;
            lblNumOfRecords.Text = dgvAllPeople.RowCount.ToString();

           

        }

        private void _FillComboBox()
        {
            cbFilter.Items.Add("None");

            for (int i = 0; i < _dtPeople.Columns.Count; i++)
            {
                cbFilter.Items.Add(_dtPeople.Columns[i].ColumnName);
            }
        }

        private void frmPeople_Load(object sender, EventArgs e)
        {
            _RefreshPeopleList();
            _FillComboBox();
        }

        private void btnCloseForm_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form frm = new frmAddEditPerson(-1);
            frm.ShowDialog();
            _RefreshPeopleList();

        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmPersonInfo((int)dgvAllPeople.CurrentRow.Cells[0].Value);

                frm.ShowDialog();
            _RefreshPeopleList();

        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmAddEditPerson(-1);

            frm.ShowDialog();
            _RefreshPeopleList();

        }

        private void editPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmAddEditPerson((int)dgvAllPeople.CurrentRow.Cells[0].Value);

            frm.ShowDialog();
            _RefreshPeopleList();

        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
          
            if (MessageBox.Show("Are you Sure You Want to delete person with Id " + dgvAllPeople.CurrentRow.Cells[0].Value.ToString()
                , "Delete Person", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2) == DialogResult.OK)
            {
                if (clsPerson.DeletePerson((int)dgvAllPeople.CurrentRow.Cells[0].Value))
                    MessageBox.Show("Deleted Successfully", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);

                else
                    MessageBox.Show("Failed In Delete this Person", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }


            _RefreshPeopleList() ;

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (cbFilter.SelectedIndex == 0 || cbFilter.SelectedIndex == -1)
                return;

            DataView dataView = _dtPeople.DefaultView;

            string dataColumnName = cbFilter.Text;
            string searchValue = textBox1.Text.ToString();


           //if (_dtPeople.Columns[dataColumnName].DataType != typeof(string))
           // {

           //     if (_dtPeople.Columns[dataColumnName].DataType == typeof(int)&& int.TryParse(searchValue, out int num))
           //     {
           //         dataView.RowFilter = string.Format("[{0}] = {1}",dataColumnName,num);
           //     }
           //     else if (_dtPeople.Columns[dataColumnName].DataType == typeof(DateTime) && DateTime.TryParse(searchValue, out DateTime Date))
           //     {
           //         dataView.RowFilter = string.Format("[{0}] = #{1:MM/dd/yyyy}#", dataColumnName, Date);
           //     }

           // } 

           // else
           // {
//                dataView.RowFilter = string.Format("[{0}] Like '%{1}%'", dataColumnName, searchValue);
            dataView.RowFilter = string.Format("CONVERT([{0}], 'System.String') LIKE '%{1}%'", dataColumnName, searchValue);
            dgvAllPeople.DataSource = dataView;
            //}

        }
    }
}
