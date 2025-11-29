namespace ORGnice
{
    partial class FrmEmailNotification
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnTestConfig = new System.Windows.Forms.Button();
            this.txtSenderName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSenderPassword = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSenderEmail = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnPreviewRecipients = new System.Windows.Forms.Button();
            this.txtRecipients = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cboRole = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cboDepartment = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.rbCustom = new System.Windows.Forms.RadioButton();
            this.rbRole = new System.Windows.Forms.RadioButton();
            this.rbDepartment = new System.Windows.Forms.RadioButton();
            this.rbAllMembers = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnAnnouncementTemplate = new System.Windows.Forms.Button();
            this.btnEventTemplate = new System.Windows.Forms.Button();
            this.chkHtml = new System.Windows.Forms.CheckBox();
            this.txtBody = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtSubject = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btnSendEmail = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnTestConfig);
            this.groupBox1.Controls.Add(this.txtSenderName);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtSenderPassword);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtSenderEmail);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(760, 120);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Gmail Configuration";
            // 
            // btnTestConfig
            // 
            this.btnTestConfig.Location = new System.Drawing.Point(650, 80);
            this.btnTestConfig.Name = "btnTestConfig";
            this.btnTestConfig.Size = new System.Drawing.Size(100, 30);
            this.btnTestConfig.TabIndex = 6;
            this.btnTestConfig.Text = "Test Configuration";
            this.btnTestConfig.UseVisualStyleBackColor = true;
            this.btnTestConfig.Click += new System.EventHandler(this.btnTestConfig_Click);
            // 
            // txtSenderName
            // 
            this.txtSenderName.Location = new System.Drawing.Point(520, 25);
            this.txtSenderName.Name = "txtSenderName";
            this.txtSenderName.Size = new System.Drawing.Size(230, 20);
            this.txtSenderName.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(450, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Sender Name:";
            // 
            // txtSenderPassword
            // 
            this.txtSenderPassword.Location = new System.Drawing.Point(100, 55);
            this.txtSenderPassword.Name = "txtSenderPassword";
            this.txtSenderPassword.PasswordChar = '*';
            this.txtSenderPassword.Size = new System.Drawing.Size(650, 20);
            this.txtSenderPassword.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "App Password:";
            // 
            // txtSenderEmail
            // 
            this.txtSenderEmail.Location = new System.Drawing.Point(100, 25);
            this.txtSenderEmail.Name = "txtSenderEmail";
            this.txtSenderEmail.Size = new System.Drawing.Size(340, 20);
            this.txtSenderEmail.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Sender Email:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnPreviewRecipients);
            this.groupBox2.Controls.Add(this.txtRecipients);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.cboRole);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.cboDepartment);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.rbCustom);
            this.groupBox2.Controls.Add(this.rbRole);
            this.groupBox2.Controls.Add(this.rbDepartment);
            this.groupBox2.Controls.Add(this.rbAllMembers);
            this.groupBox2.Location = new System.Drawing.Point(12, 138);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(760, 150);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Recipients";
            // 
            // btnPreviewRecipients
            // 
            this.btnPreviewRecipients.Location = new System.Drawing.Point(650, 110);
            this.btnPreviewRecipients.Name = "btnPreviewRecipients";
            this.btnPreviewRecipients.Size = new System.Drawing.Size(100, 30);
            this.btnPreviewRecipients.TabIndex = 10;
            this.btnPreviewRecipients.Text = "Preview Recipients";
            this.btnPreviewRecipients.UseVisualStyleBackColor = true;
            this.btnPreviewRecipients.Click += new System.EventHandler(this.btnPreviewRecipients_Click);
            // 
            // txtRecipients
            // 
            this.txtRecipients.Enabled = false;
            this.txtRecipients.Location = new System.Drawing.Point(150, 115);
            this.txtRecipients.Name = "txtRecipients";
            this.txtRecipients.Size = new System.Drawing.Size(490, 20);
            this.txtRecipients.TabIndex = 9;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(150, 99);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(156, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "Email Addresses (separated by ;):";
            // 
            // cboRole
            // 
            this.cboRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRole.Enabled = false;
            this.cboRole.FormattingEnabled = true;
            this.cboRole.Location = new System.Drawing.Point(520, 70);
            this.cboRole.Name = "cboRole";
            this.cboRole.Size = new System.Drawing.Size(230, 21);
            this.cboRole.TabIndex = 7;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(480, 73);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(32, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Role:";
            // 
            // cboDepartment
            // 
            this.cboDepartment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDepartment.Enabled = false;
            this.cboDepartment.FormattingEnabled = true;
            this.cboDepartment.Location = new System.Drawing.Point(520, 45);
            this.cboDepartment.Name = "cboDepartment";
            this.cboDepartment.Size = new System.Drawing.Size(230, 21);
            this.cboDepartment.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(450, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Department:";
            // 
            // rbCustom
            // 
            this.rbCustom.AutoSize = true;
            this.rbCustom.Location = new System.Drawing.Point(15, 95);
            this.rbCustom.Name = "rbCustom";
            this.rbCustom.Size = new System.Drawing.Size(129, 17);
            this.rbCustom.TabIndex = 3;
            this.rbCustom.Text = "Custom Email Address";
            this.rbCustom.UseVisualStyleBackColor = true;
            this.rbCustom.CheckedChanged += new System.EventHandler(this.rbCustom_CheckedChanged);
            // 
            // rbRole
            // 
            this.rbRole.AutoSize = true;
            this.rbRole.Location = new System.Drawing.Point(15, 70);
            this.rbRole.Name = "rbRole";
            this.rbRole.Size = new System.Drawing.Size(72, 17);
            this.rbRole.TabIndex = 2;
            this.rbRole.Text = "By Role";
            this.rbRole.UseVisualStyleBackColor = true;
            this.rbRole.CheckedChanged += new System.EventHandler(this.rbRole_CheckedChanged);
            // 
            // rbDepartment
            // 
            this.rbDepartment.AutoSize = true;
            this.rbDepartment.Location = new System.Drawing.Point(15, 45);
            this.rbDepartment.Name = "rbDepartment";
            this.rbDepartment.Size = new System.Drawing.Size(102, 17);
            this.rbDepartment.TabIndex = 1;
            this.rbDepartment.Text = "By Department";
            this.rbDepartment.UseVisualStyleBackColor = true;
            this.rbDepartment.CheckedChanged += new System.EventHandler(this.rbDepartment_CheckedChanged);
            // 
            // rbAllMembers
            // 
            this.rbAllMembers.AutoSize = true;
            this.rbAllMembers.Checked = true;
            this.rbAllMembers.Location = new System.Drawing.Point(15, 20);
            this.rbAllMembers.Name = "rbAllMembers";
            this.rbAllMembers.Size = new System.Drawing.Size(87, 17);
            this.rbAllMembers.TabIndex = 0;
            this.rbAllMembers.TabStop = true;
            this.rbAllMembers.Text = "All Members";
            this.rbAllMembers.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnAnnouncementTemplate);
            this.groupBox3.Controls.Add(this.btnEventTemplate);
            this.groupBox3.Controls.Add(this.chkHtml);
            this.groupBox3.Controls.Add(this.txtBody);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.txtSubject);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Location = new System.Drawing.Point(12, 294);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(760, 280);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Email Content";
            // 
            // btnAnnouncementTemplate
            // 
            this.btnAnnouncementTemplate.Location = new System.Drawing.Point(570, 245);
            this.btnAnnouncementTemplate.Name = "btnAnnouncementTemplate";
            this.btnAnnouncementTemplate.Size = new System.Drawing.Size(85, 25);
            this.btnAnnouncementTemplate.TabIndex = 12;
            this.btnAnnouncementTemplate.Text = "Announcement";
            this.btnAnnouncementTemplate.UseVisualStyleBackColor = true;
            this.btnAnnouncementTemplate.Click += new System.EventHandler(this.btnAnnouncementTemplate_Click);
            // 
            // btnEventTemplate
            // 
            this.btnEventTemplate.Location = new System.Drawing.Point(485, 245);
            this.btnEventTemplate.Name = "btnEventTemplate";
            this.btnEventTemplate.Size = new System.Drawing.Size(75, 25);
            this.btnEventTemplate.TabIndex = 11;
            this.btnEventTemplate.Text = "Event";
            this.btnEventTemplate.UseVisualStyleBackColor = true;
            this.btnEventTemplate.Click += new System.EventHandler(this.btnEventTemplate_Click);
            // 
            // chkHtml
            // 
            this.chkHtml.AutoSize = true;
            this.chkHtml.Location = new System.Drawing.Point(100, 250);
            this.chkHtml.Name = "chkHtml";
            this.chkHtml.Size = new System.Drawing.Size(103, 17);
            this.chkHtml.TabIndex = 10;
            this.chkHtml.Text = "HTML Formatted";
            this.chkHtml.UseVisualStyleBackColor = true;
            // 
            // txtBody
            // 
            this.txtBody.Location = new System.Drawing.Point(100, 50);
            this.txtBody.Multiline = true;
            this.txtBody.Name = "txtBody";
            this.txtBody.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtBody.Size = new System.Drawing.Size(650, 190);
            this.txtBody.TabIndex = 9;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(15, 53);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(77, 13);
            this.label9.TabIndex = 8;
            this.label9.Text = "Email Content:";
            // 
            // txtSubject
            // 
            this.txtSubject.Location = new System.Drawing.Point(100, 20);
            this.txtSubject.Name = "txtSubject";
            this.txtSubject.Size = new System.Drawing.Size(650, 20);
            this.txtSubject.TabIndex = 7;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(15, 23);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(46, 13);
            this.label8.TabIndex = 6;
            this.label8.Text = "Subject:";
            // 
            // btnSendEmail
            // 
            this.btnSendEmail.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.btnSendEmail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSendEmail.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.btnSendEmail.ForeColor = System.Drawing.Color.White;
            this.btnSendEmail.Location = new System.Drawing.Point(300, 580);
            this.btnSendEmail.Name = "btnSendEmail";
            this.btnSendEmail.Size = new System.Drawing.Size(200, 50);
            this.btnSendEmail.TabIndex = 3;
            this.btnSendEmail.Text = "Send Email Notification";
            this.btnSendEmail.UseVisualStyleBackColor = false;
            this.btnSendEmail.Click += new System.EventHandler(this.btnSendEmail_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(12, 640);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(760, 23);
            this.progressBar.TabIndex = 4;
            this.progressBar.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic);
            this.label4.ForeColor = System.Drawing.Color.Gray;
            this.label4.Location = new System.Drawing.Point(9, 54);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(233, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Use your Gmail App Password for authentication";
            // 
            // FrmEmailNotification
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 681);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.btnSendEmail);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "FrmEmailNotification";
            this.Text = "Email Notification System";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtSenderEmail;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSenderPassword;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSenderName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnTestConfig;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rbAllMembers;
        private System.Windows.Forms.RadioButton rbDepartment;
        private System.Windows.Forms.RadioButton rbRole;
        private System.Windows.Forms.RadioButton rbCustom;
        private System.Windows.Forms.ComboBox cboDepartment;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cboRole;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtRecipients;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnPreviewRecipients;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtSubject;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtBody;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox chkHtml;
        private System.Windows.Forms.Button btnEventTemplate;
        private System.Windows.Forms.Button btnAnnouncementTemplate;
        private System.Windows.Forms.Button btnSendEmail;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label label4;
    }
}