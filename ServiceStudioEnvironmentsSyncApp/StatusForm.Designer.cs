namespace ServiceStudioEnvironmentsSyncApp
{
    partial class StatusForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StatusForm));
            this.labelSync = new System.Windows.Forms.Label();
            this.labelError = new System.Windows.Forms.Label();
            this.labelVersion = new System.Windows.Forms.Label();
            this.linkGithub = new System.Windows.Forms.LinkLabel();
            this.labelBuiltBy = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.panelSide = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panelMainContent = new System.Windows.Forms.Panel();
            this.linkLabelViewLogs = new System.Windows.Forms.LinkLabel();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.panelSide.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panelMainContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // labelSync
            // 
            this.labelSync.AutoSize = true;
            this.labelSync.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSync.ForeColor = System.Drawing.Color.White;
            this.labelSync.Location = new System.Drawing.Point(6, 12);
            this.labelSync.Name = "labelSync";
            this.labelSync.Size = new System.Drawing.Size(110, 13);
            this.labelSync.TabIndex = 1;
            this.labelSync.Text = "Last Sync: {syncTime}";
            // 
            // labelError
            // 
            this.labelError.AutoSize = true;
            this.labelError.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelError.ForeColor = System.Drawing.Color.White;
            this.labelError.Location = new System.Drawing.Point(6, 36);
            this.labelError.Name = "labelError";
            this.labelError.Size = new System.Drawing.Size(92, 13);
            this.labelError.TabIndex = 2;
            this.labelError.Text = "Last Error: {error}";
            // 
            // labelVersion
            // 
            this.labelVersion.AutoSize = true;
            this.labelVersion.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelVersion.ForeColor = System.Drawing.Color.White;
            this.labelVersion.Location = new System.Drawing.Point(12, 237);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(94, 13);
            this.labelVersion.TabIndex = 3;
            this.labelVersion.Text = "Version: {version}";
            // 
            // linkGithub
            // 
            this.linkGithub.AutoSize = true;
            this.linkGithub.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkGithub.ForeColor = System.Drawing.Color.White;
            this.linkGithub.LinkColor = System.Drawing.Color.LightSkyBlue;
            this.linkGithub.Location = new System.Drawing.Point(12, 186);
            this.linkGithub.Name = "linkGithub";
            this.linkGithub.Size = new System.Drawing.Size(102, 13);
            this.linkGithub.TabIndex = 4;
            this.linkGithub.TabStop = true;
            this.linkGithub.Text = "GitHub Repository";
            // 
            // labelBuiltBy
            // 
            this.labelBuiltBy.AutoSize = true;
            this.labelBuiltBy.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelBuiltBy.ForeColor = System.Drawing.Color.White;
            this.labelBuiltBy.Location = new System.Drawing.Point(12, 211);
            this.labelBuiltBy.Name = "labelBuiltBy";
            this.labelBuiltBy.Size = new System.Drawing.Size(169, 13);
            this.labelBuiltBy.TabIndex = 5;
            this.labelBuiltBy.Text = "Built by Miguel \'Kelter\' Antunes";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(206, 233);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // panelSide
            // 
            this.panelSide.BackColor = System.Drawing.Color.Black;
            this.panelSide.Controls.Add(this.pictureBox1);
            this.panelSide.Controls.Add(this.labelBuiltBy);
            this.panelSide.Controls.Add(this.labelVersion);
            this.panelSide.Controls.Add(this.linkGithub);
            this.panelSide.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelSide.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelSide.Location = new System.Drawing.Point(0, 0);
            this.panelSide.Name = "panelSide";
            this.panelSide.Size = new System.Drawing.Size(200, 268);
            this.panelSide.TabIndex = 7;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBox1.Image = global::ServiceStudioEnvironmentsSyncApp.Properties.Resources.app_logo;
            this.pictureBox1.Location = new System.Drawing.Point(34, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(130, 130);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // panelMainContent
            // 
            this.panelMainContent.BackColor = System.Drawing.Color.DimGray;
            this.panelMainContent.Controls.Add(this.linkLabelViewLogs);
            this.panelMainContent.Controls.Add(this.btnClose);
            this.panelMainContent.Controls.Add(this.labelSync);
            this.panelMainContent.Controls.Add(this.labelError);
            this.panelMainContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMainContent.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelMainContent.Location = new System.Drawing.Point(200, 0);
            this.panelMainContent.Name = "panelMainContent";
            this.panelMainContent.Size = new System.Drawing.Size(293, 268);
            this.panelMainContent.TabIndex = 8;
            // 
            // linkLabelViewLogs
            // 
            this.linkLabelViewLogs.AutoSize = true;
            this.linkLabelViewLogs.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabelViewLogs.LinkColor = System.Drawing.Color.LightSkyBlue;
            this.linkLabelViewLogs.Location = new System.Drawing.Point(6, 60);
            this.linkLabelViewLogs.Name = "linkLabelViewLogs";
            this.linkLabelViewLogs.Size = new System.Drawing.Size(80, 13);
            this.linkLabelViewLogs.TabIndex = 7;
            this.linkLabelViewLogs.TabStop = true;
            this.linkLabelViewLogs.Text = "View app logs";
            this.linkLabelViewLogs.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelViewLogs_LinkClicked);
            // 
            // StatusForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(493, 268);
            this.Controls.Add(this.panelMainContent);
            this.Controls.Add(this.panelSide);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "StatusForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ServiceStudio Environments Sync App";
            this.Load += new System.EventHandler(this.StatusForm_Load);
            this.panelSide.ResumeLayout(false);
            this.panelSide.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panelMainContent.ResumeLayout(false);
            this.panelMainContent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label labelSync;
        private System.Windows.Forms.Label labelError;
        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.LinkLabel linkGithub;
        private System.Windows.Forms.Label labelBuiltBy;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.Panel panelSide;
        private System.Windows.Forms.Panel panelMainContent;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.LinkLabel linkLabelViewLogs;
    }
}