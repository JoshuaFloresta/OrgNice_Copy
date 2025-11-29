using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;

namespace ORGnice
{
    public class FinanceClass
    {
        public int GoalId { get; set; } // maps to goal_id (AUTO_INCREMENT PRIMARY KEY)
        public int EventId { get; set; }// NOT NULL, Foreign Key
   public string GoalName { get; set; }   // NOT NULL
        public string Description { get; set; }
        public decimal TargetAmount { get; set; }  // NOT NULL DEFAULT 0.00
        public decimal CollectedAmount { get; set; } // NOT NULL DEFAULT 0.00
 public decimal ExpensesAmount { get; set; } // NOT NULL DEFAULT 0.00
    public string Currency { get; set; }      // default "PHP"
    public DateTime? DueDate { get; set; }      // nullable
        public string PaymentStatus { get; set; }  // 'InProgress','Goal Reached' DEFAULT 'Planned'
        public string MemberStatus { get; set; }   // 'Paid','Not Paid','Incomplete'
  public bool IsArchived { get; set; }      // TINYINT(1) DEFAULT 0
        public DateTime? ArchivedAt { get; set; } // nullable

        private static Crud financeCrud = new Crud("event_goals");

   // Constructor to initialize finance data
        public FinanceClass(
            int eventId,
            string goalName,
    string description,
    decimal targetAmount,
            decimal collectedAmount,
          decimal expensesAmount,
         string currency,
          DateTime? dueDate,
            string paymentStatus,
       string memberStatus)
      {
            EventId = eventId;
            GoalName = goalName;
      Description = description;
            TargetAmount = targetAmount;
   CollectedAmount = collectedAmount;
      ExpensesAmount = expensesAmount;
          Currency = currency ?? "PHP";
            DueDate = dueDate;
      PaymentStatus = paymentStatus ?? "InProgress";
     MemberStatus = memberStatus ?? "Not Paid";
  IsArchived = false;
}

        public FinanceClass()
        {
        }

      // Method to add a finance record and automatically populate the database
        public static bool AddFinanceToDatabase(FinanceClass finance)
      {
      if (!IsValidFinance(finance))
         return false;

         string connectionString =
    "server=localhost;port=3306;database=orgdb;uid=root;pwd=legorocket3368.;";

        using (var connection = new MySqlConnection(connectionString))
            {
  try
{
               connection.Open();

        string sql = @"INSERT INTO event_goals
       (event_id, goal_name, description, target_amount,
           collected_amount, expenses_amount, currency, due_date, 
        paymentStatus, memberStatus)
         VALUES
   (@eventId, @goalName, @description, @targetAmount,
          @collectedAmount, @expensesAmount, @currency, @dueDate, 
        @paymentStatus, @memberStatus);";

 using (var cmd = new MySqlCommand(sql, connection))
         {
           cmd.Parameters.AddWithValue("@eventId", finance.EventId);
  cmd.Parameters.AddWithValue("@goalName", finance.GoalName);
          cmd.Parameters.AddWithValue("@description", finance.Description ?? (object)DBNull.Value);
             cmd.Parameters.AddWithValue("@targetAmount", finance.TargetAmount);
      cmd.Parameters.AddWithValue("@collectedAmount", finance.CollectedAmount);
 cmd.Parameters.AddWithValue("@expensesAmount", finance.ExpensesAmount);
   cmd.Parameters.AddWithValue("@currency", finance.Currency ?? "PHP");
     cmd.Parameters.AddWithValue("@dueDate",
       finance.DueDate.HasValue ? (object)finance.DueDate.Value.Date : DBNull.Value);
              cmd.Parameters.AddWithValue("@paymentStatus", finance.PaymentStatus ?? "InProgress");
      cmd.Parameters.AddWithValue("@memberStatus", finance.MemberStatus ?? "Not Paid");

      int rowsAffected = cmd.ExecuteNonQuery();
        return rowsAffected > 0;
   }
    }
 catch (Exception ex)
                {
            System.Diagnostics.Debug.WriteLine(
   "Error adding finance to database: " + ex.Message);
         return false;
        }
            }
        }

      // USER-FRIENDLY SEARCH METHODS

    /// <summary>
    /// Search finance records by goal name
        /// </summary>
        /// <param name="goalName">Goal name to search for</param>
  /// <param name="includeArchived">Include archived records in search</param>
        /// <returns>List of matching finance records</returns>
        public static List<FinanceClass> SearchFinanceByGoalName(string goalName, bool includeArchived = false)
        {
            List<FinanceClass> financeList = new List<FinanceClass>();

            try
     {
    DataTable dataTable = financeCrud.SearchByName(goalName, includeArchived);
        financeList = ConvertDataTableToFinanceList(dataTable);
     }
   catch (Exception ex)
            {
    System.Diagnostics.Debug.WriteLine("Error searching finance by goal name: " + ex.Message);
            }

         return financeList;
    }

        /// <summary>
        /// Search finance records by ID
     /// </summary>
        /// <param name="id">Goal ID to search for</param>
      /// <param name="includeArchived">Include archived records in search</param>
        /// <returns>List of matching finance records (usually one or empty)</returns>
    public static List<FinanceClass> SearchFinanceById(string id, bool includeArchived = false)
 {
            List<FinanceClass> financeList = new List<FinanceClass>();

  try
   {
          DataTable dataTable = financeCrud.SearchById(id, "goal_id", includeArchived);
          financeList = ConvertDataTableToFinanceList(dataTable);
       }
   catch (Exception ex)
      {
   System.Diagnostics.Debug.WriteLine("Error searching finance by ID: " + ex.Message);
      }

         return financeList;
    }

        /// <summary>
        /// Search finance records by event ID
        /// </summary>
        /// <param name="eventId">Event ID to search for</param>
      /// <param name="includeArchived">Include archived records in search</param>
        /// <returns>List of matching finance records</returns>
        public static List<FinanceClass> SearchFinanceByEventId(int eventId, bool includeArchived = false)
 {
       List<FinanceClass> financeList = new List<FinanceClass>();

            try
         {
        var searchCriteria = new Dictionary<string, string>
    {
              { "event_id", eventId.ToString() }
    };

      DataTable dataTable = financeCrud.Search(searchCriteria, includeArchived, true); // exact match
   financeList = ConvertDataTableToFinanceList(dataTable);
            }
            catch (Exception ex)
 {
                System.Diagnostics.Debug.WriteLine("Error searching finance by event ID: " + ex.Message);
  }

     return financeList;
        }

        /// <summary>
      /// Search finance records by status
        /// </summary>
        /// <param name="status">Status to search for</param>
        /// <param name="includeArchived">Include archived records in search</param>
        /// <returns>List of matching finance records</returns>
        public static List<FinanceClass> SearchFinanceByStatus(string status, bool includeArchived = false)
      {
            List<FinanceClass> financeList = new List<FinanceClass>();

    try
       {
     var searchCriteria = new Dictionary<string, string>
     {
          { "status", status }
        };

        DataTable dataTable = financeCrud.Search(searchCriteria, includeArchived, true); // exact match
       financeList = ConvertDataTableToFinanceList(dataTable);
   }
         catch (Exception ex)
       {
        System.Diagnostics.Debug.WriteLine("Error searching finance by status: " + ex.Message);
            }

return financeList;
   }

        /// <summary>
 /// Advanced search with multiple criteria
        /// </summary>
        /// <param name="goalName">Goal name (partial match)</param>
        /// <param name="goalId">Goal ID (exact match)</param>
    /// <param name="eventSet">Event (partial match)</param>
        /// <param name="status">Status (exact match)</param>
  /// <param name="currency">Currency (exact match)</param>
        /// <param name="includeArchived">Include deleted records in search</param>
        /// <returns>List of matching finance records</returns>
        public static List<FinanceClass> AdvancedSearchFinance(string goalName = null, string goalId = null, 
    string eventSet = null, string status = null, string currency = null, bool includeArchived = false)
        {
            List<FinanceClass> financeList = new List<FinanceClass>();

      try
            {
      DataTable dataTable = financeCrud.AdvancedSearch(goalName, goalId, null, eventSet, status, currency, includeArchived);
           financeList = ConvertDataTableToFinanceList(dataTable);
     }
       catch (Exception ex)
   {
                System.Diagnostics.Debug.WriteLine("Error in advanced finance search: " + ex.Message);
            }

            return financeList;
        }

        // USER-FRIENDLY UPDATE METHODS

        /// <summary>
        /// Update finance record by ID
      /// </summary>
        /// <param name="goalId">ID of the finance record to update</param>
        /// <param name="goalName">New goal name (optional)</param>
        /// <param name="description">New description (optional)</param>
        /// <param name="targetAmount">New target amount (optional)</param>
        /// <param name="collectedAmount">New collected amount (optional)</param>
        /// <param name="expensesAmount">New expenses amount (optional)</param>
        /// <param name="memberStatus">New member status (optional)</param>
  /// <param name="dueDate">New due date (optional)</param>
      /// <returns>True if update was successful</returns>
        public static bool UpdateFinanceById(string goalId, string goalName = null, string description = null,
     decimal? targetAmount = null, decimal? collectedAmount = null, decimal? expensesAmount = null, 
    string memberStatus = null, DateTime? dueDate = null)
        {
try
          {
       // Build update data dictionary with only non-null values
   var updateData = new Dictionary<string, object>();

        if (!string.IsNullOrWhiteSpace(goalName))
            updateData.Add("goal_name", goalName);

          if (!string.IsNullOrWhiteSpace(description))
         updateData.Add("description", description);

 if (targetAmount.HasValue)
    updateData.Add("target_amount", targetAmount.Value);

           if (collectedAmount.HasValue)
             {
              updateData.Add("collected_amount", collectedAmount.Value);
            
        // Auto-update payment status based on target vs collected
  if (targetAmount.HasValue)
      {
            string paymentStatus = collectedAmount.Value >= targetAmount.Value ? "Goal Reached" : "InProgress";
        updateData.Add("paymentStatus", paymentStatus);
  }
    }

        if (expensesAmount.HasValue)
      updateData.Add("expenses_amount", expensesAmount.Value);

         if (!string.IsNullOrWhiteSpace(memberStatus))
           updateData.Add("memberStatus", memberStatus);

       if (dueDate.HasValue)
    updateData.Add("due_date", dueDate.Value.Date);

         if (updateData.Count == 0)
              {
                    System.Diagnostics.Debug.WriteLine("No data provided for finance update");
           return false;
          }

  int rowsAffected = financeCrud.UpdateById(updateData, "goal_id", goalId);
      return rowsAffected > 0;
  }
  catch (Exception ex)
            {
        System.Diagnostics.Debug.WriteLine("Error updating finance record: " + ex.Message);
         return false;
  }
        }

    /// <summary>
        /// Archive (soft delete) a finance record by ID
        /// </summary>
   /// <param name="goalId">ID of the finance record to archive</param>
        /// <returns>True if finance record was archived successfully</returns>
        public static bool ArchiveFinanceById(string goalId)
        {
            try
   {
         var updateData = new Dictionary<string, object>
    {
        { "is_archived", 1 },
       { "archived_at", DateTime.Now }
    };

        int rowsAffected = financeCrud.UpdateById(updateData, "goal_id", goalId);
      return rowsAffected > 0;
   }
         catch (Exception ex)
            {
    System.Diagnostics.Debug.WriteLine("Error archiving finance record: " + ex.Message);
                return false;
            }
        }

        // EXISTING METHODS (Updated to use new search functionality)

        public static List<FinanceClass> GetAllFinanceFromDatabase()
        {
  List<FinanceClass> financeList = new List<FinanceClass>();

       try
    {
      System.Diagnostics.Debug.WriteLine("GetAllFinanceFromDatabase: Starting");

    string connectionString =
     "server=localhost;port=3306;database=orgdb;uid=root;pwd=legorocket3368.;";

     using (var connection = new MySqlConnection(connectionString))
    {
           System.Diagnostics.Debug.WriteLine("GetAllFinanceFromDatabase: Connecting to database");
    connection.Open();
     System.Diagnostics.Debug.WriteLine("GetAllFinanceFromDatabase: Database connection opened");

         // STEP 1: Check if table exists and what columns it has
      string checkTableSql = "DESCRIBE event_goals";
       try
         {
         using (var checkCmd = new MySqlCommand(checkTableSql, connection))
{
       using (var reader = checkCmd.ExecuteReader())
          {
    System.Diagnostics.Debug.WriteLine("GetAllFinanceFromDatabase: Table structure:");
       while (reader.Read())
      {
       System.Diagnostics.Debug.WriteLine($"  Column: {reader["Field"]}, Type: {reader["Type"]}");
    }
       }
      }
         }
         catch (Exception tableEx)
     {
     System.Diagnostics.Debug.WriteLine($"GetAllFinanceFromDatabase: Error checking table structure: {tableEx.Message}");
  }

         // STEP 2: Try a simple count first
 string countSql = "SELECT COUNT(*) FROM event_goals";
      using (var countCmd = new MySqlCommand(countSql, connection))
         {
      int totalRecords = Convert.ToInt32(countCmd.ExecuteScalar());
         System.Diagnostics.Debug.WriteLine($"GetAllFinanceFromDatabase: Total records in table: {totalRecords}");
         }

         // STEP 3: Check archived records specifically
         string archivedCountSql = "SELECT COUNT(*) FROM event_goals WHERE is_archived = 1";
         try
         {
      using (var archCmd = new MySqlCommand(archivedCountSql, connection))
         {
           int archivedRecords = Convert.ToInt32(archCmd.ExecuteScalar());
       System.Diagnostics.Debug.WriteLine($"GetAllFinanceFromDatabase: Archived records: {archivedRecords}");
             }
         }
catch (Exception archEx)
         {
             System.Diagnostics.Debug.WriteLine($"GetAllFinanceFromDatabase: No is_archived column or error: {archEx.Message}");
    }

     // STEP 4: Try simplified query without complex joins first
    string simpleSql = "SELECT * FROM event_goals ORDER BY goal_id DESC LIMIT 10";
      System.Diagnostics.Debug.WriteLine($"GetAllFinanceFromDatabase: Trying simple query: {simpleSql}");

         using (var simpleCmd = new MySqlCommand(simpleSql, connection))
         {
        using (var adapter = new MySqlDataAdapter(simpleCmd))
             {
       DataTable simpleTable = new DataTable();
        adapter.Fill(simpleTable);
   System.Diagnostics.Debug.WriteLine($"GetAllFinanceFromDatabase: Simple query returned {simpleTable.Rows.Count} rows");

       // Log each row
          foreach (DataRow row in simpleTable.Rows)
      {
      string goalId = row["goal_id"]?.ToString() ?? "NULL";
    string goalName = row["goal_name"]?.ToString() ?? "NULL";
            string isArchived = row.Table.Columns.Contains("is_archived") ? row["is_archived"]?.ToString() ?? "NULL" : "NO_COLUMN";
          System.Diagnostics.Debug.WriteLine($"GetAllFinanceFromDatabase: Row - ID: {goalId}, Name: {goalName}, Archived: {isArchived}");
    }

// If we got results from simple query, convert them
       if (simpleTable.Rows.Count > 0)
         {
 // Filter out archived records manually if needed
           DataTable filteredTable = simpleTable.Clone();
          foreach (DataRow row in simpleTable.Rows)
               {
              bool isArchived = false;
          if (simpleTable.Columns.Contains("is_archived") && row["is_archived"] != DBNull.Value)
   {
         isArchived = Convert.ToBoolean(row["is_archived"]);
                  }

         if (!isArchived)
           {
  filteredTable.ImportRow(row);
    }
          }

    System.Diagnostics.Debug.WriteLine($"GetAllFinanceFromDatabase: After filtering archived: {filteredTable.Rows.Count} rows");
   financeList = ConvertDataTableToFinanceList(filteredTable);
           System.Diagnostics.Debug.WriteLine($"GetAllFinanceFromDatabase: Converted to {financeList.Count} finance objects");
                 }
             }
     }

         // If simple query worked, we're done. If not, try the original complex query
         if (financeList.Count == 0)
   {
         System.Diagnostics.Debug.WriteLine("GetAllFinanceFromDatabase: Simple query returned no results, trying original complex query");

   string complexSql = @"SELECT eg.*, e.event_name, 
  CONCAT(m.first_name, ' ', m.last_name) as member_name
         FROM event_goals eg
           LEFT JOIN events e ON eg.event_id = e.event_id
       LEFT JOIN members m ON e.created_by = m.member_id
         WHERE (eg.is_archived = 0 OR eg.is_archived IS NULL)
     ORDER BY eg.goal_id DESC";

         System.Diagnostics.Debug.WriteLine($"GetAllFinanceFromDatabase: Executing complex SQL: {complexSql}");

          using (var cmd = new MySqlCommand(complexSql, connection))
     {
 using (var adapter = new MySqlDataAdapter(cmd))
   {
     DataTable dataTable = new DataTable();
     adapter.Fill(dataTable);
        System.Diagnostics.Debug.WriteLine($"GetAllFinanceFromDatabase: Complex query retrieved {dataTable.Rows.Count} rows from database");

               financeList = ConvertDataTableToFinanceList(dataTable);
     }
    }
    }

         // Log the final converted objects
         foreach (var finance in financeList)
   {
  System.Diagnostics.Debug.WriteLine($"GetAllFinanceFromDatabase: Finance object - ID: {finance.GoalId}, Name: {finance.GoalName}");
         }
}
         }
     catch (Exception ex)
    {
       System.Diagnostics.Debug.WriteLine($"Error reading finance from database: {ex.Message}");
      System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
     
        // Return an empty list instead of throwing, so the application doesn't crash
}

System.Diagnostics.Debug.WriteLine($"GetAllFinanceFromDatabase: Returning {financeList.Count} finance records");
   return financeList;
}

        /// <summary>
        /// Test database connectivity
 /// </summary>
        /// <returns>True if database is accessible</returns>
      public static bool TestDatabaseConnection()
        {
            try
            {
      string connectionString =
    "server=localhost;port=3306;database=orgdb;uid=root;pwd=legorocket3368.;";

          using (var connection = new MySqlConnection(connectionString))
       {
            connection.Open();
  
     // Simple test query
    string sql = "SELECT COUNT(*) FROM event_goals";
           using (var cmd = new MySqlCommand(sql, connection))
         {
            object result = cmd.ExecuteScalar();
    System.Diagnostics.Debug.WriteLine($"Database test successful. Event goals count: {result}");
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
        // HELPER METHODS

        /// <summary>
        /// Convert DataTable to List of FinanceClass
        /// </summary>
   /// <param name="dataTable">DataTable to convert</param>
      /// <returns>List of FinanceClass objects</returns>
        private static List<FinanceClass> ConvertDataTableToFinanceList(DataTable dataTable)
        {
            List<FinanceClass> financeList = new List<FinanceClass>();

      if (dataTable == null)
  return financeList;

      foreach (DataRow row in dataTable.Rows)
     {
                FinanceClass finance = new FinanceClass();

 // goal_id -> GoalId
           if (row.Table.Columns.Contains("goal_id") && row["goal_id"] != DBNull.Value)
         {
  int parsedId;
 try
                {
    parsedId = Convert.ToInt32(row["goal_id"]);
     }
  catch
            {
            parsedId = 0;
  }
           finance.GoalId = parsedId;
        }

         if (row.Table.Columns.Contains("event_id") && row["event_id"] != DBNull.Value)
     finance.EventId = Convert.ToInt32(row["event_id"]);

             if (row.Table.Columns.Contains("goal_name") && row["goal_name"] != DBNull.Value)
                    finance.GoalName = row["goal_name"].ToString();

     if (row.Table.Columns.Contains("description") && row["description"] != DBNull.Value)
        finance.Description = row["description"].ToString();

          if (row.Table.Columns.Contains("target_amount") && row["target_amount"] != DBNull.Value)
              {
           decimal targetAmount;
             if (decimal.TryParse(row["target_amount"].ToString(), out targetAmount))
      finance.TargetAmount = targetAmount;
                }

                if (row.Table.Columns.Contains("collected_amount") && row["collected_amount"] != DBNull.Value)
       {
          decimal collectedAmount;
       if (decimal.TryParse(row["collected_amount"].ToString(), out collectedAmount))
             finance.CollectedAmount = collectedAmount;
    }

                if (row.Table.Columns.Contains("expenses_amount") && row["expenses_amount"] != DBNull.Value)
     {
           decimal expensesAmount;
 if (decimal.TryParse(row["expenses_amount"].ToString(), out expensesAmount))
             finance.ExpensesAmount = expensesAmount;
         }

    if (row.Table.Columns.Contains("currency") && row["currency"] != DBNull.Value)
   finance.Currency = row["currency"].ToString();

     if (row.Table.Columns.Contains("due_date") && row["due_date"] != DBNull.Value)
     {
    DateTime dueDate;
            if (DateTime.TryParse(row["due_date"].ToString(), out dueDate))
       finance.DueDate = dueDate;
    }

      if (row.Table.Columns.Contains("paymentStatus") && row["paymentStatus"] != DBNull.Value)
        finance.PaymentStatus = row["paymentStatus"].ToString();

  if (row.Table.Columns.Contains("memberStatus") && row["memberStatus"] != DBNull.Value)
         finance.MemberStatus = row["memberStatus"].ToString();

      if (row.Table.Columns.Contains("is_archived") && row["is_archived"] != DBNull.Value)
          finance.IsArchived = Convert.ToBoolean(row["is_archived"]);

        if (row.Table.Columns.Contains("archived_at") && row["archived_at"] != DBNull.Value)
       {
      DateTime archivedAt;
         if (DateTime.TryParse(row["archived_at"].ToString(), out archivedAt))
       finance.ArchivedAt = archivedAt;
      }

   financeList.Add(finance);
    }

            return financeList;
    }

        private static bool IsValidFinance(FinanceClass finance)
 {
    if (finance == null) return false;

            if (finance.EventId <= 0 ||
      string.IsNullOrWhiteSpace(finance.GoalName))
            {
                return false;
         }

  // Validation for amounts (must be non-negative)
        if (finance.TargetAmount < 0)
    return false;

     if (finance.CollectedAmount < 0)
    return false;

            if (finance.ExpensesAmount < 0)
        return false;

return true;
        }

        public static FinanceClass GetFinanceById(string id)
     {
     var finance = SearchFinanceById(id);
            return finance.Count > 0 ? finance[0] : null;
        }

        public static List<FinanceClass> GetArchivedFinance()
{
         List<FinanceClass> financeList = new List<FinanceClass>();

  try
            {
    System.Diagnostics.Debug.WriteLine("GetArchivedFinance: Starting");

   string connectionString =
    "server=localhost;port=3306;database=orgdb;uid=root;pwd=legorocket3368.;";

      using (var connection = new MySqlConnection(connectionString))
    {
     connection.Open();
     string sql = @"SELECT eg.*, e.event_name, 
     CONCAT(m.first_name, ' ', m.last_name) as member_name
              FROM event_goals eg
    LEFT JOIN events e ON eg.event_id = e.event_id
      LEFT JOIN members m ON e.created_by = m.member_id
         WHERE eg.is_archived = 1
     ORDER BY eg.archived_at DESC";

            using (var cmd = new MySqlCommand(sql, connection))
        {
    using (var adapter = new MySqlDataAdapter(cmd))
 {
        DataTable dataTable = new DataTable();
       adapter.Fill(dataTable);
       financeList = ConvertDataTableToFinanceList(dataTable);
   }
         }
  }
            }
       catch (Exception ex)
  {
    System.Diagnostics.Debug.WriteLine("Error reading archived finance from database: " + ex.Message);
  }

    return financeList;
}}
    // Legacy class for backward compatibility
    public class Financials
    {
        public string goalId { get; set; }
        public string eventSet { get; set; }
        public string goalName { get; set; }
public string description { get; set; }
     public string targetAmount { get; set; }
        public string collectedAmount { get; set; }
        public string expensesAmount { get; set; }
      public string currency { get; set; }
      public DateTime? dueDate { get; set; }
    public string status { get; set; }

        public Financials()
        {
 }
    }
}