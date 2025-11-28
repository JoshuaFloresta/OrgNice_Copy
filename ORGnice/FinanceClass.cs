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
        public int FinanceId { get; set; } // maps to finance_id (AUTO_INCREMENT)
        public string GoalId { get; set; }     // NOT NULL
        public string EventSet { get; set; }
        public string GoalName { get; set; }      // NOT NULL
        public string Description { get; set; }
      public decimal? TargetAmount { get; set; }  // nullable decimal for currency
  public decimal? CollectedAmount { get; set; }
        public decimal? ExpensesAmount { get; set; }
        public string Currency { get; set; }      // default "PHP" or user preference
   public DateTime? DueDate { get; set; }      // nullable
        public string Status { get; set; }         // "Active", "Completed", "Cancelled"

        private static Crud financeCrud = new Crud("finance");

      // Constructor to initialize finance data
     public FinanceClass(
     string goalId,
   string eventSet,
       string goalName,
            string description,
            decimal? targetAmount,
     decimal? collectedAmount,
  decimal? expensesAmount,
          string currency,
            DateTime? dueDate,
            string status)
    {
            GoalId = goalId;
     EventSet = eventSet;
   GoalName = goalName;
     Description = description;
     TargetAmount = targetAmount;
            CollectedAmount = collectedAmount;
            ExpensesAmount = expensesAmount;
  Currency = currency ?? "PHP";
            DueDate = dueDate;
 Status = status ?? "Active";
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

 // finance_id & created_date are omitted – DB sets them automatically
         string sql = @"INSERT INTO finance
     (goal_id, event_set, goal_name, description, target_amount,
        collected_amount, expenses_amount, currency, due_date, status)
         VALUES
    (@goalId, @eventSet, @goalName, @description, @targetAmount,
           @collectedAmount, @expensesAmount, @currency, @dueDate, @status);";

            using (var cmd = new MySqlCommand(sql, connection))
      {
 cmd.Parameters.AddWithValue("@goalId", finance.GoalId);
        cmd.Parameters.AddWithValue("@eventSet", finance.EventSet ?? (object)DBNull.Value);
      cmd.Parameters.AddWithValue("@goalName", finance.GoalName);
         cmd.Parameters.AddWithValue("@description", finance.Description ?? (object)DBNull.Value);
         cmd.Parameters.AddWithValue("@targetAmount", 
 finance.TargetAmount.HasValue ? (object)finance.TargetAmount.Value : DBNull.Value);
      cmd.Parameters.AddWithValue("@collectedAmount", 
      finance.CollectedAmount.HasValue ? (object)finance.CollectedAmount.Value : DBNull.Value);
          cmd.Parameters.AddWithValue("@expensesAmount", 
          finance.ExpensesAmount.HasValue ? (object)finance.ExpensesAmount.Value : DBNull.Value);
             cmd.Parameters.AddWithValue("@currency", finance.Currency ?? "PHP");
   cmd.Parameters.AddWithValue("@dueDate",
        finance.DueDate.HasValue ? (object)finance.DueDate.Value.Date : DBNull.Value);
           cmd.Parameters.AddWithValue("@status", finance.Status ?? "Active");

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
     /// <param name="includeArchived">Include deleted records in search</param>
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
        /// <param name="includeArchived">Include deleted records in search</param>
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
        /// Search finance records by event
  /// </summary>
        /// <param name="eventSet">Event to search for</param>
   /// <param name="includeArchived">Include deleted records in search</param>
        /// <returns>List of matching finance records</returns>
        public static List<FinanceClass> SearchFinanceByEvent(string eventSet, bool includeArchived = false)
      {
        List<FinanceClass> financeList = new List<FinanceClass>();

         try
            {
       var searchCriteria = new Dictionary<string, string>
           {
   { "event_set", eventSet }
     };

DataTable dataTable = financeCrud.Search(searchCriteria, includeArchived);
financeList = ConvertDataTableToFinanceList(dataTable);
     }
       catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error searching finance by event: " + ex.Message);
            }

            return financeList;
        }

        /// <summary>
      /// Search finance records by status
        /// </summary>
        /// <param name="status">Status to search for</param>
        /// <param name="includeArchived">Include deleted records in search</param>
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
        /// <param name="financeId">ID of the finance record to update</param>
        /// <param name="goalName">New goal name (optional)</param>
        /// <param name="description">New description (optional)</param>
        /// <param name="targetAmount">New target amount (optional)</param>
        /// <param name="collectedAmount">New collected amount (optional)</param>
   /// <param name="expensesAmount">New expenses amount (optional)</param>
        /// <param name="status">New status (optional)</param>
        /// <param name="dueDate">New due date (optional)</param>
        /// <returns>True if update was successful</returns>
      public static bool UpdateFinanceById(string financeId, string goalName = null, string description = null,
    decimal? targetAmount = null, decimal? collectedAmount = null, decimal? expensesAmount = null, 
            string status = null, DateTime? dueDate = null)
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
        updateData.Add("collected_amount", collectedAmount.Value);

    if (expensesAmount.HasValue)
       updateData.Add("expenses_amount", expensesAmount.Value);

       if (!string.IsNullOrWhiteSpace(status))
             updateData.Add("status", status);

             if (dueDate.HasValue)
     updateData.Add("due_date", dueDate.Value.Date);

          if (updateData.Count == 0)
      {
             System.Diagnostics.Debug.WriteLine("No data provided for finance update");
         return false;
      }

     int rowsAffected = financeCrud.UpdateById(updateData, "finance_id", financeId);
                return rowsAffected > 0;
       }
            catch (Exception ex)
 {
      System.Diagnostics.Debug.WriteLine("Error updating finance record: " + ex.Message);
            return false;
 }
        }

   /// <summary>
        /// Update finance record by searching with goal name
 /// </summary>
        /// <param name="searchGoalName">Goal name to search for</param>
        /// <param name="updateData">Data to update</param>
        /// <returns>Number of finance records updated</returns>
        public static int UpdateFinanceByGoalName(string searchGoalName, Dictionary<string, object> updateData)
        {
            try
            {
          var financeFound = SearchFinanceByGoalName(searchGoalName);

  if (financeFound.Count == 0)
     return 0;

                int totalUpdated = 0;
          foreach (var finance in financeFound)
            {
           string idString = finance.FinanceId.ToString();

         if (UpdateFinanceById(idString,
   updateData.ContainsKey("goal_name") ? updateData["goal_name"]?.ToString() : null,
         updateData.ContainsKey("description") ? updateData["description"]?.ToString() : null,
         updateData.ContainsKey("target_amount") ? (decimal?)updateData["target_amount"] : null,
         updateData.ContainsKey("collected_amount") ? (decimal?)updateData["collected_amount"] : null,
          updateData.ContainsKey("expenses_amount") ? (decimal?)updateData["expenses_amount"] : null,
        updateData.ContainsKey("status") ? updateData["status"]?.ToString() : null,
         updateData.ContainsKey("due_date") ? (DateTime?)updateData["due_date"] : null))
    {
        totalUpdated++;
      }
      }

   return totalUpdated;
      }
   catch (Exception ex)
     {
    System.Diagnostics.Debug.WriteLine("Error updating finance by goal name: " + ex.Message);
      return 0;
        }
        }

        // USER-FRIENDLY DELETE (ARCHIVE) METHODS

   /// <summary>
        /// Archive (soft delete) a finance record by ID
        /// </summary>
        /// <param name="financeId">ID of the finance record to archive</param>
        /// <returns>True if finance record was archived successfully</returns>
        public static bool DeleteFinanceById(string financeId)
        {
            try
            {
 int rowsAffected = financeCrud.DeleteById("finance_id", financeId);
      return rowsAffected > 0;
    }
            catch (Exception ex)
            {
        System.Diagnostics.Debug.WriteLine("Error archiving finance record: " + ex.Message);
      return false;
            }
        }

        /// <summary>
        /// Archive (soft delete) finance records by goal name
        /// </summary>
        /// <param name="goalName">Goal name to search for and delete</param>
        /// <returns>Number of finance records archived</returns>
        public static int DeleteFinanceByGoalName(string goalName)
        {
try
       {
      // First find finance records by goal name
        var financeFound = SearchFinanceByGoalName(goalName);

     int totalDeleted = 0;
         foreach (var finance in financeFound)
             {
        if (DeleteFinanceById(finance.FinanceId.ToString()))
        {
           totalDeleted++;
            }
          }

 return totalDeleted;
            }
          catch (Exception ex)
    {
    System.Diagnostics.Debug.WriteLine("Error deleting finance by goal name: " + ex.Message);
       return 0;
          }
        }

        /// <summary>
        /// Archive (soft delete) finance records by event
        /// </summary>
        /// <param name="eventSet">Event to search for and delete</param>
     /// <returns>Number of finance records archived</returns>
        public static int DeleteFinanceByEvent(string eventSet)
        {
      try
            {
                var searchCriteria = new Dictionary<string, string>
      {
              { "event_set", eventSet }
  };

         return financeCrud.SearchAndDelete(searchCriteria);
            }
   catch (Exception ex)
          {
      System.Diagnostics.Debug.WriteLine("Error deleting finance by event: " + ex.Message);
       return 0;
       }
     }

        /// <summary>
   /// Restore archived finance record by ID
        /// </summary>
      /// <param name="financeId">ID of the finance record to restore</param>
        /// <returns>True if finance record was restored successfully</returns>
        public static bool RestoreFinanceById(string financeId)
    {
     try
{
        var whereParameters = new Dictionary<string, object>
       {
       { "@id", financeId }
    };

           var updateData = new Dictionary<string, object>
  {
         { "deleted_at", null },
             { "is_deleted", 0 }
   };

    int rowsAffected = financeCrud.Update(updateData, "finance_id = @id AND is_deleted = 1", whereParameters);
   return rowsAffected > 0;
            }
            catch (Exception ex)
        {
  System.Diagnostics.Debug.WriteLine("Error restoring finance record: " + ex.Message);
  return false;
}
        }

     // EXISTING METHODS (Updated to use new search functionality)

        public static List<FinanceClass> GetAllFinanceFromDatabase()
        {
            List<FinanceClass> financeList = new List<FinanceClass>();

            try
         {
     DataTable dataTable = financeCrud.ReadActive();
financeList = ConvertDataTableToFinanceList(dataTable);
         }
      catch (Exception ex)
      {
                System.Diagnostics.Debug.WriteLine("Error reading finance from database: " + ex.Message);
     }

            return financeList;
        }

        public static List<FinanceClass> GetFinanceByEvent(string eventSet)
        {
            return SearchFinanceByEvent(eventSet);
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
       DataTable dataTable = financeCrud.ReadArchived();
     financeList = ConvertDataTableToFinanceList(dataTable);
  }
            catch (Exception ex)
      {
      System.Diagnostics.Debug.WriteLine("Error reading archived finance from database: " + ex.Message);
            }

            return financeList;
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

      // finance_id -> FinanceId
       if (row.Table.Columns.Contains("finance_id") && row["finance_id"] != DBNull.Value)
{
             int parsedId;
          try
    {
          parsedId = Convert.ToInt32(row["finance_id"]);
       }
         catch
        {
           parsedId = 0;
           }
         finance.FinanceId = parsedId;
             }

        if (row.Table.Columns.Contains("goal_id") && row["goal_id"] != DBNull.Value)
       finance.GoalId = row["goal_id"].ToString();

           if (row.Table.Columns.Contains("event_set") && row["event_set"] != DBNull.Value)
      finance.EventSet = row["event_set"].ToString();

                if (row.Table.Columns.Contains("goal_name") && row["goal_name"] != DBNull.Value)
          finance.GoalName = row["goal_name"].ToString();

           if (row.Table.Columns.Contains("description") && row["description"] != DBNull.Value)
          finance.Description = row["description"].ToString();

           if (row.Table.Columns.Contains("target_amount") && row["target_amount"] != DBNull.Value)
  {
   decimal targetAmount;
          if (decimal.TryParse(row["target_amount"].ToString(), out targetAmount))
     finance.TargetAmount = targetAmount;
      else
finance.TargetAmount = null;
    }

       if (row.Table.Columns.Contains("collected_amount") && row["collected_amount"] != DBNull.Value)
 {
       decimal collectedAmount;
        if (decimal.TryParse(row["collected_amount"].ToString(), out collectedAmount))
       finance.CollectedAmount = collectedAmount;
      else
            finance.CollectedAmount = null;
         }

             if (row.Table.Columns.Contains("expenses_amount") && row["expenses_amount"] != DBNull.Value)
              {
  decimal expensesAmount;
             if (decimal.TryParse(row["expenses_amount"].ToString(), out expensesAmount))
     finance.ExpensesAmount = expensesAmount;
                 else
             finance.ExpensesAmount = null;
      }

    if (row.Table.Columns.Contains("currency") && row["currency"] != DBNull.Value)
  finance.Currency = row["currency"].ToString();

  if (row.Table.Columns.Contains("due_date") && row["due_date"] != DBNull.Value)
  {
        DateTime dueDate;
    if (DateTime.TryParse(row["due_date"].ToString(), out dueDate))
   finance.DueDate = dueDate;
           else
      finance.DueDate = null;
       }

      if (row.Table.Columns.Contains("status") && row["status"] != DBNull.Value)
        finance.Status = row["status"].ToString();

           financeList.Add(finance);
            }

          return financeList;
  }

        /// <summary>
     /// Initialize archiving columns for finance table if they don't exist
        /// </summary>
        /// <returns>True if columns exist or were added successfully</returns>
        public static bool InitializeArchiving()
     {
        try
        {
        return financeCrud.AddArchivingColumns();
            }
     catch (Exception ex)
            {
System.Diagnostics.Debug.WriteLine("Error initializing finance archiving columns: " + ex.Message);
                return false;
            }
        }

        private static bool IsValidFinance(FinanceClass finance)
        {
            if (finance == null) return false;

    if (string.IsNullOrWhiteSpace(finance.GoalId) ||
       string.IsNullOrWhiteSpace(finance.GoalName))
     {
     return false;
       }

            // Optional: Add validation for amounts (must be non-negative)
   if (finance.TargetAmount.HasValue && finance.TargetAmount < 0)
       return false;

       if (finance.CollectedAmount.HasValue && finance.CollectedAmount < 0)
   return false;

            if (finance.ExpensesAmount.HasValue && finance.ExpensesAmount < 0)
          return false;

            return true;
        }
    }

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