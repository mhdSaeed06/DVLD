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
    public partial class ctrlPersonInformation : UserControl
    {
        public ctrlPersonInformation()
        {
            InitializeComponent();
        }

     
        public clsPerson person { get; private set; }

            public int personId { get; private set; }



            private void _FillCountriesInComboBox()
            {
                DataTable dtCountries = clsCountry.GetAllCountries();

            //foreach (DataRow dtCountry in dtCountries.Rows)
            //{
            //    cbCountries.Items.Add(dtCountry["CountryName"]);
            //}



            cbCountries.DisplayMember = "CountryName";
            cbCountries.ValueMember = "CountryID";
            cbCountries.DataSource = dtCountries;
        }


        private void _ResetForm()
        {
            lblPersonId.Text = "";
            txtbFirstName.Text = "";
            txtbSecondName.Text = "";
            txtbThirdName.Text = "";
            txtbLastName.Text = "";
            txtbAddress.Text = "";
            txtbEmail.Text = "";
            txtbNationalNo.Text = "";
            txtbPhone.Text = "";
            dateTimePicker1.Value = dateTimePicker1.MaxDate;
            cbCountries.SelectedIndex = -1;

            lblGender.Text = "";
            pictureBox1.Image = null;
        }
        public void LoadPerson(int PersonId)
        {


            _FillCountriesInComboBox();

            this.personId = PersonId;
            if (PersonId == -1)
            {
                person = null;
                _ResetForm();
                return;
            }

            person = clsPerson.Find(PersonId);
            if (person == null)
            {
                _ResetForm();
                return;
            }

                lblPersonId.Text = person.PersonId.ToString();
                txtbFirstName.Text = person.FirstName.ToString();
                txtbSecondName.Text = person.SecondName.ToString();
                txtbThirdName.Text = person.ThirdName.ToString();
                txtbLastName.Text = person.LastName.ToString();
                txtbAddress.Text = person.Address.ToString();
                txtbEmail.Text = person.Email.ToString();
                txtbNationalNo.Text = person.NationalNumber.ToString();
                txtbPhone.Text = person.Phone.ToString();
                dateTimePicker1.Value = person.DateOfBirth;
                // cbCountries.SelectedIndex = person.NationalityCountryId;
                cbCountries.SelectedValue = person.NationalityCountryId;
            if (person.gender == clsPerson.enGender.Male)
            {

                lblGender.Text = "Male";
                pictureBox1.Image = Properties.Resources.Male_512;
            }
            else
            {
                lblGender.Text = "Female";
                pictureBox1.Image = Properties.Resources.Female_512;
            }

            if (!string.IsNullOrEmpty(person.ImagePath))
                {
                try
                {
                    pictureBox1.Load(person.ImagePath);

                }
                catch (Exception ex) { MessageBox.Show("No Image in this path(Wrong Path)"); }
                }
                


            }

            private void groupBox1_Enter(object sender, EventArgs e)
            {
                cbCountries.SelectedIndex = cbCountries.FindString("Syria");
            }

            
            private void radioButton2_CheckedChanged(object sender, EventArgs e)
            {


            }

            

        private void ctrlPersonInformation_Load(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void lblPersonId_Click(object sender, EventArgs e)
        {

        }

        private void llUpdatePerson_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form frm = new frmAddEditPerson(personId);
            frm.ShowDialog();
            this.LoadPerson(personId);
        }

        private void lblGender_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void txtbNationalNo_TextChanged(object sender, EventArgs e)
        {

        }
    }
   
}

