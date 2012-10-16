using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace GL_Utility
{
    public partial class Settings_Parallel : Form
    {
        private string Config_Path = Environment.CurrentDirectory + "\\Config";
        private string Ini_Filename = "Settings.ini";
        private Settings_Parallel_Common common;
        //private const int Settings_Email_CommonID = 91;
        //private const int Settings_Char_CommonID = 92;
        private const int Settings_Parallel_CommonID = 94;

        public Settings_Parallel()
        {
            InitializeComponent();
        }

        private void Settings_Parallel_Load(object sender, EventArgs e)
        {
            common = Settings_Parallel_Common.GetInstance(Settings_Parallel_CommonID);
            common.ReadFile_Settings(Config_Path + "\\" + Ini_Filename);
            txt_parallel_num.Text = common.Parallel_Num.ToString();
        }

        private void btnSetValue_Click(object sender, EventArgs e)
        {
            int num;
            try
            {
                num = Convert.ToInt32(txt_parallel_num.Text.Trim());
                common.WriteFile_Settings(Config_Path + "\\" + Ini_Filename, new string[] { num.ToString() });
            }
            catch
            {
                MessageBox.Show("Input data is invalid!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}