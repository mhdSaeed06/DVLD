using DVLDBusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    public partial class frmAddEditPerson : Form
    {
        int _PersonId ;
        clsPerson _Person;

        public enum enMode { AddNew = 0, Update = 1 };
        private enMode _Mode;


        public delegate void PersonAddedHandler(clsPerson person);
        public event PersonAddedHandler PersonAdded;

        public frmAddEditPerson(int PersonId)
        {
            InitializeComponent();
            _PersonId = PersonId;

            if (PersonId == -1)
            {
                _Mode = enMode.AddNew;
            }
            else
            {
                _Mode= enMode.Update;
            }

        }
            

        private void _LoadData()
        {
            _FillCountriesInComboBox();
            cbCountries.SelectedIndex = cbCountries.FindString("Syria");
            dateTimePicker1.MaxDate = DateTime.Today.AddYears(-18);

            if (_Mode == enMode.AddNew)
            {
                lblTitle.Text = "Add New Person";
                _Person = new clsPerson();
                return; 
            }


            _Person = clsPerson.Find(_PersonId);

            if (_Person == null)
            {
                MessageBox.Show("This form will be closed because No Person with ID = " + _PersonId);
                this.Close();

                return;
            }

            lblTitle.Text = "Update Mode";
            lblPersonId.Text = _PersonId.ToString();

            txtbFirstName.Text = _Person.FirstName;
            txtbSecondName.Text = _Person.SecondName;
            txtbThirdName.Text = _Person.ThirdName;
            txtbLastName.Text = _Person.LastName;
            txtbNationalNo.Text = _Person.NationalNumber.ToString();
            txtbPhone.Text = _Person.Phone;
            if (_Person.gender == clsPerson.enGender.Male)
                rbtnMale.Select();
            else 
                rbtnFemale.Select();

            cbCountries.SelectedIndex = _Person.NationalityCountryId;

            if (txtbEmail != null)
                txtbEmail.Text = _Person.Email;
            
            txtbAddress.Text = _Person.Address;
            dateTimePicker1.Value = _Person.DateOfBirth;
            

        }

        private void _FillCountriesInComboBox()
        {
            DataTable dataTable = clsCountry.GetAllCountries();

            foreach (DataRow row in dataTable.Rows)
            {

                cbCountries.Items.Add(row["CountryName"]);
            }

        }

        private void frmAddEditPerson_Load(object sender, EventArgs e)
        {
            _LoadData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
                return;
            

            _Person.FirstName = txtbFirstName.Text;
            _Person.SecondName = txtbSecondName.Text;
            _Person.ThirdName = txtbThirdName.Text;
            _Person.LastName = txtbLastName.Text;
            _Person.NationalNumber = txtbNationalNo.Text;
            _Person.Phone = txtbPhone.Text;
            _Person.Address = txtbAddress.Text;
            _Person.Email = txtbEmail.Text;
            _Person.NationalityCountryId = cbCountries.SelectedIndex;
            _Person.DateOfBirth = dateTimePicker1.Value;

            if (rbtnMale.Checked)
                _Person.gender = clsPerson.enGender.Male;
            else
                _Person.gender = clsPerson.enGender.Female;




            if (pictureBox1.ImageLocation != null)
                _Person.ImagePath = pictureBox1.ImageLocation;
            else
                _Person.ImagePath = "";


             if (_Person.Save())
            {
                MessageBox.Show("Person Saved Successfully with Id = " + _Person.PersonId);
                _Mode = enMode.Update;
                lblTitle.Text = "Update Person";
                lblPersonId.Text = _Person.PersonId.ToString();
                PersonAdded?.Invoke(_Person);
            }
            else
                MessageBox.Show("Failed In save This Person");

            


        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Process the selected file
                string selectedFilePath = openFileDialog1.FileName;
                //MessageBox.Show("Selected Image is:" + selectedFilePath);

                pictureBox1.Load(selectedFilePath);
                // ...
            }
        }

        private void rbtnFemale_CheckedChanged(object sender, EventArgs e)
        {
            _Person.gender = clsPerson.enGender.Female;
            pictureBox1.Image = Properties.Resources.Female_512;
        }

        private void rbtnMale_CheckedChanged(object sender, EventArgs e)
        {
            _Person.gender = clsPerson.enGender.Male;
            pictureBox1.Image = Properties.Resources.Male_512;
        }

        private void txtbFirstName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtbFirstName.Text))
            {
                e.Cancel = true;
                txtbFirstName.Focus();
                errorProvider1.SetError(txtbFirstName, "FirstName should have a value");

            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtbFirstName, "");
            }
        }

        private void txtbSecondName_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtbThirdName_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtbLastName_TextChanged(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void txtbSecondName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtbSecondName.Text))
            {
                e.Cancel = true;
                txtbSecondName.Focus();
                errorProvider1.SetError(txtbSecondName, "SecondName should have a value");

            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtbSecondName, "");
            }
        }

        private void txtbThirdName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtbThirdName.Text))
            {
                e.Cancel = true;
                txtbThirdName.Focus();
                errorProvider1.SetError(txtbThirdName, "ThirdName should have a value");

            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtbThirdName, "");
            }
        }

        private void txtbLastName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtbLastName.Text))
            {
                e.Cancel = true;
                txtbLastName.Focus();
                errorProvider1.SetError(txtbLastName, "LastName should have a value");

            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtbLastName, "");
            }
        }

        private void txtbNationalNo_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtbNationalNo.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtbNationalNo, "National should have a value");
            }
            else if (clsPerson.IsPersonExist(txtbNationalNo.Text) && _Mode == enMode.AddNew)
            {
               
                e.Cancel = true;
                errorProvider1.SetError(txtbNationalNo, "NationalNo is used");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtbNationalNo, "");
            }


        }

       

        private void txtbEmail_Validating(object sender, CancelEventArgs e)
        {
            if (!txtbEmail.Text.EndsWith("@gmail.com"))
            {
                e.Cancel = true;
                txtbEmail.Focus();
                errorProvider1.SetError(txtbEmail, "Email should have a value");

            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtbEmail, "");
            }
        }

        private void txtbPhone_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtbAddress_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void txtbAddress_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtbAddress.Text))
            {
                e.Cancel = true;
                txtbAddress.Focus();
                errorProvider1.SetError(txtbAddress, "Address should have a value");

            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtbAddress, "");
            }
        }

        private void txtbPhone_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtbPhone.Text))
            {
                e.Cancel = true;
                txtbPhone.Focus();
                errorProvider1.SetError(txtbPhone, "Phone should have a value");

            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtbPhone, "");
            }
        }

        private void txtbEmail_TextChanged(object sender, EventArgs e)
        {
                
        }
    }
}
