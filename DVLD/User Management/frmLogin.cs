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
    public partial class frmLogin : Form
    {
        public frmLogin(bool isSignOut = false)
        {
            InitializeComponent();
            _isSignOut = isSignOut;
        }

        bool _isSignOut; 
        clsUser user;
        private void button1_Click(object sender, EventArgs e)
        {
            DataRow[] rows;
            try
            {
               rows = dtUsers.Select($"UserName = '{txtbUserName.Text}'");
            }
            catch (Exception ex){
                MessageBox.Show("Wrong UserName");
                    MessageBox.Show(ex.Message);
                return;
            }
            if (rows.Length > 0)
            {
                user = new clsUser();
                user.UserName = (string)rows[0]["UserName"];
                user.Password = (string)rows[0]["Password"];
                user.UserId = (int)rows[0]["UserId"];
                user.PersonId = (int)rows[0]["PersonId"];
                user.IsActive = (bool)rows[0]["IsActive"];
            }
            else
            {
                MessageBox.Show("Wrong UserName");
                return;
            }

            if (!user.IsActive)
            {
                MessageBox.Show("User is not Active");
                return;
            }
            if (txtbPassword.Text == user.Password)
            {
                this.Hide();
                RememberMe();
                clsFormSettings.User = user;    
                Form frm = new frmMain();
                frm.ShowDialog();
                this.Close();
            }
            else
            {
                MessageBox.Show("Wrong Password");
            }
        }

        DataTable dtUsers =clsUser.GetAllUsersInfo();

        private void RememberMe()
        {
            if (cbRememberMe.Checked)
            {
                Properties.Settings.Default.UserName = txtbUserName.Text;
                Properties.Settings.Default.Password = txtbPassword.Text;
                
            }
            else
            {
                Properties.Settings.Default.UserName = null;
                Properties.Settings.Default.Password = null;

            }

            Properties.Settings.Default.Save();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {

            if (_isSignOut)
            {
                txtbUserName.Text = "";
                txtbPassword.Text = "";
                cbRememberMe.Checked = false;
                return;
            }

            if (!string.IsNullOrEmpty(Properties.Settings.Default.UserName))
            {
                txtbUserName.Text = Properties.Settings.Default.UserName;
                txtbPassword.Text = Properties.Settings.Default.Password;
                cbRememberMe.Checked = true;
                button1_Click(null, null);
                return;
            }

        }

        private void cbRememberMe_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
