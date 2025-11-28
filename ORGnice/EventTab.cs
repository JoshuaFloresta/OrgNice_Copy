using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            ////Load data when form opens
            LoadEventData();

            for (int i = 0; i<100; i++)
            {
               
            }

        }

        private void LoadEventData()
        {
            DataTable data = eventsCrud.GetActiveRecordsForDisplay();
            eventDGV.DataSource = data;

            // Hide all, then show only selected columns
            foreach (DataGridViewColumn col in eventDGV.Columns)
                col.Visible = false;

            eventDGV.Columns["event_id"].Visible = true;
            eventDGV.Columns["event_name"].Visible = true;
            eventDGV.Columns["start_datetime"].Visible = true;
            eventDGV.Columns["end_datetime"].Visible = true;
            eventDGV.Columns["department"].Visible = true;
            eventDGV.Columns["status"].Visible = true;
            eventDGV.Columns["venue"].Visible = true;

            // Add Details button column once
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

            // Order: Details first, then event fields
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
            CreateEvent CEvent = new CreateEvent(LoadEventData);
            CEvent.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void Search_btn_Click(object sender, EventArgs e)
        {
            string searchText = MemberSearchBox.Text;

            if (string.IsNullOrEmpty(searchText))
            {
                // If search box is empty, show all active data
                LoadEventData();
            }
            else
            {
                // Search by name (already filters out deleted records)
                DataTable searchResults = eventsCrud.SearchByName(searchText);
                eventDGV.DataSource = searchResults;
            }
        }

        private void eventDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (eventDGV.Columns[e.ColumnIndex].Name == "DetailsColumn")
            {
                // get the member_id of the clicked row
                int eventId = Convert.ToInt32(eventDGV.Rows[e.RowIndex].Cells["event_id"].Value);

                //TODO: open a details form or modal and load data by memberId
                var detailsForm = new EventDetailsForm(eventId); // your own form
                detailsForm.ShowDialog(this);
            }
        }
    }
}
