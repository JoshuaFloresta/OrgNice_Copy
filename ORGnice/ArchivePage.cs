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
    public partial class ArchivePage : Form
    {
        public ArchivePage()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

         // Initialize database structure if needed
  InitializeDatabaseStructure();

     // Wire up event handlers
    if (btnClose != null)
     btnClose.Click += btnClose_Click;

if (ArchiveDGV != null)
       ArchiveDGV.CellContentClick += ArchiveDGV_CellContentClick;

    // Load archived data when form opens
            LoadArchivedData();
  }

        private void InitializeDatabaseStructure()
  {
    try
 {
  string connectionString = "server=localhost;port=3306;database=orgdb;uid=root;pwd=legorocket3368.;";

     using (var connection = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
      {
         connection.Open();

   // Check if is_archived column exists, add it if it doesn't
      string checkColumnSql = @"SELECT COUNT(*) FROM information_schema.columns 
     WHERE table_schema = 'orgdb' 
      AND table_name = 'event_goals' 
      AND column_name = 'is_archived'";

    using (var cmd = new MySql.Data.MySqlClient.MySqlCommand(checkColumnSql, connection))
     {
   int columnExists = Convert.ToInt32(cmd.ExecuteScalar());
      
if (columnExists == 0)
       {
               // Add is_archived column
      string addColumnSql = "ALTER TABLE event_goals ADD COLUMN is_archived TINYINT(1) DEFAULT 0";
          using (var addCmd = new MySql.Data.MySqlClient.MySqlCommand(addColumnSql, connection))
         {
 addCmd.ExecuteNonQuery();
   }
      
       // Add archived_at column
        string addDateColumnSql = "ALTER TABLE event_goals ADD COLUMN archived_at DATETIME NULL";
   using (var addDateCmd = new MySql.Data.MySqlClient.MySqlCommand(addDateColumnSql, connection))
           {
  addDateCmd.ExecuteNonQuery();
  }
  }
   }
  }
    }
            catch (Exception ex)
     {
   System.Diagnostics.Debug.WriteLine($"Error initializing database structure: {ex.Message}");
    // Don't throw here, let the form load anyway
            }
   }

        private void ArchivePage_Load(object sender, EventArgs e)
        {
    try
  {
       LoadArchivedData();
          }
            catch (Exception ex)
 {
   System.Diagnostics.Debug.WriteLine($"ArchivePage Load Error: {ex.Message}");
       MessageBox.Show($"Error loading archive data: {ex.Message}", "Load Error", 
          MessageBoxButtons.OK, MessageBoxIcon.Error);
  }
 }

        private void LoadArchivedData()
        {
        try
        {
                System.Diagnostics.Debug.WriteLine("LoadArchivedData: Starting");

                // Check if ArchiveDGV exists
          if (ArchiveDGV == null)
            {
         System.Diagnostics.Debug.WriteLine("ArchiveDGV is null - cannot load data");
    MessageBox.Show("Archive data grid is not available.", "Error", 
MessageBoxButtons.OK, MessageBoxIcon.Error);
                 return;
    }

        // Test database connection first
  if (!TestDatabaseConnection())
       {
    MessageBox.Show("Cannot connect to database. Please check your database connection.", 
       "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
       return;
    }

                System.Diagnostics.Debug.WriteLine("LoadArchivedData: Getting archived finances");
                var archivedFinances = FinanceClass.GetArchivedFinance();
System.Diagnostics.Debug.WriteLine($"LoadArchivedData: Retrieved {archivedFinances?.Count ?? 0} archived finance records");

  // Convert to DataTable for DataGridView
          DataTable dataTable = ConvertFinanceListToDataTable(archivedFinances);
             System.Diagnostics.Debug.WriteLine($"LoadArchivedData: DataTable created with {dataTable.Rows.Count} rows");

   ArchiveDGV.DataSource = dataTable;
        System.Diagnostics.Debug.WriteLine("LoadArchivedData: DataSource set");

    // Configure column visibility and headers
      ConfigureDataGridView();
   System.Diagnostics.Debug.WriteLine("LoadArchivedData: ConfigureDataGridView completed");
            }
  catch (Exception ex)
{
                System.Diagnostics.Debug.WriteLine($"Error loading archived finance data: {ex.Message}");
 System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
   
      MessageBox.Show($"Error loading archived finance data: {ex.Message}\n\nPlease check the database connection and try again.", 
          "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

   // Try to show an empty table at least
        try
    {
   if (ArchiveDGV != null)
      {
       ArchiveDGV.DataSource = new DataTable();
      }
    }
    catch
       {
          // If even this fails, we have a serious problem
                }
}
        }

        private bool TestDatabaseConnection()
   {
            try
            {
       System.Diagnostics.Debug.WriteLine("Testing database connection");
      string connectionString = "server=localhost;port=3306;database=orgdb;uid=root;pwd=legorocket3368.;";

                using (var connection = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
        {
               connection.Open();
   
       // Simple test query
      string sql = "SELECT COUNT(*) FROM event_goals WHERE is_archived = 1";
         using (var cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, connection))
 {
 object result = cmd.ExecuteScalar();
          System.Diagnostics.Debug.WriteLine($"Database connection test successful. Archived records count: {result}");
         return true;
       }
 }
        }
            catch (Exception ex)
  {
       System.Diagnostics.Debug.WriteLine($"Database connection test failed: {ex.Message}");
          return false;
          }
        }

        private DataTable ConvertFinanceListToDataTable(List<FinanceClass> finances)
        {
    DataTable dt = new DataTable();

    try
       {
         System.Diagnostics.Debug.WriteLine("ConvertFinanceListToDataTable: Starting");

     // Add columns
         dt.Columns.Add("goal_id", typeof(int));
     dt.Columns.Add("goal_name", typeof(string));
        dt.Columns.Add("event_id", typeof(int));
        dt.Columns.Add("target_amount", typeof(decimal));
          dt.Columns.Add("collected_amount", typeof(decimal));
           dt.Columns.Add("expenses_amount", typeof(decimal));
    dt.Columns.Add("payment_status", typeof(string));
            dt.Columns.Add("member_status", typeof(string));
          dt.Columns.Add("due_date", typeof(DateTime));
          dt.Columns.Add("currency", typeof(string));
    dt.Columns.Add("archived_at", typeof(DateTime));

     System.Diagnostics.Debug.WriteLine("ConvertFinanceListToDataTable: Columns added");

             // Add rows
         if (finances != null)
        {
          foreach (var finance in finances)
     {
       DataRow row = dt.NewRow();
        row["goal_id"] = finance.GoalId;
         row["goal_name"] = finance.GoalName ?? "";
  row["event_id"] = finance.EventId;
          row["target_amount"] = finance.TargetAmount;
 row["collected_amount"] = finance.CollectedAmount;
   row["expenses_amount"] = finance.ExpensesAmount;
         row["payment_status"] = finance.PaymentStatus ?? "";
               row["member_status"] = finance.MemberStatus ?? "";
        row["due_date"] = finance.DueDate.HasValue ? (object)finance.DueDate.Value : DBNull.Value;
          row["currency"] = finance.Currency ?? "PHP";
     row["archived_at"] = finance.ArchivedAt.HasValue ? (object)finance.ArchivedAt.Value : DBNull.Value;
    dt.Rows.Add(row);
    }
           System.Diagnostics.Debug.WriteLine($"ConvertFinanceListToDataTable: Added {finances.Count} rows");
    }
          else
             {
           System.Diagnostics.Debug.WriteLine("ConvertFinanceListToDataTable: finances list is null");
        }
    }
            catch (Exception ex)
     {
     System.Diagnostics.Debug.WriteLine($"Error converting finance list to DataTable: {ex.Message}");
         throw;
     }

        return dt;
        }

        private void ConfigureDataGridView()
 {
        try
       {
  System.Diagnostics.Debug.WriteLine("ConfigureDataGridView: Starting");

      if (ArchiveDGV == null || ArchiveDGV.DataSource == null)
       {
      System.Diagnostics.Debug.WriteLine("ArchiveDGV or DataSource is null - cannot configure");
          return;
   }

      // Hide all columns first
             foreach (DataGridViewColumn col in ArchiveDGV.Columns)
           {
          col.Visible = false;
     }

       System.Diagnostics.Debug.WriteLine("ConfigureDataGridView: All columns hidden");

            // Show only selected columns
 if (ArchiveDGV.Columns.Contains("goal_id"))
    {
   ArchiveDGV.Columns["goal_id"].Visible = true;
         ArchiveDGV.Columns["goal_id"].HeaderText = "Goal ID";
                  ArchiveDGV.Columns["goal_id"].Width = 80;
      }

    if (ArchiveDGV.Columns.Contains("goal_name"))
      {
    ArchiveDGV.Columns["goal_name"].Visible = true;
                    ArchiveDGV.Columns["goal_name"].HeaderText = "Goal Name";
     ArchiveDGV.Columns["goal_name"].Width = 150;
    }

     if (ArchiveDGV.Columns.Contains("target_amount"))
                {
   ArchiveDGV.Columns["target_amount"].Visible = true;
        ArchiveDGV.Columns["target_amount"].HeaderText = "Target Amount";
          ArchiveDGV.Columns["target_amount"].Width = 120;
          }

                if (ArchiveDGV.Columns.Contains("collected_amount"))
{
         ArchiveDGV.Columns["collected_amount"].Visible = true;
          ArchiveDGV.Columns["collected_amount"].HeaderText = "Collected";
     ArchiveDGV.Columns["collected_amount"].Width = 120;
       }

           if (ArchiveDGV.Columns.Contains("payment_status"))
     {
      ArchiveDGV.Columns["payment_status"].Visible = true;
      ArchiveDGV.Columns["payment_status"].HeaderText = "Payment Status";
         ArchiveDGV.Columns["payment_status"].Width = 120;
 }

                if (ArchiveDGV.Columns.Contains("archived_at"))
      {
 ArchiveDGV.Columns["archived_at"].Visible = true;
 ArchiveDGV.Columns["archived_at"].HeaderText = "Archived Date";
ArchiveDGV.Columns["archived_at"].Width = 120;
                }

   System.Diagnostics.Debug.WriteLine("ConfigureDataGridView: Columns configured");

 // Add Restore button column if not exists
       if (!ArchiveDGV.Columns.Contains("RestoreColumn"))
     {
          var btnCol = new DataGridViewButtonColumn();
               btnCol.Name = "RestoreColumn";
     btnCol.HeaderText = "Actions";
         btnCol.Text = "Restore";
        btnCol.UseColumnTextForButtonValue = true;
              btnCol.Width = 100;

          btnCol.DefaultCellStyle.BackColor = Color.White;
        btnCol.DefaultCellStyle.ForeColor = Color.FromArgb(40, 167, 69);
         btnCol.DefaultCellStyle.SelectionBackColor = Color.FromArgb(40, 167, 69);
         btnCol.DefaultCellStyle.SelectionForeColor = Color.White;

      ArchiveDGV.Columns.Add(btnCol);
          System.Diagnostics.Debug.WriteLine("ConfigureDataGridView: Restore button column added");
          }

              // Set column display order
         SetColumnDisplayOrder();
 System.Diagnostics.Debug.WriteLine("ConfigureDataGridView: Column display order set");
     }
            catch (Exception ex)
   {
           System.Diagnostics.Debug.WriteLine($"Error configuring DataGridView: {ex.Message}");
       System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
           throw;
            }
        }

   private void SetColumnDisplayOrder()
  {
       try
        {
       if (ArchiveDGV == null) return;

           if (ArchiveDGV.Columns.Contains("RestoreColumn"))
                    ArchiveDGV.Columns["RestoreColumn"].DisplayIndex = 0;

       if (ArchiveDGV.Columns.Contains("goal_id"))
   ArchiveDGV.Columns["goal_id"].DisplayIndex = 1;

       if (ArchiveDGV.Columns.Contains("goal_name"))
             ArchiveDGV.Columns["goal_name"].DisplayIndex = 2;

                if (ArchiveDGV.Columns.Contains("target_amount"))
      ArchiveDGV.Columns["target_amount"].DisplayIndex = 3;

         if (ArchiveDGV.Columns.Contains("collected_amount"))
          ArchiveDGV.Columns["collected_amount"].DisplayIndex = 4;

              if (ArchiveDGV.Columns.Contains("payment_status"))
   ArchiveDGV.Columns["payment_status"].DisplayIndex = 5;

    if (ArchiveDGV.Columns.Contains("archived_at"))
      ArchiveDGV.Columns["archived_at"].DisplayIndex = 6;
      }
  catch (Exception ex)
{
                System.Diagnostics.Debug.WriteLine($"Error setting column display order: {ex.Message}");
  }
        }

  private void ArchiveDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
     {
            try
          {
                if (e.RowIndex < 0 || ArchiveDGV == null) return;

          if (ArchiveDGV.Columns[e.ColumnIndex].Name == "RestoreColumn")
  {
           // Get the goal_id of the clicked row
         var goalIdCell = ArchiveDGV.Rows[e.RowIndex].Cells["goal_id"];
 if (goalIdCell?.Value == null) return;

            int goalId = Convert.ToInt32(goalIdCell.Value);

   DialogResult result = MessageBox.Show(
  "Are you sure you want to restore this finance record? This will move it back to the active list.",
           "Confirm Restore",
 MessageBoxButtons.YesNo,
              MessageBoxIcon.Question);

      if (result == DialogResult.Yes)
             {
     bool success = RestoreFinanceById(goalId.ToString());

      if (success)
 {
         MessageBox.Show("Finance record restored successfully!", "Success",
   MessageBoxButtons.OK, MessageBoxIcon.Information);
            // Refresh the archive data grid
    LoadArchivedData();
        }
    else
           {
               MessageBox.Show("Failed to restore finance record. Please try again.",
        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
  }
     }
     }
            }
  catch (Exception ex)
         {
          System.Diagnostics.Debug.WriteLine($"Error in ArchiveDGV_CellContentClick: {ex.Message}");
              MessageBox.Show($"An error occurred: {ex.Message}", "Error",
      MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
  /// Restore archived finance record by ID
        /// </summary>
 /// <param name="goalId">ID of the finance record to restore</param>
        /// <returns>True if finance record was restored successfully</returns>
        private bool RestoreFinanceById(string goalId)
        {
            try
            {
   System.Diagnostics.Debug.WriteLine($"RestoreFinanceById: Attempting to restore goal ID {goalId}");

              string connectionString = "server=localhost;port=3306;database=orgdb;uid=root;pwd=legorocket3368.;";

  using (var connection = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
   {
        connection.Open();

                    string sql = @"UPDATE event_goals 
       SET is_archived = 0, archived_at = NULL 
  WHERE goal_id = @goalId AND is_archived = 1";

        using (var cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, connection))
             {
   cmd.Parameters.AddWithValue("@goalId", goalId);
           int rowsAffected = cmd.ExecuteNonQuery();
              
    System.Diagnostics.Debug.WriteLine($"RestoreFinanceById: {rowsAffected} rows affected");
  return rowsAffected > 0;
  }
  }
            }
   catch (Exception ex)
            {
         System.Diagnostics.Debug.WriteLine($"Error restoring finance record: {ex.Message}");
          System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
        return false;
            }
     }

        private void btnClose_Click(object sender, EventArgs e)
     {
       try
            {
                this.Close();
     }
   catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error closing ArchivePage: {ex.Message}");
  }
        }
    }
}