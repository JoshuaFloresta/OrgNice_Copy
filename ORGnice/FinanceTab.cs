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
    public partial class FinanceTab : UserControl
    {
      private Crud financeCrud;
        private bool isInitialized = false;
        private int currentGoalId = 0;

        public FinanceTab()
        {
         try
 {
 InitializeComponent();
       System.Diagnostics.Debug.WriteLine("FinanceTab: InitializeComponent completed");
    
       financeCrud = new Crud("event_goals");
 System.Diagnostics.Debug.WriteLine("FinanceTab: Crud initialized");
    
     // IMMEDIATE TEST - Check database right away
     TestDatabaseImmediately();
    
     InitializeEventHandlers();
    System.Diagnostics.Debug.WriteLine("FinanceTab: Event handlers initialized");

          isInitialized = true;
      
   // Add temporary debug button for testing
      //AddDebugButton();
    
   // Load data with delay to ensure everything is ready
    this.Load += FinanceTab_Load;
 }
 catch (Exception ex)
         {
   System.Diagnostics.Debug.WriteLine($"FinanceTab Constructor Error: {ex.Message}");
    MessageBox.Show($"Error initializing Finance Tab: {ex.Message}", "Initialization Error", 
        MessageBoxButtons.OK, MessageBoxIcon.Error);
  }
}

/// <summary>
/// Test database connection and data immediately during construction
/// </summary>
private void TestDatabaseImmediately()
{
    try
 {
   string connectionString = "server=localhost;port=3306;database=orgdb;uid=root;pwd=legorocket3368.;";
     
        using (var connection = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
        {
    connection.Open();
    System.Diagnostics.Debug.WriteLine("IMMEDIATE TEST: Database connection successful");
       
      // Test if table exists
            string checkTableSql = "SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = 'orgdb' AND table_name = 'event_goals'";
         using (var checkCmd = new MySql.Data.MySqlClient.MySqlCommand(checkTableSql, connection))
  {
       int tableExists = Convert.ToInt32(checkCmd.ExecuteScalar());
    System.Diagnostics.Debug.WriteLine($"IMMEDIATE TEST: event_goals table exists: {tableExists > 0}");
        }
        
         // Test simple count
            string countSql = "SELECT COUNT(*) FROM event_goals";
     using (var countCmd = new MySql.Data.MySqlClient.MySqlCommand(countSql, connection))
 {
                int totalCount = Convert.ToInt32(countCmd.ExecuteScalar());
           System.Diagnostics.Debug.WriteLine($"IMMEDIATE TEST: Total records in event_goals: {totalCount}");
 }
     
          // Test active count
            string activeSql = "SELECT COUNT(*) FROM event_goals WHERE (is_archived = 0 OR is_archived IS NULL)";
   using (var activeCmd = new MySql.Data.MySqlClient.MySqlCommand(activeSql, connection))
            {
            int activeCount = Convert.ToInt32(activeCmd.ExecuteScalar());
  System.Diagnostics.Debug.WriteLine($"IMMEDIATE TEST: Active records: {activeCount}");
            }
        
        // Get actual data
            string dataSql = "SELECT goal_id, goal_name, is_archived FROM event_goals LIMIT 5";
            using (var dataCmd = new MySql.Data.MySqlClient.MySqlCommand(dataSql, connection))
    {
             using (var reader = dataCmd.ExecuteReader())
      {
        System.Diagnostics.Debug.WriteLine("IMMEDIATE TEST: Sample records:");
        while (reader.Read())
             {
        System.Diagnostics.Debug.WriteLine($"  ID: {reader["goal_id"]}, Name: {reader["goal_name"]}, Archived: {reader["is_archived"]}");
        }
         }
   }
        }
    }
    catch (Exception ex)
{
        System.Diagnostics.Debug.WriteLine($"IMMEDIATE TEST ERROR: {ex.Message}");
      MessageBox.Show($"Database test failed: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}

  /// <summary>
  /// Temporary method to add debug button for testing
        /// </summary>
//     private void AddDebugButton()
//     {
//    try
//  {
//    Button debugBtn = new Button();
//   debugBtn.Text = "🔧 Debug";
//      debugBtn.Size = new Size(80, 30);
//    debugBtn.Location = new Point(650, 15);
//         debugBtn.BackColor = Color.Orange;
//     debugBtn.ForeColor = Color.White;
//      debugBtn.Click += (s, e) => TestDataRetrieval();
//   this.Controls.Add(debugBtn);
    
//   Button refreshBtn = new Button();
//      refreshBtn.Text = "🔄 Refresh";
//   refreshBtn.Size = new Size(80, 30);
//        refreshBtn.Location = new Point(740, 15);
//   refreshBtn.BackColor = Color.Green;
//      refreshBtn.ForeColor = Color.White;
//    refreshBtn.Click += (s, e) => {
//         System.Diagnostics.Debug.WriteLine("Manual refresh button clicked");
//  LoadFinanceGoals();
//     };
//      this.Controls.Add(refreshBtn);
      
// Button testComboBtn = new Button();
// testComboBtn.Text = "📋 Test CB";
//     testComboBtn.Size = new Size(80, 30);
//   testComboBtn.Location = new Point(830, 15);
//      testComboBtn.BackColor = Color.Blue;
//     testComboBtn.ForeColor = Color.White;
//     testComboBtn.Click += (s, e) => {
//      if (cbFinancial != null) {
//         cbFinancial.Items.Clear();
//    cbFinancial.Items.Add("Test Item 1");
//      cbFinancial.Items.Add("Test Item 2");
//          MessageBox.Show($"ComboBox test: Added 2 items. Count: {cbFinancial.Items.Count}");
//  } else {
//  MessageBox.Show("cbFinancial is NULL!");
//    }
//   };
//   this.Controls.Add(testComboBtn);
  
//  // Add test members loading button
//  Button testMembersBtn = new Button();
//        testMembersBtn.Text = "👥 Test Members";
//  testMembersBtn.Size = new Size(100, 30);
//     testMembersBtn.Location = new Point(650, 50);
// testMembersBtn.BackColor = Color.Purple;
//  testMembersBtn.ForeColor = Color.White;
//   testMembersBtn.Click += (s, e) => {
//   if (currentGoalId > 0) {
//    LoadMembersForGoal(currentGoalId);
//    } else if (cbFinancial.SelectedItem is ComboBoxItem item) {
//  LoadMembersForGoal(item.Value);
//  } else {
// MessageBox.Show("Please select a finance goal first!");
//     }
//   };
//      this.Controls.Add(testMembersBtn);
//    }
//      catch
//{
//     // If button creation fails, continue without it
//   }
// }

        private void FinanceTab_Load(object sender, EventArgs e)
        {
  try
       {
  if (isInitialized)
  {
    System.Diagnostics.Debug.WriteLine("FinanceTab_Load: Starting initial data load");
      LoadFinanceGoals();
       ConfigureMembersDataGridView();
     System.Diagnostics.Debug.WriteLine("FinanceTab_Load: Initial data load completed");
    }
          }
  catch (Exception ex)
  {
        System.Diagnostics.Debug.WriteLine($"FinanceTab Load Error: {ex.Message}");
        MessageBox.Show($"Error loading finance data: {ex.Message}", "Load Error", 
      MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
        }

        /// <summary>
        /// Method to test database connectivity and data retrieval - for debugging purposes
        /// </summary>
     public void TestDataRetrieval()
   {
            try
            {
                System.Diagnostics.Debug.WriteLine("TestDataRetrieval: Testing finance data retrieval");

                // DIRECT SQL TEST - bypass all class methods
                string connectionString = "server=localhost;port=3306;database=orgdb;uid=root;pwd=legorocket3368.;";
                using (var connection = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Test 1: Basic count
                    int count = 0;
                    string countSql = "SELECT COUNT(*) FROM event_goals";
                    using (var countCmd = new MySql.Data.MySqlClient.MySqlCommand(countSql, connection))
                    {
                        count = Convert.ToInt32(countCmd.ExecuteScalar());
                        System.Diagnostics.Debug.WriteLine($"TestDataRetrieval: Direct count: {count}");
                    }

                    // Test 2: Get actual data with minimal query
                    string dataSql = "SELECT goal_id, goal_name FROM event_goals ORDER BY goal_id DESC LIMIT 5";
                    using (var dataCmd = new MySql.Data.MySqlClient.MySqlCommand(dataSql, connection))
                    {
                        using (var reader = dataCmd.ExecuteReader())
                        {
                            List<string> directResults = new List<string>();
                            while (reader.Read())
                            {
                                string result = $"ID: {reader["goal_id"]}, Name: {reader["goal_name"]}";
                                directResults.Add(result);
                                System.Diagnostics.Debug.WriteLine($"TestDataRetrieval: Direct result: {result}");
                            }

                            string directMessage = $"Direct SQL Results:\n\nTotal Records: {count}\n\nSample Records:\n";
                            directMessage += string.Join("\n", directResults);

                            // Now test the class method
                            var goals = FinanceClass.GetAllFinanceFromDatabase();
                            System.Diagnostics.Debug.WriteLine($"TestDataRetrieval: Class method returned: {goals.Count} goals");

                            string debugMessage = directMessage + $"\n\nClass Method Results: {goals.Count}\n\n";

                            if (goals.Count > 0)
                            {
                                debugMessage += "Goals from class method:\n";
                                foreach (var goal in goals)
                                {
                                    debugMessage += $"- ID: {goal.GoalId}, Name: {goal.GoalName}\n";
                                }
                            }
                            else
                            {
                                debugMessage += "Class method returned no goals.\n\n";
                                debugMessage += "This means there's a problem with:\n";
                                debugMessage += "1. The SQL query filtering\n";
                                debugMessage += "2. The data conversion process\n";
                                debugMessage += "3. The is_archived column logic";
                            }

                            // Test ComboBox state
                            if (cbFinancial != null)
                            {
                                debugMessage += $"\n\nComboBox State:\n";
                                debugMessage += $"- Items count: {cbFinancial.Items.Count}\n";
                                debugMessage += $"- Selected index: {cbFinancial.SelectedIndex}\n";
                            }

                            MessageBox.Show(debugMessage, "Debug Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"TestDataRetrieval Error: {ex.Message}", "Debug Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
 }
        private void InitializeEventHandlers()
        {
  try
      {
        if (btnAddFinance != null)
    {
    btnAddFinance.Click += btnAddFinance_Click;
       System.Diagnostics.Debug.WriteLine("btnAddFinance event handler added");
             }

      if (btnArchive != null)
                {
     btnArchive.Click += btnArchive_Click;
      System.Diagnostics.Debug.WriteLine("btnArchive event handler added");
   }

              if (cbFinancial != null)
    {
         cbFinancial.SelectedIndexChanged += cbFinancial_SelectedIndexChanged;
        System.Diagnostics.Debug.WriteLine("cbFinancial event handler added");
     }

    if (MembersDGV != null)
     {
       MembersDGV.CellValueChanged += MembersDGV_CellValueChanged;
         MembersDGV.CurrentCellDirtyStateChanged += MembersDGV_CurrentCellDirtyStateChanged;
  System.Diagnostics.Debug.WriteLine("MembersDGV event handlers added");
 }
            }
          catch (Exception ex)
          {
   System.Diagnostics.Debug.WriteLine($"Error initializing event handlers: {ex.Message}");
            }
        }

        public void LoadFinanceGoals()
        {
         try
      {
 System.Diagnostics.Debug.WriteLine("LoadFinanceGoals: =====STARTING=====");

      if (cbFinancial == null) 
    {
 System.Diagnostics.Debug.WriteLine("LoadFinanceGoals: ERROR - cbFinancial is null");
     MessageBox.Show("cbFinancial ComboBox is null! Check Designer file.", "Error");
      return;
        }

       System.Diagnostics.Debug.WriteLine($"LoadFinanceGoals: ComboBox found. Current items: {cbFinancial.Items.Count}");

    // Clear existing items
     cbFinancial.Items.Clear();
    cbFinancial.ResetText();
    System.Diagnostics.Debug.WriteLine("LoadFinanceGoals: Cleared existing items");

    // Get all active finance goals
   System.Diagnostics.Debug.WriteLine("LoadFinanceGoals: Calling GetAllFinanceFromDatabase...");
var financeGoals = FinanceClass.GetAllFinanceFromDatabase();
 System.Diagnostics.Debug.WriteLine($"LoadFinanceGoals: Retrieved {financeGoals?.Count ?? 0} finance goals");

    // Debug: Show what we got
   if (financeGoals != null)
    {
        foreach (var goal in financeGoals)
   {
     System.Diagnostics.Debug.WriteLine($"LoadFinanceGoals: Found goal - ID: {goal.GoalId}, Name: '{goal.GoalName}'");
    }
        }

    // Add goals to combobox
   if (financeGoals != null && financeGoals.Count > 0)
    {
       System.Diagnostics.Debug.WriteLine("LoadFinanceGoals: Adding finance goals to ComboBox");
   
      // SIMPLIFIED APPROACH - Just add strings first to test
 foreach (var goal in financeGoals)
    {
     try
   {
      // Add as simple string first
       cbFinancial.Items.Add($"{goal.GoalName} (ID: {goal.GoalId})");
        System.Diagnostics.Debug.WriteLine($"LoadFinanceGoals: Added string item: {goal.GoalName}");
       }
       catch (Exception addEx)
     {
       System.Diagnostics.Debug.WriteLine($"LoadFinanceGoals: Error adding item: {addEx.Message}");
  }
   }

     System.Diagnostics.Debug.WriteLine($"LoadFinanceGoals: ComboBox now has {cbFinancial.Items.Count} items");
     
  // Force refresh of the ComboBox
   cbFinancial.Refresh();
     cbFinancial.Update();
   cbFinancial.Invalidate();
     
     // Test if items are really there
   System.Diagnostics.Debug.WriteLine("LoadFinanceGoals: Verifying items in ComboBox:");
for (int i = 0; i < cbFinancial.Items.Count; i++)
    {
      System.Diagnostics.Debug.WriteLine($"  Item {i}: {cbFinancial.Items[i]}");
   }
 }
  else
          {
  // Add placeholder item
  cbFinancial.Items.Add("No finance goals available");
    System.Diagnostics.Debug.WriteLine("LoadFinanceGoals: No finance goals found, added placeholder");
   }

    UpdateGoalInfo();
    System.Diagnostics.Debug.WriteLine("LoadFinanceGoals: =====COMPLETED=====");
    }
       catch (Exception ex)
 {
     System.Diagnostics.Debug.WriteLine($"LoadFinanceGoals ERROR: {ex.Message}");
     System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
    MessageBox.Show($"Error loading finance goals: {ex.Message}\n\nStack: {ex.StackTrace}", "Error", 
     MessageBoxButtons.OK, MessageBoxIcon.Error);
     }
      }

   private void cbFinancial_SelectedIndexChanged(object sender, EventArgs e)
        {
     try
  {
    System.Diagnostics.Debug.WriteLine("cbFinancial_SelectedIndexChanged: Selection changed");
          System.Diagnostics.Debug.WriteLine($"cbFinancial_SelectedIndexChanged: Selected index: {cbFinancial.SelectedIndex}");
    System.Diagnostics.Debug.WriteLine($"cbFinancial_SelectedIndexChanged: Selected item: {cbFinancial.SelectedItem}");
      
     if (cbFinancial.SelectedIndex >= 0 && cbFinancial.SelectedItem != null)
  {
    string selectedText = cbFinancial.SelectedItem.ToString();
    System.Diagnostics.Debug.WriteLine($"cbFinancial_SelectedIndexChanged: Selected text: '{selectedText}'");
    
   // Check if it's our placeholder
     if (selectedText == "No finance goals available")
     {
  System.Diagnostics.Debug.WriteLine("cbFinancial_SelectedIndexChanged: Placeholder selected, clearing members");
    MembersDGV.DataSource = null;
  UpdateGoalInfo();
   return;
      }
   
    // Extract goal ID from the string format "GoalName (ID: X)"
    if (selectedText.Contains("(ID: ") && selectedText.EndsWith(")"))
    {
 try
 {
     int startPos = selectedText.LastIndexOf("(ID: ") + "(ID: ".Length;
  int endPos = selectedText.LastIndexOf(")");
       string idStr = selectedText.Substring(startPos, endPos - startPos);
      
   if (int.TryParse(idStr, out int goalId))
{
System.Diagnostics.Debug.WriteLine($"cbFinancial_SelectedIndexChanged: Parsed goal ID: {goalId}");
   currentGoalId = goalId;
      LoadMembersForGoal(currentGoalId);
         UpdateGoalInfo();
       }
   else
 {
   System.Diagnostics.Debug.WriteLine($"cbFinancial_SelectedIndexChanged: Failed to parse goal ID from '{idStr}'");
       MessageBox.Show($"Could not parse goal ID from selection: {selectedText}", "Parse Error");
    }
      }
      catch (Exception parseEx)
    {
 System.Diagnostics.Debug.WriteLine($"cbFinancial_SelectedIndexChanged: Parse error: {parseEx.Message}");
 MessageBox.Show($"Error parsing selected goal: {parseEx.Message}", "Parse Error");
  }
   }
  else
    {
System.Diagnostics.Debug.WriteLine($"cbFinancial_SelectedIndexChanged: Unexpected format: '{selectedText}'");
   MessageBox.Show($"Unexpected goal format: {selectedText}", "Format Error");
    }
   }
    else
   {
     System.Diagnostics.Debug.WriteLine("cbFinancial_SelectedIndexChanged: No valid selection, clearing members");
      // Clear members grid
      MembersDGV.DataSource = null;
       UpdateGoalInfo();
        }
   }
    catch (Exception ex)
  {
   System.Diagnostics.Debug.WriteLine($"Error in cbFinancial_SelectedIndexChanged: {ex.Message}");
    MessageBox.Show($"Error selecting finance goal: {ex.Message}", "Error", 
  MessageBoxButtons.OK, MessageBoxIcon.Error);
   }
}

  private void LoadMembersForGoal(int goalId)
        {
      try
   {
           System.Diagnostics.Debug.WriteLine($"LoadMembersForGoal: =====STARTING===== Loading members for goal {goalId}");

 if (MembersDGV == null) 
                {
   System.Diagnostics.Debug.WriteLine("LoadMembersForGoal: ERROR - MembersDGV is null");
    return;
      }

 // Get the finance goal details
    System.Diagnostics.Debug.WriteLine($"LoadMembersForGoal: Getting finance goal by ID {goalId}");
    var financeGoal = FinanceClass.GetFinanceById(goalId.ToString());
          if (financeGoal == null)
    {
      System.Diagnostics.Debug.WriteLine($"LoadMembersForGoal: ERROR - Finance goal with ID {goalId} not found");
         MessageBox.Show($"Finance goal with ID {goalId} not found in database.", "Goal Not Found", 
MessageBoxButtons.OK, MessageBoxIcon.Warning);
    MembersDGV.DataSource = null;
        return;
}

 System.Diagnostics.Debug.WriteLine($"LoadMembersForGoal: Found finance goal - Name: '{financeGoal.GoalName}', Description: '{financeGoal.Description}'");

      // Get department from goal description (or implement a proper department field)
        string department = ExtractDepartmentFromGoal(financeGoal);
System.Diagnostics.Debug.WriteLine($"LoadMembersForGoal: Extracted department: '{department}'");
   
   if (string.IsNullOrEmpty(department))
        {
          System.Diagnostics.Debug.WriteLine("LoadMembersForGoal: ERROR - No department found");
              
          // Try to show a more helpful error with the goal description
    string errorMessage = $"No department found for goal '{financeGoal.GoalName}'.\n\n";
                 errorMessage += $"Goal Description: '{financeGoal.Description}'\n\n";
                  errorMessage += "The goal description should contain 'Department: [DepartmentName]' format.\n\n";
     errorMessage += "Available departments in system:\n";
        
 // Get available departments
      try
             {
        var allMembers = Members.GetAllMembersFromDatabase();
            var departments = allMembers.Select(m => m.Department)
  .Where(d => !string.IsNullOrWhiteSpace(d))
         .Distinct()
            .OrderBy(d => d);
  errorMessage += string.Join(", ", departments);
        }
  catch
         {
       errorMessage += "Could not retrieve available departments.";
     }
         
     MessageBox.Show(errorMessage, "Department Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
      return;
     }

    // Get members from the department
 System.Diagnostics.Debug.WriteLine($"LoadMembersForGoal: Searching for members in department '{department}'");
  var departmentMembers = Members.SearchMembersByDepartment(department, false);
            System.Diagnostics.Debug.WriteLine($"LoadMembersForGoal: Found {departmentMembers?.Count ?? 0} members in department {department}");

      if (departmentMembers == null || departmentMembers.Count == 0)
         {
            System.Diagnostics.Debug.WriteLine($"LoadMembersForGoal: No members found in department '{department}'");
         
     string noMembersMessage = $"No members found in department '{department}' for goal '{financeGoal.GoalName}'.\n\n";
    noMembersMessage += "Possible solutions:\n";
  noMembersMessage += "1. Add members to this department\n";
       noMembersMessage += "2. Check if department name matches exactly\n";
          noMembersMessage += "3. Verify goal description format";
         
        MessageBox.Show(noMembersMessage, "No Members Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
         
  // Show empty grid
MembersDGV.DataSource = null;
  return;
                }

            // Log member details
   foreach (var member in departmentMembers)
  {
 System.Diagnostics.Debug.WriteLine($"LoadMembersForGoal: Member - ID: {member.MemberId}, Name: {member.FirstName} {member.LastName}, Dept: {member.Department}");
    }

   // Create DataTable for members
           System.Diagnostics.Debug.WriteLine("LoadMembersForGoal: Creating members DataTable");
DataTable membersTable = CreateMembersDataTable(departmentMembers, financeGoal);
     System.Diagnostics.Debug.WriteLine($"LoadMembersForGoal: DataTable created with {membersTable.Rows.Count} rows");
 
         // Log DataTable contents
     foreach (DataRow row in membersTable.Rows)
       {
       System.Diagnostics.Debug.WriteLine($"LoadMembersForGoal: DataTable row - ID: {row["member_id"]}, Name: {row["member_name"]}, Amount: {row["individual_amount"]}, Status: {row["payment_status"]}");
              }
    
    // Set the data source
System.Diagnostics.Debug.WriteLine("LoadMembersForGoal: Setting DataGridView DataSource");
 MembersDGV.DataSource = membersTable;
     System.Diagnostics.Debug.WriteLine($"LoadMembersForGoal: DataGridView now has {MembersDGV.Rows.Count} rows");
 
     // Configure the DataGridView
      System.Diagnostics.Debug.WriteLine("LoadMembersForGoal: Configuring DataGridView");
   ConfigureMembersDataGridView();
     System.Diagnostics.Debug.WriteLine("LoadMembersForGoal: =====COMPLETED=====");
    }
       catch (Exception ex)
            {
   System.Diagnostics.Debug.WriteLine($"LoadMembersForGoal ERROR: {ex.Message}");
           System.Diagnostics.Debug.WriteLine($"LoadMembersForGoal STACK: {ex.StackTrace}");
     MessageBox.Show($"Error loading members for goal: {ex.Message}\n\nStack Trace:\n{ex.StackTrace}", "Error", 
     MessageBoxButtons.OK, MessageBoxIcon.Error);
     }
    }

        private string ExtractDepartmentFromGoal(FinanceClass goal)
        {
         try
            {
   System.Diagnostics.Debug.WriteLine($"ExtractDepartmentFromGoal: Starting extraction for goal '{goal.GoalName}'");
       System.Diagnostics.Debug.WriteLine($"ExtractDepartmentFromGoal: Goal description: '{goal.Description}'");
     
       // Try to extract department from description
 if (!string.IsNullOrWhiteSpace(goal.Description) && goal.Description.Contains("Department:"))
    {
 System.Diagnostics.Debug.WriteLine("ExtractDepartmentFromGoal: Found 'Department:' in description");
              string desc = goal.Description;
 int startIndex = desc.IndexOf("Department:") + "Department:".Length;
      int endIndex = desc.IndexOf("|", startIndex);
      if (endIndex == -1) endIndex = desc.Length;
       
     string department = desc.Substring(startIndex, endIndex - startIndex).Trim();
   System.Diagnostics.Debug.WriteLine($"ExtractDepartmentFromGoal: Extracted department: '{department}'");
return department;
 }
else
       {
      System.Diagnostics.Debug.WriteLine("ExtractDepartmentFromGoal: No 'Department:' found in description");
}
     
     // If no department in description, return empty (will need user to specify)
     System.Diagnostics.Debug.WriteLine("ExtractDepartmentFromGoal: No department found, returning empty string");
  return string.Empty;
      }
      catch (Exception ex)
  {
    System.Diagnostics.Debug.WriteLine($"ExtractDepartmentFromGoal ERROR: {ex.Message}");
     return string.Empty;
 }
     }

private DataTable CreateMembersDataTable(List<Members> members, FinanceClass goal)
        {
     DataTable dt = new DataTable();
    
        // Add columns
    dt.Columns.Add("member_id", typeof(int));
  dt.Columns.Add("member_name", typeof(string));
     dt.Columns.Add("individual_amount", typeof(decimal));
dt.Columns.Add("payment_status", typeof(string));
            dt.Columns.Add("paid_amount", typeof(decimal));

    if (members != null && members.Count > 0)
   {
     // Calculate individual amount (target amount divided by number of members)
        decimal individualAmount = members.Count > 0 ? goal.TargetAmount / members.Count : 0;

            foreach (var member in members)
     {
       DataRow row = dt.NewRow();
     row["member_id"] = member.MemberId;
   row["member_name"] = $"{member.FirstName} {member.LastName}";
        row["individual_amount"] = Math.Round(individualAmount, 2);
           
       // Get current payment status from database
           string paymentStatus = GetMemberPaymentStatus(goal.GoalId, member.MemberId);
     row["payment_status"] = paymentStatus;
           row["paid_amount"] = paymentStatus == "Paid" ? individualAmount : 0;
   
           dt.Rows.Add(row);
    }
  }

            return dt;
  }

        private string GetMemberPaymentStatus(int goalId, int memberId)
        {
       try
      {
                // Check member_payments table for this specific goal and member
         string connectionString = "server=localhost;port=3306;database=orgdb;uid=root;pwd=legorocket3368.;";
      
       using (var connection = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
     {
  connection.Open();
          
    string sql = "SELECT payment_status FROM member_payments WHERE goal_id = @goalId AND member_id = @memberId";
    using (var cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, connection))
           {
  cmd.Parameters.AddWithValue("@goalId", goalId);
      cmd.Parameters.AddWithValue("@memberId", memberId);
               
         object result = cmd.ExecuteScalar();
   return result?.ToString() ?? "Not Paid";
        }
  }
            }
 catch
            {
                return "Not Paid"; // Default status
            }
        }

        private void ConfigureMembersDataGridView()
 {
      try
         {
    if (MembersDGV == null || MembersDGV.DataSource == null) return;

     // Configure basic properties
         MembersDGV.AllowUserToAddRows = false;
        MembersDGV.AllowUserToDeleteRows = false;
       MembersDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

        // Hide member_id column
       if (MembersDGV.Columns.Contains("member_id"))
      {
    MembersDGV.Columns["member_id"].Visible = false;
   }

    // Configure member_name column
              if (MembersDGV.Columns.Contains("member_name"))
    {
            MembersDGV.Columns["member_name"].HeaderText = "Member Name";
          MembersDGV.Columns["member_name"].ReadOnly = true;
          MembersDGV.Columns["member_name"].Width = 200;
          }

       // Configure individual_amount column
                if (MembersDGV.Columns.Contains("individual_amount"))
  {
            MembersDGV.Columns["individual_amount"].HeaderText = "Individual Amount";
             MembersDGV.Columns["individual_amount"].ReadOnly = true;
                MembersDGV.Columns["individual_amount"].DefaultCellStyle.Format = "C2";
       MembersDGV.Columns["individual_amount"].Width = 120;
                }

 // Configure payment_status as ComboBox column
                if (MembersDGV.Columns.Contains("payment_status"))
       {
           // Remove existing column and replace with ComboBox column
       int statusColumnIndex = MembersDGV.Columns["payment_status"].Index;
MembersDGV.Columns.RemoveAt(statusColumnIndex);

  DataGridViewComboBoxColumn statusComboColumn = new DataGridViewComboBoxColumn();
   statusComboColumn.Name = "payment_status";
 statusComboColumn.HeaderText = "Payment Status";
 statusComboColumn.Items.AddRange(new string[] { "Not Paid", "Paid" });
 statusComboColumn.Width = 120;
          statusComboColumn.FlatStyle = FlatStyle.Flat;

          MembersDGV.Columns.Insert(statusColumnIndex, statusComboColumn);
         }

    // Configure paid_amount column
       if (MembersDGV.Columns.Contains("paid_amount"))
     {
    MembersDGV.Columns["paid_amount"].HeaderText = "Paid Amount";
          MembersDGV.Columns["paid_amount"].ReadOnly = true;
      MembersDGV.Columns["paid_amount"].DefaultCellStyle.Format = "C2";
           MembersDGV.Columns["paid_amount"].Width = 120;
           }

 System.Diagnostics.Debug.WriteLine("ConfigureMembersDataGridView: Configuration completed");
        }
       catch (Exception ex)
  {
           System.Diagnostics.Debug.WriteLine($"Error configuring members DataGridView: {ex.Message}");
     }
      }

    private void MembersDGV_CurrentCellDirtyStateChanged(object sender, EventArgs e)
    {
            // Commit changes immediately when ComboBox value changes
   if (MembersDGV.IsCurrentCellDirty)
            {
     MembersDGV.CommitEdit(DataGridViewDataErrorContexts.Commit);
}
        }

        private void MembersDGV_CellValueChanged(object sender, DataGridViewCellEventArgs e)
      {
            try
   {
          if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

     // Check if payment_status column was changed
         if (MembersDGV.Columns[e.ColumnIndex].Name == "payment_status")
     {
      DataGridViewRow row = MembersDGV.Rows[e.RowIndex];
            int memberId = Convert.ToInt32(row.Cells["member_id"].Value);
      string newStatus = row.Cells["payment_status"].Value?.ToString() ?? "Not Paid";
          decimal individualAmount = Convert.ToDecimal(row.Cells["individual_amount"].Value);

            // Update paid_amount based on payment status
          decimal paidAmount = newStatus == "Paid" ? individualAmount : 0;
        row.Cells["paid_amount"].Value = paidAmount;

   // Update database
     UpdateMemberPaymentStatus(currentGoalId, memberId, newStatus, paidAmount);

        // Update goal's collected amount
           UpdateGoalCollectedAmount();
                  
       // Refresh goal info display
     UpdateGoalInfo();
 }
            }
    catch (Exception ex)
    {
       System.Diagnostics.Debug.WriteLine($"Error in MembersDGV_CellValueChanged: {ex.Message}");
           MessageBox.Show($"Error updating payment status: {ex.Message}", "Error", 
           MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateMemberPaymentStatus(int goalId, int memberId, string paymentStatus, decimal paidAmount)
        {
       try
          {
     string connectionString = "server=localhost;port=3306;database=orgdb;uid=root;pwd=legorocket3368.;";
  
            using (var connection = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
    {
     connection.Open();
     
        // Create member_payments table if it doesn't exist
     string createTableSql = @"CREATE TABLE IF NOT EXISTS member_payments (
           id INT AUTO_INCREMENT PRIMARY KEY,
               goal_id INT NOT NULL,
              member_id INT NOT NULL,
      payment_status VARCHAR(20) DEFAULT 'Not Paid',
paid_amount DECIMAL(10,2) DEFAULT 0.00,
              updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
      UNIQUE KEY unique_goal_member (goal_id, member_id)
    )";
        
         using (var createCmd = new MySql.Data.MySqlClient.MySqlCommand(createTableSql, connection))
 {
             createCmd.ExecuteNonQuery();
                    }
           
           // Insert or update member payment status
               string upsertSql = @"INSERT INTO member_payments (goal_id, member_id, payment_status, paid_amount) 
          VALUES (@goalId, @memberId, @paymentStatus, @paidAmount)
       ON DUPLICATE KEY UPDATE 
           payment_status = @paymentStatus, 
        paid_amount = @paidAmount";
            
        using (var cmd = new MySql.Data.MySqlClient.MySqlCommand(upsertSql, connection))
          {
          cmd.Parameters.AddWithValue("@goalId", goalId);
 cmd.Parameters.AddWithValue("@memberId", memberId);
  cmd.Parameters.AddWithValue("@paymentStatus", paymentStatus);
          cmd.Parameters.AddWithValue("@paidAmount", paidAmount);

     cmd.ExecuteNonQuery();
          }
          }
  
        System.Diagnostics.Debug.WriteLine($"Updated payment status for member {memberId} in goal {goalId}: {paymentStatus}");
       }
         catch (Exception ex)
        {
   System.Diagnostics.Debug.WriteLine($"Error updating member payment status: {ex.Message}");
  throw;
}
        }

        private void UpdateGoalCollectedAmount()
        {
 try
 {
    if (currentGoalId == 0) return;

                // Calculate total collected amount from all paid members
      decimal totalCollected = 0;
    
     if (MembersDGV.DataSource is DataTable dt)
          {
    foreach (DataRow row in dt.Rows)
{
  decimal paidAmount = Convert.ToDecimal(row["paid_amount"]);
       totalCollected += paidAmount;
    }
    }

            // Update the goal's collected amount in database
      FinanceClass.UpdateFinanceById(currentGoalId.ToString(), collectedAmount: totalCollected);
        
         System.Diagnostics.Debug.WriteLine($"Updated goal {currentGoalId} collected amount to {totalCollected}");
            }
            catch (Exception ex)
            {
  System.Diagnostics.Debug.WriteLine($"Error updating goal collected amount: {ex.Message}");
 }
        }

     private void UpdateGoalInfo()
     {
        try
          {
        if (lblGoalInfo == null) return;

         if (currentGoalId == 0 || cbFinancial.SelectedItem == null)
     {
          lblGoalInfo.Text = "Select a finance goal to view details and manage member payments.";
         return;
       }

           var financeGoal = FinanceClass.GetFinanceById(currentGoalId.ToString());
    if (financeGoal != null)
        {
     string department = ExtractDepartmentFromGoal(financeGoal);
 string info = $"Goal: {financeGoal.GoalName}\n" +
              $"Department: {department}\n" +
    $"Target: {financeGoal.TargetAmount:C}\n" +
     $"Collected: {financeGoal.CollectedAmount:C}\n" +
          $"Status: {financeGoal.PaymentStatus}";
    
       lblGoalInfo.Text = info;
             }
            }
        catch (Exception ex)
            {
         System.Diagnostics.Debug.WriteLine($"Error updating goal info: {ex.Message}");
         }
     }

   private void btnAddFinance_Click(object sender, EventArgs e)
        {
      try
       {
      System.Diagnostics.Debug.WriteLine("btnAddFinance_Click: Opening CreateFinance form");
       CreateFinance createFinanceForm = new CreateFinance(RefreshFinanceData);
           DialogResult result = createFinanceForm.ShowDialog(this);
     
      System.Diagnostics.Debug.WriteLine($"btnAddFinance_Click: CreateFinance dialog closed with result: {result}");
          
        // Force refresh regardless of dialog result to ensure data is current
  RefreshFinanceData();
         }
       catch (Exception ex)
    {
    System.Diagnostics.Debug.WriteLine($"Error in btnAddFinance_Click: {ex.Message}");
       MessageBox.Show($"Error opening Create Finance form: {ex.Message}", "Error", 
   MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      }

        private void btnArchive_Click(object sender, EventArgs e)
   {
   try
  {
                ArchivePage archiveForm = new ArchivePage();
      archiveForm.ShowDialog(this);
            }
 catch (Exception ex)
    {
  MessageBox.Show($"Error opening Archive form: {ex.Message}", "Error", 
              MessageBoxButtons.OK, MessageBoxIcon.Error);
   }
        }

        public void RefreshData()
   {
            try
    {
       System.Diagnostics.Debug.WriteLine("RefreshData: Starting refresh");
        LoadFinanceGoals();
 if (currentGoalId > 0)
     {
         LoadMembersForGoal(currentGoalId);
            UpdateGoalInfo();
  }
           System.Diagnostics.Debug.WriteLine("RefreshData: Refresh completed");
        }
      catch (Exception ex)
            {
    System.Diagnostics.Debug.WriteLine($"RefreshData error: {ex.Message}");
 }
      }

     /// <summary>
      /// Public method to refresh the FinanceTab from external forms like CreateFinance
/// </summary>
    public void RefreshFinanceData()
   {
      System.Diagnostics.Debug.WriteLine("RefreshFinanceData: Called from external form");
 RefreshData();
 
    // Force the control to repaint
      this.Invalidate();
      this.Update();
       
   // If no goal is selected, but goals are available, show instruction
        if (cbFinancial != null && cbFinancial.Items.Count > 1 && cbFinancial.SelectedIndex < 0)
     {
          // Select the first actual goal (skip placeholder items)
     for (int i = 0; i < cbFinancial.Items.Count; i++)
       {
       if (cbFinancial.Items[i] is ComboBoxItem)
    {
           cbFinancial.SelectedIndex = i;
  break;
      }
 }
      }
        }

   // Helper class for ComboBox items
        public class ComboBoxItem
        {
     public string Text { get; set; }
public int Value { get; set; }
     
   public override string ToString()
  {
   return Text;
          }
        }
    }
}