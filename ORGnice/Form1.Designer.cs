namespace ORGnicee
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            panel8 = new Panel();
            label2 = new Label();
            label15 = new Label();
            lblTarGoal = new Label();
            label11 = new Label();
            label12 = new Label();
            label13 = new Label();
            label14 = new Label();
            panel11 = new Panel();
            label17 = new Label();
            progBar = new ReaLTaiizor.Controls.HopeRoundProgressBar();
            panel3 = new Panel();
            panel4 = new Panel();
            label10 = new Label();
            lblPaidMembers = new Label();
            lblDeadline = new Label();
            label7 = new Label();
            label6 = new Label();
            label5 = new Label();
            label4 = new Label();
            panel5 = new Panel();
            btnAddTarGoal = new Button();
            dgMembers = new DataGridView();
            dgName = new DataGridViewTextBoxColumn();
            dgPaid = new DataGridViewCheckBoxColumn();
            dgINC = new DataGridViewCheckBoxColumn();
            dgNotPaid = new DataGridViewCheckBoxColumn();
            button1 = new Button();
            panel1 = new Panel();
            cbDept = new ComboBox();
            comboBox1 = new ComboBox();
            panel8.SuspendLayout();
            panel11.SuspendLayout();
            panel3.SuspendLayout();
            panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgMembers).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.White;
            label1.Font = new Font("Century Gothic", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(100, 15);
            label1.Name = "label1";
            label1.Size = new Size(148, 28);
            label1.TabIndex = 2;
            label1.Text = "Target Goal";
            // 
            // panel8
            // 
            panel8.BackColor = Color.White;
            panel8.BorderStyle = BorderStyle.FixedSingle;
            panel8.Controls.Add(label2);
            panel8.Controls.Add(label15);
            panel8.Location = new Point(374, 434);
            panel8.Name = "panel8";
            panel8.Size = new Size(596, 84);
            panel8.TabIndex = 5;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Century Gothic", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(241, 18);
            label2.Name = "label2";
            label2.Size = new Size(122, 28);
            label2.TabIndex = 5;
            label2.Text = "Members";
            // 
            // label15
            // 
            label15.BackColor = SystemColors.HotTrack;
            label15.Location = new Point(62, 34);
            label15.Name = "label15";
            label15.Size = new Size(486, 2);
            label15.TabIndex = 1;
            label15.Click += label15_Click;
            // 
            // lblTarGoal
            // 
            lblTarGoal.AutoSize = true;
            lblTarGoal.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTarGoal.Location = new Point(169, 200);
            lblTarGoal.Name = "lblTarGoal";
            lblTarGoal.Size = new Size(0, 29);
            lblTarGoal.TabIndex = 8;
            lblTarGoal.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Font = new Font("Century Gothic", 8F, FontStyle.Bold);
            label11.Location = new Point(49, 49);
            label11.Name = "label11";
            label11.Size = new Size(44, 19);
            label11.TabIndex = 6;
            label11.Text = "Paid";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Font = new Font("Century Gothic", 8F, FontStyle.Bold);
            label12.ForeColor = Color.Crimson;
            label12.Location = new Point(315, 49);
            label12.Name = "label12";
            label12.Size = new Size(74, 19);
            label12.TabIndex = 6;
            label12.Text = "Not Paid";
            label12.Click += label12_Click;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Font = new Font("Century Gothic", 8F, FontStyle.Bold);
            label13.ForeColor = SystemColors.HotTrack;
            label13.Location = new Point(196, 49);
            label13.Name = "label13";
            label13.Size = new Size(37, 19);
            label13.TabIndex = 7;
            label13.Text = "INC";
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Font = new Font("Century Gothic", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label14.Location = new Point(109, 18);
            label14.Name = "label14";
            label14.Size = new Size(188, 28);
            label14.TabIndex = 6;
            label14.Text = "Payment Status";
            // 
            // panel11
            // 
            panel11.BackColor = Color.White;
            panel11.BorderStyle = BorderStyle.FixedSingle;
            panel11.Controls.Add(label14);
            panel11.Controls.Add(label11);
            panel11.Controls.Add(label17);
            panel11.Controls.Add(label13);
            panel11.Controls.Add(label12);
            panel11.Location = new Point(967, 434);
            panel11.Name = "panel11";
            panel11.Size = new Size(420, 84);
            panel11.TabIndex = 12;
            // 
            // label17
            // 
            label17.BackColor = SystemColors.HotTrack;
            label17.Location = new Point(90, 34);
            label17.Name = "label17";
            label17.Size = new Size(224, 2);
            label17.TabIndex = 7;
            // 
            // progBar
            // 
            progBar.BackColor = Color.White;
            progBar.BarColor = Color.FromArgb(64, 158, 255);
            progBar.BorderColor = Color.FromArgb(220, 223, 230);
            progBar.DangerColor = Color.FromArgb(245, 108, 108);
            progBar.DangerTextColorA = Color.FromArgb(245, 108, 108);
            progBar.DangerTextColorB = Color.FromArgb(245, 108, 108);
            progBar.Font = new Font("Segoe UI", 12F);
            progBar.ForeColor = Color.FromArgb(64, 158, 255);
            progBar.FullBarColor = Color.FromArgb(103, 194, 58);
            progBar.FullTextColorA = Color.FromArgb(103, 194, 58);
            progBar.FullTextColorB = Color.FromArgb(103, 194, 58);
            progBar.IsError = false;
            progBar.Location = new Point(98, 47);
            progBar.Name = "progBar";
            progBar.PercentText = "%";
            progBar.Size = new Size(150, 150);
            progBar.TabIndex = 13;
            progBar.Text = "hopeRoundProgressBar1";
            progBar.ValueNumber = 0;
            // 
            // panel3
            // 
            panel3.BackColor = Color.White;
            panel3.BorderStyle = BorderStyle.FixedSingle;
            panel3.Controls.Add(lblTarGoal);
            panel3.Controls.Add(label1);
            panel3.Controls.Add(progBar);
            panel3.Location = new Point(374, 114);
            panel3.Name = "panel3";
            panel3.Size = new Size(364, 246);
            panel3.TabIndex = 14;
            // 
            // panel4
            // 
            panel4.BackColor = Color.White;
            panel4.BorderStyle = BorderStyle.FixedSingle;
            panel4.Controls.Add(label10);
            panel4.Controls.Add(lblPaidMembers);
            panel4.Controls.Add(lblDeadline);
            panel4.Controls.Add(label7);
            panel4.Controls.Add(label6);
            panel4.Controls.Add(label5);
            panel4.Controls.Add(label4);
            panel4.Location = new Point(789, 114);
            panel4.Name = "panel4";
            panel4.Size = new Size(598, 246);
            panel4.TabIndex = 15;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.BackColor = Color.White;
            label10.Font = new Font("Century Gothic", 8F, FontStyle.Bold);
            label10.ForeColor = SystemColors.ControlDarkDark;
            label10.Location = new Point(23, 44);
            label10.Name = "label10";
            label10.Size = new Size(102, 19);
            label10.TabIndex = 19;
            label10.Text = "Loren ipsum";
            // 
            // lblPaidMembers
            // 
            lblPaidMembers.AutoSize = true;
            lblPaidMembers.BackColor = Color.White;
            lblPaidMembers.Font = new Font("Century Gothic", 8F, FontStyle.Bold);
            lblPaidMembers.ForeColor = SystemColors.ControlDarkDark;
            lblPaidMembers.ImageAlign = ContentAlignment.BottomLeft;
            lblPaidMembers.Location = new Point(16, 207);
            lblPaidMembers.Name = "lblPaidMembers";
            lblPaidMembers.Size = new Size(0, 19);
            lblPaidMembers.TabIndex = 18;
            // 
            // lblDeadline
            // 
            lblDeadline.AutoSize = true;
            lblDeadline.BackColor = Color.White;
            lblDeadline.Font = new Font("Century Gothic", 8F, FontStyle.Bold);
            lblDeadline.ForeColor = SystemColors.ControlDarkDark;
            lblDeadline.Location = new Point(16, 147);
            lblDeadline.Name = "lblDeadline";
            lblDeadline.Size = new Size(0, 19);
            lblDeadline.TabIndex = 17;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.BackColor = Color.White;
            label7.Font = new Font("Century Gothic", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label7.ImageAlign = ContentAlignment.BottomLeft;
            label7.Location = new Point(16, 184);
            label7.Name = "label7";
            label7.Size = new Size(172, 26);
            label7.TabIndex = 16;
            label7.Text = "Paid Members:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.BackColor = Color.White;
            label6.Font = new Font("Century Gothic", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.Location = new Point(16, 125);
            label6.Name = "label6";
            label6.Size = new Size(165, 28);
            label6.TabIndex = 15;
            label6.Text = "Deadline Set:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = Color.White;
            label5.Font = new Font("Century Gothic", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(16, 15);
            label5.Name = "label5";
            label5.Size = new Size(142, 28);
            label5.TabIndex = 14;
            label5.Text = "Description";
            // 
            // label4
            // 
            label4.BackColor = SystemColors.HotTrack;
            label4.Location = new Point(71, 113);
            label4.Name = "label4";
            label4.Size = new Size(486, 2);
            label4.TabIndex = 14;
            // 
            // panel5
            // 
            panel5.BackColor = Color.FromArgb(40, 39, 41);
            panel5.Location = new Point(-1, 0);
            panel5.Name = "panel5";
            panel5.Size = new Size(280, 1085);
            panel5.TabIndex = 16;
            // 
            // btnAddTarGoal
            // 
            btnAddTarGoal.Font = new Font("Century Gothic", 8F);
            btnAddTarGoal.Location = new Point(967, 379);
            btnAddTarGoal.Name = "btnAddTarGoal";
            btnAddTarGoal.Size = new Size(158, 34);
            btnAddTarGoal.TabIndex = 18;
            btnAddTarGoal.Text = "Add Target Goal";
            btnAddTarGoal.UseVisualStyleBackColor = true;
            btnAddTarGoal.Click += btnAddTarGoal_Click;
            // 
            // dgMembers
            // 
            dgMembers.BackgroundColor = Color.White;
            dgMembers.BorderStyle = BorderStyle.None;
            dgMembers.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgMembers.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgMembers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgMembers.Columns.AddRange(new DataGridViewColumn[] { dgName, dgPaid, dgINC, dgNotPaid });
            dgMembers.GridColor = Color.White;
            dgMembers.Location = new Point(374, 484);
            dgMembers.Name = "dgMembers";
            dgMembers.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgMembers.RowHeadersVisible = false;
            dgMembers.RowHeadersWidth = 62;
            dgMembers.Size = new Size(1013, 486);
            dgMembers.TabIndex = 19;
            // 
            // dgName
            // 
            dgName.HeaderText = "Name";
            dgName.MinimumWidth = 8;
            dgName.Name = "dgName";
            dgName.Width = 596;
            // 
            // dgPaid
            // 
            dgPaid.HeaderText = "Paid";
            dgPaid.MinimumWidth = 8;
            dgPaid.Name = "dgPaid";
            dgPaid.Width = 140;
            // 
            // dgINC
            // 
            dgINC.HeaderText = "INC";
            dgINC.MinimumWidth = 8;
            dgINC.Name = "dgINC";
            dgINC.Width = 140;
            // 
            // dgNotPaid
            // 
            dgNotPaid.HeaderText = "Not Paid";
            dgNotPaid.MinimumWidth = 8;
            dgNotPaid.Name = "dgNotPaid";
            dgNotPaid.Width = 140;
            // 
            // button1
            // 
            button1.Font = new Font("Century Gothic", 8F);
            button1.Location = new Point(759, 380);
            button1.Name = "button1";
            button1.Size = new Size(130, 34);
            button1.TabIndex = 20;
            button1.Text = "Add Members";
            button1.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(40, 39, 41);
            panel1.Location = new Point(274, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1210, 31);
            panel1.TabIndex = 21;
            // 
            // cbDept
            // 
            cbDept.FormattingEnabled = true;
            cbDept.Location = new Point(374, 380);
            cbDept.Name = "cbDept";
            cbDept.Size = new Size(170, 33);
            cbDept.TabIndex = 6;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(568, 380);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(170, 33);
            comboBox1.TabIndex = 22;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ButtonFace;
            ClientSize = new Size(1478, 1029);
            Controls.Add(comboBox1);
            Controls.Add(panel5);
            Controls.Add(panel1);
            Controls.Add(button1);
            Controls.Add(panel8);
            Controls.Add(panel11);
            Controls.Add(dgMembers);
            Controls.Add(btnAddTarGoal);
            Controls.Add(panel4);
            Controls.Add(cbDept);
            Controls.Add(panel3);
            Name = "Form1";
            Text = "Form1";
            panel8.ResumeLayout(false);
            panel8.PerformLayout();
            panel11.ResumeLayout(false);
            panel11.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgMembers).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private Label label1;
        private Panel panel8;
        private Label lblTarGoal;
        private Label label12;
        private Label label11;
        private Label label13;
        private Label label14;
        private Label label15;
        private Label label2;
        private Panel panel11;
        private Label label17;
        private ReaLTaiizor.Controls.HopeRoundProgressBar progBar;
        private Panel panel3;
        private Panel panel4;
        private Panel panel5;
        private Label label5;
        private Label label4;
        private Button btnAddTarGoal;
        private DataGridView dgMembers;
        private Label label7;
        private Label label6;
        private Label lblDeadline;
        private Label lblPaidMembers;
        private Label label10;
        private Button button1;
        private Panel panel1;
        private DataGridViewTextBoxColumn dgName;
        private DataGridViewCheckBoxColumn dgPaid;
        private DataGridViewCheckBoxColumn dgINC;
        private DataGridViewCheckBoxColumn dgNotPaid;
        private ComboBox cbDept;
        private ComboBox comboBox1;
    }
}
