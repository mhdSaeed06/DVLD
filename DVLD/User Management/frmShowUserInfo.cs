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
    public partial class frmShowUserInfo : Form
    {
        public frmShowUserInfo(int UserId)
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

        private void frmShowUserInfo_Load(object sender, EventArgs e)
        {
            _LoadForm();
        }
    }
}
