using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Security.Principal;
using System.IO;
using System.Net;
using System.Threading;

namespace GL_Utility
{
    public delegate void NetDiskDialog_OKEventHandler(object sender, NetDiskDialogEventArgs e); //Get username and password
    public delegate void FTPDialog_OKEventHandler(object sender, FTPDialogEventArgs e); //Get username and password
    public delegate void AddKeyEventHandler(object sender, AddKeyEventArgs e);

    public partial class NetworkDisk_Form : Form
    {
        public event NetDiskEventHandler OnNetDiskEvent;

        private NetworkDisk_NetDisk NetDisk_NetDiskDialog;
        private NetworkDisk_FTP NetDisk_FTPDialog;
        
        //Declare variable for load ini file
        private string Secure_Path = Environment.CurrentDirectory + "\\Secure";
        private string Key_Filename = "Visit.key";
        private SecureCommon common;
        private const int SecureCommonID = 9;

        //Declare variable for SubForm info
        private const int Method_LocalHost = 1;
        private const int Method_NetDisk = 2; //(include Domain Machine)
        private const int Method_FTP = 3;

        private string RootPath = "";
        private string CurrentPath = "";

        public NetworkDisk_Form()
        {
            InitializeComponent();
        }

        private void NetworkDisk_Form_Load(object sender, EventArgs e)
        {
            common = SecureCommon.GetInstance(SecureCommonID);
        }

        private void rbNetworkDisk_CheckedChanged(object sender, EventArgs e)
        {
            txtNetworkDisk.Enabled = true;
            txtFTP.Enabled = false;
            btnRefresh.Enabled = false;
            btnShowFolder.Enabled = true;
            btnOK.Enabled = false;
        }

        private void rbFTP_CheckedChanged(object sender, EventArgs e)
        {
            txtNetworkDisk.Enabled = false;
            txtFTP.Enabled = true;
            btnRefresh.Enabled = false;
            btnShowFolder.Enabled = true;
            btnOK.Enabled = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string UNC_Path = RootPath + "\\" + CurrentPath;
            OnNetDiskEvent(this, new NetDiskEventArgs(UNC_Path));  //Send Destination to the DR_Tool Form
            this.Close();
        }

        private void doUNCAccess(string unc_path, string domain, string userName, string password, bool ClearNodes)
        {
            using (clsDR_tool.UNCAccess unc = new clsDR_tool.UNCAccess())
            {
                if (unc.NetUseWithCredentials(unc_path, userName, domain, password))
                {
                    DirectoryInfo dir_info = new DirectoryInfo(unc_path);
                    List<string> folders = new List<string>();
                    foreach(DirectoryInfo dir in dir_info.GetDirectories())
                        folders.Add(dir.Name);

                    DisplayDirectoryView(treeFolder, folders.ToArray(), ClearNodes);
                }
                else
                {
                    MessageBox.Show("Failed to connect to " + unc_path + "\r\nLastError = " + unc.LastError.ToString(), "Failed to connect", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        private void DisplayDirectoryView(TreeView tree, string[] dirs, bool ClearNodes)
        {
            if (ClearNodes == true)
                tree.Nodes.Clear();

            if (tree.SelectedNode == null || ClearNodes == true)
            {
                foreach (string dir in dirs)
                    tree.Nodes.Add(dir);
            }
            else
            {
                foreach (string dir in dirs)
                    tree.SelectedNode.Nodes.Add(dir);
            }
        }

        private void btnShowFolder_Click(object sender, EventArgs e)
        {
            string[] dirs;
            if (rbNetworkDisk.Checked)
            {
                #region Check txtNetworkDisk format
                bool chkFormat = true;
                if (!txtNetworkDisk.Text.StartsWith(@"\\"))
                {
                    chkFormat = false;
                }
                else if (txtNetworkDisk.Text.Contains(".") && txtNetworkDisk.Text.Split('.').Length != 4) //incorrect IP
                {
                    chkFormat = false;
                }
                else if (txtNetworkDisk.Text.EndsWith("\\") || txtNetworkDisk.Text.EndsWith("/") || txtNetworkDisk.Text.EndsWith(":") || txtNetworkDisk.Text.EndsWith("*") || txtNetworkDisk.Text.EndsWith("?") || txtNetworkDisk.Text.EndsWith("\"") || txtNetworkDisk.Text.EndsWith("<") || txtNetworkDisk.Text.EndsWith(">") || txtNetworkDisk.Text.EndsWith("|"))
                {
                    chkFormat = false;
                }
                if (!chkFormat)
                {
                    MessageBox.Show("Input path is not a UNC path!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                #endregion
                RootPath = txtNetworkDisk.Text.ToLower();

                string machineName;
                bool canReach;
                using(System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping())
                {
                    machineName = txtNetworkDisk.Text.TrimStart('\\');
                    if (machineName.Contains("\\"))
                        machineName = machineName.Substring(0, machineName.IndexOf('\\'));

                    try
                    {
                        canReach = ping.Send(machineName).Status == System.Net.NetworkInformation.IPStatus.Success ? true : false;
                    }
                    catch(Exception ex)
                    {
                        canReach = false;
                    }
                }

                if (!canReach)
                {
                    MessageBox.Show("Can't access the Host", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else if (!txtNetworkDisk.Text.TrimStart('\\').Contains("\\"))
                {
                    using (clsDR_tool.UNCAccess unc = new clsDR_tool.UNCAccess())
                    {
                        dirs = unc.EnumNetShares(machineName);
                    }
                    treeFolder.Nodes.Clear();
                    foreach (string dir in dirs)
                    {
                        treeFolder.Nodes.Add(dir);
                    }
                }
                else
                {
                    if (NetDisk_NetDiskDialog == null)
                    {
                        string UNC_Path = RootPath;
                        foreach (KeyFileObject record in common.GetFileInfo().Values)
                        {
                            if (record.Path.StartsWith("\\") || record.Path.ToLower().StartsWith("ftp://"))
                            {
                                if (UNC_Path.Contains(record.Path.ToLower()))
                                {
                                    doUNCAccess(UNC_Path, record.Domain, record.Username, record.Password, true);
                                    return;
                                }
                            }
                        }

                        NetDisk_NetDiskDialog = new NetworkDisk_NetDisk();
                        NetDisk_NetDiskDialog.LabelName = txtNetworkDisk.Text;
                        NetDisk_NetDiskDialog.OnNetDiskDialogOKEvent += new NetDiskDialog_OKEventHandler(OnNetDiskDialog_OKEventHandler); 
                        NetDisk_NetDiskDialog.FormClosed += delegate(object asender, FormClosedEventArgs ae) { NetDisk_NetDiskDialog = null; };
                        NetDisk_NetDiskDialog.ShowDialog(this);
                    }
                }
            }
            else if (rbFTP.Checked)
            {
                #region Check txtFTP format
                bool chkFormat = true;
                if (!txtFTP.Text.StartsWith("ftp://") && !txtFTP.Text.StartsWith("FTP://"))
                {
                    chkFormat = false;
                }
                else if (txtFTP.Text.Contains(".") && txtFTP.Text.Split('.').Length != 4) //incorrect IP
                {
                    chkFormat = false;
                }
                else if (txtNetworkDisk.Text.EndsWith("\\") || txtNetworkDisk.Text.EndsWith("/") || txtFTP.Text.EndsWith(":") || txtFTP.Text.EndsWith("*") || txtFTP.Text.EndsWith("?") || txtFTP.Text.EndsWith("\"") || txtFTP.Text.EndsWith("<") || txtFTP.Text.EndsWith(">") || txtFTP.Text.EndsWith("|"))
                {
                    chkFormat = false;
                }
                if (!chkFormat)
                {
                    MessageBox.Show("Input format is incorrect", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                #endregion
                RootPath = txtFTP.Text.ToLower();

                if (NetDisk_FTPDialog == null)
                {
                    NetDisk_FTPDialog = new NetworkDisk_FTP();
                    NetDisk_FTPDialog.OnFTPDialogOKEvent += new FTPDialog_OKEventHandler(OnFTPDialog_OKEventHandler); 
                    NetDisk_FTPDialog.FormClosed += delegate(object asender, FormClosedEventArgs ae) { NetDisk_FTPDialog = null; };
                    NetDisk_FTPDialog.ShowDialog(this);
                }
            }
        }

        private void treeFolder_AfterSelect(object sender, TreeViewEventArgs e)
        {
            CurrentPath = e.Node.FullPath.ToLower();
            btnOK.Enabled = true;
        }

        private void treeFolder_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            CurrentPath = e.Node.FullPath.ToLower();
            
            if (rbNetworkDisk.Checked)
            {
                if (NetDisk_NetDiskDialog == null)
                {
                    string UNC_Path = RootPath + "\\" + CurrentPath;
                    foreach (KeyFileObject record in common.GetFileInfo().Values)
                    {
                        if (record.Path.StartsWith("\\") || record.Path.ToLower().StartsWith("ftp://"))
                        {
                            if (UNC_Path.Contains(record.Path.ToLower()))
                            {
                                doUNCAccess(UNC_Path, record.Domain, record.Username, record.Password, false);
                                return;
                            }
                        }
                    }

                    NetDisk_NetDiskDialog = new NetworkDisk_NetDisk();
                    NetDisk_NetDiskDialog.LabelName = UNC_Path;
                    NetDisk_NetDiskDialog.OnNetDiskDialogOKEvent += new NetDiskDialog_OKEventHandler(OnNetDiskDialog_OKEventHandler); 
                    NetDisk_NetDiskDialog.FormClosed += delegate(object asender, FormClosedEventArgs ae) { NetDisk_NetDiskDialog = null; };
                    NetDisk_NetDiskDialog.ShowDialog(this);
                }
            }
            else if (rbFTP.Checked)
            {

            }
        }

        private void OnNetDiskDialog_OKEventHandler(object sender, NetDiskDialogEventArgs e)
        {
            if (!e.PathIsMachineName)
            {
                KeyFileObject key = new KeyFileObject(e.Path, Convert.ToString(Method_NetDisk), e.Domain, e.Username, e.Password);
                common.ValidateNewRecord(key, e.Save_Pwd);
                if (e.Save_Pwd)
                {
                    AESEncrypt AES = new AESEncrypt();
                    string new_record = e.Path + "\t" + AES.Encrypt_AES(key);
                    common.WriteFile(Secure_Path + "\\" + Key_Filename, new_record);
                }
                doUNCAccess(e.Path, e.Domain, e.Username, e.Password, true);
                txtNetworkDisk.Text = e.Path;
                RootPath = txtNetworkDisk.Text.ToLower();
            }    
        }

        private void OnFTPDialog_OKEventHandler(object sender, FTPDialogEventArgs e)
        {
            
        }

    }

    public class NetDiskDialogEventArgs : EventArgs
    {
        public string Path;
        public bool PathIsMachineName;
        public string Domain;
        public string Username;
        public string Password;
        public bool Save_Pwd;

        public NetDiskDialogEventArgs(string Path, bool PathIsMachineName, string Domain, string Username, string Password, bool Save_Pwd)
        {
            this.Path = Path;
            this.PathIsMachineName = PathIsMachineName;
            this.Domain = Domain;
            this.Username = Username;
            this.Password = Password;
            this.Save_Pwd = Save_Pwd;
        }
    }

    public class FTPDialogEventArgs : EventArgs
    {
        public string FTP_IP;
        public string Username;
        public string Password;
        public bool Save_Pwd;

        public FTPDialogEventArgs(string FTP_IP, string Username, string Password, bool Save_Pwd)
        {
            this.FTP_IP = FTP_IP;
            this.Username = Username;
            this.Password = Password;
            this.Save_Pwd = Save_Pwd;
        }
    }

    public class NetDiskEventArgs : EventArgs
    {
        public string Destination;

        public NetDiskEventArgs(string Destination)
        {
            this.Destination = Destination;
        }
    }

    #region Secure key file object
    public class KeyFileObject
    {
        private string _Path;
        private string _MethodType;
        private string _Domain;
        private string _Username;
        private string _Password;

        public string Path { get { return _Path; } set { _Path = value; } }
        public string MethodType { get { return _MethodType; } set { _MethodType = value; } }
        public string Domain { get { return _Domain; } set { _Domain = value; } }
        public string Username { get { return _Username; } set { _Username = value; } }
        public string Password { get { return _Password; } set { _Password = value; } }

        public KeyFileObject(string Path, string MethodType, string Domain, string Username, string Password)
        {
            this.Path = Path;
            this.MethodType = MethodType;
            this.Domain = Domain;
            this.Username = Username;
            this.Password = Password;
        }
    }
    #endregion
}