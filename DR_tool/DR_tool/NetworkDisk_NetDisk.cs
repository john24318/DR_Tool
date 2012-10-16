using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Security.Principal;

namespace GL_Utility
{
    public partial class NetworkDisk_NetDisk : Form
    {
        public event NetDiskDialog_OKEventHandler OnNetDiskDialogOKEvent;

        private string _labelName;
        public string LabelName { get { return _labelName; } set { _labelName = value; } }

        public NetworkDisk_NetDisk()
        {
            InitializeComponent();
        }

        private void NetworkDisk_NetDisk_Load(object sender, EventArgs e)
        {
            lblMachine_Value.Text = this.LabelName;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            using (clsDR_tool.UNCAccess unc = new clsDR_tool.UNCAccess())
            {
                string username = txtUsername.Text;
                WindowsIdentity currentIdentity = WindowsIdentity.GetCurrent();
                string domain = currentIdentity.Name.Substring(0, currentIdentity.Name.IndexOf('\\'));
                if (txtUsername.Text.Contains("\\"))
                {
                    domain = txtUsername.Text.Substring(0, txtUsername.Text.IndexOf('\\'));
                    username = txtUsername.Text.Substring(txtUsername.Text.IndexOf('\\') + 1, txtUsername.Text.Length - txtUsername.Text.IndexOf('\\') - 1);
                }

                if (unc.NetUseWithCredentials(this.LabelName.Contains("\\") ? this.LabelName : "\\\\" + this.LabelName + "\\C$", username, domain, txtPassword.Text))
                {
                    OnNetDiskDialogOKEvent(this, new NetDiskDialogEventArgs(this.LabelName, !this.LabelName.Contains("\\"), domain, username, txtPassword.Text, chSavePassword.Checked)); //Send event to the NetworkDisk_Form
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Input name or password is incorrect!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

    }
}