using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ORGnice
{
    public class ArchivedEvents : UserControl
    {
        private readonly Crud eventsCrud;
        private readonly DataGridView archivedDGV;
        private readonly TextBox searchBox;
        private readonly Button searchBtn;
        private readonly Button refreshBtn;

        public ArchivedEvents()
        {
            this.Dock = DockStyle.Fill;
            eventsCrud = new Crud("events");

            // Controls
            searchBox = new TextBox { Location = new Point(12, 10), Width = 300 };
            searchBtn = new Button { Text = "Search", Location = new Point(322, 8), Width = 80 };
            refreshBtn = new Button { Text = "Refresh", Location = new Point(408, 8), Width = 80 };

            archivedDGV = new DataGridView
            {
                Location = new Point(12, 44),
                Size = new Size(792, 480),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AutoGenerateColumns = true
            };

            this.Controls.Add(searchBox);
            this.Controls.Add(searchBtn);
            this.Controls.Add(refreshBtn);
            this.Controls.Add(archivedDGV);

            // Wire events
            searchBtn.Click += SearchBtn_Click;
            refreshBtn.Click += (s, e) => LoadArchivedData();
            archivedDGV.CellContentClick += ArchivedDGV_CellContentClick;

            LoadArchivedData();
        }

        private void LoadArchivedData()
        {
            try
            {
                DataTable data = eventsCrud.ReadArchived();
                archivedDGV.DataSource = data;

                // Add Restore button column AFTER binding
                if (!archivedDGV.Columns.Contains("RestoreColumn"))
                {
                    var restoreCol = new DataGridViewButtonColumn
                    {
                        Name = "RestoreColumn",
                        HeaderText = "Restore",
                        Text = "Restore",
                        UseColumnTextForButtonValue = true,
                        DefaultCellStyle = { ForeColor = Color.FromArgb(34, 96, 174) }
                    };
                    archivedDGV.Columns.Insert(0, restoreCol);
                }

                foreach (DataGridViewColumn col in archivedDGV.Columns)
                    col.Visible = false;

                if (archivedDGV.Columns.Contains("RestoreColumn"))
                    archivedDGV.Columns["RestoreColumn"].Visible = true;

                ShowColumnIfExists("event_id", 1, "ID");
                ShowColumnIfExists("event_name", 2, "Event");
                ShowColumnIfExists("start_datetime", 3, "Start");
                ShowColumnIfExists("end_datetime", 4, "End");
                ShowColumnIfExists("department", 5, "Department");
                ShowColumnIfExists("status", 6, "Status");
                ShowColumnIfExists("venue", 7, "Venue");
                ShowColumnIfExists("archived_at", 8, "Archived At");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to load archived events: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowColumnIfExists(string columnName, int displayIndex, string headerText = null)
        {
            if (archivedDGV.Columns.Contains(columnName))
            {
                var col = archivedDGV.Columns[columnName];
                col.Visible = true;
                col.DisplayIndex = displayIndex;
                if (!string.IsNullOrEmpty(headerText)) col.HeaderText = headerText;
            }
        }

        private void SearchBtn_Click(object sender, EventArgs e)
        {
            string txt = searchBox.Text?.Trim();
            if (string.IsNullOrEmpty(txt))
            {
                LoadArchivedData();
                return;
            }

            try
            {
                DataTable results = eventsCrud.SearchByName(txt, includeArchived: true);
                archivedDGV.DataSource = results;

                if (!archivedDGV.Columns.Contains("RestoreColumn"))
                {
                    var restoreCol = new DataGridViewButtonColumn
                    {
                        Name = "RestoreColumn",
                        HeaderText = "Restore",
                        Text = "Restore",
                        UseColumnTextForButtonValue = true,
                        DefaultCellStyle = { ForeColor = Color.FromArgb(34, 96, 174) }
                    };
                    archivedDGV.Columns.Insert(0, restoreCol);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Search failed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ArchivedDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var colName = archivedDGV.Columns[e.ColumnIndex].Name;

            if (colName == "RestoreColumn")
            {
                object idObj = archivedDGV.Rows[e.RowIndex].Cells["event_id"].Value;
                if (idObj == null || !int.TryParse(idObj.ToString(), out int eventId))
                {
                    MessageBox.Show("Invalid event selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var res = MessageBox.Show("Restore this event? It will reappear in the active events list.", "Confirm Restore", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res != DialogResult.Yes) return;

                try
                {
                    var updateData = new Dictionary<string, object>
                    {
                        { "is_archived", 0 },
                        { "archived_at", DBNull.Value }
                    };

                    int rows = eventsCrud.UpdateById(updateData, "event_id", eventId);
                    if (rows > 0)
                    {
                        MessageBox.Show("Event restored.");
                        LoadArchivedData();
                    }
                    else
                    {
                        MessageBox.Show("Failed to restore event.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error restoring event: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}