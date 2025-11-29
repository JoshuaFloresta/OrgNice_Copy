namespace ORGnice
{
    partial class ArchivePage
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
 this.btnClose = new System.Windows.Forms.Button();
 this.label1 = new System.Windows.Forms.Label();
     this.ArchiveDGV = new System.Windows.Forms.DataGridView();
    this.label2 = new System.Windows.Forms.Label();
     this.panel1.SuspendLayout();
       ((System.ComponentModel.ISupportInitialize)(this.ArchiveDGV)).BeginInit();
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
            this.panel1.Size = new System.Drawing.Size(800, 50);
       this.panel1.TabIndex = 0;
     // 
    // btnClose
 // 
       this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
      this.btnClose.FlatAppearance.BorderSize = 0;
       this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
     this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
    this.btnClose.ForeColor = System.Drawing.Color.White;
      this.btnClose.Location = new System.Drawing.Point(750, 10);
        this.btnClose.Name = "btnClose";
     this.btnClose.Size = new System.Drawing.Size(30, 30);
    this.btnClose.TabIndex = 1;
   this.btnClose.Text = "×";
      this.btnClose.UseVisualStyleBackColor = false;
 // 
     // label1
  // 
        this.label1.AutoSize = true;
  this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
     this.label1.ForeColor = System.Drawing.Color.White;
     this.label1.Location = new System.Drawing.Point(15, 15);
 this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(125, 24);
     this.label1.TabIndex = 0;
   this.label1.Text = "Archive Page";
   // 
      // ArchiveDGV
       // 
this.ArchiveDGV.AllowUserToAddRows = false;
     this.ArchiveDGV.AllowUserToDeleteRows = false;
          this.ArchiveDGV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
      this.ArchiveDGV.BackgroundColor = System.Drawing.Color.White;
       this.ArchiveDGV.BorderStyle = System.Windows.Forms.BorderStyle.None;
 this.ArchiveDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
    this.ArchiveDGV.Location = new System.Drawing.Point(20, 100);
   this.ArchiveDGV.Name = "ArchiveDGV";
      this.ArchiveDGV.ReadOnly = true;
       this.ArchiveDGV.RowHeadersVisible = false;
    this.ArchiveDGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.ArchiveDGV.Size = new System.Drawing.Size(760, 400);
     this.ArchiveDGV.TabIndex = 1;
   // 
    // label2
        // 
    this.label2.AutoSize = true;
     this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
  this.label2.Location = new System.Drawing.Point(20, 70);
      this.label2.Name = "label2";
  this.label2.Size = new System.Drawing.Size(343, 17);
     this.label2.TabIndex = 2;
   this.label2.Text = "Archived Finance Records - Click Restore to recover";
   // 
 // ArchivePage
            // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
   this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(244)))), ((int)(((byte)(247)))));
      this.ClientSize = new System.Drawing.Size(800, 520);
      this.Controls.Add(this.label2);
   this.Controls.Add(this.ArchiveDGV);
     this.Controls.Add(this.panel1);
  this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
        this.Name = "ArchivePage";
     this.Text = "Archive Page";
      this.panel1.ResumeLayout(false);
     this.panel1.PerformLayout();
       ((System.ComponentModel.ISupportInitialize)(this.ArchiveDGV)).EndInit();
        this.ResumeLayout(false);
       this.PerformLayout();

      }

        #endregion

        private System.Windows.Forms.Panel panel1;
     private System.Windows.Forms.Button btnClose;
private System.Windows.Forms.Label label1;
      private System.Windows.Forms.DataGridView ArchiveDGV;
        private System.Windows.Forms.Label label2;
    }
}