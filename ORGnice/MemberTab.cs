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
    public partial class Members_tab : UserControl
    {
        private Crud membersCrud;

        public Members_tab()
        {
            InitializeComponent();
            membersCrud = new Crud("members");
            Search_btn.Click += Search_btn_Click;
            memberDGV.CellContentClick += memberDGV_CellContentClick;

            ////Load data when form opens
            LoadData();

        }

        private void LoadData()
        {
            DataTable data = membersCrud.GetActiveRecordsForDisplay();
            memberDGV.DataSource = data;

            foreach (DataGridViewColumn col in memberDGV.Columns)
                col.Visible = false;

            memberDGV.Columns["member_id"].Visible = true;
            memberDGV.Columns["last_name"].Visible = true;
            memberDGV.Columns["first_name"].Visible = true;
            memberDGV.Columns["gender"].Visible = true;
            memberDGV.Columns["email"].Visible = true;
            memberDGV.Columns["username"].Visible = true;
            memberDGV.Columns["department"].Visible = true;

            if (!memberDGV.Columns.Contains("DetailsColumn"))
            {
                var btnCol = new DataGridViewButtonColumn();
                btnCol.Name = "DetailsColumn";
                btnCol.HeaderText = "Details";              // column name
                btnCol.Text = "Details";
                btnCol.UseColumnTextForButtonValue = true;
  
                // cell style: border/background + forecolor
                btnCol.DefaultCellStyle.BackColor = Color.White;
                btnCol.DefaultCellStyle.ForeColor = Color.FromArgb(34, 96, 174);
                btnCol.DefaultCellStyle.SelectionBackColor = Color.FromArgb(34, 96, 174);
                btnCol.DefaultCellStyle.SelectionForeColor = Color.White;

                memberDGV.Columns.Add(btnCol);
            }

            // header style for this column
            var headerStyle = memberDGV.Columns["DetailsColumn"].HeaderCell.Style;

             // order: Details first
            memberDGV.Columns["DetailsColumn"].DisplayIndex = 0;
            memberDGV.Columns["member_id"].DisplayIndex = 1;
            memberDGV.Columns["last_name"].DisplayIndex = 2;
            memberDGV.Columns["first_name"].DisplayIndex = 3;
            memberDGV.Columns["gender"].DisplayIndex = 4;
            memberDGV.Columns["email"].DisplayIndex = 5;
            memberDGV.Columns["username"].DisplayIndex = 6;
            memberDGV.Columns["department"].DisplayIndex = 7;
        }

        private void memberDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (memberDGV.Columns[e.ColumnIndex].Name == "DetailsColumn")
            {
                // get the member_id of the clicked row
                int memberId = Convert.ToInt32(memberDGV.Rows[e.RowIndex].Cells["member_id"].Value);

            //TODO: open a details form or modal and load data by memberId
                 var detailsForm = new MemberDetailsForm(memberId); // your own form
                detailsForm.ShowDialog(this);
            }
        }





        private void Search_btn_Click(object sender, EventArgs e)
        {
            string searchText = MemberSearchBox.Text;

            if (string.IsNullOrEmpty(searchText))
            {
                // If search box is empty, show all active data
                LoadData();
            }
            else
            {
                // Search by name (already filters out deleted records)
                DataTable searchResults = membersCrud.SearchByName(searchText);
                memberDGV.DataSource = searchResults;
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            var cm = new CreateMembers(LoadData);
            cm.ShowDialog(this);  
        }
    }
}
