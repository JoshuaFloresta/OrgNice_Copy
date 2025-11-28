using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            this.btnSave.Click += btnSave_Click;
            this.btnClear.Click += btnClear_Click;

            // optional: default status
            if (cbStatus != null && cbStatus.Items.Count > 0)
                cbStatus.SelectedIndex = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Collect input values
            string eventName = txtEventName.Text.Trim();
            string department = cbDepartment.Text;    // participants derived from this
            string venue = txtVenue.Text.Trim();
            string organizer = txtOrganizer.Text.Trim(); // or a member pick, if you change later
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

            // Build event model (adapt property names to your Event class)
            var ev = new Events
            {
                EventName = eventName,
                StartDateTime = start,
                EndDateTime = end,
                Department = department,
                Venue = venue,
                OrganizerMemberId = null,                 // or parse from a hidden ID textbox
                Status = string.IsNullOrWhiteSpace(status) ? "Planned" : status,
                Notes = txtNotes.Text.Trim()
            };


            bool success = ORGnice.Events.AddEventToDatabase(ev);

            if (success)
            {
                MessageBox.Show("Event created successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                _refreshEvents?.Invoke();
                ClearFormFields();
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
            txtOrganizer.Clear();
            txtNotes.Clear();

            if (cbDepartment != null) cbDepartment.SelectedIndex = -1;
            if (cbStatus != null && cbStatus.Items.Count > 0) cbStatus.SelectedIndex = 0;

            dtStart.Value = DateTime.Today;
            dtEnd.Value = DateTime.Today;
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            ClearFormFields();
        }

        private void close_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    
            

        private void btnClear_Click(object sender, EventArgs e)
        {

        }
    }
}
