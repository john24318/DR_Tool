namespace GL_Utility
{
    partial class Settings_Parallel
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
            this.txt_parallel_num = new System.Windows.Forms.TextBox();
            this.btnSetValue = new System.Windows.Forms.Button();
            this.lblParallel_title = new System.Windows.Forms.Label();
            this.lblParallel_num = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txt_parallel_num
            // 
            this.txt_parallel_num.Location = new System.Drawing.Point(96, 25);
            this.txt_parallel_num.Name = "txt_parallel_num";
            this.txt_parallel_num.Size = new System.Drawing.Size(100, 20);
            this.txt_parallel_num.TabIndex = 24;
            this.txt_parallel_num.Text = "5";
            // 
            // btnSetValue
            // 
            this.btnSetValue.Location = new System.Drawing.Point(212, 20);
            this.btnSetValue.Name = "btnSetValue";
            this.btnSetValue.Size = new System.Drawing.Size(83, 28);
            this.btnSetValue.TabIndex = 23;
            this.btnSetValue.Text = "Set Value";
            this.btnSetValue.UseVisualStyleBackColor = true;
            this.btnSetValue.Click += new System.EventHandler(this.btnSetValue_Click);
            // 
            // lblParallel_title
            // 
            this.lblParallel_title.AutoSize = true;
            this.lblParallel_title.Location = new System.Drawing.Point(2, 2);
            this.lblParallel_title.Name = "lblParallel_title";
            this.lblParallel_title.Size = new System.Drawing.Size(195, 13);
            this.lblParallel_title.TabIndex = 22;
            this.lblParallel_title.Text = "Parallel threads will execute at one time:";
            // 
            // lblParallel_num
            // 
            this.lblParallel_num.AutoSize = true;
            this.lblParallel_num.Location = new System.Drawing.Point(12, 28);
            this.lblParallel_num.Name = "lblParallel_num";
            this.lblParallel_num.Size = new System.Drawing.Size(82, 13);
            this.lblParallel_num.TabIndex = 21;
            this.lblParallel_num.Text = "Parallel number:";
            // 
            // Settings_Parallel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(295, 269);
            this.Controls.Add(this.txt_parallel_num);
            this.Controls.Add(this.btnSetValue);
            this.Controls.Add(this.lblParallel_title);
            this.Controls.Add(this.lblParallel_num);
            this.Name = "Settings_Parallel";
            this.Text = "Settings_Parallel";
            this.Load += new System.EventHandler(this.Settings_Parallel_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_parallel_num;
        private System.Windows.Forms.Button btnSetValue;
        private System.Windows.Forms.Label lblParallel_title;
        private System.Windows.Forms.Label lblParallel_num;

    }
}