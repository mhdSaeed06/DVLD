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

namespace DVLD.Applications.Manage_ApplicationTypes
{
    public partial class frmEditApplicationTypes : Form
    {
        public frmEditApplicationTypes(int applicationTypeId)
        {
            InitializeComponent();                      
            _ApplicationTypeId = applicationTypeId;
        }
        int _ApplicationTypeId;

        private void _LoadInfo()
        {
            clsApplicationType applicationTypes = clsApplicationType.Find(_ApplicationTypeId);
            if (applicationTypes != null)
            {
                lblId.Text = applicationTypes.ApplicationTypeId.ToString();
                txtbFees.Text = applicationTypes.Fees.ToString();
                txtbTitle.Text = applicationTypes.ApplicationTitle.ToString();
            }
            else
            {
                MessageBox.Show("This Form Will Close because No Application Type In This ID");
                this.Close();
            }
        }
        
        private void frmEditApplicationTypes_Load(object sender, EventArgs e)
        {
            _LoadInfo();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            decimal Fees = Convert.ToDecimal(txtbFees.Text);
            string Title = txtbTitle.Text;
            if (clsApplicationType.UpdateApplicationType(_ApplicationTypeId, Fees, Title))
                MessageBox.Show("Updated Successfully","Updated",MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            else
                MessageBox.Show("Failed In Update This Application Type", "Failed", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
        }

        private void txtbFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // يلغي الإدخال   
            }
        
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtbFees_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
