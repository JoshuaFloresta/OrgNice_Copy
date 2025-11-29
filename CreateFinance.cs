csharp ORGnice\CreateFinance.cs
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace ORGnice
{
    public class CreateFinance : Form
    {
        private readonly Action _refresh;
        private readonly string _connString = "server=localhost;port=3306;database=orgdb;uid=root;pwd=Joshua@2004;";

        private readonly TextBox txtDescription;
        private readonly TextBox txtAmount;
        private readonly DateTimePicker dtDate;
        private readonly ComboBox cbCategory;
        private readonly ComboBox cbStatus;
        private readonly TextBox txtNotes;
        private readonly Button btnSave;
        private readonly Button btnClear;

        public CreateFinance(Action refresh)
        {
            _refresh = refresh;
            this.Text = "Create Finance";
            this.StartPosition = FormStartPosition.CenterParent;
            this.ClientSize = new Size(520, 300);

            // controls
            var lblDesc = new Label { Text = "Description", Location = new Point(12, 12) };
            txtDescription = new TextBox { Location = new Point(12, 30), Width = 480 };

            var lblAmount = new Label { Text = "Amount", Location = new Point(12, 60) };
            txtAmount = new TextBox { Location = new Point(12, 78), Width = 200 };

            var lblDate = new Label { Text = "Date", Location = new Point(230, 60) };
            dtDate = new DateTimePicker { Location = new Point(230, 78), Width = 150, Format = DateTimePickerFormat.Custom, CustomFormat = "yyyy-MM-dd HH:mm" };

            var lblCategory = new Label { Text = "Category", Location = new Point(12, 110) };
            cbCategory = new ComboBox { Location = new Point(12, 128), Width = 200 };
            cbCategory.Items.AddRange(new object[] { "Sales", "Purchase", "Salary", "Other" });

            var lblStatus = new Label { Text = "Status", Location = new Point(230, 110) };
            cbStatus = new ComboBox { Location = new Point(230, 128), Width = 150 };
            cbStatus.Items.AddRange(new object[] { "Planned", "Completed", "Cancelled" });
            cbStatus.SelectedIndex = 0;

            var lblNotes = new Label { Text = "Notes", Location = new Point(12, 160) };
            txtNotes = new TextBox { Location = new Point(12, 178), Width = 480, Height = 60, Multiline = true };

            btnSave = new Button { Text = "Save", Location = new Point(350, 246), Size = new Size(70, 28) };
            btnClear = new Button { Text = "Clear", Location = new Point(430, 246), Size = new Size(70, 28) };

            Controls.AddRange(new Control[] {
                lblDesc, txtDescription,
                lblAmount, txtAmount,
                lblDate, dtDate,
                lblCategory, cbCategory,
                lblStatus, cbStatus,
                lblNotes, txtNotes,
                btnSave, btnClear
            });

            btnSave.Click += BtnSave_Click;
            btnClear.Click += (s,e) => ClearForm();
        }

        private void ClearForm()
        {
            txtDescription.Clear();
            txtAmount.Clear();
            dtDate.Value = DateTime.Today;
            if (cbCategory.Items.Count > 0) cbCategory.SelectedIndex = -1;
            if (cbStatus.Items.Count > 0) cbStatus.SelectedIndex = 0;
            txtNotes.Clear();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                MessageBox.Show("Description required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtAmount.Text, out decimal amount))
            {
                MessageBox.Show("Invalid amount.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var conn = new MySqlConnection(_connString))
                {
                    conn.Open();
                    string sql = @"
                        INSERT INTO finances (description, amount, date, category, status, notes, created_at)
                        VALUES (@description, @amount, @date, @category, @status, @notes, NOW())";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@description", txtDescription.Text.Trim());
                        cmd.Parameters.AddWithValue("@amount", amount);
                        cmd.Parameters.AddWithValue("@date", dtDate.Value);
                        cmd.Parameters.AddWithValue("@category", string.IsNullOrWhiteSpace(cbCategory.Text) ? (object)DBNull.Value : cbCategory.Text);
                        cmd.Parameters.AddWithValue("@status", string.IsNullOrWhiteSpace(cbStatus.Text) ? "Planned" : cbStatus.Text);
                        cmd.Parameters.AddWithValue("@notes", string.IsNullOrWhiteSpace(txtNotes.Text) ? (object)DBNull.Value : txtNotes.Text.Trim());

                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("Finance record created.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            _refresh?.Invoke();
                            this.DialogResult = DialogResult.OK;
                            Close();
                        }
                        else
                        {
                            MessageBox.Show("Failed to create finance record.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving record: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}