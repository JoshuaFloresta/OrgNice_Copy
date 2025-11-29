namespace ORGnice
{
    partial class MemberDetailsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MemberDetailsForm));
            this.panel2 = new System.Windows.Forms.Panel();
            this.close_btn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.roundedPanel1 = new ORGnice.RoundedPanel();
            this.ArchiveBtn = new RoundedButton();
            this.Clr_btn = new RoundedButton();
            this.roundedButton1 = new RoundedButton();
            this.panel5 = new System.Windows.Forms.Panel();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtDepartment = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cbRole = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label11 = new System.Windows.Forms.Label();
            this.rbOthers = new System.Windows.Forms.RadioButton();
            this.rbFem = new System.Windows.Forms.RadioButton();
            this.rbMale = new System.Windows.Forms.RadioButton();
            this.dtBirthday = new System.Windows.Forms.DateTimePicker();
            this.label10 = new System.Windows.Forms.Label();
            this.txtSection = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtFirstName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtLastName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtMemberId = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.uploadImage = new RoundedButton();
            this.imageUploadLbl = new System.Windows.Forms.Label();
            this.picProfile = new System.Windows.Forms.PictureBox();
            this.txtStudentId = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            this.roundedPanel1.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picProfile)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(39)))), ((int)(((byte)(41)))));
            this.panel2.Controls.Add(this.close_btn);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Location = new System.Drawing.Point(1, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(568, 43);
            this.panel2.TabIndex = 10;
            // 
            // close_btn
            // 
            this.close_btn.BackColor = System.Drawing.Color.Transparent;
            this.close_btn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("close_btn.BackgroundImage")));
            this.close_btn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.close_btn.FlatAppearance.BorderSize = 0;
            this.close_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.close_btn.ForeColor = System.Drawing.Color.Transparent;
            this.close_btn.Location = new System.Drawing.Point(536, 11);
            this.close_btn.Name = "close_btn";
            this.close_btn.Size = new System.Drawing.Size(20, 20);
            this.close_btn.TabIndex = 8;
            this.close_btn.UseVisualStyleBackColor = false;
            this.close_btn.Click += new System.EventHandler(this.close_btn_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(19, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(132, 18);
            this.label2.TabIndex = 6;
            this.label2.Text = "Create Members";
            // 
            // roundedPanel1
            // 
            this.roundedPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(242)))), ((int)(((byte)(246)))));
            this.roundedPanel1.Controls.Add(this.ArchiveBtn);
            this.roundedPanel1.Controls.Add(this.Clr_btn);
            this.roundedPanel1.Controls.Add(this.roundedButton1);
            this.roundedPanel1.Controls.Add(this.panel5);
            this.roundedPanel1.Controls.Add(this.panel4);
            this.roundedPanel1.Controls.Add(this.panel1);
            this.roundedPanel1.CornerRadius = 16;
            this.roundedPanel1.Location = new System.Drawing.Point(1, 31);
            this.roundedPanel1.Name = "roundedPanel1";
            this.roundedPanel1.Padding = new System.Windows.Forms.Padding(16);
            this.roundedPanel1.ShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.roundedPanel1.ShadowOffset = 3;
            this.roundedPanel1.Size = new System.Drawing.Size(568, 461);
            this.roundedPanel1.TabIndex = 9;
            // 
            // ArchiveBtn
            // 
            this.ArchiveBtn.BackColor = System.Drawing.Color.Firebrick;
            this.ArchiveBtn.CornerRadius = 10;
            this.ArchiveBtn.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.ArchiveBtn.ForeColor = System.Drawing.Color.White;
            this.ArchiveBtn.Location = new System.Drawing.Point(19, 404);
            this.ArchiveBtn.Name = "ArchiveBtn";
            this.ArchiveBtn.Size = new System.Drawing.Size(72, 30);
            this.ArchiveBtn.TabIndex = 36;
            this.ArchiveBtn.Text = "Archive";
            this.ArchiveBtn.UseVisualStyleBackColor = false;
            this.ArchiveBtn.Click += new System.EventHandler(this.ArchiveBtn_Click);
            // 
            // Clr_btn
            // 
            this.Clr_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Clr_btn.BackColor = System.Drawing.Color.Transparent;
            this.Clr_btn.CornerRadius = 10;
            this.Clr_btn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(39)))), ((int)(((byte)(41)))));
            this.Clr_btn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(39)))), ((int)(((byte)(41)))));
            this.Clr_btn.Location = new System.Drawing.Point(323, 405);
            this.Clr_btn.Name = "Clr_btn";
            this.Clr_btn.Size = new System.Drawing.Size(77, 29);
            this.Clr_btn.TabIndex = 35;
            this.Clr_btn.Text = "Clear";
            this.Clr_btn.UseVisualStyleBackColor = false;
            this.Clr_btn.Click += new System.EventHandler(this.Clr_btn_Click);
            // 
            // roundedButton1
            // 
            this.roundedButton1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(39)))), ((int)(((byte)(41)))));
            this.roundedButton1.CornerRadius = 10;
            this.roundedButton1.ForeColor = System.Drawing.Color.White;
            this.roundedButton1.Location = new System.Drawing.Point(418, 405);
            this.roundedButton1.Name = "roundedButton1";
            this.roundedButton1.Size = new System.Drawing.Size(112, 29);
            this.roundedButton1.TabIndex = 20;
            this.roundedButton1.Text = "Submit";
            this.roundedButton1.UseVisualStyleBackColor = false;
            this.roundedButton1.Click += new System.EventHandler(this.roundedButton1_Click);
            // 
            // panel5
            // 
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.txtPassword);
            this.panel5.Controls.Add(this.lblPassword);
            this.panel5.Controls.Add(this.txtDepartment);
            this.panel5.Controls.Add(this.label9);
            this.panel5.Controls.Add(this.cbRole);
            this.panel5.Controls.Add(this.label8);
            this.panel5.Location = new System.Drawing.Point(250, 228);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(299, 140);
            this.panel5.TabIndex = 3;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(92, 84);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(155, 20);
            this.txtPassword.TabIndex = 27;
            this.txtPassword.Visible = false;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(20, 87);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(56, 13);
            this.lblPassword.TabIndex = 27;
            this.lblPassword.Text = "Password:";
            this.lblPassword.Visible = false;
            // 
            // txtDepartment
            // 
            this.txtDepartment.FormattingEnabled = true;
            this.txtDepartment.Items.AddRange(new object[] {
            "Singing",
            "Dancing",
            "Instrument"});
            this.txtDepartment.Location = new System.Drawing.Point(92, 51);
            this.txtDepartment.Name = "txtDepartment";
            this.txtDepartment.Size = new System.Drawing.Size(155, 21);
            this.txtDepartment.TabIndex = 30;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(20, 54);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(52, 13);
            this.label9.TabIndex = 29;
            this.label9.Text = "Category:";
            // 
            // cbRole
            // 
            this.cbRole.FormattingEnabled = true;
            this.cbRole.Items.AddRange(new object[] {
            "Admin",
            "Officer",
            "Member"});
            this.cbRole.Location = new System.Drawing.Point(92, 17);
            this.cbRole.Name = "cbRole";
            this.cbRole.Size = new System.Drawing.Size(155, 21);
            this.cbRole.TabIndex = 28;
            this.cbRole.SelectedIndexChanged += new System.EventHandler(this.cbRole_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(20, 20);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(32, 13);
            this.label8.TabIndex = 27;
            this.label8.Text = "Role:";
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.label11);
            this.panel4.Controls.Add(this.rbOthers);
            this.panel4.Controls.Add(this.rbFem);
            this.panel4.Controls.Add(this.rbMale);
            this.panel4.Controls.Add(this.dtBirthday);
            this.panel4.Controls.Add(this.label10);
            this.panel4.Controls.Add(this.txtSection);
            this.panel4.Controls.Add(this.label7);
            this.panel4.Controls.Add(this.txtEmail);
            this.panel4.Controls.Add(this.label4);
            this.panel4.Controls.Add(this.txtFirstName);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Controls.Add(this.txtLastName);
            this.panel4.Controls.Add(this.label6);
            this.panel4.Location = new System.Drawing.Point(250, 11);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(299, 228);
            this.panel4.TabIndex = 2;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(20, 186);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(45, 13);
            this.label11.TabIndex = 34;
            this.label11.Text = "Gender:";
            // 
            // rbOthers
            // 
            this.rbOthers.AutoSize = true;
            this.rbOthers.Location = new System.Drawing.Point(211, 183);
            this.rbOthers.Name = "rbOthers";
            this.rbOthers.Size = new System.Drawing.Size(56, 17);
            this.rbOthers.TabIndex = 33;
            this.rbOthers.Text = "Others";
            this.rbOthers.UseVisualStyleBackColor = true;
            // 
            // rbFem
            // 
            this.rbFem.AutoSize = true;
            this.rbFem.Location = new System.Drawing.Point(146, 183);
            this.rbFem.Name = "rbFem";
            this.rbFem.Size = new System.Drawing.Size(59, 17);
            this.rbFem.TabIndex = 32;
            this.rbFem.Text = "Female";
            this.rbFem.UseVisualStyleBackColor = true;
            // 
            // rbMale
            // 
            this.rbMale.AutoSize = true;
            this.rbMale.Location = new System.Drawing.Point(90, 183);
            this.rbMale.Name = "rbMale";
            this.rbMale.Size = new System.Drawing.Size(48, 17);
            this.rbMale.TabIndex = 31;
            this.rbMale.Text = "Male";
            this.rbMale.UseVisualStyleBackColor = true;
            // 
            // dtBirthday
            // 
            this.dtBirthday.Location = new System.Drawing.Point(92, 153);
            this.dtBirthday.Name = "dtBirthday";
            this.dtBirthday.Size = new System.Drawing.Size(175, 20);
            this.dtBirthday.TabIndex = 28;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(20, 153);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(48, 13);
            this.label10.TabIndex = 27;
            this.label10.Text = "Birthday:";
            // 
            // txtSection
            // 
            this.txtSection.Location = new System.Drawing.Point(92, 116);
            this.txtSection.Name = "txtSection";
            this.txtSection.Size = new System.Drawing.Size(175, 20);
            this.txtSection.TabIndex = 26;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(20, 120);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(46, 13);
            this.label7.TabIndex = 25;
            this.label7.Text = "Section:";
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(92, 83);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(175, 20);
            this.txtEmail.TabIndex = 24;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 87);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 23;
            this.label4.Text = "Email:";
            // 
            // txtFirstName
            // 
            this.txtFirstName.Location = new System.Drawing.Point(92, 50);
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(175, 20);
            this.txtFirstName.TabIndex = 22;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 21;
            this.label1.Text = "First Name:";
            // 
            // txtLastName
            // 
            this.txtLastName.Location = new System.Drawing.Point(92, 17);
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(175, 20);
            this.txtLastName.TabIndex = 20;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(20, 21);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(61, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Last Name:";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.txtMemberId);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtUsername);
            this.panel1.Controls.Add(this.label12);
            this.panel1.Controls.Add(this.uploadImage);
            this.panel1.Controls.Add(this.imageUploadLbl);
            this.panel1.Controls.Add(this.picProfile);
            this.panel1.Controls.Add(this.txtStudentId);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Location = new System.Drawing.Point(19, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(233, 363);
            this.panel1.TabIndex = 1;
            // 
            // txtMemberId
            // 
            this.txtMemberId.AutoSize = true;
            this.txtMemberId.Location = new System.Drawing.Point(115, 179);
            this.txtMemberId.Name = "txtMemberId";
            this.txtMemberId.Size = new System.Drawing.Size(0, 13);
            this.txtMemberId.TabIndex = 38;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(47, 179);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 13);
            this.label3.TabIndex = 37;
            this.label3.Text = "Member ID:";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(30, 297);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(171, 20);
            this.txtUsername.TabIndex = 36;
            this.txtUsername.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(87, 326);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(58, 13);
            this.label12.TabIndex = 35;
            this.label12.Text = "Username:";
            // 
            // uploadImage
            // 
            this.uploadImage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(39)))), ((int)(((byte)(41)))));
            this.uploadImage.CornerRadius = 10;
            this.uploadImage.ForeColor = System.Drawing.Color.White;
            this.uploadImage.Location = new System.Drawing.Point(62, 204);
            this.uploadImage.Name = "uploadImage";
            this.uploadImage.Size = new System.Drawing.Size(112, 27);
            this.uploadImage.TabIndex = 36;
            this.uploadImage.Text = "Upload Image";
            this.uploadImage.UseVisualStyleBackColor = false;
            this.uploadImage.Click += new System.EventHandler(this.roundedButton3_Click);
            // 
            // imageUploadLbl
            // 
            this.imageUploadLbl.AutoSize = true;
            this.imageUploadLbl.Location = new System.Drawing.Point(87, 85);
            this.imageUploadLbl.Name = "imageUploadLbl";
            this.imageUploadLbl.Size = new System.Drawing.Size(61, 13);
            this.imageUploadLbl.TabIndex = 1;
            this.imageUploadLbl.Text = "Upload Img";
            // 
            // picProfile
            // 
            this.picProfile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picProfile.Location = new System.Drawing.Point(30, 26);
            this.picProfile.Name = "picProfile";
            this.picProfile.Size = new System.Drawing.Size(171, 149);
            this.picProfile.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picProfile.TabIndex = 0;
            this.picProfile.TabStop = false;
            this.picProfile.Tag = "Upload Image";
            // 
            // txtStudentId
            // 
            this.txtStudentId.Location = new System.Drawing.Point(30, 242);
            this.txtStudentId.Name = "txtStudentId";
            this.txtStudentId.Size = new System.Drawing.Size(171, 20);
            this.txtStudentId.TabIndex = 15;
            this.txtStudentId.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(84, 269);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Student No:";
            // 
            // MemberDetailsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(571, 490);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.roundedPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MemberDetailsForm";
            this.Text = "MemberDetailsForm";
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.roundedPanel1.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picProfile)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button close_btn;
        private System.Windows.Forms.Label label2;
        private RoundedPanel roundedPanel1;
        private RoundedButton Clr_btn;
        private RoundedButton roundedButton1;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.ComboBox txtDepartment;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cbRole;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.RadioButton rbOthers;
        private System.Windows.Forms.RadioButton rbFem;
        private System.Windows.Forms.RadioButton rbMale;
        private System.Windows.Forms.DateTimePicker dtBirthday;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtSection;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtFirstName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtLastName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label label12;
        private RoundedButton uploadImage;
        private System.Windows.Forms.Label imageUploadLbl;
        private System.Windows.Forms.PictureBox picProfile;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.TextBox txtStudentId;
        private System.Windows.Forms.Label txtMemberId;
        private System.Windows.Forms.Label label3;
        private RoundedButton ArchiveBtn;
    }
}