using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace ORGnice
{
    public partial class Members_tab : UserControl
    {
        private Crud membersCrud;

        public Members_tab()
        {
            InitializeComponent();
            membersCrud = new Crud("members");

            Search_btn.Click += Search_btn_Click;
            memberDGV.CellContentClick += memberDGV_CellContentClick;

            // Wire email notification button -> ensure 'button6' is the actual control name in the designer
            button6.Click += button6_Click;

            // Create Details button column once
            if (!memberDGV.Columns.Contains("DetailsColumn"))
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

                memberDGV.Columns.Add(btnCol);
            }

            LoadData();
        }

        private void LoadData()
        {
            DataTable data = membersCrud.GetActiveRecordsForDisplay();
            memberDGV.DataSource = data;

            foreach (DataGridViewColumn col in memberDGV.Columns)
                col.Visible = false;

            memberDGV.Columns["member_id"].Visible  = true;
            memberDGV.Columns["last_name"].Visible  = true;
            memberDGV.Columns["first_name"].Visible = true;
            memberDGV.Columns["gender"].Visible     = true;
            memberDGV.Columns["email"].Visible      = true;
            memberDGV.Columns["username"].Visible   = true;
            memberDGV.Columns["department"].Visible = true;
            memberDGV.Columns["DetailsColumn"].Visible = true;

            // place Details at the front
            memberDGV.Columns["DetailsColumn"].DisplayIndex = 0;
            memberDGV.Columns["member_id"].DisplayIndex     = 1;
            memberDGV.Columns["last_name"].DisplayIndex     = 2;
            memberDGV.Columns["first_name"].DisplayIndex    = 3;
            memberDGV.Columns["gender"].DisplayIndex        = 4;
            memberDGV.Columns["email"].DisplayIndex         = 5;
            memberDGV.Columns["username"].DisplayIndex      = 6;
            memberDGV.Columns["department"].DisplayIndex    = 7;
        }

        private void memberDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            string colName = memberDGV.Columns[e.ColumnIndex].Name;

            if (colName == "DetailsColumn")
            {
                int memberId = Convert.ToInt32(
                    memberDGV.Rows[e.RowIndex].Cells["member_id"].Value);

                using (var detailsForm = new MemberDetailsForm(memberId))
                {
                    if (detailsForm.ShowDialog(this) == DialogResult.OK)
                        LoadData(); // refresh after edits or archive from details form
                }
            }
        }

        private void Search_btn_Click(object sender, EventArgs e)
        {
            string searchText = MemberSearchBox.Text;

            if (string.IsNullOrEmpty(searchText))
            {
                LoadData();
            }
            else
            {
                DataTable searchResults = membersCrud.SearchByName(searchText);
                memberDGV.DataSource = searchResults;
            }       
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var cm = new CreateMembers(LoadData))
            {
                cm.ShowDialog(this);
            }
        }

        private void Archive_Click(object sender, EventArgs e)
        {
            // Open a modal form that hosts the ArchivedMembers UserControl.
            // Use a using block so the temporary form is disposed after close.
            using (var archivedForm = new Form())
            {
                archivedForm.Text = "Archived Members";
                archivedForm.StartPosition = FormStartPosition.CenterParent;
                archivedForm.ClientSize = new Size(700, 540);
                archivedForm.FormBorderStyle = FormBorderStyle.Sizable;

                var archivedControl = new ArchivedMembers();
                archivedControl.Dock = DockStyle.Fill;

                archivedForm.Controls.Add(archivedControl);

                var owner = this.FindForm(); // parent form
                archivedForm.ShowDialog(owner);
            }

            // Refresh main grid in case any archived member was restored while the dialog was open
            LoadData();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            using (var cm = new FrmEmailNotification())
            {
                cm.ShowDialog(this);
            }
        }
    }
}
