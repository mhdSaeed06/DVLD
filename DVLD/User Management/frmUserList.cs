using DVLD.User_Management;
using DVLDBusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DVLD
{
    public partial class frmUserList : Form
    {
        public frmUserList()
        {
            InitializeComponent();
        }

        private void btnCloseForm_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        DataTable _dtUsers;

        private void _RefreshUserList()
        {
            _dtUsers = clsUser.GetAllUsers();
            dgvAllUsers.DataSource = _dtUsers;
            lblNumOfRecords.Text = dgvAllUsers.RowCount.ToString();
        }

        private void _FillComboBox()
        {
            cbFilter.Items.Add("None");

            for (int i = 0; i < _dtUsers.Columns.Count; i++)
            {
                cbFilter.Items.Add(_dtUsers.Columns[i].ColumnName);
            }


        }

        private void frmUserList_Load(object sender, EventArgs e)
        {
            _RefreshUserList();
            _FillComboBox();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form form = new frmAddEditUser(-1);
            form.ShowDialog();
            _RefreshUserList();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form form = new frmShowUserInfo((int)dgvAllUsers.CurrentRow.Cells[0].Value);
            form.ShowDialog();
            
            _RefreshUserList();
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form form = new frmAddEditUser(-1);
            form.ShowDialog();
            _RefreshUserList();
        }

        private void editPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form form = new frmAddEditUser((int)dgvAllUsers.CurrentRow.Cells[0].Value);
            form.ShowDialog();
             
            _RefreshUserList();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you Sure You Want to delete User with Id " + dgvAllUsers.CurrentRow.Cells[0].Value.ToString()
               , "Delete Person", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning,
               MessageBoxDefaultButton.Button2) == DialogResult.OK)
            {
                if (clsUser.DeleteUser((int)dgvAllUsers.CurrentRow.Cells[0].Value))
                    MessageBox.Show("Deleted Successfully", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);

                else
                    MessageBox.Show("Failed In Delete this User", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }


            _RefreshUserList();
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            

        }

        private void changePasswordToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Form form = new frmChangeUserPassword((int)dgvAllUsers.CurrentRow.Cells[0].Value);
            form.ShowDialog();
            _RefreshUserList();
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataView dvUser = _dtUsers.DefaultView;

            if (cbFilter.SelectedItem != null && cbFilter.SelectedItem.ToString() == "IsActive")
            {
                cbActive.Visible = true;
                txtbFilter.Visible = false;

            }
            else
            {
                cbActive.Visible = false;
                txtbFilter.Visible = true;

               
            }

            //if (cbFilter.SelectedIndex == -1 || cbFilter.SelectedIndex == 0)
            //{
            //    cbFilter.Visible = false;
            //    txtbFilter.Visible = false;
            //}
            //if (cbFilter.SelectedValue.ToString() == "IsActive")
            //{
            //    cbActive.Visible = true;
            //    txtbFilter.Visible = false;
            //    if (cbActive.SelectedIndex == 1)
            //        dvUser.RowFilter = string.Format("IsActive = 1");
            //    else if (cbActive.SelectedIndex == 2)
            //    {
            //        dvUser.RowFilter = string.Format("IsActive = 0");
            //        dgvAllUsers.DataSource = dvUser;
            //    }
            //    else
            //        dgvAllUsers.DataSource= _dtUsers;


            //}
            //else if (cbFilter.SelectedValue.ToString() != "IsActive")
            //{
            //    cbActive.Visible = false;
            //    txtbFilter.Visible = true;

            //    string dataColumnName = cbFilter.Text;
            //    string searchValue = txtbFilter.Text.ToString();

            //    dvUser.RowFilter = string.Format("CONVERT([{0}], 'System.String') LIKE '%{1}%'", dataColumnName, searchValue);
            //    dgvAllUsers.DataSource = dvUser;
            //}


        }

        private void cbActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataView dvUser = _dtUsers.DefaultView;
            if (cbActive.SelectedIndex == 1)
            {
                dvUser.RowFilter = "IsActive = 1";
                dgvAllUsers.DataSource = dvUser;

            }
            else if (cbActive.SelectedIndex == 2)
            {
                dvUser.RowFilter = "IsActive = 0";
                dgvAllUsers.DataSource = dvUser;
            }
            else if (cbActive.SelectedIndex == 0)
            {
                _dtUsers.DefaultView.RowFilter = "";
                dgvAllUsers.DataSource = _dtUsers;
            }
        }

        private void txtbFilter_TextChanged(object sender, EventArgs e)
        {
            DataView dvUser = _dtUsers.DefaultView;

            if (!string.IsNullOrWhiteSpace(txtbFilter.Text))
            {
                string dataColumnName = cbFilter.Text;
                string searchValue = txtbFilter.Text;

                try
                {
                    dvUser.RowFilter =
                        string.Format("CONVERT([{0}], 'System.String') LIKE '%{1}%'", dataColumnName, searchValue);

                    dgvAllUsers.DataSource = dvUser;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            if (string.IsNullOrWhiteSpace(txtbFilter.Text))
            {
                _dtUsers.DefaultView.RowFilter = "";
                dgvAllUsers.DataSource = _dtUsers;
            }
        }
    }
}
