using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ORGnice
{
    public partial class FrmEmailNotification : Form
    {
        private EmailNotification emailService;

        public FrmEmailNotification()
        {
            InitializeComponent();
            emailService = new EmailNotification();
            InitializeForm();
        }

        private void InitializeForm()
        {
            this.Text = "Email Notification System";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(800, 700);

            LoadDepartments();
            LoadRoles();
        }

        private void LoadDepartments()
        {
            try
            {
                var members = Members.GetAllMembersFromDatabase();
                var departments = members.Select(m => m.department)
                                        .Where(d => !string.IsNullOrEmpty(d))
                                        .Distinct()
                                        .OrderBy(d => d)
                                        .ToList();

                cboDepartment.Items.Clear();
                cboDepartment.Items.Add("All Departments");
                foreach (var dept in departments)
                {
                    cboDepartment.Items.Add(dept);
                }
                cboDepartment.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading departments: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadRoles()
        {
            try
            {
                var members = Members.GetAllMembersFromDatabase();
                var roles = members.Select(m => m.role)
                                 .Where(r => !string.IsNullOrEmpty(r))
                                 .Distinct()
                                 .OrderBy(r => r)
                                 .ToList();

                cboRole.Items.Clear();
                cboRole.Items.Add("All Roles");
                foreach (var role in roles)
                {
                    cboRole.Items.Add(role);
                }
                cboRole.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading roles: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTestConfig_Click(object sender, EventArgs e)
        {
            if (!ValidateEmailSettings())
                return;

            try
            {
                btnTestConfig.Enabled = false;
                btnTestConfig.Text = "Testing...";

                var settings = GetEmailSettings();
                bool isValid = emailService.TestEmailConfiguration(settings);

                if (isValid)
                {
                    MessageBox.Show("Email configuration is valid! Test email sent successfully.", "Configuration Test", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Email configuration failed. Please check your settings.", "Configuration Test", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error testing configuration: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnTestConfig.Enabled = true;
                btnTestConfig.Text = "Test Configuration";
            }
        }

        private void btnSendEmail_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
                return;

            try
            {
                btnSendEmail.Enabled = false;
                btnSendEmail.Text = "Sending...";
                progressBar.Visible = true;
                progressBar.Style = ProgressBarStyle.Marquee;

                var settings = GetEmailSettings();
                string subject = txtSubject.Text.Trim();
                string body = txtBody.Text.Trim();
                bool isHtml = chkHtml.Checked;

                EmailNotification.NotificationResult result;

                // Determine send method based on selected options
                if (rbAllMembers.Checked)
                {
                    result = emailService.SendNotificationToAllMembers(settings, subject, body, isHtml);
                }
                else if (rbDepartment.Checked)
                {
                    string department = cboDepartment.SelectedItem.ToString();
                    result = emailService.SendNotificationByDepartment(settings, department, subject, body, isHtml);
                }
                else if (rbRole.Checked)
                {
                    string role = cboRole.SelectedItem.ToString();
                    result = emailService.SendNotificationByRole(settings, role, subject, body, isHtml);
                }
                else // Custom recipients
                {
                    var recipients = txtRecipients.Text.Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries)
                                                      .Select(r => r.Trim())
                                                      .ToList();
                    result = emailService.SendBulkNotification(settings, recipients, subject, body, isHtml);
                }

                ShowResult(result);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending notifications: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnSendEmail.Enabled = true;
                btnSendEmail.Text = "Send Email";
                progressBar.Visible = false;
            }
        }

        private void ShowResult(EmailNotification.NotificationResult result)
        {
            string message = result.Message;
            
            if (result.FailedRecipients.Count > 0)
            {
                message += "\n\nFailed Recipients:\n" + string.Join("\n", result.FailedRecipients);
            }

            MessageBox.Show(message, 
                result.Success ? "Notification Sent" : "Notification Error", 
                MessageBoxButtons.OK, 
                result.Success ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

            // Clear form after successful send
            if (result.Success)
            {
                txtSubject.Clear();
                txtBody.Clear();
                txtRecipients.Clear();
            }
        }

        private bool ValidateForm()
        {
            if (!ValidateEmailSettings())
                return false;

            if (string.IsNullOrWhiteSpace(txtSubject.Text))
            {
                MessageBox.Show("Please enter an email subject.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSubject.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtBody.Text))
            {
                MessageBox.Show("Please enter email content.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBody.Focus();
                return false;
            }

            if (rbCustom.Checked && string.IsNullOrWhiteSpace(txtRecipients.Text))
            {
                MessageBox.Show("Please enter recipient email addresses.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtRecipients.Focus();
                return false;
            }

            if (rbDepartment.Checked && (cboDepartment.SelectedItem == null || cboDepartment.SelectedItem.ToString() == "All Departments"))
            {
                MessageBox.Show("Please select a specific department.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboDepartment.Focus();
                return false;
            }

            if (rbRole.Checked && (cboRole.SelectedItem == null || cboRole.SelectedItem.ToString() == "All Roles"))
            {
                MessageBox.Show("Please select a specific role.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboRole.Focus();
                return false;
            }

            return true;
        }

        private bool ValidateEmailSettings()
        {
            if (string.IsNullOrWhiteSpace(txtSenderEmail.Text))
            {
                MessageBox.Show("Please enter sender email address.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSenderEmail.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtSenderPassword.Text))
            {
                MessageBox.Show("Please enter sender email password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSenderPassword.Focus();
                return false;
            }

            if (!txtSenderEmail.Text.Contains("@") || !txtSenderEmail.Text.Contains("."))
            {
                MessageBox.Show("Please enter a valid email address.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSenderEmail.Focus();
                return false;
            }

            return true;
        }

        private EmailNotification.EmailSettings GetEmailSettings()
        {
            return new EmailNotification.EmailSettings
            {
                SenderEmail = txtSenderEmail.Text.Trim(),
                SenderPassword = txtSenderPassword.Text.Trim(),
                SenderName = string.IsNullOrWhiteSpace(txtSenderName.Text) ? txtSenderEmail.Text.Trim() : txtSenderName.Text.Trim()
            };
        }

        private void btnPreviewRecipients_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> recipients = new List<string>();

                if (rbAllMembers.Checked)
                {
                    recipients = emailService.GetAllMemberEmails();
                }
                else if (rbDepartment.Checked && cboDepartment.SelectedItem != null && cboDepartment.SelectedItem.ToString() != "All Departments")
                {
                    var members = Members.GetMembersByDepartment(cboDepartment.SelectedItem.ToString());
                    recipients = members.Where(m => !string.IsNullOrEmpty(m.email))
                                       .Select(m => m.email)
                                       .ToList();
                }
                else if (rbRole.Checked && cboRole.SelectedItem != null && cboRole.SelectedItem.ToString() != "All Roles")
                {
                    var members = Members.AdvancedSearchMembers(role: cboRole.SelectedItem.ToString());
                    recipients = members.Where(m => !string.IsNullOrEmpty(m.email))
                                       .Select(m => m.email)
                                       .ToList();
                }
                else if (rbCustom.Checked)
                {
                    recipients = txtRecipients.Text.Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries)
                                                  .Select(r => r.Trim())
                                                  .ToList();
                }

                string message = $"Total Recipients: {recipients.Count}\n\nEmail Addresses:\n" + string.Join("\n", recipients);
                
                MessageBox.Show(message, "Recipient Preview", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error previewing recipients: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void rbCustom_CheckedChanged(object sender, EventArgs e)
        {
            txtRecipients.Enabled = rbCustom.Checked;
        }

        private void rbDepartment_CheckedChanged(object sender, EventArgs e)
        {
            cboDepartment.Enabled = rbDepartment.Checked;
        }

        private void rbRole_CheckedChanged(object sender, EventArgs e)
        {
            cboRole.Enabled = rbRole.Checked;
        }

        private void btnEventTemplate_Click(object sender, EventArgs e)
        {
            txtSubject.Text = "New Event Notification";
            txtBody.Text = @"Dear Members,

We are excited to announce a new event:

Event Name: [EVENT NAME]
Date: [EVENT DATE]
Time: [EVENT TIME]
Venue: [VENUE]

Description:
[EVENT DESCRIPTION]

We look forward to your participation!

Best regards,
Organization Management";

            chkHtml.Checked = false;
        }

        private void btnAnnouncementTemplate_Click(object sender, EventArgs e)
        {
            txtSubject.Text = "Important Announcement";
            txtBody.Text = @"Dear Members,

We would like to make the following announcement:

[ANNOUNCEMENT CONTENT]

If you have any questions, please don't hesitate to contact us.

Best regards,
Organization Management";

            chkHtml.Checked = false;
        }
    }
}
