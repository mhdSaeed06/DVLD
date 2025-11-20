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

namespace DVLD.User_Management
{
    public partial class frmChangeUserPassword : Form
    {
        public frmChangeUserPassword(int UserId)
        {
            InitializeComponent();
            
            _User = clsUser.Find(UserId);

            if (_User == null)
            {
                MessageBox.Show("This Form will close because no User in Id = " + UserId);
                this.Close();
            }

        }

        clsUser _User;

        private void _LoadForm()
        {
            
            ctrlUserInfo1.LoadUser(_User);
            ctrlPersonInformation1.LoadPerson(_User.PersonId);
        }

        private void ctrlUserInfo1_Load(object sender, EventArgs e)
        {

        }

        private void ctrlPersonInformation1_Load(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void frmChangeUserPassword_Load(object sender, EventArgs e)
        {
            _LoadForm();
        }

        private void txtbConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtbConfirmPassword.Text))
            {
                txtbConfirmPassword.Focus();
                errorProvider1.SetError(txtbConfirmPassword, "CofirmPassword should have a value");

            }
            else if (txtbConfirmPassword.Text != txtbNewPassword.Text)
            {
                txtbConfirmPassword.Focus();
                errorProvider1.SetError(txtbConfirmPassword, "CofirmPassword should match Password");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtbConfirmPassword, "");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtbOldPassword.Text != _User.Password)
            {
                MessageBox.Show("Wrong Password");
                return;
            }
            else if (txtbNewPassword.Text != txtbConfirmPassword.Text)
            {
                MessageBox.Show("Password should match");
                return;
            }

            _User.Password = txtbNewPassword.Text;

            if (_User.Save())
                MessageBox.Show("Password Updated Successfully");
            else
            {
                MessageBox.Show("Failed In Updated Password");
            }

        }
    }
}
