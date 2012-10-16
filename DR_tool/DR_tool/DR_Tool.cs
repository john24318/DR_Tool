using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using clsDR_tool;
using System.IO;
using System.Threading;
using Microsoft.VisualBasic;
using GL_Utility.Properties;

namespace GL_Utility
{
    public delegate void CancelEventHandler(CancelEventArgs e);
    public delegate void AddIniEventHandler(object sender, AddIniEventArgs e);
    public delegate void NetDiskEventHandler(object sender, NetDiskEventArgs e);

    public partial class DR_Tool : Form
    {
        private Settings SettingsForm;
        private NetworkDisk_Form NetDiskForm;
        private NetworkDisk_NetDisk NetDisk_NetDiskDialog; //for Domain Machine form use
        private BindingSource LogDataSource = new BindingSource();

        //Declare variable for load ini file
        private string Config_Path = Environment.CurrentDirectory + "\\Config";
        private string Secure_Path = Environment.CurrentDirectory + "\\Secure";
        private string Ini_Filename;
        private string Key_Filename = "Visit.key";
        private string Settings_Filename = "Settings.ini";
        private IniCommon common;
        private SecureCommon secureCommon;
        private const int StructureCommonID = 1;
        private const int DeploymentCommonID = 2;
        private const int SourceCodeCommonID = 3;
        private const int SecureCommonID = 9;
        private const string Domain_Machine_Mark = "<Domain Machine>...";
        private const string Network_Path_Mark = "<Network Path>...";
        private bool[] isFirstLoadIni = new bool[] { true, true, true }; //for 3 tabs use
        private bool[] isRunClick = new bool[] { false, false, false }; //for 3 tabs use
        private Dictionary<string, BindingList<string>> NeedReuseComboBox = new Dictionary<string, BindingList<string>>();

        //Declare variable for SubForm info
        private const int Method_LocalHost = 1;
        private const int Method_NetDisk = 2; //(include Domain Machine)
        private const int Method_FTP = 3;
        private int _MethodType;
        private string _LocalHost_Path;
        private string _NetDisk_Path;
        private string _FTP_IP;
        private string _Username;
        private string _Password;
        public int MethodType { get { return _MethodType; } set { _MethodType = value; } }
        public string LocalHost_Path { get { return _LocalHost_Path; } set { _LocalHost_Path = value; } }
        public string NetDisk_Path { get { return _NetDisk_Path; } set { _NetDisk_Path = value; } }
        public string FTP_IP { get { return _FTP_IP; } set { _FTP_IP = value; } }
        public string Username { get { return _Username; } set { _Username = value; } }
        public string Password { get { return _Password; } set { _Password = value; } }

        //Declare delegate name for progress status
        private delegate void startValueDelegate(ProgressEventArgs e);
        private delegate void doValueDelegate(ProgressEventArgs e);
        private delegate void endValueDelegate(ProgressEventArgs e);

        //Declare delegate name for output log
        private delegate void displayLogDelegate(LogEventArgs e);

        //Declare delegate name for add ini record
        private delegate void displayIniDelegate(AddIniEventArgs e);

        public event CancelEventHandler OnCancelEvent; //Cancel to search files
        
        public DR_Tool()
        {
            InitializeComponent();
        }

        private void DR_Tool_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists(Config_Path))
                Directory.CreateDirectory(Config_Path);
            if (!Directory.Exists(Secure_Path))
                Directory.CreateDirectory(Secure_Path);
            
            secureCommon = SecureCommon.GetInstance(SecureCommonID);
            secureCommon.OnAddKeyEvent += delegate(object asender, AddKeyEventArgs ae)
            {
                secureCommon.AddKeyDictionary(ae.New_Record);

                if (ae.Save_Pwd)
                {
                    secureCommon.SetAdd_ComboBox(ae.New_Record.Path);

                    #region Set ComboBox
                    if (chk1_SameStructure.Checked)
                        Destination_SameStructure_DataSource(this.cb1_Destination);
                    else
                        Destination_DifferentStructure_DataSource(this.cb1_Destination);

                    if (chk2_SameStructure.Checked)
                        Destination_SameStructure_DataSource(this.cb2_Destination);
                    else
                        Destination_DifferentStructure_DataSource(this.cb2_Destination);

                    if (chk3_SameStructure.Checked)
                        Destination_SameStructure_DataSource(this.cb3_Destination);
                    else
                        Destination_DifferentStructure_DataSource(this.cb3_Destination);
                    #endregion
                }
            };
            secureCommon.ReadFile_ExcludeEOF(Secure_Path + "\\" + Key_Filename);

            #region Set ComboBox
            if (chk1_SameStructure.Checked)
                Destination_SameStructure_DataSource(this.cb1_Destination);
            else
                Destination_DifferentStructure_DataSource(this.cb1_Destination);

            if (chk2_SameStructure.Checked)
                Destination_SameStructure_DataSource(this.cb2_Destination);
            else
                Destination_DifferentStructure_DataSource(this.cb2_Destination);

            if (chk3_SameStructure.Checked)
                Destination_SameStructure_DataSource(this.cb3_Destination);
            else
                Destination_DifferentStructure_DataSource(this.cb3_Destination);
            #endregion
        }

        private void Destination_SameStructure_DataSource(ComboBox comboBox)
        {
            BindingSource source = new BindingSource();
            List<string> list = secureCommon.GetAdd_ComboBox();
            list.Sort();
            foreach(string record in list)
            {
                if (!(record.StartsWith("\\") || record.ToLower().StartsWith("ftp://")))
                    source.Add(record);
            }
            source.Add(Domain_Machine_Mark);
            comboBox.DataSource = source;
        }

        private void Destination_DifferentStructure_DataSource(ComboBox comboBox)
        {
            BindingSource source = new BindingSource();
            source.Add("C:\\");
            source.Add("D:\\");
            List<string> list = secureCommon.GetAdd_ComboBox();
            list.Sort();
            foreach (string record in list)
            {
                if (record.StartsWith("\\") || record.ToLower().StartsWith("ftp://"))
                    source.Add(record);
            }
            source.Add(Network_Path_Mark);
            comboBox.DataSource = source;
        }

        private void chk2_SameStructure_CheckedChanged(object sender, EventArgs e)
        {
            if (chk2_SameStructure.Checked)
                Destination_SameStructure_DataSource(this.cb2_Destination);
            else
                Destination_DifferentStructure_DataSource(this.cb2_Destination);
        }

        private void chk3_SameStructure_CheckedChanged(object sender, EventArgs e)
        {
            if (chk3_SameStructure.Checked)
                Destination_SameStructure_DataSource(this.cb3_Destination);
            else
                Destination_DifferentStructure_DataSource(this.cb3_Destination);
        }

        private void OnStartEventHandler(object sender, ProgressEventArgs e)
        {
            startValueDelegate start = delegate(ProgressEventArgs ae)
            {
                if (isRunClick[0] == true)
                {
                    lbl1_Current_Path_Value.Text = ae.Current_Dir;
                    lbl1_Complete_Path_Value.Text = ae.Dir_Info[0].ToString();
                    lbl1Total_Path_Value.Text = ae.Dir_Info[1].ToString();
                }

                if (isRunClick[1] == true)
                {
                    lbl2_Current_Path_Value.Text = ae.Current_Dir;
                    lbl2_Complete_Path_Value.Text = ae.Dir_Info[0].ToString();
                    lbl2Total_Path_Value.Text = ae.Dir_Info[1].ToString();
                    lbl2Current_KBytes_Value.Text = (ae.Bytes[0] / 1000).ToString();
                    lbl2Total_Bytes_Value.Text = (ae.Bytes[1] / 1000).ToString();
                }

                if (isRunClick[2] == true)
                {
                    lbl3_Current_Path_Value.Text = ae.Current_Dir;
                    lbl3_Complete_Path_Value.Text = ae.Dir_Info[0].ToString();
                    lbl3Total_Path_Value.Text = ae.Dir_Info[1].ToString();
                    lbl3Current_KBytes_Value.Text = (ae.Bytes[0] / 1000).ToString();
                    lbl3Total_Bytes_Value.Text = (ae.Bytes[1] / 1000).ToString();
                }
            };
            this.Invoke(start, e);
        }

        private void OnDoEventHandler(object sender, ProgressEventArgs e)
        {
            doValueDelegate now = delegate(ProgressEventArgs ae)
            {
                if (isRunClick[0] == true)
                {
                    lbl1_Current_Path_Value.Text = ae.Current_Dir;
                    lbl1_Complete_Path_Value.Text = ae.Dir_Info[0].ToString();
                }

                if (isRunClick[1] == true)
                {
                    lbl2_Current_Path_Value.Text = ae.Current_Dir;
                    lbl2_Complete_Path_Value.Text = ae.Dir_Info[0].ToString();
                    lbl2Current_KBytes_Value.Text = (ae.Bytes[0] / 1000).ToString();
                }

                if (isRunClick[2] == true)
                {
                    lbl3_Current_Path_Value.Text = ae.Current_Dir;
                    lbl3_Complete_Path_Value.Text = ae.Dir_Info[0].ToString();
                    lbl3Current_KBytes_Value.Text = (ae.Bytes[0] / 1000).ToString();
                }
            };
            this.Invoke(now, e);
        }

        private void OnEndEventHandler(object sender, ProgressEventArgs e)
        {
            endValueDelegate end = delegate(ProgressEventArgs ae)
            {
                if (isRunClick[0] == true)
                {
                    btn1_Run.Text = "Run";
                    isRunClick[0] = false;
                }
                if (isRunClick[1] == true)
                {
                    btn2_Run.Text = "Run";
                    isRunClick[1] = false;
                }
                if (isRunClick[2] == true)
                {
                    btn3_Run.Text = "Run";
                    isRunClick[2] = false;
                }

                Settings_Email_Common common = Settings_Email_Common.GetInstance(0);
                common.ReadFile_Settings(Config_Path + "\\" + Settings_Filename);
                if (common.EnableSend)
                {
                    string subject = "DR_tool completed";
                    string body = "DR_tool has been completed on " + System.DateTime.Now.ToString() + "\n\r" +
                                  "Total " + ae.Dir_Info[1].ToString() + " directories with " + (ae.Bytes[1] / 1000).ToString() + "KB.";
                    common.SendMail(subject, body, common.From, common.SMTP, common.Port);
                }
            };
            this.Invoke(end, e);
        }

        private void OnLogEventHandler(object sender, LogEventArgs e)
        {
            displayLogDelegate display = delegate(LogEventArgs ae)
            {
                string info = null;
                if (ae.InfoType == true)
                    info = "Info";
                else
                    info = "Error";

                LogDataSource.Add(new LogFileObject(ae.Datetime.ToString(), info, ae.Message));
                gvLog.DataSource = LogDataSource;
            };
            this.Invoke(display, e);
        }

        private void OnAddIniEventHandler(object sender, AddIniEventArgs e)
        {
            Dictionary<int, string> dic;

            displayIniDelegate display = delegate(AddIniEventArgs ae)
            {
                Button btn_Run = null;
                if (ae.CommonID == StructureCommonID)
                {
                    btn_Run = this.btn1_Run;
                }
                else if (ae.CommonID == DeploymentCommonID)
                {
                    btn_Run = this.btn2_Run;
                }
                else if (ae.CommonID == SourceCodeCommonID)
                {
                    btn_Run = this.btn3_Run;
                }

                if (ae.Record_Index.Length == 0 ? false : ae.Record_Index[0] == -1)
                {
                    MessageBox.Show("Input value is existed!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (ae.Record_Index.Length > 0)
                {
                    dic = common.GetFileInfo();
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < ae.Record_Index.Length; i++)
                    {
                        sb.Append("[" + ae.Record_Index[i].ToString() + "]  " + dic[ae.Record_Index[i]].ToString() + "\n");
                    }
                    string showMessage = "";
                    if (ae.Record_Index.Length == 1)
                        showMessage = "Below source path already contains input path:\n\n" + sb.ToString() + "\nRemove it?";
                    else
                        showMessage = "Below source pathes already contain input path:\n\n" + sb.ToString() + "\nRemove them?";

                    if (MessageBox.Show(showMessage, "Info", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    {
                        common.RemoveIniDictionary(ae.Record_Index);
                        common.WriteDictionaryToFile(Config_Path + "\\" + Ini_Filename);
                        common.AddIniDictionary(e.New_Record);
                        common.WriteFile(Config_Path + "\\" + Ini_Filename, e.New_Record);
                        dic = common.GetFileInfo();
                        BindingSource source = new BindingSource();
                        foreach (KeyValuePair<int, string> record in common.GetFileInfo())
                        {
                            source.Add(new IniFileObject(record.Key, record.Value));
                        }
                        gvIniFile.DataSource = source;
                    }
                    sb.Remove(0, sb.Length);
                }
                else
                {
                    common.AddIniDictionary(e.New_Record);
                    common.WriteFile(Config_Path + "\\" + Ini_Filename, e.New_Record);
                    dic = common.GetFileInfo();
                    BindingSource source = new BindingSource();
                    foreach (KeyValuePair<int, string> record in common.GetFileInfo())
                    {
                        source.Add(new IniFileObject(record.Key, record.Value));
                    }
                    gvIniFile.DataSource = source;
                }

                btn_Run.Enabled = true;
            };
            this.Invoke(display, e);
        }

        private void SendOperationEvents(EventArgs e)
        {
            if (e is CancelEventArgs)
            {
                OnCancelEvent((CancelEventArgs)e);
            }
        }

        private void btn1_Loadini_Click(object sender, EventArgs e)
        {
            common = StructureCommon.GetInstance(StructureCommonID);
            if (isFirstLoadIni[0] == true)
                common.OnAddIniEvent += new AddIniEventHandler(this.OnAddIniEventHandler);
            
            Ini_Filename = "Structure.ini";
            common.RefreshFile(Config_Path + "\\" + Ini_Filename);
            common.ReadFile_ExcludeEOF(Config_Path + "\\" + Ini_Filename);
            Dictionary<int, string> dic = common.GetFileInfo();
            BindingSource source = new BindingSource();
            foreach (KeyValuePair<int, string> record in dic)
                source.Add(new IniFileObject(record.Key, record.Value));
            gvIniFile.DataSource = source;
            
            isFirstLoadIni[0] = false;
            EnableRunButton(btn1_Run, !(this.NetDisk_Path == null));
            btn2_Run.Enabled = false;
            btn3_Run.Enabled = false;
            btnAdd_Ini.Enabled = true;
            btnDelete_Ini.Enabled = true;
            lblIni_Info.Text = Ini_Filename + " file:";
        }

        private void btn2_Loadini_Click(object sender, EventArgs e)
        {
            common = DeploymentCommon.GetInstance(DeploymentCommonID);
            if (isFirstLoadIni[1] == true)
                common.OnAddIniEvent += new AddIniEventHandler(this.OnAddIniEventHandler);
            
            Ini_Filename = "Deployment.ini";
            common.RefreshFile(Config_Path + "\\" + Ini_Filename);
            common.ReadFile_ExcludeEOF(Config_Path + "\\" + Ini_Filename);
            Dictionary<int, string> dic = common.GetFileInfo();
            BindingSource source = new BindingSource();
            foreach (KeyValuePair<int, string> record in dic)
                source.Add(new IniFileObject(record.Key, record.Value));
            gvIniFile.DataSource = source;
            
            isFirstLoadIni[1] = false;
            btn1_Run.Enabled = false;
            EnableRunButton(btn2_Run, !(this.NetDisk_Path == null && this.FTP_IP == null));
            btn3_Run.Enabled = false;
            btnAdd_Ini.Enabled = true;
            btnDelete_Ini.Enabled = true;
            lblIni_Info.Text = Ini_Filename + " file:";
        }

        private void btn3_Loadini_Click(object sender, EventArgs e)
        {
            common = SourceCodeCommon.GetInstance(SourceCodeCommonID);
            if (isFirstLoadIni[2] == true)
                common.OnAddIniEvent += new AddIniEventHandler(this.OnAddIniEventHandler);

            Ini_Filename = "SourceCode.ini";
            common.RefreshFile(Config_Path + "\\" + Ini_Filename);
            common.ReadFile_ExcludeEOF(Config_Path + "\\" + Ini_Filename);
            Dictionary<int, string> dic = common.GetFileInfo();
            BindingSource source = new BindingSource();
            foreach (KeyValuePair<int, string> record in dic)
                source.Add(new IniFileObject(record.Key, record.Value));
            gvIniFile.DataSource = source;
            
            isFirstLoadIni[2] = false;
            btn1_Run.Enabled = false;
            btn2_Run.Enabled = false;
            EnableRunButton(btn3_Run, !(this.NetDisk_Path == null && this.FTP_IP == null));
            btnAdd_Ini.Enabled = true;
            btnDelete_Ini.Enabled = true;
            lblIni_Info.Text = Ini_Filename + " file:";
        }

        private void btn1_Run_Click(object sender, EventArgs e)
        {
            if (btn1_Run.Text == "Run")
            {
                btn1_Run.Text = "Cancel";
                isRunClick[0] = true;

                StructureCreater sc = new StructureCreater();

                //Delegate method for progress event
                sc.OnStartEvent += new ProgressEventHandler(this.OnStartEventHandler);
                sc.OnDoEvent += new ProgressEventHandler(this.OnDoEventHandler);
                sc.OnEndEvent += new ProgressEventHandler(this.OnEndEventHandler);

                //Delegate method for output log event
                sc.OnLogEvent += new LogEventHandler(this.OnLogEventHandler);

                //Delegate method for cancel event
                this.OnCancelEvent += new CancelEventHandler(sc.CancelReplicate);

                //Start create folders
                #region Set Properties for FileReplicator
                sc.NetDiskPath = this.NetDisk_Path;
                sc.Username = this.Username;
                sc.Password = this.Password;
                sc.SourcePath = common.GetFileInfo();
                //Settings_Parallel_Common parallel_common = Settings_Parallel_Common.GetInstance(0);
                //parallel_common.ReadFile_Settings(Config_Path + "\\" + Settings_Filename);
                //sc.THREAD_NUM = parallel_common.Parallel_Num;
                Settings_Char_Common char_common = Settings_Char_Common.GetInstance(0);
                char_common.ReadFile_ExcludeEOF(Config_Path + "\\Settings_Char.ini");
                if (char_common.GetFileInfo().Count != 0)
                    sc.ExcludeChar_FilePath = Config_Path + "\\Settings_Char.ini";
                #endregion

                //sc.CreateFolder(null);
                Thread thd = new Thread(new ThreadStart(delegate() { sc.SearchFolder(null); }));
                thd.IsBackground = true;
                thd.Start();
            }
            else
            {
                if (MessageBox.Show("Do you want to cancel?", "Info", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    SendOperationEvents(new CancelEventArgs(true)); //send cancel event
                    btn1_Run.Text = "Run";
                    btn1_Run.Enabled = false;
                    return;
                }
            }
        }

        private void btn2_Run_Click(object sender, EventArgs e)
        {
            if (btn2_Run.Text == "Run")
            {
                btn2_Run.Text = "Cancel";
                isRunClick[1] = true;

                FileReplicator fr = new FileReplicator();

                //Delegate method for progress event
                fr.OnStartEvent += new ProgressEventHandler(this.OnStartEventHandler);
                fr.OnDoEvent += new ProgressEventHandler(this.OnDoEventHandler);
                fr.OnEndEvent += new ProgressEventHandler(this.OnEndEventHandler);

                //Delegate method for output log event
                fr.OnLogEvent += new LogEventHandler(this.OnLogEventHandler);

                //Delegate method for cancel event
                this.OnCancelEvent += new CancelEventHandler(fr.CancelReplicate);

                //Start copy files
                #region Set Properties for FileReplicator
                if (this.MethodType == Method_NetDisk)
                {
                    fr.MethodType = Method.Method_NetDisk;
                    fr.NetDiskPath = this.NetDisk_Path;
                }
                else if (this.MethodType == Method_FTP)
                {
                    fr.MethodType = Method.Method_FTP;
                    fr.FTP_IP = this.FTP_IP;
                    fr.Username = this.Username;
                    fr.Password = this.Password;
                }
                else if (this.MethodType == Method_LocalHost)
                {
                    fr.MethodType = Method.Method_LocalHost;
                    fr.LocalPath = this.LocalHost_Path;
                }

                fr.CloneStructure = this.chk2_SameStructure.Checked;
                fr.SourcePath = common.GetFileInfo();
                Settings_Parallel_Common parallel_common = Settings_Parallel_Common.GetInstance(0);
                parallel_common.ReadFile_Settings(Config_Path + "\\" + Settings_Filename);
                fr.THREAD_NUM = parallel_common.Parallel_Num;
                Settings_Char_Common char_common = Settings_Char_Common.GetInstance(0);
                char_common.ReadFile_ExcludeEOF(Config_Path + "\\Settings_Char.ini");
                if (char_common.GetFileInfo().Count != 0)
                    fr.ExcludeChar_FilePath = Config_Path + "\\Settings_Char.ini";
                #endregion

                //fr.SearchFolder(null);
                Thread thd = new Thread(new ThreadStart(delegate() { fr.SearchFolder(null); }));
                thd.IsBackground = true;
                thd.Start();
            }
            else
            {
                if (MessageBox.Show("Do you want to cancel?", "Info", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    {
                        SendOperationEvents(new CancelEventArgs(true)); //send cancel event
                        btn2_Run.Text = "Run";
                        btn2_Run.Enabled = false;
                        return;
                    }
            }
        }

        private void btn3_Run_Click(object sender, EventArgs e)
        {
            if (btn3_Run.Text == "Run")
            {
                btn3_Run.Text = "Cancel";
                isRunClick[2] = true;

                FileReplicator fr = new FileReplicator();

                //Delegate method for progress event
                fr.OnStartEvent += new ProgressEventHandler(this.OnStartEventHandler);
                fr.OnDoEvent += new ProgressEventHandler(this.OnDoEventHandler);
                fr.OnEndEvent += new ProgressEventHandler(this.OnEndEventHandler);

                //Delegate method for output log event
                fr.OnLogEvent += new LogEventHandler(this.OnLogEventHandler);

                //Delegate method for cancel event
                this.OnCancelEvent += new CancelEventHandler(fr.CancelReplicate);

                //Start copy files
                #region Set Properties for FileReplicator
                if (this.MethodType == Method_NetDisk)
                {
                    fr.MethodType = Method.Method_NetDisk;
                    fr.NetDiskPath = this.NetDisk_Path;
                }
                else if (this.MethodType == Method_FTP)
                {
                    fr.MethodType = Method.Method_FTP;
                    fr.FTP_IP = this.FTP_IP;
                    fr.Username = this.Username;
                    fr.Password = this.Password;
                }
                else if (this.MethodType == Method_LocalHost)
                {
                    fr.MethodType = Method.Method_LocalHost;
                    fr.LocalPath = this.LocalHost_Path;
                }

                fr.CloneStructure = this.chk3_SameStructure.Checked;
                fr.SourcePath = common.GetFileInfo();
                Settings_Parallel_Common parallel_common = Settings_Parallel_Common.GetInstance(0);
                parallel_common.ReadFile_Settings(Config_Path + "\\" + Settings_Filename);
                fr.THREAD_NUM = parallel_common.Parallel_Num;
                Settings_Char_Common char_common = Settings_Char_Common.GetInstance(0);
                char_common.ReadFile_ExcludeEOF(Config_Path + "\\Settings_Char.ini");
                if (char_common.GetFileInfo().Count != 0)
                    fr.ExcludeChar_FilePath = Config_Path + "\\Settings_Char.ini";
                #endregion

                //fr.SearchFolder(null);
                Thread thd = new Thread(new ThreadStart(delegate() { fr.SearchFolder(null); }));
                thd.IsBackground = true;
                thd.Start();
            }
            else
            {
                if (MessageBox.Show("Do you want to cancel?", "Info", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    SendOperationEvents(new CancelEventArgs(true)); //send cancel event
                    btn3_Run.Text = "Run";
                    btn3_Run.Enabled = false;
                    return;
                }
            }
        }

        private void btnAdd_Ini_Click(object sender, EventArgs e)
        {
            string new_path = "";
            if ((new_path = Microsoft.VisualBasic.Interaction.InputBox("Please input source path:", "Info", "", 200, 100).TrimEnd('\\')) != "")
            {
                if (new_path.IndexOf(':') == new_path.Length - 1)
                    new_path += "\\";

                btn1_Run.Enabled = false;
                btn2_Run.Enabled = false;
                btn3_Run.Enabled = false;
                //common.ValidateNewRecord(new_path);
                Thread thd = new Thread(new ThreadStart(delegate() { common.ValidateNewRecord(new_path); }));
                thd.IsBackground = true;
                thd.Start();
            }
        }

        private void btnDelete_Ini_Click(object sender, EventArgs e)
        {
            List<int> remove_index = new List<int>();
            foreach (DataGridViewRow row in gvIniFile.Rows)
            {
                if (Convert.ToBoolean(row.Cells[0].Value) == true)
                {
                    remove_index.Add(Convert.ToInt32(row.Cells[1].Value));
                }
            }
            common.RemoveIniDictionary(remove_index.ToArray());
            common.WriteDictionaryToFile(Config_Path + "\\" + Ini_Filename);
            Dictionary<int, string> dic = common.GetFileInfo();
            BindingSource source = new BindingSource();
            foreach (KeyValuePair<int, string> record in common.GetFileInfo())
            {
                source.Add(new IniFileObject(record.Key, record.Value));
            }
            gvIniFile.DataSource = source;
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SettingsForm == null)
            {
                SettingsForm = new Settings();
                SettingsForm.FormClosed += delegate(object asender, FormClosedEventArgs ae) { SettingsForm = null; };
                SettingsForm.ShowDialog(this);
            }
        }

        private void cb1_Destination_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ComboBox cbox = (ComboBox)sender;
            if (cbox.Items.Count > 0)
            {
                if (cbox.SelectedItem.ToString() == Domain_Machine_Mark)
                {
                    this.NetDisk_Path = null;
                    pic1_Hdd.Image = Resources.network_machine_icon;
                    ShowDomainForm(cbox);
                }
            }
        }

        private void cb2_Destination_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ComboBox cbox = (ComboBox) sender;
            if (cbox.Items.Count > 0)
            {
                if (cbox.SelectedItem.ToString() == Domain_Machine_Mark || (cbox.SelectedItem.ToString().StartsWith("\\") == false && cbox.SelectedItem.ToString().Contains(":") == false && cbox.SelectedItem.ToString().Contains("<") == false))
                {
                    this.NetDisk_Path = null;
                    pic2_Hdd.Image = Resources.network_machine_icon;
                    ShowDomainForm(cbox);
                }
                else if (cbox.SelectedItem.ToString() == Network_Path_Mark || cbox.SelectedItem.ToString().StartsWith("\\") || cbox.SelectedItem.ToString().StartsWith("ftp://"))
                {
                    this.NetDisk_Path = null;
                    this.FTP_IP = null;
                    pic2_Hdd.Image = Resources.network_hdd_icon;
                    ShowNetworkDiskForm(cbox);
                }
                else //Local disk
                {
                    pic2_Hdd.Image = Resources.local_hdd_icon;
                }
            }
        }

        private void cb3_Destination_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ComboBox cbox = (ComboBox)sender;
            if (cbox.Items.Count > 0)
            {
                if (cbox.SelectedItem.ToString() == Domain_Machine_Mark || (cbox.SelectedItem.ToString().StartsWith("\\") == false && cbox.SelectedItem.ToString().Contains(":") == false && cbox.SelectedItem.ToString().Contains("<") == false))
                {
                    this.NetDisk_Path = null;
                    pic3_Hdd.Image = Resources.network_machine_icon;
                    ShowDomainForm(cbox);
                }
                else if (cbox.SelectedItem.ToString() == Network_Path_Mark || cbox.SelectedItem.ToString().StartsWith("\\") || cbox.SelectedItem.ToString().StartsWith("ftp://"))
                {
                    this.NetDisk_Path = null;
                    this.FTP_IP = null;
                    pic3_Hdd.Image = Resources.network_hdd_icon;
                    ShowNetworkDiskForm(cbox);
                }
                else //Local disk
                {
                    pic3_Hdd.Image = Resources.local_hdd_icon;
                }
            }
        }

        private void ShowDomainForm(ComboBox comboBox)
        {
            string machineName;
            if ((machineName = Microsoft.VisualBasic.Interaction.InputBox("Please input Machine Name or IP:", "Info", "", 200, 100).Trim()) != "")
            {
                bool chkFormat = true;
                if (machineName.Contains("\\"))
                {
                    chkFormat = false;
                }
                else if (machineName.Contains(".") && machineName.Split('.').Length != 4) //incorrect IP
                {
                    chkFormat = false;
                }
                if (!chkFormat)
                {
                    MessageBox.Show("Input format is incorrect!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                bool canReach;
                using (System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping())
                {
                    try
                    {
                        canReach = ping.Send(machineName).Status == System.Net.NetworkInformation.IPStatus.Success ? true : false;
                    }
                    catch (Exception ex)
                    {
                        canReach = false;
                    }
                }
                if (!canReach)
                {
                    MessageBox.Show("Can't access the Host", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (NetDisk_NetDiskDialog == null && comboBox.SelectedItem.ToString() == Domain_Machine_Mark) //for Domain Machine form use
                {
                    foreach (KeyFileObject record in secureCommon.GetFileInfo().Values)
                    {
                        if (!(record.Path.StartsWith("\\") || record.Path.ToLower().StartsWith("ftp://")))
                        {
                            if (record.Path.ToLower() == machineName)
                            {
                                SetDestinationParams(comboBox, machineName);
                                return;
                            }
                        }
                    }

                    NetDisk_NetDiskDialog = new NetworkDisk_NetDisk();
                    NetDisk_NetDiskDialog.OnNetDiskDialogOKEvent += delegate(object asender, NetDiskDialogEventArgs ae)
                    {
                        if (ae.PathIsMachineName)
                        {
                            this.NetDisk_Path = ae.Path;
                            this.MethodType = Method_NetDisk;

                            KeyFileObject key = new KeyFileObject(ae.Path, Convert.ToString(Method_NetDisk), ae.Domain, ae.Username, ae.Password);
                            secureCommon.ValidateNewRecord(key, ae.Save_Pwd);
                            if (ae.Save_Pwd)
                            {
                                AESEncrypt AES = new AESEncrypt();
                                string new_record = ae.Path + "\t" + AES.Encrypt_AES(key);
                                secureCommon.WriteFile(Secure_Path + "\\" + Key_Filename, new_record);
                            }

                        }
                        SetDestinationParams(comboBox, machineName);
                    };
                    NetDisk_NetDiskDialog.LabelName = machineName;
                    NetDisk_NetDiskDialog.FormClosed += delegate(object asender, FormClosedEventArgs ae) { NetDisk_NetDiskDialog = null; };
                    NetDisk_NetDiskDialog.ShowDialog(this);
                }
            }
        }

        private void ShowNetworkDiskForm(ComboBox comboBox)
        {
            if (NetDiskForm == null && comboBox.SelectedItem.ToString() == Network_Path_Mark)
            {
                NetDiskForm = new NetworkDisk_Form();
                NetDiskForm.OnNetDiskEvent += delegate(object asender, NetDiskEventArgs ae)
                {
                    SetDestinationParams(comboBox, ae.Destination);
                };
                NetDiskForm.FormClosed += delegate(object asender, FormClosedEventArgs ae) { NetDiskForm = null; };
                NetDiskForm.ShowDialog(this);
            }

            if (comboBox.SelectedItem.ToString() != Network_Path_Mark)
                SetDestinationParams(comboBox, comboBox.SelectedItem.ToString());
        }

        private void SetLocalHostForm()
        {
        }

        private void SetDestinationParams(ComboBox comboBox, string Destination)
        {
            if (!NeedReuseComboBox.ContainsKey(comboBox.Name))
            {
                //Keep the old binging source
                BindingList<string> needReuse = new BindingList<string>();
                BindingSource old_source = new BindingSource();
                old_source.DataSource = comboBox.DataSource;
                foreach (string record in old_source)
                {
                    needReuse.Add(record);
                }
                NeedReuseComboBox.Add(comboBox.Name, needReuse);
            }
            BindingSource source = new BindingSource();
            source.Add(Destination);
            comboBox.DataSource = source;

            //Set params for Run
            KeyFileObject secure_info = null;
            List<string> MachineName_Pathes = new List<string>();
            List<string> UNC_Pathes = new List<string>();
            foreach (KeyFileObject record in secureCommon.GetFileInfo().Values)
            {
                if (record.Path.StartsWith("\\") || record.Path.ToLower().StartsWith("ftp://"))
                {
                    UNC_Pathes.Add(record.Path.ToLower());
                }
                else
                {
                    MachineName_Pathes.Add(record.Path.ToLower());
                }
            }

            if (MachineName_Pathes.Contains(Destination.ToLower()))
            {
                secure_info = secureCommon.GetFileInfo()[Destination];
            }
            else
            {
                foreach (string unc_path in UNC_Pathes)
                {
                    if (Destination.ToLower().Contains(unc_path))
                    {
                        secure_info = secureCommon.GetFileInfo()[unc_path];
                        break;
                    }
                }
            }

            string Method = secure_info.MethodType;
            string Domain = secure_info.Domain;
            string Username = secure_info.Username;
            string Password = secure_info.Password;
            this.MethodType = Convert.ToInt32(Method);
            this.NetDisk_Path = Destination;
            this.Username = Domain + "\\" + Username;
            this.Password = Password;

            if (comboBox.Name == "cb1_Destination")
                EnableRunButton(this.btn1_Run, !isFirstLoadIni[0]);
            if (comboBox.Name == "cb2_Destination")
                EnableRunButton(this.btn2_Run, !isFirstLoadIni[1]);
            if (comboBox.Name == "cb3_Destination")
                EnableRunButton(this.btn3_Run, !isFirstLoadIni[2]);
        }

        private void OnComboBox_MouseDown(object sender, MouseEventArgs e)
        {
            ComboBox cbox = (ComboBox)sender;
            if (NeedReuseComboBox.ContainsKey(cbox.Name))
            {
                //Reload old binging source
                BindingSource old_source = new BindingSource();
                old_source.DataSource = NeedReuseComboBox[cbox.Name];
                cbox.DataSource = old_source;
                NeedReuseComboBox.Remove(cbox.Name);
            }
        }

        private void EnableRunButton(Button RunButton, bool Enable)
        {
            RunButton.Enabled = Enable;
        }

    }

    #region ini/log file object
    class IniFileObject
    {
        private int _Index;
        private string _Record;

        public int Index { get { return _Index; } set { _Index = value; } }
        public string Record { get { return _Record; } set { _Record = value; } }

        public IniFileObject(int Index, string Record)
        {
            this.Index = Index;
            this.Record = Record;
        }
    }

    class LogFileObject
    {
        private string _DateTime;
        private string _Info;
        private string _Message;

        public string DateTime { get { return _DateTime; } set { _DateTime = value; } }
        public string Info { get { return _Info; } set { _Info = value; } }
        public string Message { get { return _Message; } set { _Message = value; } }

        public LogFileObject(string DateTime, string Info, string Message)
        {
            this.DateTime = DateTime;
            this.Info = Info;
            this.Message = Message;
        }
    }
    #endregion
}