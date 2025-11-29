using System;
using System.Data;
using System.Windows.Forms;

namespace ORGnice
{
    public partial class CreateEvent : Form
    {
        private readonly Action _refreshEvents;

        public CreateEvent(Action refreshEvents)
        {
            InitializeComponent();
            _refreshEvents = refreshEvents;
            this.StartPosition = FormStartPosition.CenterScreen;

            // Wire buttons
            btnSave.Click += btnSave_Click;
            btnClear.Click += BtnClear_Click;
            //buttonClose.Click += buttonClose_Click;   // your close button

            // Do not trigger validation when closing
            //buttonClose.CausesValidation = false;

            if (cbStatus != null && cbStatus.Items.Count > 0)
                cbStatus.SelectedIndex = 0;

            LoadOrganizers();
        }

        // Load active members into organizer ComboBox
        private void LoadOrganizers()
        {
            try
            {
                Crud membersCrud = new Crud("members");
                DataTable dt = membersCrud.GetActiveRecordsForDisplay(); // must include member_id, first_name, last_name

                if (!dt.Columns.Contains("FullName"))
                    dt.Columns.Add("FullName", typeof(string));

                foreach (DataRow row in dt.Rows)
                {
                    string fn = row["first_name"]?.ToString() ?? "";
                    string ln = row["last_name"]?.ToString() ?? "";
                    row["FullName"] = (fn + " " + ln).Trim();
                }

                cbOrganizer.DisplayMember = "FullName";  // name shown
                cbOrganizer.ValueMember = "member_id"; // underlying ID
                cbOrganizer.DataSource = dt;
                cbOrganizer.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to load organizers: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string eventName = txtEventName.Text.Trim();
            string department = cbDepartment.Text;
            string venue = txtVenue.Text.Trim();
            string status = cbStatus.Text;

            DateTime start = dtStart.Value;
            DateTime end = dtEnd.Value;

            if (string.IsNullOrWhiteSpace(eventName))
            {
                MessageBox.Show("Event name is required.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (end < start)
            {
                MessageBox.Show("End date/time cannot be before Start.",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int? organizerId = null;
            if (cbOrganizer.SelectedValue != null &&
                int.TryParse(cbOrganizer.SelectedValue.ToString(), out int id))
            {
                organizerId = id;
            }

            var ev = new Events
            {
                EventName = eventName,
                StartDateTime = start,
                EndDateTime = end,
                Department = department,
                Venue = venue,
                OrganizerMemberId = organizerId,
                Status = string.IsNullOrWhiteSpace(status) ? "Planned" : status,
                Notes = txtNotes.Text.Trim()
            };

            bool success = ORGnice.Events.AddEventToDatabase(ev);

            if (success)
            {
                MessageBox.Show("Event created successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                _refreshEvents?.Invoke();
                this.DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Failed to create event. Please check your input and try again.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearFormFields()
        {
            txtEventName.Clear();
            txtVenue.Clear();
            txtNotes.Clear();

            if (cbDepartment != null) cbDepartment.SelectedIndex = -1;
            if (cbStatus != null && cbStatus.Items.Count > 0) cbStatus.SelectedIndex = 0;
            if (cbOrganizer != null) cbOrganizer.SelectedIndex = -1;

            dtStart.Value = DateTime.Today;
            dtEnd.Value = DateTime.Today;
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            ClearFormFields();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            // no validation here; just close
            Close();
        }
    }
}
