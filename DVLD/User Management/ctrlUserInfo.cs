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
    public partial class ctrlUserInfo : UserControl
    {
        public ctrlUserInfo()
        {
            InitializeComponent();
        }

        private void ctrlUserInfo_Load(object sender, EventArgs e)
        {

        }

        public void LoadUser(clsUser User)
        {
            lblUserId.Text = User.UserId.ToString();

            lblUserName.Text = User.UserName.ToString();

            lblIsActive.Text = (User.IsActive) ? "Yes" : "No";

 
        }
    }
}
