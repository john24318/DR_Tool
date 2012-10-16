namespace GL_Utility
{
    partial class Settings_Email
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnDelete_Ini = new System.Windows.Forms.Button();
            this.btnAdd_Ini = new System.Windows.Forms.Button();
            this.gvEmail = new System.Windows.Forms.DataGridView();
            this.CheckItem = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Index = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Email = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtInput = new System.Windows.Forms.TextBox();
            this.lblInput = new System.Windows.Forms.Label();
            this.panel_Add = new System.Windows.Forms.Panel();
            this.chk_Enable = new System.Windows.Forms.CheckBox();
            this.lblSMTP = new System.Windows.Forms.Label();
            this.lblPort = new System.Windows.Forms.Label();
            this.txtSMTP = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.lblFrom = new System.Windows.Forms.Label();
            this.txtFrom = new System.Windows.Forms.TextBox();
            this.btnTest = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gvEmail)).BeginInit();
            this.panel_Add.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnDelete_Ini
            // 
            this.btnDelete_Ini.Location = new System.Drawing.Point(213, 55);
            this.btnDelete_Ini.Name = "btnDelete_Ini";
            this.btnDelete_Ini.Size = new System.Drawing.Size(83, 28);
            this.btnDelete_Ini.TabIndex = 23;
            this.btnDelete_Ini.Text = "Delete";
            this.btnDelete_Ini.UseVisualStyleBackColor = true;
            this.btnDelete_Ini.Click += new System.EventHandler(this.btnDelete_Ini_Click);
            // 
            // btnAdd_Ini
            // 
            this.btnAdd_Ini.Location = new System.Drawing.Point(213, 21);
            this.btnAdd_Ini.Name = "btnAdd_Ini";
            this.btnAdd_Ini.Size = new System.Drawing.Size(83, 28);
            this.btnAdd_Ini.TabIndex = 22;
            this.btnAdd_Ini.Text = "Add Value";
            this.btnAdd_Ini.UseVisualStyleBackColor = true;
            this.btnAdd_Ini.Click += new System.EventHandler(this.btn_char_Add_Click);
            // 
            // gvEmail
            // 
            this.gvEmail.AllowUserToAddRows = false;
            this.gvEmail.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gvEmail.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvEmail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvEmail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CheckItem,
            this.Index,
            this.Email});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gvEmail.DefaultCellStyle = dataGridViewCellStyle2;
            this.gvEmail.Location = new System.Drawing.Point(1, 55);
            this.gvEmail.Name = "gvEmail";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gvEmail.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.gvEmail.RowHeadersVisible = false;
            this.gvEmail.Size = new System.Drawing.Size(203, 151);
            this.gvEmail.TabIndex = 20;
            // 
            // CheckItem
            // 
            this.CheckItem.HeaderText = "";
            this.CheckItem.Name = "CheckItem";
            this.CheckItem.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.CheckItem.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.CheckItem.Width = 30;
            // 
            // Index
            // 
            this.Index.DataPropertyName = "Index";
            this.Index.HeaderText = "#";
            this.Index.Name = "Index";
            this.Index.ReadOnly = true;
            this.Index.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Index.Width = 30;
            // 
            // Email
            // 
            this.Email.DataPropertyName = "Record";
            this.Email.HeaderText = "E-mail";
            this.Email.Name = "Email";
            this.Email.Width = 140;
            // 
            // txtInput
            // 
            this.txtInput.Location = new System.Drawing.Point(43, 26);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(161, 20);
            this.txtInput.TabIndex = 27;
            // 
            // lblInput
            // 
            this.lblInput.AutoSize = true;
            this.lblInput.Location = new System.Drawing.Point(3, 29);
            this.lblInput.Name = "lblInput";
            this.lblInput.Size = new System.Drawing.Size(34, 13);
            this.lblInput.TabIndex = 28;
            this.lblInput.Text = "Input:";
            // 
            // panel_Add
            // 
            this.panel_Add.Controls.Add(this.chk_Enable);
            this.panel_Add.Controls.Add(this.lblInput);
            this.panel_Add.Controls.Add(this.gvEmail);
            this.panel_Add.Controls.Add(this.txtInput);
            this.panel_Add.Controls.Add(this.btnAdd_Ini);
            this.panel_Add.Controls.Add(this.btnDelete_Ini);
            this.panel_Add.Location = new System.Drawing.Point(-1, -1);
            this.panel_Add.Name = "panel_Add";
            this.panel_Add.Size = new System.Drawing.Size(298, 206);
            this.panel_Add.TabIndex = 29;
            // 
            // chk_Enable
            // 
            this.chk_Enable.AutoSize = true;
            this.chk_Enable.Location = new System.Drawing.Point(3, 0);
            this.chk_Enable.Name = "chk_Enable";
            this.chk_Enable.Size = new System.Drawing.Size(231, 17);
            this.chk_Enable.TabIndex = 29;
            this.chk_Enable.Text = "When job finished, send e-mail to below list:";
            this.chk_Enable.UseVisualStyleBackColor = true;
            // 
            // lblSMTP
            // 
            this.lblSMTP.AutoSize = true;
            this.lblSMTP.Location = new System.Drawing.Point(-1, 212);
            this.lblSMTP.Name = "lblSMTP";
            this.lblSMTP.Size = new System.Drawing.Size(40, 13);
            this.lblSMTP.TabIndex = 30;
            this.lblSMTP.Text = "SMTP:";
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(134, 212);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(29, 13);
            this.lblPort.TabIndex = 31;
            this.lblPort.Text = "Port:";
            // 
            // txtSMTP
            // 
            this.txtSMTP.Location = new System.Drawing.Point(45, 209);
            this.txtSMTP.Name = "txtSMTP";
            this.txtSMTP.Size = new System.Drawing.Size(83, 20);
            this.txtSMTP.TabIndex = 32;
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(169, 209);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(34, 20);
            this.txtPort.TabIndex = 33;
            // 
            // lblFrom
            // 
            this.lblFrom.AutoSize = true;
            this.lblFrom.Location = new System.Drawing.Point(-1, 237);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(33, 13);
            this.lblFrom.TabIndex = 34;
            this.lblFrom.Text = "From:";
            // 
            // txtFrom
            // 
            this.txtFrom.Location = new System.Drawing.Point(45, 234);
            this.txtFrom.Name = "txtFrom";
            this.txtFrom.Size = new System.Drawing.Size(158, 20);
            this.txtFrom.TabIndex = 35;
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(212, 209);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(83, 45);
            this.btnTest.TabIndex = 36;
            this.btnTest.Text = "Send Test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // Settings_Email
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(295, 269);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.txtFrom);
            this.Controls.Add(this.lblFrom);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.txtSMTP);
            this.Controls.Add(this.lblPort);
            this.Controls.Add(this.lblSMTP);
            this.Controls.Add(this.panel_Add);
            this.Name = "Settings_Email";
            this.Text = "Settings_Email";
            this.Load += new System.EventHandler(this.Settings_Email_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gvEmail)).EndInit();
            this.panel_Add.ResumeLayout(false);
            this.panel_Add.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnDelete_Ini;
        private System.Windows.Forms.Button btnAdd_Ini;
        private System.Windows.Forms.DataGridView gvEmail;
        private System.Windows.Forms.TextBox txtInput;
        private System.Windows.Forms.Label lblInput;
        private System.Windows.Forms.DataGridViewCheckBoxColumn CheckItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn Index;
        private System.Windows.Forms.DataGridViewTextBoxColumn Email;
        private System.Windows.Forms.Panel panel_Add;
        private System.Windows.Forms.CheckBox chk_Enable;
        private System.Windows.Forms.Label lblSMTP;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.TextBox txtSMTP;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.TextBox txtFrom;
        private System.Windows.Forms.Button btnTest;
    }
}