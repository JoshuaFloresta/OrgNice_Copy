namespace ORGnice
{
    partial class FinanceDetail
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
        this.panel1 = new System.Windows.Forms.Panel();
      this.label1 = new System.Windows.Forms.Label();
         this.btnClose = new System.Windows.Forms.Button();
     this.label2 = new System.Windows.Forms.Label();
       this.txtGoalName = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
       this.txtDescription = new System.Windows.Forms.TextBox();
      this.label4 = new System.Windows.Forms.Label();
            this.txtTargetAmount = new System.Windows.Forms.TextBox();
       this.label5 = new System.Windows.Forms.Label();
      this.txtCollectedAmount = new System.Windows.Forms.TextBox();
   this.label6 = new System.Windows.Forms.Label();
  this.txtExpensesAmount = new System.Windows.Forms.TextBox();
    this.label7 = new System.Windows.Forms.Label();
            this.cbDepartment = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cbCurrency = new System.Windows.Forms.ComboBox();
this.label9 = new System.Windows.Forms.Label();
 this.dtDueDate = new System.Windows.Forms.DateTimePicker();
    this.label10 = new System.Windows.Forms.Label();
        this.cbPaymentStatus = new System.Windows.Forms.ComboBox();
      this.label11 = new System.Windows.Forms.Label();
  this.cbMemberStatus = new System.Windows.Forms.ComboBox();
            this.btnArchive = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
        this.SuspendLayout();
   // 
            // panel1
    // 
  this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panel1.Controls.Add(this.btnClose);
        this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
  this.panel1.Location = new System.Drawing.Point(0, 0);
    this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(640, 50);
        this.panel1.TabIndex = 0;
      // 
      // label1
        // 
   this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(15, 15);
            this.label1.Name = "label1";
     this.label1.Size = new System.Drawing.Size(180, 24);
       this.label1.TabIndex = 0;
this.label1.Text = "?? Edit Finance Goal";
         // 
            // btnClose
        // 
this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
       this.btnClose.FlatAppearance.BorderSize = 0;
       this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
  this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
     this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(590, 10);
      this.btnClose.Name = "btnClose";
    this.btnClose.Size = new System.Drawing.Size(30, 30);
 this.btnClose.TabIndex = 1;
       this.btnClose.Text = "×";
       this.btnClose.UseVisualStyleBackColor = false;
            // 
  // label2
            // 
        this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
     this.label2.Location = new System.Drawing.Point(30, 70);
            this.label2.Name = "label2";
          this.label2.Size = new System.Drawing.Size(86, 17);
       this.label2.TabIndex = 1;
     this.label2.Text = "Goal Name:*";
 // 
            // txtGoalName
            // 
            this.txtGoalName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.txtGoalName.Location = new System.Drawing.Point(150, 67);
         this.txtGoalName.Name = "txtGoalName";
  this.txtGoalName.Size = new System.Drawing.Size(200, 23);
            this.txtGoalName.TabIndex = 2;
// 
     // label3
          // 
        this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(30, 110);
       this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 17);
          this.label3.TabIndex = 3;
      this.label3.Text = "Description:";
            // 
   // txtDescription
            // 
        this.txtDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.txtDescription.Location = new System.Drawing.Point(150, 107);
            this.txtDescription.Multiline = true;
     this.txtDescription.Name = "txtDescription";
      this.txtDescription.Size = new System.Drawing.Size(200, 60);
       this.txtDescription.TabIndex = 4;
            // 
  // label4
            // 
      this.label4.AutoSize = true;
  this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
          this.label4.Location = new System.Drawing.Point(30, 190);
            this.label4.Name = "label4";
     this.label4.Size = new System.Drawing.Size(114, 17);
            this.label4.TabIndex = 5;
         this.label4.Text = "Target Amount:*";
            // 
       // txtTargetAmount
       // 
    this.txtTargetAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.txtTargetAmount.Location = new System.Drawing.Point(150, 187);
            this.txtTargetAmount.Name = "txtTargetAmount";
   this.txtTargetAmount.Size = new System.Drawing.Size(200, 23);
        this.txtTargetAmount.TabIndex = 6;
      // 
            // label5
      // 
          this.label5.AutoSize = true;
 this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label5.Location = new System.Drawing.Point(30, 230);
       this.label5.Name = "label5";
 this.label5.Size = new System.Drawing.Size(129, 17);
            this.label5.TabIndex = 7;
        this.label5.Text = "Collected Amount:";
// 
            // txtCollectedAmount
          // 
     this.txtCollectedAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCollectedAmount.Location = new System.Drawing.Point(150, 227);
            this.txtCollectedAmount.Name = "txtCollectedAmount";
         this.txtCollectedAmount.Size = new System.Drawing.Size(200, 23);
       this.txtCollectedAmount.TabIndex = 8;
      // 
     // label6
            // 
         this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
  this.label6.Location = new System.Drawing.Point(30, 270);
  this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(124, 17);
            this.label6.TabIndex = 9;
            this.label6.Text = "Expenses Amount:";
          // 
        // txtExpensesAmount
  // 
   this.txtExpensesAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
  this.txtExpensesAmount.Location = new System.Drawing.Point(150, 267);
            this.txtExpensesAmount.Name = "txtExpensesAmount";
            this.txtExpensesAmount.Size = new System.Drawing.Size(200, 23);
      this.txtExpensesAmount.TabIndex = 10;
            // 
  // label7
        // 
        this.label7.AutoSize = true;
       this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label7.Location = new System.Drawing.Point(380, 70);
       this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(96, 17);
      this.label7.TabIndex = 11;
  this.label7.Text = "Department:*";
            // 
            // cbDepartment
            // 
            this.cbDepartment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDepartment.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
          this.cbDepartment.FormattingEnabled = true;
      this.cbDepartment.Location = new System.Drawing.Point(490, 67);
      this.cbDepartment.Name = "cbDepartment";
  this.cbDepartment.Size = new System.Drawing.Size(120, 24);
   this.cbDepartment.TabIndex = 12;
        // 
   // label8
            // 
            this.label8.AutoSize = true;
 this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
     this.label8.Location = new System.Drawing.Point(380, 110);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(70, 17);
      this.label8.TabIndex = 13;
     this.label8.Text = "Currency:";
            // 
       // cbCurrency
 // 
            this.cbCurrency.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCurrency.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbCurrency.FormattingEnabled = true;
     this.cbCurrency.Items.AddRange(new object[] {
     "PHP",
     "USD",
    "EUR"});
      this.cbCurrency.Location = new System.Drawing.Point(490, 107);
      this.cbCurrency.Name = "cbCurrency";
   this.cbCurrency.Size = new System.Drawing.Size(120, 24);
   this.cbCurrency.TabIndex = 14;
    // 
 // label9
          // 
       this.label9.AutoSize = true;
     this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label9.Location = new System.Drawing.Point(380, 150);
     this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(76, 17);
  this.label9.TabIndex = 15;
   this.label9.Text = "Due Date:";
          // 
            // dtDueDate
            // 
    this.dtDueDate.Checked = false;
            this.dtDueDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtDueDate.Location = new System.Drawing.Point(490, 147);
    this.dtDueDate.Name = "dtDueDate";
            this.dtDueDate.ShowCheckBox = true;
         this.dtDueDate.Size = new System.Drawing.Size(120, 23);
 this.dtDueDate.TabIndex = 16;
          // 
      // label10
            // 
    this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(380, 190);
       this.label10.Name = "label10";
      this.label10.Size = new System.Drawing.Size(112, 17);
            this.label10.TabIndex = 17;
    this.label10.Text = "Payment Status:";
   // 
         // cbPaymentStatus
            // 
   this.cbPaymentStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
   this.cbPaymentStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
     this.cbPaymentStatus.FormattingEnabled = true;
   this.cbPaymentStatus.Items.AddRange(new object[] {
 "InProgress",
         "Goal Reached"});
    this.cbPaymentStatus.Location = new System.Drawing.Point(490, 187);
            this.cbPaymentStatus.Name = "cbPaymentStatus";
     this.cbPaymentStatus.Size = new System.Drawing.Size(120, 24);
      this.cbPaymentStatus.TabIndex = 18;
            // 
            // label11
         // 
        this.label11.AutoSize = true;
     this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
    this.label11.Location = new System.Drawing.Point(380, 230);
      this.label11.Name = "label11";
    this.label11.Size = new System.Drawing.Size(107, 17);
 this.label11.TabIndex = 19;
       this.label11.Text = "Member Status:";
       // 
  // cbMemberStatus
        // 
   this.cbMemberStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMemberStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbMemberStatus.FormattingEnabled = true;
            this.cbMemberStatus.Items.AddRange(new object[] {
    "Not Paid",
            "Paid",
            "Incomplete"});
          this.cbMemberStatus.Location = new System.Drawing.Point(490, 227);
  this.cbMemberStatus.Name = "cbMemberStatus";
            this.cbMemberStatus.Size = new System.Drawing.Size(120, 24);
            this.cbMemberStatus.TabIndex = 20;
            // 
            // btnArchive
      // 
            this.btnArchive.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
        this.btnArchive.FlatAppearance.BorderSize = 0;
         this.btnArchive.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnArchive.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnArchive.ForeColor = System.Drawing.Color.White;
   this.btnArchive.Location = new System.Drawing.Point(400, 330);
     this.btnArchive.Name = "btnArchive";
  this.btnArchive.Size = new System.Drawing.Size(80, 35);
            this.btnArchive.TabIndex = 21;
  this.btnArchive.Text = "Archive";
     this.btnArchive.UseVisualStyleBackColor = false;
        // 
      // btnUpdate
        // 
            this.btnUpdate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(96)))), ((int)(((byte)(174)))));
            this.btnUpdate.FlatAppearance.BorderSize = 0;
            this.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdate.ForeColor = System.Drawing.Color.White;
            this.btnUpdate.Location = new System.Drawing.Point(500, 330);
       this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(80, 35);
            this.btnUpdate.TabIndex = 22;
        this.btnUpdate.Text = "Update";
       this.btnUpdate.UseVisualStyleBackColor = false;
   // 
            // FinanceDetail
        // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
     this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
       this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(244)))), ((int)(((byte)(247)))));
    this.ClientSize = new System.Drawing.Size(640, 390);
         this.Controls.Add(this.btnUpdate);
         this.Controls.Add(this.btnArchive);
    this.Controls.Add(this.cbMemberStatus);
            this.Controls.Add(this.label11);
 this.Controls.Add(this.cbPaymentStatus);
     this.Controls.Add(this.label10);
this.Controls.Add(this.dtDueDate);
         this.Controls.Add(this.label9);
            this.Controls.Add(this.cbCurrency);
  this.Controls.Add(this.label8);
   this.Controls.Add(this.cbDepartment);
       this.Controls.Add(this.label7);
            this.Controls.Add(this.txtExpensesAmount);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtCollectedAmount);
       this.Controls.Add(this.label5);
   this.Controls.Add(this.txtTargetAmount);
     this.Controls.Add(this.label4);
   this.Controls.Add(this.txtDescription);
    this.Controls.Add(this.label3);
            this.Controls.Add(this.txtGoalName);
            this.Controls.Add(this.label2);
   this.Controls.Add(this.panel1);
     this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
     this.Name = "FinanceDetail";
            this.Text = "Finance Detail";
      this.panel1.ResumeLayout(false);
 this.panel1.PerformLayout();
this.ResumeLayout(false);
         this.PerformLayout();

 }

      #endregion

      private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label1;
   private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtGoalName;
        private System.Windows.Forms.Label label3;
   private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtTargetAmount;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtCollectedAmount;
        private System.Windows.Forms.Label label6;
private System.Windows.Forms.TextBox txtExpensesAmount;
        private System.Windows.Forms.Label label7;
   private System.Windows.Forms.ComboBox cbDepartment;
   private System.Windows.Forms.Label label8;
      private System.Windows.Forms.ComboBox cbCurrency;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.DateTimePicker dtDueDate;
        private System.Windows.Forms.Label label10;
   private System.Windows.Forms.ComboBox cbPaymentStatus;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cbMemberStatus;
        private System.Windows.Forms.Button btnArchive;
        private System.Windows.Forms.Button btnUpdate;
}
}