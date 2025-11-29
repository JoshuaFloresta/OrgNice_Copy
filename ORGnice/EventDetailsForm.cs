using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;


namespace ORGnice
{
    public partial class EventDetailsForm : Form
    {
        private readonly int _eventId;
        private readonly string _connString =
            "server=localhost;port=3306;database=orgdb;uid=root;pwd=Joshua@2004;";

        private readonly Action _refreshEvents;

        public EventDetailsForm(int eventId, Action refreshEvents = null)
        {
            InitializeComponent();
            _eventId = eventId;
            _refreshEvents = refreshEvents;
            this.StartPosition = FormStartPosition.CenterParent;

            LoadOrganizers();
            LoadEvent();

            cbOrganizer.SelectedIndexChanged += cbOrganizer_SelectedIndexChanged;
            btnSave.Click += btnSave_Click;
            btnClear.Click += btnClear_Click;
            btnArchive.Click += btnArchive_Click;
            close_btn.Click += close_btn_Click;
        }

        private void LoadOrganizers()
        {
            try
            {
                Crud membersCrud = new Crud("members");
                DataTable dt = membersCrud.GetActiveRecordsForDisplay(); // must have member_id, first_name, last_name

                if (!dt.Columns.Contains("FullName"))
                    dt.Columns.Add("FullName", typeof(string));

                foreach (DataRow row in dt.Rows)
                {
                    string fn = row["first_name"]?.ToString() ?? "";
                    string ln = row["last_name"]?.ToString() ?? "";
                    row["FullName"] = (fn + " " + ln).Trim();
                }

                cbOrganizer.DisplayMember = "FullName";
                cbOrganizer.ValueMember = "member_id";
                cbOrganizer.DataSource = dt;
                cbOrganizer.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to load organizers: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadEvent()
        {
            using (var conn = new MySqlConnection(_connString))
            {
                conn.Open();

                string sql = @"
                    SELECT event_id, event_name, start_datetime, end_datetime,
                           department, organizer_member_id, venue, status, notes
                    FROM events
                    WHERE event_id = @id";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", _eventId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtEventId.Text = reader["event_id"].ToString();
                            txtEventName.Text = reader["event_name"]?.ToString();

                            DateTime start = Convert.ToDateTime(reader["start_datetime"]);
                            DateTime end = Convert.ToDateTime(reader["end_datetime"]);
                            dtStart.Value = start;
                            dtEnd.Value = end;

                            cbDepartment.Text = reader["department"]?.ToString();
                            txtVenue.Text = reader["venue"]?.ToString();
                            cbStatus.Text = reader["status"]?.ToString();
                            txtNotes.Text = reader["notes"] == DBNull.Value
                                ? string.Empty
                                : reader["notes"].ToString();

                            if (reader["organizer_member_id"] != DBNull.Value)
                            {
                                int organizerId = Convert.ToInt32(reader["organizer_member_id"]);
                                cbOrganizer.SelectedValue = organizerId;
                                txtOrganizer.Text = GetMemberFullName(organizerId);
                            }
                            else
                            {
                                cbOrganizer.SelectedIndex = -1;
                                txtOrganizer.Text = string.Empty;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Event not found.");
                            // Do not call Close() here — closing/disposing a form inside its constructor
                            // causes the caller's ShowDialog to throw ObjectDisposedException.
                            DialogResult = DialogResult.Cancel;
                            return;
                        }
                    }
                }
            }
        }

        private string GetMemberFullName(int memberId)
        {
            using (var conn = new MySqlConnection(_connString))
            {
                conn.Open();
                string sql = @"SELECT CONCAT(first_name, ' ', last_name) AS full_name
                               FROM members
                               WHERE member_id = @id";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", memberId);
                    object result = cmd.ExecuteScalar();
                    return (result != null && result != DBNull.Value)
                        ? result.ToString()
                        : string.Empty;
                }
            }
        }

        private void cbOrganizer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbOrganizer.SelectedValue != null &&
                int.TryParse(cbOrganizer.SelectedValue.ToString(), out _))
            {
                txtOrganizer.Text = cbOrganizer.Text;
            }
            else
            {
                txtOrganizer.Text = string.Empty;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                using (var conn = new MySqlConnection(_connString))
                {
                    conn.Open();

                    string sql = @"
                UPDATE events SET
                    event_name          = @name,
                    start_datetime      = @start,
                    end_datetime        = @end,
                    department          = @department,
                    organizer_member_id = @organizerId,
                    venue               = @venue,
                    status              = @status,
                    notes               = @notes,
                    updated_at          = CURRENT_TIMESTAMP
                WHERE event_id          = @id";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", _eventId);
                        cmd.Parameters.AddWithValue("@name", txtEventName.Text.Trim());
                        cmd.Parameters.AddWithValue("@start", dtStart.Value);
                        cmd.Parameters.AddWithValue("@end", dtEnd.Value);
                        cmd.Parameters.AddWithValue("@department", cbDepartment.Text);

                        int? organizerId = null;
                        if (cbOrganizer.SelectedValue != null &&
                            int.TryParse(cbOrganizer.SelectedValue.ToString(), out int orgId))
                            organizerId = orgId;

                        cmd.Parameters.AddWithValue("@organizerId",
                            organizerId.HasValue ? (object)organizerId.Value : DBNull.Value);

                        cmd.Parameters.AddWithValue("@venue", txtVenue.Text.Trim());
                        cmd.Parameters.AddWithValue("@status",
                            string.IsNullOrWhiteSpace(cbStatus.Text) ? "Planned" : cbStatus.Text);
                        cmd.Parameters.AddWithValue("@notes",
                            string.IsNullOrWhiteSpace(txtNotes.Text) ? (object)DBNull.Value : txtNotes.Text.Trim());

                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("Event updated successfully.");
                            _refreshEvents?.Invoke();
                            DialogResult = DialogResult.OK;
                            Close();
                        }
                        else
                        {
                            MessageBox.Show("No changes were saved.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Show actual error so you can diagnose why the update fails
                MessageBox.Show("Error saving event: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnArchive_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Archive this event? You can restore it later from the archive.",
                "Confirm Archive", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result != DialogResult.Yes) return;

            try
            {
                using (var conn = new MySqlConnection(_connString))
                {
                    conn.Open();

                    string sql = @"
                UPDATE events
                SET is_archived = 1,
                    archived_at = NOW()
                WHERE event_id = @id";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", _eventId);

                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("Event archived.");
                            _refreshEvents?.Invoke();
                            DialogResult = DialogResult.OK;
                            Close();
                        }
                        else
                        {
                            MessageBox.Show("Failed to archive event.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error archiving event: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtEventName.Clear();
            txtVenue.Clear();
            txtNotes.Clear();
            txtOrganizer.Text = "";

            if (cbOrganizer != null) cbOrganizer.SelectedIndex = -1;
            if (cbDepartment != null && cbDepartment.Items.Count > 0) cbDepartment.SelectedIndex = -1;
            if (cbStatus != null && cbStatus.Items.Count > 0) cbStatus.SelectedIndex = 0;

            dtStart.Value = DateTime.Today;
            dtEnd.Value = DateTime.Today;
        }

        private void close_btn_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}