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
    public partial class frmAddEditUser : Form
    {
        public frmAddEditUser(int userId)
        {
            InitializeComponent();

            _UserId = userId;
            if (userId == -1)
            {
                _Mode = enMode.AddNew;
            }
            else
            {
                _Mode = enMode.Update;
            }

        }

        DataTable _dtUsers = clsUser.GetAllUsers();

        DataTable _dtPeople = clsPerson.GetAllPeopleInfo();

        clsPerson _Person;



        public enum enMode { AddNew = 0, Update = 1 };

        private enMode _Mode;

        int _UserId;

        clsUser _User;


        private void _FillComboBox()
        {
            cbFilter.Items.Add("PersonId");
            cbFilter.Items.Add("NationalNo");
        }


        private void _LoadData()
        {
            _FillComboBox();

            if (_Mode == enMode.AddNew)
            {
                lblTitle.Text = "Add New Person";
                _User = new clsUser();
                return;
            }

            _User = clsUser.Find(_UserId);

            _Person = clsPerson.Find(_User.PersonId);

            if (_User == null)
            {
                MessageBox.Show("This Form Will Close beause No User With Id " + _UserId);
                this.Close();
                return;
            }

            groupBox1.Enabled = false;
            groupBox1.Enabled = false;

            lblTitle.Text = "Update User ";

            // cause a big bug
            //_User.PersonId = _Person.PersonId;

            ctrlPersonInformation1.LoadPerson(_User.PersonId);

            lblUserId.Text = _User.UserId.ToString();
            txtbUserName.Text = _User.UserName.ToString();
            txtbPassword.Text = _User.Password.ToString();
            txtbCofirmPassword.Text = _User.Password.ToString();
            cbIsActive.Checked = _User.IsActive;

        }

        private void ctrlPersonInformation1_Load(object sender, EventArgs e)
        {

        }

        private void ctrlPersonInformation1_Load_1(object sender, EventArgs e)
        {

        }

        private void frmAddEditUser_Load(object sender, EventArgs e)
        {
            _LoadData();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (cbFilter.SelectedIndex == -1)
            {
                MessageBox.Show("Please Select Filter");
                return;
            }

            
            string dataColumnName = cbFilter.Text;
            string searchValue = textBox1.Text;


            _Person = clsPerson.Find(_dtPeople, dataColumnName, searchValue);

            if (_Person != null)
            {
                ctrlPersonInformation1.LoadPerson(_Person.PersonId);
            }
            else
            {
                MessageBox.Show("Not Found");
                ctrlPersonInformation1.LoadPerson(-1);

            }




        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (ctrlPersonInformation1.person != null)
                tabControl1.SelectedIndex = 1;
            else
                MessageBox.Show("Please Selete Person");
        }

        private void txtbUserName_Validating(object sender, CancelEventArgs e)
            {
            if (string.IsNullOrWhiteSpace(txtbUserName.Text))
            {
                txtbUserName.Focus();
                errorProvider1.SetError(txtbUserName, "UserName should have a value");

            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtbUserName, "");
            }
        }

        private void txtbPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtbPassword.Text))
            {
                txtbPassword.Focus();
                errorProvider1.SetError(txtbPassword, "Password should have a value");

            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtbPassword, "");
            }
        }

        private void txtbCofirmPassword_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void txtbCofirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtbCofirmPassword.Text))
            {
                txtbCofirmPassword.Focus();
                errorProvider1.SetError(txtbCofirmPassword, "CofirmPassword should have a value");

            }
            else if (txtbCofirmPassword.Text != txtbPassword.Text)
            {
                txtbCofirmPassword.Focus();
                errorProvider1.SetError(txtbCofirmPassword, "CofirmPassword should match Password");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtbCofirmPassword, "");
            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
                return;

           
            _User.PersonId = _Person.PersonId;
            _User.UserName = txtbUserName.Text;
            _User.Password = txtbPassword.Text;
            _User.IsActive = cbIsActive.Checked;

            DataRow[] rows = _dtUsers.Select($"PersonId = {_Person.PersonId}");
            if (rows.Length > 0 && _Mode == enMode.AddNew) 
            {
                MessageBox.Show("User already exists");
                return;
            }

            if (_User.Save())
            {
                MessageBox.Show("User Saved Successfully with Id = " + _User.UserId);
                _Mode = enMode.Update;
                lblTitle.Text = "Update User";
                lblUserId.Text = _User.UserId.ToString();
            }
            else
                MessageBox.Show("Failed In save This User");
        

        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmAddEditPerson frm = new frmAddEditPerson(-1);
            frm.PersonAdded += frm_PersonAdded;
            frm.ShowDialog();
        }

        private void frm_PersonAdded(clsPerson person)
        {
            _Person = person;

            ctrlPersonInformation1.LoadPerson(person.PersonId);
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            if (ctrlPersonInformation1.person != null)
                tabControl1.SelectedIndex = 1;
            else
                MessageBox.Show("Please Select Person");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
