namespace GL_Utility
{
    partial class Settings_Char
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
            this.lblCharExclude_list = new System.Windows.Forms.Label();
            this.gvCharExclude = new System.Windows.Forms.DataGridView();
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Index = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Char = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtInput = new System.Windows.Forms.TextBox();
            this.lblInput = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.gvCharExclude)).BeginInit();
            this.SuspendLayout();
            // 
            // btnDelete_Ini
            // 
            this.btnDelete_Ini.Location = new System.Drawing.Point(212, 54);
            this.btnDelete_Ini.Name = "btnDelete_Ini";
            this.btnDelete_Ini.Size = new System.Drawing.Size(83, 28);
            this.btnDelete_Ini.TabIndex = 25;
            this.btnDelete_Ini.Text = "Delete";
            this.btnDelete_Ini.UseVisualStyleBackColor = true;
            this.btnDelete_Ini.Click += new System.EventHandler(this.btnDelete_Ini_Click);
            // 
            // btnAdd_Ini
            // 
            this.btnAdd_Ini.Location = new System.Drawing.Point(212, 20);
            this.btnAdd_Ini.Name = "btnAdd_Ini";
            this.btnAdd_Ini.Size = new System.Drawing.Size(83, 28);
            this.btnAdd_Ini.TabIndex = 24;
            this.btnAdd_Ini.Text = "Add Value";
            this.btnAdd_Ini.UseVisualStyleBackColor = true;
            this.btnAdd_Ini.Click += new System.EventHandler(this.btn_char_Add_Click);
            // 
            // lblCharExclude_list
            // 
            this.lblCharExclude_list.AutoSize = true;
            this.lblCharExclude_list.Location = new System.Drawing.Point(2, 1);
            this.lblCharExclude_list.Name = "lblCharExclude_list";
            this.lblCharExclude_list.Size = new System.Drawing.Size(235, 13);
            this.lblCharExclude_list.TabIndex = 23;
            this.lblCharExclude_list.Text = "Exclude folders which contain below characters:";
            // 
            // gvCharExclude
            // 
            this.gvCharExclude.AllowUserToAddRows = false;
            this.gvCharExclude.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gvCharExclude.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvCharExclude.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvCharExclude.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewCheckBoxColumn1,
            this.Index,
            this.Char});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gvCharExclude.DefaultCellStyle = dataGridViewCellStyle2;
            this.gvCharExclude.Location = new System.Drawing.Point(0, 54);
            this.gvCharExclude.Name = "gvCharExclude";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gvCharExclude.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.gvCharExclude.RowHeadersVisible = false;
            this.gvCharExclude.Size = new System.Drawing.Size(203, 211);
            this.gvCharExclude.TabIndex = 22;
            // 
            // dataGridViewCheckBoxColumn1
            // 
            this.dataGridViewCheckBoxColumn1.HeaderText = "";
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            this.dataGridViewCheckBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewCheckBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dataGridViewCheckBoxColumn1.Width = 30;
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
            // Char
            // 
            this.Char.DataPropertyName = "Record";
            this.Char.HeaderText = "Characters";
            this.Char.Name = "Char";
            this.Char.Width = 140;
            // 
            // txtInput
            // 
            this.txtInput.Location = new System.Drawing.Point(42, 25);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(161, 20);
            this.txtInput.TabIndex = 26;
            // 
            // lblInput
            // 
            this.lblInput.AutoSize = true;
            this.lblInput.Location = new System.Drawing.Point(2, 28);
            this.lblInput.Name = "lblInput";
            this.lblInput.Size = new System.Drawing.Size(34, 13);
            this.lblInput.TabIndex = 27;
            this.lblInput.Text = "Input:";
            // 
            // Settings_Char
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(295, 269);
            this.Controls.Add(this.lblInput);
            this.Controls.Add(this.txtInput);
            this.Controls.Add(this.btnDelete_Ini);
            this.Controls.Add(this.btnAdd_Ini);
            this.Controls.Add(this.lblCharExclude_list);
            this.Controls.Add(this.gvCharExclude);
            this.Name = "Settings_Char";
            this.Text = "Settings_Char";
            this.Load += new System.EventHandler(this.Settings_Char_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gvCharExclude)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnDelete_Ini;
        private System.Windows.Forms.Button btnAdd_Ini;
        private System.Windows.Forms.Label lblCharExclude_list;
        private System.Windows.Forms.DataGridView gvCharExclude;
        private System.Windows.Forms.TextBox txtInput;
        private System.Windows.Forms.Label lblInput;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Index;
        private System.Windows.Forms.DataGridViewTextBoxColumn Char;

    }
}