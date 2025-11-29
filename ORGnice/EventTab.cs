using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ORGnice
{
    public partial class Event_tab : UserControl
    {
        private Crud eventsCrud;

        public Event_tab()
        {
            InitializeComponent();
            eventsCrud = new Crud("events");

            Search_btn.Click += Search_btn_Click;
            eventDGV.CellContentClick += eventDGV_CellContentClick;
            // wire your Archive button to button3_Click (or ArchiveBtn_Click) in designer or here:
            // ArchiveBtn.Click += button3_Click;

            // Create Details button column once
            if (!eventDGV.Columns.Contains("DetailsColumn"))
            {
                var btnCol = new DataGridViewButtonColumn();
                btnCol.Name = "DetailsColumn";
                btnCol.HeaderText = "Details";
                btnCol.Text = "Details";
                btnCol.UseColumnTextForButtonValue = true;

                btnCol.DefaultCellStyle.BackColor = Color.White;
                btnCol.DefaultCellStyle.ForeColor = Color.FromArgb(34, 96, 174);
                btnCol.DefaultCellStyle.SelectionBackColor = Color.FromArgb(34, 96, 174);
                btnCol.DefaultCellStyle.SelectionForeColor = Color.White;

                eventDGV.Columns.Add(btnCol);
            }

            LoadEventData();
        }

        private void LoadEventData()
        {
            // This Crud method must use WHERE is_archived = 0 in its SQL
            DataTable data = eventsCrud.GetActiveRecordsForDisplay();
            eventDGV.DataSource = data;

            foreach (DataGridViewColumn col in eventDGV.Columns)
                col.Visible = false;

            eventDGV.Columns["event_id"].Visible = true;
            eventDGV.Columns["event_name"].Visible = true;
            eventDGV.Columns["start_datetime"].Visible = true;
            eventDGV.Columns["end_datetime"].Visible = true;
            eventDGV.Columns["department"].Visible = true;
            eventDGV.Columns["status"].Visible = true;
            eventDGV.Columns["venue"].Visible = true;
            eventDGV.Columns["DetailsColumn"].Visible = true;

            eventDGV.Columns["DetailsColumn"].DisplayIndex = 0;
            eventDGV.Columns["event_id"].DisplayIndex = 1;
            eventDGV.Columns["event_name"].DisplayIndex = 2;
            eventDGV.Columns["start_datetime"].DisplayIndex = 3;
            eventDGV.Columns["end_datetime"].DisplayIndex = 4;
            eventDGV.Columns["department"].DisplayIndex = 5;
            eventDGV.Columns["status"].DisplayIndex = 6;
            eventDGV.Columns["venue"].DisplayIndex = 7;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            using (var cEvent = new CreateEvent(LoadEventData))
            {
                cEvent.ShowDialog(this);
            }
        }

        private void Search_btn_Click(object sender, EventArgs e)
        {
            string searchText = MemberSearchBox.Text;

            if (string.IsNullOrEmpty(searchText))
            {
                LoadEventData();
            }
            else
            {
                DataTable searchResults = eventsCrud.SearchByName(searchText);
                eventDGV.DataSource = searchResults;
            }
        }

        private void eventDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (eventDGV.Columns[e.ColumnIndex].Name == "DetailsColumn")
            {
                int eventId = Convert.ToInt32(
                    eventDGV.Rows[e.RowIndex].Cells["event_id"].Value);

                // IMPORTANT: pass LoadEventData so details form can refresh
                using (var detailsForm = new EventDetailsForm(eventId, LoadEventData))
                {
                    if (detailsForm.ShowDialog(this) == DialogResult.OK)
                        LoadEventData();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Host the ArchivedEvents UserControl inside a temporary modal Form
            using (var archivedForm = new Form())
            {
                archivedForm.Text = "Archived Events";
                archivedForm.StartPosition = FormStartPosition.CenterParent;
                archivedForm.ClientSize = new Size(700, 540);
                archivedForm.FormBorderStyle = FormBorderStyle.Sizable;

                var archivedControl = new ArchivedEvents(); // now a UserControl
                archivedControl.Dock = DockStyle.Fill;

                archivedForm.Controls.Add(archivedControl);

                var owner = this.FindForm();
                archivedForm.ShowDialog(owner);
            }

            // Refresh in case any events were restored in the archived UI
            LoadEventData();
        }

        private void ArchiveBtn_Click(object sender, EventArgs e)
        {
            button3_Click(sender, e);
        }
    }
}
