using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ORGnice
{
    public class ArchivedMembers : UserControl
    {
        private readonly Crud membersCrud;
        private readonly DataGridView archivedDGV;
        private readonly TextBox searchBox;
        private readonly Button searchBtn;
        private readonly Button refreshBtn;

        public ArchivedMembers()
        {
            this.Dock = DockStyle.Fill;
            membersCrud = new Crud("members");

            // Controls
            searchBox = new TextBox { Location = new Point(12, 10), Width = 220 };
            searchBtn = new Button { Text = "Search", Location = new Point(240, 8), Width = 80 };
            refreshBtn = new Button { Text = "Refresh", Location = new Point(330, 8), Width = 80 };

            archivedDGV = new DataGridView
            {
                Location = new Point(12, 44),
                Size = new Size(500, 420),
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
                // Use Crud.ReadArchived to get archived records (WHERE is_archived = 1)
                DataTable data = membersCrud.ReadArchived();

                archivedDGV.DataSource = data;

                // Ensure Restore button column exists AFTER setting DataSource
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

                // Hide all, then enable specific columns if they exist
                foreach (DataGridViewColumn col in archivedDGV.Columns)
                    col.Visible = false;

                if (archivedDGV.Columns.Contains("RestoreColumn"))
                    archivedDGV.Columns["RestoreColumn"].Visible = true;

                ShowColumnIfExists("member_id", 1, "ID");
                ShowColumnIfExists("last_name", 2, "Last Name");
                ShowColumnIfExists("first_name", 3, "First Name");
                ShowColumnIfExists("gender", 4, "Gender");
                ShowColumnIfExists("email", 5, "Email");
                ShowColumnIfExists("username", 6, "Username");
                ShowColumnIfExists("department", 7, "Department");
                ShowColumnIfExists("archived_at", 8, "Archived At");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to load archived members: " + ex.Message, "Error",
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
                    // Use Crud.SearchByName with includeArchived = true
                DataTable results = membersCrud.SearchByName(txt, includeArchived: true);
                archivedDGV.DataSource = results;

                // Ensure Restore column exists after search result bind
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
                object idObj = archivedDGV.Rows[e.RowIndex].Cells["member_id"].Value;
                if (idObj == null || !int.TryParse(idObj.ToString(), out int memberId))
                {
                    MessageBox.Show("Invalid member selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var res = MessageBox.Show("Restore this member? They will reappear in active members list.", "Confirm Restore", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res != DialogResult.Yes) return;

                try
                {
                    var updateData = new Dictionary<string, object>
                    {
                        { "is_archived", 0 },
                        { "archived_at", DBNull.Value }
                    };

                    int rows = membersCrud.UpdateById(updateData, "member_id", memberId);
                    if (rows > 0)
                    {
                        MessageBox.Show("Member restored.");
                        LoadArchivedData();
                    }
                    else
                    {
                        MessageBox.Show("Failed to restore member.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error restoring member: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}