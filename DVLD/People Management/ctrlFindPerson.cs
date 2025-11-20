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
    public partial class ctrlFindPerson : UserControl
    {
        public ctrlFindPerson()
        {
            InitializeComponent();
        }

        // DataTable _dtUsers = clsUser.GetAllUsers();

        DataTable _dtPeople = clsPerson.GetAllPeopleInfo();



        public clsPerson Person { get;private set; }

        public enum enMode { AddNew = 0, Update = 1 };

        private enMode _Mode;

        int _UserId;



        private void ctrlPersonInformation1_Load(object sender, EventArgs e)
        {
            _FillComboBox();

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void _FillComboBox()
        {
            cbFilter.Items.Add("PersonId");
            cbFilter.Items.Add("NationalNo");
            cbFilter.SelectedIndex = 1;
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


            Person = clsPerson.Find(_dtPeople, dataColumnName, searchValue);

            if (Person != null)
            {
                ctrlPersonInformation1.LoadPerson(Person.PersonId);
            }
            else
            {
                MessageBox.Show("Not Found");
                ctrlPersonInformation1.LoadPerson(-1);

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmAddEditPerson frm = new frmAddEditPerson(-1);
            frm.PersonAdded += frm_PersonAdded;
            frm.ShowDialog();
        }

        private void frm_PersonAdded(clsPerson person)
        {
            Person = person;

            ctrlPersonInformation1.LoadPerson(person.PersonId);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilter.SelectedItem.ToString() == "PersonId")
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            }

            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }
    }
}
