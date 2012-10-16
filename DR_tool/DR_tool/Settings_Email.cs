using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace GL_Utility
{
    public partial class Settings_Email : Form
    {
        private string Config_Path = Environment.CurrentDirectory + "\\Config";
        private string Ini_Filename = "Settings_Email.ini";
        private Settings_Email_Common common;
        private const int Settings_Email_CommonID = 91;
        //private const int Settings_Char_CommonID = 92;
        //private const int Settings_Parallel_CommonID = 93;
        private bool isFirstLoadIni = true;

        //Declare delegate name for add ini record
        private delegate void displayIniDelegate(AddIniEventArgs e);


        public Settings_Email()
        {
            InitializeComponent();
        }

        private void Settings_Email_Load(object sender, EventArgs e)
        {
            common = Settings_Email_Common.GetInstance(Settings_Email_CommonID);
            if (isFirstLoadIni == true)
                common.OnAddIniEvent += new AddIniEventHandler(this.OnAddIniEventHandler);

            #region Read SMTP setting
            common.ReadFile_Settings(Config_Path + "\\" + "Settings.ini");
            txtSMTP.Text = common.SMTP;
            txtPort.Text = (common.Port == 0 ? 25 : common.Port).ToString();
            txtFrom.Text = common.From;
            chk_Enable.Checked = common.EnableSend;
            #endregion

            #region Read mail list
            //common.RefreshFile(Config_Path + "\\" + Ini_Filename);
            common.ReadFile_ExcludeEOF(Config_Path + "\\" + Ini_Filename);
            isFirstLoadIni = false;

            Dictionary<int, string> dic = common.GetFileInfo();
            BindingSource source = new BindingSource();
            foreach (KeyValuePair<int, string> record in dic)
                source.Add(new IniFileObject(record.Key, record.Value));
            gvEmail.DataSource = source;
            #endregion
        }

        private void btn_char_Add_Click(object sender, EventArgs e)
        {
            string new_record = "";

            if ((new_record = txtInput.Text) != "")
            {
                //common.ValidateNewRecord(new_record);
                Thread thd = new Thread(new ThreadStart(delegate() { common.ValidateNewRecord(new_record); }));
                thd.IsBackground = true;
                thd.Start();
            }
        }

        private void OnAddIniEventHandler(object sender, AddIniEventArgs e)
        {
            Dictionary<int, string> dic;

            displayIniDelegate display = delegate(AddIniEventArgs ae)
            {
                if (ae.Record_Index.Length == 0 ? false : ae.Record_Index[0] == -1)
                {
                    MessageBox.Show("Input value is existed!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    gvEmail.DataSource = source;
                }
            };
            this.Invoke(display, e);
        }

        private void btnDelete_Ini_Click(object sender, EventArgs e)
        {
            List<int> remove_index = new List<int>();
            foreach (DataGridViewRow row in gvEmail.Rows)
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
            gvEmail.DataSource = source;
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            common.WriteFile_Settings(Config_Path + "\\" + "Settings.ini", new string[] { chk_Enable.Enabled == true ? "1" : "0", txtSMTP.Text, txtPort.Text, txtFrom.Text });
            
            string subject = "DR_tool test";
            string body = "This is a test mail from DR_tool.";
            common.SendMail(subject, body, txtFrom.Text.Trim(), txtSMTP.Text.Trim(), txtPort.Text.Trim() == "" ? 25 : Convert.ToInt32(txtPort.Text.Trim()));
        }
    }
}