csharp ORGnice\FinanceDetailsForm.cs
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace ORGnice
{
    public class FinanceDetailsForm : Form
    {
        private readonly int _financeId;
        private readonly Action _refresh;
        private readonly string _connString = "server=localhost;port=3306;database=orgdb;uid=root;pwd=Joshua@2004;";

        private readonly Label lblId;
        private readonly TextBox txtDescription;
        private readonly TextBox txtAmount;
        private readonly DateTimePicker dtDate;
        private readonly ComboBox cbCategory;
        private readonly ComboBox cbStatus;
        private readonly TextBox txtNotes;
        private readonly Button btnSave;
        private readonly Button btnArchive;
        private readonly Button btnClear;

        public FinanceDetailsForm(int financeId, Action refresh = null)
        {
            _financeId = financeId;
            _refresh = refresh;
            this.Text = "Finance Details";
            this.StartPosition = FormStartPosition.CenterParent;
            this.ClientSize = new Size(520, 320);

            lblId = new Label { Text = "ID: " + financeId, Location = new Point(12, 12) };
            var lblDesc = new Label { Text = "Description", Location = new Point(12, 36) };
            txtDescription = new TextBox { Location = new Point(12, 54), Width = 480 };

            var lblAmount = new Label { Text = "Amount", Location = new Point(12, 84) };
            txtAmount = new TextBox { Location = new Point(12, 102), Width = 200 };

            var lblDate = new Label { Text = "Date", Location = new Point(230, 84) };
            dtDate = new DateTimePicker { Location = new Point(230, 102), Width = 150, Format = DateTimePickerFormat.Custom, CustomFormat = "yyyy-MM-dd HH:mm" };

            var lblCategory = new Label { Text = "Category", Location = new Point(12, 134) };
            cbCategory = new ComboBox { Location = new Point(12, 152), Width = 200 };
            cbCategory.Items.AddRange(new object[] { "Sales", "Purchase", "Salary", "Other" });

            var lblStatus = new Label { Text = "Status", Location = new Point(230, 134) };
            cbStatus = new ComboBox { Location = new Point(230, 152), Width = 150 };
            cbStatus.Items.AddRange(new object[] { "Planned", "Completed", "Cancelled" });

            var lblNotes = new Label { Text = "Notes", Location = new Point(12, 184) };
            txtNotes = new TextBox { Location = new Point(12, 202), Width = 480, Height = 60, Multiline = true };

            btnSave = new Button { Text = "Save", Location = new Point(300, 272), Size = new Size(70, 28) };
            btnArchive = new Button { Text = "Archive", Location = new Point(380, 272), Size = new Size(70, 28), BackColor = Color.Firebrick, ForeColor = Color.White };
            btnClear = new Button { Text = "Clear", Location = new Point(460, 272), Size = new Size(70, 28) };

            Controls.AddRange(new Control[] {
                lblId, lblDesc, txtDescription,
                lblAmount, txtAmount,
                lblDate, dtDate,
                lblCategory, cbCategory,
                lblStatus, cbStatus,
                lblNotes, txtNotes,
                btnSave, btnArchive, btnClear
            });

            btnSave.Click += BtnSave_Click;
            btnArchive.Click += BtnArchive_Click;
            btnClear.Click += (s,e) => ClearForm();

            LoadFinance();
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

        private void LoadFinance()
        {
            try
            {
                using (var conn = new MySqlConnection(_connString))
                {
                    conn.Open();
                    string sql = @"
                        SELECT finance_id, description, amount, date, category, status, notes
                        FROM finances
                        WHERE finance_id = @id";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", _financeId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtDescription.Text = reader["description"]?.ToString();
                                txtAmount.Text = reader["amount"]?.ToString();
                                if (reader["date"] != DBNull.Value && DateTime.TryParse(reader["date"].ToString(), out DateTime d))
                                {
                                    dtDate.Value = d;
                                }

                                cbCategory.Text = reader["category"]?.ToString();
                                cbStatus.Text = reader["status"]?.ToString();
                                txtNotes.Text = reader["notes"] == DBNull.Value ? string.Empty : reader["notes"].ToString();
                            }
                            else
                            {
                                MessageBox.Show("Finance record not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                DialogResult = DialogResult.Cancel;
                                Close();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to load record: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                        UPDATE finances SET
                            description = @description,
                            amount = @amount,
                            date = @date,
                            category = @category,
                            status = @status,
                            notes = @notes,
                            updated_at = NOW()
                        WHERE finance_id = @id";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", _financeId);
                        cmd.Parameters.AddWithValue("@description", txtDescription.Text.Trim());
                        cmd.Parameters.AddWithValue("@amount", amount);
                        cmd.Parameters.AddWithValue("@date", dtDate.Value);
                        cmd.Parameters.AddWithValue("@category", string.IsNullOrWhiteSpace(cbCategory.Text) ? (object)DBNull.Value : cbCategory.Text);
                        cmd.Parameters.AddWithValue("@status", string.IsNullOrWhiteSpace(cbStatus.Text) ? "Planned" : cbStatus.Text);
                        cmd.Parameters.AddWithValue("@notes", string.IsNullOrWhiteSpace(txtNotes.Text) ? (object)DBNull.Value : txtNotes.Text.Trim());

                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("Finance updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            _refresh?.Invoke();
                            DialogResult = DialogResult.OK;
                            Close();
                        }
                        else
                        {
                            MessageBox.Show("No changes saved.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnArchive_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Archive this finance record? You can restore it later.", "Confirm Archive", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result != DialogResult.Yes) return;

            try
            {
                using (var conn = new MySqlConnection(_connString))
                {
                    conn.Open();
                    string sql = @"
                        UPDATE finances
                        SET is_archived = 1, archived_at = NOW()
                        WHERE finance_id = @id";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", _financeId);
                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("Archived.");
                            _refresh?.Invoke();
                            DialogResult = DialogResult.OK;
                            Close();
                        }
                        else
                        {
                            MessageBox.Show("Failed to archive.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error archiving: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}