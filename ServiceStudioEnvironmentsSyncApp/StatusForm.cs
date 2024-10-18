using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ServiceStudioEnvironmentsSyncApp
{
    public partial class StatusForm : Form
    {

        const string githubUrl = "https://github.com/kelter-antunes/ServiceStudioEnvironmentsSyncApp";

        public StatusForm(string syncTime, string error, Version version)
        {
            InitializeComponent();

            // Initialize labels with the provided information
          
            labelSync.Text = $"Last Sync: {syncTime}";
            labelError.Text = $"Last Error: {error}";
            labelVersion.Text = $"Version: {version}";
            linkGithub.Text = "GitHub Repository";
            linkGithub.Links.Add(0, linkGithub.Text.Length, githubUrl);
            linkGithub.LinkClicked += LinkGithub_LinkClicked;
         
        }

        private void LinkGithub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string target = e.Link.LinkData as string;
            if (!string.IsNullOrEmpty(target))
            {
                Process.Start(new ProcessStartInfo { FileName = target, UseShellExecute = true });
            }
        }

        private void StatusForm_Load(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabelViewLogs_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Specify the folder relative to your project
            string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");

            // Open Windows Explorer at the specified folder
            try
            {
                Process.Start("explorer.exe", folderPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not open the folder: " + ex.Message);
            }
        }
    }
}