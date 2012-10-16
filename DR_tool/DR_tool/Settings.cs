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
    public partial class Settings : Form
    {
        private string Config_Path = Environment.CurrentDirectory + "\\Config";
        private string Ini_Filename = "Settings.ini";

        private Settings_Email EmailForm;
        private Settings_Char CharForm;
        private Settings_Parallel ParallelForm;

        public Settings()
        {
            InitializeComponent();
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            if (!File.Exists(Config_Path + "\\" + Ini_Filename))
            {
                FileStream f = File.Create(Config_Path + "\\" + Ini_Filename);
                f.Close();
                StreamWriter w = File.AppendText(Config_Path + "\\" + Ini_Filename);
                w.WriteLine("[Parallel Number]");
                w.WriteLine("1");
                w.WriteLine("");
                w.WriteLine("[SMTP]");
                w.WriteLine("0");
                w.Close();
            }
        }

        private void treeLeft_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Index == 0)
            {
                CharForm = new Settings_Char();
                CharForm.TopLevel = false;
                CharForm.FormBorderStyle = FormBorderStyle.None;
                CharForm.WindowState = FormWindowState.Maximized;
                splitContainer1.Panel2.Controls.Add(CharForm);
                CharForm.Show();

                EmailForm = null;
                ParallelForm = null;
            }
            else if (e.Node.Index == 1)
            {
                EmailForm = new Settings_Email();
                EmailForm.TopLevel = false;
                EmailForm.FormBorderStyle = FormBorderStyle.None;
                EmailForm.WindowState = FormWindowState.Maximized;
                splitContainer1.Panel2.Controls.Add(EmailForm);
                EmailForm.Show();

                CharForm = null;
                ParallelForm = null;
            }
            else if (e.Node.Index == 2)
            {
                ParallelForm = new Settings_Parallel();
                ParallelForm.TopLevel = false;
                ParallelForm.FormBorderStyle = FormBorderStyle.None;
                ParallelForm.WindowState = FormWindowState.Maximized;
                splitContainer1.Panel2.Controls.Add(ParallelForm);
                ParallelForm.Show();

                EmailForm = null;
                CharForm = null;
            }
        }

    }
}