using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GL_Utility
{
    public partial class NetworkDisk_FTP : Form
    {
        public event FTPDialog_OKEventHandler OnFTPDialogOKEvent;

        private string _labelName;
        public string LabelName { get { return _labelName; } set { _labelName = value; } }

        public NetworkDisk_FTP()
        {
            InitializeComponent();
        }

        private void NetworkDisk_FTP_Load(object sender, EventArgs e)
        {
            lblHost_Value.Text = this.LabelName;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {

        }

    }
}