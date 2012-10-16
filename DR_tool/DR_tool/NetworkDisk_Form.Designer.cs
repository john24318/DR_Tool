namespace GL_Utility
{
    partial class NetworkDisk_Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.rbNetworkDisk = new System.Windows.Forms.RadioButton();
            this.group = new System.Windows.Forms.GroupBox();
            this.treeFolder = new System.Windows.Forms.TreeView();
            this.lblFolderInfo = new System.Windows.Forms.Label();
            this.txtFTP = new System.Windows.Forms.TextBox();
            this.txtNetworkDisk = new System.Windows.Forms.TextBox();
            this.rbFTP = new System.Windows.Forms.RadioButton();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnShowFolder = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.group.SuspendLayout();
            this.SuspendLayout();
            // 
            // rbNetworkDisk
            // 
            this.rbNetworkDisk.AutoSize = true;
            this.rbNetworkDisk.Checked = true;
            this.rbNetworkDisk.Location = new System.Drawing.Point(6, 10);
            this.rbNetworkDisk.Name = "rbNetworkDisk";
            this.rbNetworkDisk.Size = new System.Drawing.Size(93, 17);
            this.rbNetworkDisk.TabIndex = 0;
            this.rbNetworkDisk.TabStop = true;
            this.rbNetworkDisk.Text = "Network Path:";
            this.rbNetworkDisk.UseVisualStyleBackColor = true;
            this.rbNetworkDisk.CheckedChanged += new System.EventHandler(this.rbNetworkDisk_CheckedChanged);
            // 
            // group
            // 
            this.group.Controls.Add(this.treeFolder);
            this.group.Controls.Add(this.lblFolderInfo);
            this.group.Controls.Add(this.txtFTP);
            this.group.Controls.Add(this.txtNetworkDisk);
            this.group.Controls.Add(this.rbFTP);
            this.group.Controls.Add(this.rbNetworkDisk);
            this.group.Location = new System.Drawing.Point(0, 2);
            this.group.Name = "group";
            this.group.Size = new System.Drawing.Size(491, 178);
            this.group.TabIndex = 1;
            this.group.TabStop = false;
            // 
            // treeFolder
            // 
            this.treeFolder.Location = new System.Drawing.Point(237, 33);
            this.treeFolder.Name = "treeFolder";
            this.treeFolder.Size = new System.Drawing.Size(248, 139);
            this.treeFolder.TabIndex = 32;
            this.treeFolder.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeFolder_NodeMouseDoubleClick);
            this.treeFolder.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeFolder_AfterSelect);
            // 
            // lblFolderInfo
            // 
            this.lblFolderInfo.AutoSize = true;
            this.lblFolderInfo.Location = new System.Drawing.Point(234, 12);
            this.lblFolderInfo.Name = "lblFolderInfo";
            this.lblFolderInfo.Size = new System.Drawing.Size(39, 13);
            this.lblFolderInfo.TabIndex = 31;
            this.lblFolderInfo.Text = "Folder:";
            // 
            // txtFTP
            // 
            this.txtFTP.Enabled = false;
            this.txtFTP.Location = new System.Drawing.Point(12, 91);
            this.txtFTP.Name = "txtFTP";
            this.txtFTP.Size = new System.Drawing.Size(194, 20);
            this.txtFTP.TabIndex = 29;
            this.txtFTP.Text = "ftp://";
            // 
            // txtNetworkDisk
            // 
            this.txtNetworkDisk.Location = new System.Drawing.Point(12, 33);
            this.txtNetworkDisk.Name = "txtNetworkDisk";
            this.txtNetworkDisk.Size = new System.Drawing.Size(194, 20);
            this.txtNetworkDisk.TabIndex = 28;
            this.txtNetworkDisk.Text = "\\\\";
            // 
            // rbFTP
            // 
            this.rbFTP.AutoSize = true;
            this.rbFTP.Location = new System.Drawing.Point(6, 68);
            this.rbFTP.Name = "rbFTP";
            this.rbFTP.Size = new System.Drawing.Size(48, 17);
            this.rbFTP.TabIndex = 1;
            this.rbFTP.TabStop = true;
            this.rbFTP.Text = "FTP:";
            this.rbFTP.UseVisualStyleBackColor = true;
            this.rbFTP.CheckedChanged += new System.EventHandler(this.rbFTP_CheckedChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(411, 190);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(74, 28);
            this.btnCancel.TabIndex = 25;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Enabled = false;
            this.btnOK.Location = new System.Drawing.Point(321, 190);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(74, 28);
            this.btnOK.TabIndex = 24;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnShowFolder
            // 
            this.btnShowFolder.Location = new System.Drawing.Point(104, 190);
            this.btnShowFolder.Name = "btnShowFolder";
            this.btnShowFolder.Size = new System.Drawing.Size(141, 28);
            this.btnShowFolder.TabIndex = 26;
            this.btnShowFolder.Text = "Show Folders";
            this.btnShowFolder.UseVisualStyleBackColor = true;
            this.btnShowFolder.Click += new System.EventHandler(this.btnShowFolder_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Enabled = false;
            this.btnRefresh.Location = new System.Drawing.Point(12, 190);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(74, 28);
            this.btnRefresh.TabIndex = 27;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            // 
            // NetworkDisk_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(492, 230);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnShowFolder);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.group);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NetworkDisk_Form";
            this.Text = "Network Drive";
            this.Load += new System.EventHandler(this.NetworkDisk_Form_Load);
            this.group.ResumeLayout(false);
            this.group.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton rbNetworkDisk;
        private System.Windows.Forms.GroupBox group;
        private System.Windows.Forms.RadioButton rbFTP;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox txtNetworkDisk;
        private System.Windows.Forms.TextBox txtFTP;
        private System.Windows.Forms.Label lblFolderInfo;
        private System.Windows.Forms.TreeView treeFolder;
        private System.Windows.Forms.Button btnShowFolder;
        private System.Windows.Forms.Button btnRefresh;
    }
}