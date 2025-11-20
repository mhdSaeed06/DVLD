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
    public partial class frmShowLicenseInfo : Form
    {
        public frmShowLicenseInfo(int LocalDrivingLicenseAppId)
        {
            InitializeComponent();
            _LocalDrivingLicenseAppId = LocalDrivingLicenseAppId;
        }


        int _LocalDrivingLicenseAppId;
        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void frmShowLicenseInfo_Load(object sender, EventArgs e)
        {
            ctrlDriverLicenseInfo1.LoadInfo(_LocalDrivingLicenseAppId);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void ctrlDriverLicenseInfo1_Load(object sender, EventArgs e)
        {

        }
    }
}
