namespace GL_Utility
{
    partial class Settings
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Characters Filter");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("E-mail");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Parallel Number");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Schedule Job");
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeLeft = new System.Windows.Forms.TreeView();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(12, 12);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeLeft);
            this.splitContainer1.Size = new System.Drawing.Size(465, 266);
            this.splitContainer1.SplitterDistance = 155;
            this.splitContainer1.TabIndex = 5;
            // 
            // treeLeft
            // 
            this.treeLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeLeft.Location = new System.Drawing.Point(0, 0);
            this.treeLeft.Name = "treeLeft";
            treeNode1.Name = "Char";
            treeNode1.Text = "Characters Filter";
            treeNode2.Name = "Email";
            treeNode2.Text = "E-mail";
            treeNode3.Name = "Parallel";
            treeNode3.Text = "Parallel Number";
            treeNode4.Name = "Shedule";
            treeNode4.Text = "Schedule Job";
            this.treeLeft.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4});
            this.treeLeft.Size = new System.Drawing.Size(155, 266);
            this.treeLeft.TabIndex = 0;
            this.treeLeft.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeLeft_AfterSelect);
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(479, 290);
            this.Controls.Add(this.splitContainer1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Settings";
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.Settings_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeLeft;

    }
}