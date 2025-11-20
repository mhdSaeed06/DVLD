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

namespace DVLD.Applications.Manage_Test_Type
{
    public partial class frmEditTestType : Form
    {
        public frmEditTestType(int TestTypeId)
        {
            InitializeComponent();
            _TestTypeId = TestTypeId;
        }

        int _TestTypeId;

        private void _LoadInfo()
        {
            clsTestType testType = clsTestType.Find(_TestTypeId);
            if (testType != null)
            {
                lblId.Text = testType.ApplicationTypeId.ToString();
                txtbFees.Text = testType.Fees.ToString();
                txtbTitle.Text = testType.ApplicationTitle.ToString();
                txtbDescription.Text = testType.ApplicationDescription.ToString();
            }
            else
            {
                MessageBox.Show("This Form Will Close because No Test Type In This ID");
                this.Close();
            }
        }


        private void frmEditTestType_Load(object sender, EventArgs e)
        {
            _LoadInfo();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            double Fees = Convert.ToDouble(txtbFees.Text);
            string Title = txtbTitle.Text;
            string Description = txtbDescription.Text;
           
            if (clsTestType.UpdateTestType(_TestTypeId,Fees,Description,Title))
                MessageBox.Show("Updated Successfully", "Updated", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            else
                MessageBox.Show("Failed In Update This Application Type", "Failed", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtbFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; 
            }
        }
    }
}
