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

namespace DVLD.Applications
{
    public partial class ctrlApplicationInfo : UserControl
    {
        public ctrlApplicationInfo()
        {
            InitializeComponent();
        }
        public void LoadInfo(clsApplication application)
        {
            lblId.Text = application.ApplicationId.ToString();
            lblStatus.Text = application.ApplicationStatus.ToString();
            lblFees.Text = application.PaidFees.ToString();
            clsApplicationType applicationType = clsApplicationType.Find(application.ApplicationTypeId);
            lblType.Text = applicationType.ApplicationTitle;
            lblDate.Text = application.ApplicationDate.ToString();
            lblStatusDate.Text = application.LastStatusDate.ToString();
            lblCreatedBy.Text = clsUser.Find(application.CreatedByUserId).UserName;
            lblApplicant.Text = clsPerson.Find(application.ApplicantPersonId).FullName; 
            PersonId = application.ApplicantPersonId;
        }

        int PersonId;
        
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form form = new frmPersonInfo(PersonId);
            form.ShowDialog();
        }
    }
}
