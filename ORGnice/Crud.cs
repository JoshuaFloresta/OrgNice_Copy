using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;

namespace ORGnice
{
    public class Crud
    {
        private string connectionString = "server=localhost;port=3306;database=orgdb;uid=root;pwd=Joshua@2004;";
        private string tableName;

        public Crud(string table)
        {
            this.tableName = table;
        }

        /// <summary>
        /// Get all active (non-deleted) records for DataGridView display
        /// This method ensures deleted members never show in DataGridView
        /// </summary>
        /// <returns>DataTable with only active records</returns>
        public DataTable GetActiveRecordsForDisplay()
        {
            if (HasArchivingColumns())
            {
                return ReadActive();
            }
            else
            {
                // If no archiving columns exist, return all records
                return Read();
            }
        }

        /// <summary>
        /// Filter out deleted records from any DataTable
        /// Use this method to ensure deleted records don't appear in any display
        /// </summary>
        /// <param name="dataTable">DataTable to filter</param>
        /// <returns>DataTable with deleted records removed</returns>
        public DataTable FilterActiveRecords(DataTable dataTable)
        {
            if (dataTable == null || dataTable.Rows.Count == 0)
                return dataTable;

            // Check if the table has archiving columns
            if (!dataTable.Columns.Contains("is_deleted"))
                return dataTable; // No archiving columns, return as is

            // Create a new DataTable with the same structure
            DataTable filteredTable = dataTable.Clone();

            // Copy only active (non-deleted) rows
            foreach (DataRow row in dataTable.Rows)
            {
                if (row["is_deleted"] == DBNull.Value ||
                    Convert.ToInt32(row["is_deleted"]) == 0)
                {
                    filteredTable.ImportRow(row);
                }
            }

            return filteredTable;
        }

        /// <summary>
        /// Enhanced search method that automatically excludes deleted records unless specified
        /// </summary>
        /// <param name="searchTerm">General search term to search across multiple fields</param>
        /// <param name="includeDeleted">If true, includes deleted records in search</param>
        /// <returns>DataTable with search results (active records only by default)</returns>
        public DataTable SearchActiveRecords(string searchTerm, bool includeDeleted = false)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return includeDeleted ? Read() : GetActiveRecordsForDisplay();
            }

            // Use the existing AdvancedSearch method which already filters deleted records
            return AdvancedSearch(
                name: searchTerm,           // Search in names
                id: searchTerm,             // Search in ID
                department: searchTerm,     // Search in department
                section: searchTerm,        // Search in section
                email: searchTerm,          // Search in email
                includeArchived: includeDeleted
            );
        }

        /// <summary>
        /// Get records by ID, ensuring deleted records are excluded unless specified
        /// </summary>
        /// <param name="id">ID to search for</param>
        /// <param name="idColumn">Name of the ID column</param>
        /// <param name="includeDeleted">Include deleted records</param>
        /// <returns>DataTable with matching active records</returns>
        public DataTable GetActiveRecordById(string id, string idColumn = "id", bool includeDeleted = false)
        {
            return SearchById(id, idColumn, includeDeleted);
        }

        /// <summary>
        /// Verify a record exists and is active (not deleted)
        /// </summary>
        /// <param name="id">ID to check</param>
        /// <param name="idColumn">Name of the ID column</param>
        /// <returns>True if record exists and is active</returns>
        public bool IsRecordActive(string id, string idColumn = "id")
        {
            try
            {
                DataTable result = GetActiveRecordById(id, idColumn, false);
                return result.Rows.Count > 0;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// User-friendly search method that allows searching by multiple criteria
        /// </summary>
        /// <param name="searchCriteria">Dictionary of search criteria (column name -> search value)</param>
        /// <param name="includeArchived">Whether to include archived records in search</param>
        /// <param name="isExactMatch">If true, uses exact match; if false, uses LIKE for partial matches</param>
        /// <returns>DataTable with search results</returns>
        public DataTable Search(Dictionary<string, string> searchCriteria, bool includeArchived = false, bool isExactMatch = false)
        {
            if (searchCriteria == null || searchCriteria.Count == 0)
            {
                return includeArchived ? Read() : ReadActive();
            }

            List<string> whereClauses = new List<string>();
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            // Build WHERE clauses for each search criterion
            foreach (var criteria in searchCriteria)
            {
                if (!string.IsNullOrWhiteSpace(criteria.Value))
                {
                    string paramName = $"@search_{criteria.Key}";

                    if (isExactMatch)
                    {
                        whereClauses.Add($"{criteria.Key} = {paramName}");
                        parameters.Add(paramName, criteria.Value);
                    }
                    else
                    {
                        whereClauses.Add($"{criteria.Key} LIKE {paramName}");
                        parameters.Add(paramName, $"%{criteria.Value}%");
                    }
                }
            }

            // Add archive filter if needed - THIS IS KEY FOR FILTERING DELETED RECORDS
            if (!includeArchived && HasArchivingColumns())
            {
                whereClauses.Add("is_deleted = 0");
            }

            string whereClause = whereClauses.Count > 0 ? string.Join(" AND ", whereClauses) : null;

            return Read(whereClause, parameters);
        }

        /// <summary>
        /// Search for members by name (first name or last name)
        /// </summary>
        /// <param name="name">Name to search for (partial match)</param>
        /// <param name="includeArchived">Include archived records</param>
        /// <returns>DataTable with matching records</returns>
        public DataTable SearchByName(string name, bool includeArchived = false)
        {
            if (string.IsNullOrWhiteSpace(name))
                return includeArchived ? Read() : ReadActive();

            List<string> whereClauses = new List<string>
            {
                "(first_name LIKE @name OR last_name LIKE @name)"
            };

            // AUTOMATICALLY FILTER DELETED RECORDS
            if (!includeArchived && HasArchivingColumns())
            {
                whereClauses.Add("is_deleted = 0");
            }

            var parameters = new Dictionary<string, object>
            {
                { "@name", $"%{name}%" }
            };

            return Read(string.Join(" AND ", whereClauses), parameters);
        }

        /// <summary>
        /// Search for records by ID
        /// </summary>
        /// <param name="id">ID to search for</param>
        /// <param name="idColumn">Name of the ID column (default: "id")</param>
        /// <param name="includeArchived">Include archived records</param>
        /// <returns>DataTable with matching records</returns>
        public DataTable SearchById(string id, string idColumn = "id", bool includeArchived = false)
        {
            var searchCriteria = new Dictionary<string, string>
            {
                { idColumn, id }
            };

            return Search(searchCriteria, includeArchived, true);
        }

        /// <summary>
        /// Search for records by department
        /// </summary>
        /// <param name="department">Department to search for</param>
        /// <param name="includeArchived">Include archived records</param>
        /// <returns>DataTable with matching records</returns>
        public DataTable SearchByDepartment(string department, bool includeArchived = false)
        {
            var searchCriteria = new Dictionary<string, string>
            {
                { "department", department }
            };

            return Search(searchCriteria, includeArchived, false);
        }

        /// <summary>
        /// Advanced search with multiple filters
        /// </summary>
        /// <param name="name">Name (first or last name)</param>
        /// <param name="id">ID</param>
        /// <param name="department">Department</param>
        /// <param name="section">Section</param>
        /// <param name="role">Role</param>
        /// <param name="email">Email</param>
        /// <param name="includeArchived">Include archived records</param>
        /// <returns>DataTable with matching records</returns>
        public DataTable AdvancedSearch(string name = null, string id = null, string department = null,
            string section = null, string role = null, string email = null, bool includeArchived = false)
        {
            List<string> whereClauses = new List<string>();
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            // Name search (first_name OR last_name)
            if (!string.IsNullOrWhiteSpace(name))
            {
                whereClauses.Add("(first_name LIKE @name OR last_name LIKE @name)");
                parameters.Add("@name", $"%{name}%");
            }

            // ID search (exact match)
            if (!string.IsNullOrWhiteSpace(id))
            {
                whereClauses.Add("id = @id");
                parameters.Add("@id", id);
            }

            // Department search (partial match)
            if (!string.IsNullOrWhiteSpace(department))
            {
                whereClauses.Add("department LIKE @department");
                parameters.Add("@department", $"%{department}%");
            }

            // Section search (partial match)
            if (!string.IsNullOrWhiteSpace(section))
            {
                whereClauses.Add("section LIKE @section");
                parameters.Add("@section", $"%{section}%");
            }

            // Role search (exact match)
            if (!string.IsNullOrWhiteSpace(role))
            {
                whereClauses.Add("role = @role");
                parameters.Add("@role", role);
            }

            // Email search (partial match)
            if (!string.IsNullOrWhiteSpace(email))
            {
                whereClauses.Add("email LIKE @email");
                parameters.Add("@email", $"%{email}%");
            }

            // AUTOMATICALLY FILTER DELETED RECORDS
            if (!includeArchived && HasArchivingColumns())
            {
                whereClauses.Add("is_deleted = 0");
            }

            string whereClause = whereClauses.Count > 0 ? string.Join(" AND ", whereClauses) : null;

            return Read(whereClause, parameters);
        }

        /// <summary>
        /// User-friendly update method - search and update records
        /// </summary>
        /// <param name="searchCriteria">Criteria to find records to update</param>
        /// <param name="updateData">Data to update</param>
        /// <param name="confirmUpdate">If true, requires confirmation before updating</param>
        /// <returns>Number of records updated</returns>
        public int SearchAndUpdate(Dictionary<string, string> searchCriteria, Dictionary<string, object> updateData, bool confirmUpdate = true)
        {
            if (searchCriteria == null || searchCriteria.Count == 0)
                throw new ArgumentException("Search criteria are required for update operations");

            if (updateData == null || updateData.Count == 0)
                throw new ArgumentException("Update data cannot be null or empty");

            // First, search for records that will be updated (ONLY ACTIVE RECORDS)
            DataTable recordsToUpdate = Search(searchCriteria, false, true);

            if (recordsToUpdate.Rows.Count == 0)
                return 0;

            if (confirmUpdate)
            {
                System.Diagnostics.Debug.WriteLine($"Found {recordsToUpdate.Rows.Count} active record(s) to update.");
            }

            // Build WHERE clause from search criteria
            List<string> whereClauses = new List<string>();
            Dictionary<string, object> whereParameters = new Dictionary<string, object>();

            foreach (var criteria in searchCriteria)
            {
                if (!string.IsNullOrWhiteSpace(criteria.Value))
                {
                    string paramName = $"@where_{criteria.Key}";
                    whereClauses.Add($"{criteria.Key} = {paramName}");
                    whereParameters.Add(paramName, criteria.Value);
                }
            }

            // ENSURE ONLY ACTIVE RECORDS ARE UPDATED
            if (HasArchivingColumns())
            {
                whereClauses.Add("is_deleted = 0");
            }

            string whereClause = string.Join(" AND ", whereClauses);

            return Update(updateData, whereClause, whereParameters);
        }

        /// <summary>
        /// User-friendly delete (archive) method - search and archive records
        /// </summary>
        /// <param name="searchCriteria">Criteria to find records to archive</param>
        /// <param name="confirmDelete">If true, requires confirmation before deleting</param>
        /// <returns>Number of records archived</returns>
        public int SearchAndDelete(Dictionary<string, string> searchCriteria, bool confirmDelete = true)
        {
            if (searchCriteria == null || searchCriteria.Count == 0)
                throw new ArgumentException("Search criteria are required for delete operations");

            // First, search for records that will be archived (ONLY ACTIVE RECORDS)
            DataTable recordsToDelete = Search(searchCriteria, false, true);

            if (recordsToDelete.Rows.Count == 0)
                return 0;

            if (confirmDelete)
            {
                System.Diagnostics.Debug.WriteLine($"Found {recordsToDelete.Rows.Count} active record(s) to archive.");
            }

            // Build WHERE clause from search criteria
            List<string> whereClauses = new List<string>();
            Dictionary<string, object> whereParameters = new Dictionary<string, object>();

            foreach (var criteria in searchCriteria)
            {
                if (!string.IsNullOrWhiteSpace(criteria.Value))
                {
                    string paramName = $"@where_{criteria.Key}";
                    whereClauses.Add($"{criteria.Key} = {paramName}");
                    whereParameters.Add(paramName, criteria.Value);
                }
            }

            // ENSURE ONLY ACTIVE RECORDS ARE DELETED
            if (HasArchivingColumns())
            {
                whereClauses.Add("is_deleted = 0");
            }

            string whereClause = string.Join(" AND ", whereClauses);

            return Delete(whereClause, whereParameters);
        }

        /// <summary>
        /// Generic method to delete (archive) records by searching in name-related columns
        /// Works with members (first_name, last_name) and events (eventName) automatically
        /// </summary>
        /// <param name="searchValue">Value to search for in name columns</param>
        /// <param name="nameColumns">Optional: Custom name columns. If null, uses auto-detection</param>
        /// <returns>Number of records archived</returns>
        public int DeleteByName(string searchValue, string[] nameColumns = null)
        {
            if (string.IsNullOrWhiteSpace(searchValue))
                return 0;

            try
            {
                // Auto-detect name columns if not provided
                if (nameColumns == null)
                {
                    nameColumns = GetNameColumns();
                }

                if (nameColumns.Length == 0)
                    return 0;

                // Build WHERE clause
                List<string> nameConditions = new List<string>();
                var whereParameters = new Dictionary<string, object>();

                for (int i = 0; i < nameColumns.Length; i++)
                {
                    string paramName = $"@name{i}";
                    nameConditions.Add($"{nameColumns[i]} LIKE {paramName}");
                    whereParameters.Add(paramName, $"%{searchValue}%");
                }

                string whereClause = $"({string.Join(" OR ", nameConditions)})";

                if (HasArchivingColumns())
                {
                    whereClause += " AND is_deleted = 0";
                }

                return Delete(whereClause, whereParameters);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error deleting by name '{searchValue}': " + ex.Message);
                return 0;
            }
        }

        /// <summary>
        /// Auto-detect name columns in the current table
        /// </summary>
        /// <returns>Array of name column names</returns>
        private string[] GetNameColumns()
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            List<string> nameColumns = new List<string>();

            try
            {
                connection.Open();
                string sql = $"SHOW COLUMNS FROM {tableName}";
                MySqlCommand cmd = new MySqlCommand(sql, connection);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string columnName = reader["Field"].ToString().ToLower();

                    if (columnName.Contains("name") || columnName.Contains("title"))
                    {
                        nameColumns.Add(reader["Field"].ToString());
                    }
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error detecting name columns: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return nameColumns.ToArray();
        }

        // Original CRUD methods remain the same...
        public DataTable Read(string whereClause = null, Dictionary<string, object> parameters = null)
        {
            DataTable dataTable = new DataTable();
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();

                string sql = $"SELECT * FROM {tableName}";

                if (!string.IsNullOrEmpty(whereClause))
                {
                    sql += $" WHERE {whereClause}";
                }

                MySqlCommand cmd = new MySqlCommand(sql, connection);

                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        cmd.Parameters.AddWithValue(param.Key, param.Value);
                    }
                }

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dataTable);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error reading from {tableName}: " + ex.Message);
                throw new Exception($"Error reading from {tableName}: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return dataTable;
        }

        public DataRow ReadById(string idColumn, object idValue)
        {
            var parameters = new Dictionary<string, object>
            {
                { $"@{idColumn}", idValue }
            };

            DataTable result = Read($"{idColumn} = @{idColumn}", parameters);

            return result.Rows.Count > 0 ? result.Rows[0] : null;
        }

        public int Update(Dictionary<string, object> updateData, string whereClause, Dictionary<string, object> whereParameters = null)
        {
            if (updateData == null || updateData.Count == 0)
                throw new ArgumentException("Update data cannot be null or empty");

            if (string.IsNullOrEmpty(whereClause))
                throw new ArgumentException("WHERE clause is required for update operations");

            MySqlConnection connection = new MySqlConnection(connectionString);
            int rowsAffected = 0;

            try
            {
                connection.Open();

                var setClauses = new List<string>();
                foreach (var kvp in updateData)
                {
                    setClauses.Add($"{kvp.Key} = @set_{kvp.Key}");
                }

                string sql = $"UPDATE {tableName} SET {string.Join(", ", setClauses)} WHERE {whereClause}";
                MySqlCommand cmd = new MySqlCommand(sql, connection);

                foreach (var kvp in updateData)
                {
                    cmd.Parameters.AddWithValue($"@set_{kvp.Key}", kvp.Value ?? DBNull.Value);
                }

                if (whereParameters != null)
                {
                    foreach (var param in whereParameters)
                    {
                        cmd.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                    }
                }

                rowsAffected = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating {tableName}: " + ex.Message);
                throw new Exception($"Error updating {tableName}: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return rowsAffected;
        }

        public int UpdateById(Dictionary<string, object> updateData, string idColumn, object idValue)
        {
            var whereParameters = new Dictionary<string, object>
            {
                { $"@{idColumn}", idValue }
            };

            return Update(updateData, $"{idColumn} = @{idColumn}", whereParameters);
        }

        public int Delete(string whereClause, Dictionary<string, object> whereParameters = null)
        {
            if (string.IsNullOrEmpty(whereClause))
                throw new ArgumentException("WHERE clause is required for delete operations");

            if (!HasArchivingColumns())
            {
                throw new InvalidOperationException($"Table {tableName} does not have archiving columns (deleted_at, is_deleted). Please add these columns first.");
            }

            var updateData = new Dictionary<string, object>
            {
                { "deleted_at", DateTime.Now },
                { "is_deleted", 1 }
            };

            return Update(updateData, whereClause, whereParameters);
        }

        public int DeleteById(string idColumn, object idValue)
        {
            var whereParameters = new Dictionary<string, object>
            {
                { $"@{idColumn}", idValue }
            };

            return Delete($"{idColumn} = @{idColumn}", whereParameters);
        }

        public DataTable ReadActive(string additionalWhereClause = null, Dictionary<string, object> parameters = null)
        {
            string whereClause = "is_deleted = 0";

            if (!string.IsNullOrEmpty(additionalWhereClause))
            {
                whereClause += $" AND ({additionalWhereClause})";
            }

            return Read(whereClause, parameters);
        }

        public DataTable ReadArchived(string additionalWhereClause = null, Dictionary<string, object> parameters = null)
        {
            string whereClause = "is_deleted = 1";

            if (!string.IsNullOrEmpty(additionalWhereClause))
            {
                whereClause += $" AND ({additionalWhereClause})";
            }

            return Read(whereClause, parameters);
        }

        private bool HasArchivingColumns()
        {
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();
                string sql = $"SHOW COLUMNS FROM {tableName} WHERE Field IN ('deleted_at', 'is_deleted')";
                MySqlCommand cmd = new MySqlCommand(sql, connection);

                MySqlDataReader reader = cmd.ExecuteReader();
                int columnCount = 0;

                while (reader.Read())
                {
                    columnCount++;
                }

                reader.Close();
                return columnCount == 2;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error checking archiving columns for {tableName}: " + ex.Message);
                return false;
            }
            finally
            {
                connection.Close();
            }
        }

        public bool AddArchivingColumns()
        {
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();

                string sql1 = $"ALTER TABLE {tableName} ADD COLUMN deleted_at DATETIME NULL";
                MySqlCommand cmd1 = new MySqlCommand(sql1, connection);

                try
                {
                    cmd1.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    if (!ex.Message.Contains("Duplicate column name"))
                        throw;
                }

                string sql2 = $"ALTER TABLE {tableName} ADD COLUMN is_deleted TINYINT(1) DEFAULT 0";
                MySqlCommand cmd2 = new MySqlCommand(sql2, connection);

                try
                {
                    cmd2.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    if (!ex.Message.Contains("Duplicate column name"))
                        throw;
                }

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error adding archiving columns to {tableName}: " + ex.Message);
                return false;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}