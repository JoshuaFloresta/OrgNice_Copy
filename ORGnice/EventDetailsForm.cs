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

        public EventDetailsForm(int eventId)
        {
            InitializeComponent();
            _eventId = eventId;
            this.StartPosition = FormStartPosition.CenterParent;

            for (int i = 0; i < _eventId; i++)
            {
                txtOrganizerId.Items.Add(i.ToString());
            }

            LoadEvent();
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

                            // organizer: store ID (hidden) and show name
                            if (reader["organizer_member_id"] != DBNull.Value)
                            {
                                int organizerId = Convert.ToInt32(reader["organizer_member_id"]);
                                txtOrganizerId.Text = organizerId.ToString();
                                txtOrganizer.Text = GetMemberFullName(organizerId);
                            }
                            else
                            {
                                txtOrganizerId.Text = string.Empty;
                                txtOrganizer.Text = string.Empty;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Event not found.");
                            DialogResult = DialogResult.Cancel;
                            Close();
                        }
                    }
                }
            }
        }

        // FIXED: query MEMBERS table, and use member_id column
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (var conn = new MySqlConnection(_connString))
            {
                conn.Open();

                string sql = @"
            UPDATE events SET
                event_name         = @name,
                start_datetime     = @start,
                end_datetime       = @end,
                department         = @department,
                organizer_member_id = @organizerId,
                venue              = @venue,
                status             = @status,
                notes              = @notes,
                updated_at         = CURRENT_TIMESTAMP
            WHERE event_id         = @id";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", _eventId);
                    cmd.Parameters.AddWithValue("@name", txtEventName.Text.Trim());
                    cmd.Parameters.AddWithValue("@start", dtStart.Value);
                    cmd.Parameters.AddWithValue("@end", dtEnd.Value);
                    cmd.Parameters.AddWithValue("@department", cbDepartment.Text);

                    if (int.TryParse(txtOrganizerId.Text.Trim(), out int orgId))
                        cmd.Parameters.AddWithValue("@organizerId", (object)orgId);
                    else
                        cmd.Parameters.AddWithValue("@organizerId", DBNull.Value);

                    cmd.Parameters.AddWithValue("@venue", txtVenue.Text.Trim());
                    cmd.Parameters.AddWithValue("@status",
                        string.IsNullOrWhiteSpace(cbStatus.Text) ? "Planned" : cbStatus.Text);
                    cmd.Parameters.AddWithValue("@notes",
                        string.IsNullOrWhiteSpace(txtNotes.Text) ? (object)DBNull.Value : txtNotes.Text.Trim());

                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        MessageBox.Show("Event updated successfully.");
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

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtEventName.Clear();
            txtVenue.Clear();
            txtNotes.Clear();
            txtOrganizerId.Text = "";
            txtOrganizer.Text = "";

            if (cbDepartment != null) cbDepartment.SelectedIndex = -1;
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
