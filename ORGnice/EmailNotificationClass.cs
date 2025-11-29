using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;

namespace ORGnice
{
    public class EmailNotification
    {
        private string connectionString = "server=localhost;port=3306;database=orgdb;uid=root;pwd=Joshua@2004;";
        
        // Read SMTP configuration from App.config
        private static readonly string SmtpServer = ConfigurationManager.AppSettings["SmtpServer"] ?? "smtp.gmail.com";
        private static readonly int SmtpPort = int.Parse(ConfigurationManager.AppSettings["SmtpPort"] ?? "587");
        private static readonly bool EnableSsl = bool.Parse(ConfigurationManager.AppSettings["EnableSsl"] ?? "true");
        private static readonly int EmailTimeout = int.Parse(ConfigurationManager.AppSettings["EmailTimeout"] ?? "30000");
        private static readonly string OrganizationName = ConfigurationManager.AppSettings["OrganizationName"] ?? "Organization";
        
        public class EmailSettings
        {
            public string SenderEmail { get; set; }
            public string SenderPassword { get; set; } // Use App Password for Gmail
            public string SenderName { get; set; }
        }
        
        public class NotificationResult
        {
            public bool Success { get; set; }
            public string Message { get; set; }
            public int TotalAttempted { get; set; }
            public int SuccessfulSent { get; set; }
            public List<string> FailedRecipients { get; set; } = new List<string>();
        }

        /// <summary>
        /// Send notification to all members in the database
        /// </summary>
        public NotificationResult SendNotificationToAllMembers(EmailSettings settings, string subject, string body, bool isHtml = false)
        {
            try
            {
                List<Members> allMembers = Members.GetAllMembersFromDatabase();
                List<string> recipients = allMembers.Where(m => !string.IsNullOrEmpty(m.email))
                                                  .Select(m => m.email)
                                                  .ToList();
                
                return SendBulkNotification(settings, recipients, subject, body, isHtml);
            }
            catch (Exception ex)
            {
                return new NotificationResult
                {
                    Success = false,
                    Message = $"Error getting member list: {ex.Message}",
                    TotalAttempted = 0,
                    SuccessfulSent = 0
                };
            }
        }

        /// <summary>
        /// Send notification to members by department
        /// </summary>
        public NotificationResult SendNotificationByDepartment(EmailSettings settings, string department, string subject, string body, bool isHtml = false)
        {
            try
            {
                List<Members> departmentMembers = Members.GetMembersByDepartment(department);
                List<string> recipients = departmentMembers.Where(m => !string.IsNullOrEmpty(m.email))
                                                          .Select(m => m.email)
                                                          .ToList();
                
                return SendBulkNotification(settings, recipients, subject, body, isHtml);
            }
            catch (Exception ex)
            {
                return new NotificationResult
                {
                    Success = false,
                    Message = $"Error getting department members: {ex.Message}",
                    TotalAttempted = 0,
                    SuccessfulSent = 0
                };
            }
        }

        /// <summary>
        /// Send notification to members by role
        /// </summary>
        public NotificationResult SendNotificationByRole(EmailSettings settings, string role, string subject, string body, bool isHtml = false)
        {
            try
            {
                List<Members> roleMembers = Members.AdvancedSearchMembers(role: role);
                List<string> recipients = roleMembers.Where(m => !string.IsNullOrEmpty(m.email))
                                                    .Select(m => m.email)
                                                    .ToList();
                
                return SendBulkNotification(settings, recipients, subject, body, isHtml);
            }
            catch (Exception ex)
            {
                return new NotificationResult
                {
                    Success = false,
                    Message = $"Error getting role members: {ex.Message}",
                    TotalAttempted = 0,
                    SuccessfulSent = 0
                };
            }
        }

        /// <summary>
        /// Send notification to specific email addresses
        /// </summary>
        public NotificationResult SendBulkNotification(EmailSettings settings, List<string> recipients, string subject, string body, bool isHtml = false)
        {
            var result = new NotificationResult
            {
                TotalAttempted = recipients.Count,
                SuccessfulSent = 0
            };

            if (recipients.Count == 0)
            {
                result.Success = false;
                result.Message = "No recipients provided";
                return result;
            }

            try
            {
                using (SmtpClient smtpClient = new SmtpClient(SmtpServer, SmtpPort))
                {
                    smtpClient.EnableSsl = EnableSsl;
                    smtpClient.Credentials = new NetworkCredential(settings.SenderEmail, settings.SenderPassword);
                    smtpClient.Timeout = EmailTimeout;

                    foreach (string recipient in recipients)
                    {
                        try
                        {
                            if (IsValidEmail(recipient))
                            {
                                using (MailMessage mail = new MailMessage())
                                {
                                    mail.From = new MailAddress(settings.SenderEmail, settings.SenderName);
                                    mail.To.Add(recipient);
                                    mail.Subject = subject;
                                    mail.Body = body;
                                    mail.IsBodyHtml = isHtml;
                                    mail.Priority = MailPriority.Normal;

                                    smtpClient.Send(mail);
                                    result.SuccessfulSent++;
                                }
                            }
                            else
                            {
                                result.FailedRecipients.Add($"{recipient} (invalid email format)");
                            }
                        }
                        catch (Exception ex)
                        {
                            result.FailedRecipients.Add($"{recipient} (error: {ex.Message})");
                        }
                    }
                }

                result.Success = result.SuccessfulSent > 0;
                result.Message = $"Successfully sent {result.SuccessfulSent} out of {result.TotalAttempted} notifications.";
                
                if (result.FailedRecipients.Count > 0)
                {
                    result.Message += $" {result.FailedRecipients.Count} failed.";
                }

                LogNotification(subject, body, result.TotalAttempted, result.SuccessfulSent, settings.SenderEmail);
                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"SMTP Error: {ex.Message}";
                return result;
            }
        }

        /// <summary>
        /// Send event notification with enhanced formatting
        /// </summary>
        public NotificationResult SendEventNotification(EmailSettings settings, string eventName, string eventDate, string eventTime, string venue, string description)
        {
            string subject = $"New Event: {eventName} - {OrganizationName}";
            
            string body = $@"
                <html>
                <body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333;'>
                    <div style='max-width: 600px; margin: 0 auto; padding: 20px;'>
                        <div style='text-align: center; background-color: #f8f9fa; padding: 20px; border-radius: 10px; margin-bottom: 20px;'>
                            <h1 style='color: #2c3e50; margin: 0;'>{OrganizationName}</h1>
                            <h2 style='color: #e74c3c; margin: 10px 0;'>Event Notification</h2>
                        </div>
                        
                        <div style='background-color: #ffffff; padding: 30px; border: 1px solid #ddd; border-radius: 10px; box-shadow: 0 2px 5px rgba(0,0,0,0.1);'>
                            <h3 style='color: #2980b9; margin-top: 0; font-size: 24px;'>{eventName}</h3>
                            
                            <div style='background-color: #f8f9fa; padding: 20px; border-radius: 5px; margin: 15px 0;'>
                                <p style='margin: 10px 0;'><strong style='color: #34495e;'>📅 Date:</strong> <span style='color: #2c3e50;'>{eventDate}</span></p>
                                <p style='margin: 10px 0;'><strong style='color: #34495e;'>🕐 Time:</strong> <span style='color: #2c3e50;'>{eventTime}</span></p>
                                <p style='margin: 10px 0;'><strong style='color: #34495e;'>📍 Venue:</strong> <span style='color: #2c3e50;'>{venue}</span></p>
                            </div>
                            
                            <div style='margin: 20px 0;'>
                                <h4 style='color: #34495e; margin-bottom: 10px;'>Event Description:</h4>
                                <div style='background-color: #ecf0f1; padding: 15px; border-left: 4px solid #3498db; border-radius: 0 5px 5px 0;'>
                                    <p style='margin: 0; line-height: 1.6;'>{description}</p>
                                </div>
                            </div>
                            
                            <div style='text-align: center; margin-top: 30px; padding-top: 20px; border-top: 1px solid #eee;'>
                                <p style='color: #7f8c8d; font-size: 14px; margin: 0;'>
                                    We look forward to your participation!<br>
                                    <strong>{OrganizationName}</strong>
                                </p>
                            </div>
                        </div>
                        
                        <div style='text-align: center; margin-top: 20px;'>
                            <p style='color: #95a5a6; font-size: 12px; margin: 0;'>
                                This is an automated notification from {OrganizationName}<br>
                                Please do not reply to this email.
                            </p>
                        </div>
                    </div>
                </body>
                </html>";

            return SendNotificationToAllMembers(settings, subject, body, true);
        }

        /// <summary>
        /// Test email configuration
        /// </summary>
        public bool TestEmailConfiguration(EmailSettings settings)
        {
            try
            {
                var testResult = SendSingleNotification(settings, settings.SenderEmail, 
                    "Test Email Configuration", 
                    "This is a test email to verify your configuration. If you received this message, your email settings are working correctly.", 
                    false);
                return testResult.Success;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Send notification to a single recipient
        /// </summary>
        public NotificationResult SendSingleNotification(EmailSettings settings, string recipient, string subject, string body, bool isHtml = false)
        {
            return SendBulkNotification(settings, new List<string> { recipient }, subject, body, isHtml);
        }

        /// <summary>
        /// Get all email addresses from members table
        /// </summary>
        public List<string> GetAllMemberEmails()
        {
            try
            {
                List<Members> allMembers = Members.GetAllMembersFromDatabase();
                return allMembers.Where(m => !string.IsNullOrEmpty(m.email) && IsValidEmail(m.email))
                                .Select(m => m.email)
                                .Distinct()
                                .ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting member emails: {ex.Message}");
                return new List<string>();
            }
        }

        /// <summary>
        /// Log notification to database for tracking
        /// </summary>
        private void LogNotification(string subject, string body, int totalRecipients, int successfulSent, string senderEmail)
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string createTableSql = @"
                    CREATE TABLE IF NOT EXISTS notification_logs (
                        id INT AUTO_INCREMENT PRIMARY KEY,
                        subject VARCHAR(500) NOT NULL,
                        body TEXT,
                        total_recipients INT NOT NULL,
                        successful_sent INT NOT NULL,
                        sender_email VARCHAR(255) NOT NULL,
                        sent_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                    )";

                MySqlCommand createCmd = new MySqlCommand(createTableSql, connection);
                createCmd.ExecuteNonQuery();

                string insertSql = @"
                    INSERT INTO notification_logs (subject, body, total_recipients, successful_sent, sender_email) 
                    VALUES (@subject, @body, @totalRecipients, @successfulSent, @senderEmail)";

                MySqlCommand insertCmd = new MySqlCommand(insertSql, connection);
                insertCmd.Parameters.AddWithValue("@subject", subject);
                insertCmd.Parameters.AddWithValue("@body", body.Length > 1000 ? body.Substring(0, 1000) : body);
                insertCmd.Parameters.AddWithValue("@totalRecipients", totalRecipients);
                insertCmd.Parameters.AddWithValue("@successfulSent", successfulSent);
                insertCmd.Parameters.AddWithValue("@senderEmail", senderEmail);

                insertCmd.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error logging notification: {ex.Message}");
            }
        }

        /// <summary>
        /// Validate email address format
        /// </summary>
        private bool IsValidEmail(string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                    return false;

                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email && email.Contains("@") && email.Contains(".");
            }
            catch
            {
                return false;
            }
        }
    }
}
