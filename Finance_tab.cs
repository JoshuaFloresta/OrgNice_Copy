csharp ORGnice\Finance_Tab.cs
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ORGnice
{
    public class Finance_tab : UserControl
    {
        private readonly Crud financesCrud;
        private readonly DataGridView financeDGV;
        private readonly Button createBtn;
        private readonly Button refreshBtn;
        private readonly Button archivedBtn;
        private readonly TextBox searchBox;
        private readonly Button searchBtn;

        public Finance_tab()
        {
            this.Dock = DockStyle.Fill;
            financesCrud = new Crud("finances");

            // controls (simple layout)
            createBtn = new Button { Text = "+ Add Finance", Location = new Point(12, 8), Size = new Size(100, 28) };
            refreshBtn = new Button { Text = "Refresh", Location = new Point(122, 8), Size = new Size(80, 28) };
            archivedBtn = new Button { Text = "Archived", Location = new Point(208, 8), Size = new Size(80, 28) };
            searchBox = new TextBox { Location = new Point(300, 12), Width = 200 };
            searchBtn = new Button { Text = "Search", Location = new Point(508, 8), Size = new Size(80, 28) };

            financeDGV = new DataGridView
            {
                Location = new Point(12, 48),
                Size = new Size(820, 460),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AutoGenerateColumns = true
            };

            Controls.Add(createBtn);
            Controls.Add(refreshBtn);
            Controls.Add(archivedBtn);
            Controls.Add(searchBox);
            Controls.Add(searchBtn);
            Controls.Add(financeDGV);

            // Wire events
            createBtn.Click += CreateBtn_Click;
            refreshBtn.Click += (s, e) => LoadFinanceData();
            archivedBtn.Click += ArchivedBtn_Click;
            searchBtn.Click += SearchBtn_Click;
            financeDGV.CellContentClick += FinanceDGV_CellContentClick;

            // Add Details button column once after datasource bound
            LoadFinanceData();
        }

        private void LoadFinanceData()
        {
            try
            {
                DataTable data = financesCrud.GetActiveRecordsForDisplay();
                financeDGV.DataSource = data;

                // ensure details column exists and is first
                if (!financeDGV.Columns.Contains("DetailsColumn"))
                {
                    var btnCol = new DataGridViewButtonColumn
                    {
                        Name = "DetailsColumn",
                        HeaderText = "Details",
                        Text = "Details",
                        UseColumnTextForButtonValue = true
                    };
                    financeDGV.Columns.Insert(0, btnCol);
                }

                foreach (DataGridViewColumn col in financeDGV.Columns)
                    col.Visible = false;

                // show columns if present
                ShowColumnIfExists("DetailsColumn", 0);
                ShowColumnIfExists("finance_id", 1, "ID");
                ShowColumnIfExists("description", 2, "Description");
                ShowColumnIfExists("amount", 3, "Amount");
                ShowColumnIfExists("date", 4, "Date");
                ShowColumnIfExists("category", 5, "Category");
                ShowColumnIfExists("status", 6, "Status");
                ShowColumnIfExists("notes", 7, "Notes");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to load finance records: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowColumnIfExists(string columnName, int displayIndex, string headerText = null)
        {
            if (financeDGV.Columns.Contains(columnName))
            {
                var col = financeDGV.Columns[columnName];
                col.Visible = true;
                col.DisplayIndex = displayIndex;
                if (!string.IsNullOrEmpty(headerText)) col.HeaderText = headerText;
            }
        }

        private void CreateBtn_Click(object sender, EventArgs e)
        {
            using (var createForm = new CreateFinance(LoadFinanceData))
            {
                createForm.ShowDialog(this.FindForm());
            }
        }

        private void FinanceDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            string colName = financeDGV.Columns[e.ColumnIndex].Name;
            if (colName == "DetailsColumn")
            {
                object idObj = financeDGV.Rows[e.RowIndex].Cells["finance_id"].Value;
                if (idObj == null || !int.TryParse(idObj.ToString(), out int financeId))
                {
                    MessageBox.Show("Invalid finance selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (var details = new FinanceDetailsForm(financeId, LoadFinanceData))
                {
                    var owner = this.FindForm();
                    if (details.ShowDialog(owner) == DialogResult.OK)
                        LoadFinanceData();
                }
            }
        }

        private void SearchBtn_Click(object sender, EventArgs e)
        {
            string search = searchBox.Text?.Trim();
            if (string.IsNullOrEmpty(search))
            {
                LoadFinanceData();
                return;
            }

            try
            {
                DataTable results = financesCrud.SearchByName(search); // assumes implementation searches relevant columns
                financeDGV.DataSource = results;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Search failed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ArchivedBtn_Click(object sender, EventArgs e)
        {
            using (var form = new Form())
            {
                form.Text = "Archived Finances";
                form.StartPosition = FormStartPosition.CenterParent;
                form.ClientSize = new Size(860, 560);
                form.FormBorderStyle = FormBorderStyle.Sizable;

                var archivedControl = new ArchivedEvents(); // reuse ArchivedEvents if you want same behavior, or create ArchivedFinances
                archivedControl.Dock = DockStyle.Fill;
                form.Controls.Add(archivedControl);

                form.ShowDialog(this.FindForm());
            }

            LoadFinanceData();
        }
    }
}