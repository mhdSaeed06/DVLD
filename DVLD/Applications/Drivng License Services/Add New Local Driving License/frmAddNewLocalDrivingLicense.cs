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

namespace DVLD.Applications.Add_New_Local_Driving_License
{
    public partial class frmAddNewLocalDrivingLicense : Form
    {
        public frmAddNewLocalDrivingLicense()
        {
            InitializeComponent();
        }

        private void ctrlFindPerson1_Load(object sender, EventArgs e)
        {

        }

        DataTable dtLicenseClass;

        DataTable dtLicenseType;

        private void frmAddNewLocalDrivingLicense_Load(object sender, EventArgs e)
        {
            dtLicenseClass = clsLicenseClass.GetAllLicenseClassInfo();

            cbLicenseClass.DataSource = dtLicenseClass;
            cbLicenseClass.DisplayMember = "ClassName";
            cbLicenseClass.ValueMember = "LicenseClassID";
            cbLicenseClass.SelectedValue = 3;

            lblApplicationDate.Text = DateTime.Now.ToShortDateString();

            int SelectedId = (int)cbLicenseClass.SelectedValue;
            lblApplicationFees.Text = clsApplicationType.GetApplicationFeesById(SelectedId).ToString();

            lblCreatedBy.Text = clsFormSettings.User.UserName;
                
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (ctrlFindPerson1.Person != null)
                tabControl1.SelectedIndex = 1;
            else
                MessageBox.Show("Please Select Person");
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {
                
        }

        private void label4_Click(object sender, EventArgs e)
        {
                
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            clsApplication application = new clsApplication();
            application.ApplicantPersonId = ctrlFindPerson1.Person.PersonId;
            application.ApplicationDate = DateTime.Now;
            application.ApplicationTypeId = 1;
            application.ApplicationStatus = clsApplication.enApplicationStatus.New;
            application.LastStatusDate = DateTime.Now;
            application.PaidFees = (decimal)clsApplicationType.GetApplicationFeesById((int)cbLicenseClass.SelectedValue);
            application.CreatedByUserId = clsFormSettings.User.UserId;

            int SelectedLicenseClassId = Convert.ToInt32(cbLicenseClass.SelectedValue);
            if (clsLocalDrivingLicenseApplication.IsLicenseClassForThisPersonExist(application.ApplicantPersonId,SelectedLicenseClassId))
            {
                MessageBox.Show("This Person has already an Application In This License Class");
                return;
            }
            else
            {

                if (application.Save())
                {
                    MessageBox.Show("Application Save");
                    clsLocalDrivingLicenseApplication clsLocal = new clsLocalDrivingLicenseApplication();
                    clsLocal.ApplicationId = application.ApplicationId;
                    clsLocal.LicenseClassId = Convert.ToInt32(cbLicenseClass.SelectedValue);

                    if (clsLocal.Save())
                    {
                        MessageBox.Show("Local Driving License Application Saved Successfully");
                        lblApplicationId.Text = clsLocal.LocalDrivingLicenseAppID.ToString();
                    }
                    else
                        MessageBox.Show("Failed");

                }

                else
                {
                    MessageBox.Show("Failed");
                }
            }

        }
    }
}
