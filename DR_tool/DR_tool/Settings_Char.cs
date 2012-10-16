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
    public partial class Settings_Char : Form
    {
        private string Config_Path = Environment.CurrentDirectory + "\\Config";
        private string Ini_Filename = "Settings_Char.ini";
        private Settings_Char_Common common;
        //private const int Settings_Email_CommonID = 91;
        private const int Settings_Char_CommonID = 92;
        //private const int Settings_Parallel_CommonID = 93;
        private bool isFirstLoadIni = true;

        //Declare delegate name for add ini record
        private delegate void displayIniDelegate(AddIniEventArgs e);

        public Settings_Char()
        {
            InitializeComponent();
        }

        private void Settings_Char_Load(object sender, EventArgs e)
        {
            common = Settings_Char_Common.GetInstance(Settings_Char_CommonID);
            if (isFirstLoadIni == true)
                common.OnAddIniEvent += new AddIniEventHandler(this.OnAddIniEventHandler);

            //common.RefreshFile(Config_Path + "\\" + Ini_Filename);
            common.ReadFile_ExcludeEOF(Config_Path + "\\" + Ini_Filename);
            isFirstLoadIni = false;
            
            Dictionary<int, string> dic = common.GetFileInfo();
            BindingSource source = new BindingSource();
            foreach (KeyValuePair<int, string> record in dic)
                source.Add(new IniFileObject(record.Key, record.Value));
            gvCharExclude.DataSource = source;
        }

        private void btn_char_Add_Click(object sender, EventArgs e)
        {
            string new_record = "";

            if ((new_record = txtInput.Text.ToLower()) != "")
            {
                //common.ValidateNewRecord(new_char);
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
                        showMessage = "Below record already contains input value:\n\n" + sb.ToString() + "\nRemove it?";
                    else
                        showMessage = "Below records already contain input value:\n\n" + sb.ToString() + "\nRemove them?";

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
                        gvCharExclude.DataSource = source;
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
                    gvCharExclude.DataSource = source;
                }
            };
            this.Invoke(display, e);
        }

        private void btnDelete_Ini_Click(object sender, EventArgs e)
        {
            List<int> remove_index = new List<int>();
            foreach (DataGridViewRow row in gvCharExclude.Rows)
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
            gvCharExclude.DataSource = source;
        }
    }
}