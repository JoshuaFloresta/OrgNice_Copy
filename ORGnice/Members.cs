using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;

namespace ORGnice
{
    public class Members
    {
        public int MemberId { get; set; }          // maps to member_id (AUTO_INCREMENT)
        public string StudentId { get; set; }      // NOT NULL, UNIQUE
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }         // "Male", "Female", "Other"
        public DateTime? Birthday { get; set; }    // nullable
        public string Section { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }
        public string Role { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ProfileImagePath { get; set; }


        private static Crud membersCrud = new Crud("members");

        // Constructor to initialize member data
        public Members(
         string studentId,
         string firstName,
         string lastName,
         string gender,
         DateTime? birthday,
         string section,
         string email,
         string department,
         string role,
         string username,
         string password,
         string profileImagePath)
        {
            StudentId = studentId;
            FirstName = firstName;
            LastName = lastName;
            Gender = gender;
            Birthday = birthday;
            Section = section;
            Email = email;
            Department = department;
            Role = role;
            Username = username;
            Password = password;        // can be null/empty
            ProfileImagePath = profileImagePath;
        }
        public Members()
        {
        }

        // Method to add a member and automatically populate the database
        public static bool AddMemberToDatabase(Members member)
        {
            if (!IsValidMember(member))
                return false;

            string connectionString =
                "server=localhost;port=3306;database=orgdb;uid=root;pwd=Joshua@2004;";

            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // member_id & joined_date are omitted – DB sets them automatically
                    string sql = @" INSERT INTO members
                           (student_id, first_name, last_name, gender, birthday,
                             section, email, department, role,
                             username, password, profile_image_path)
                        VALUES
                            (@studentId, @firstName, @lastName, @gender, @birthday,
                             @section, @email, @department, @role,
                             @username, @password, @profileImagePath);";

                    using (var cmd = new MySqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@studentId", member.StudentId);
                        cmd.Parameters.AddWithValue("@firstName", member.FirstName);
                        cmd.Parameters.AddWithValue("@lastName", member.LastName);
                        cmd.Parameters.AddWithValue("@gender", (object)member.Gender ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@birthday",
                            member.Birthday.HasValue ? (object)member.Birthday.Value.Date : DBNull.Value);
                        cmd.Parameters.AddWithValue("@section", member.Section);
                        cmd.Parameters.AddWithValue("@email", member.Email);
                        cmd.Parameters.AddWithValue("@department", member.Department);
                        cmd.Parameters.AddWithValue("@role", member.Role);
                        cmd.Parameters.AddWithValue("@username", member.Username ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@password", member.Password ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@profileImagePath",
                            string.IsNullOrWhiteSpace(member.ProfileImagePath)
                                ? (object)DBNull.Value
                                : member.ProfileImagePath);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(
                        "Error adding member to database: " + ex.Message);
                    return false;
                }
            }
        }


        // these updated schema 

        // USER-FRIENDLY SEARCH METHODS

        /// <summary>
        /// Search members by name (searches both first name and last name)
        /// </summary>
        /// <param name="name">Name to search for</param>
        /// <param name="includeArchived">Include deleted members in search</param>
        /// <returns>List of matching members</returns>
        public static List<Members> SearchMembersByName(string name, bool includeArchived = false)
        {
            List<Members> membersList = new List<Members>();

            try
            {
                DataTable dataTable = membersCrud.SearchByName(name, includeArchived);
                membersList = ConvertDataTableToMembersList(dataTable);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error searching members by name: " + ex.Message);
            }

            return membersList;
        }

        /// <summary>
        /// Search members by ID
        /// </summary>
        /// <param name="id">Student ID to search for</param>
        /// <param name="includeArchived">Include deleted members in search</param>
        /// <returns>List of matching members (usually one or empty)</returns>
        public static List<Members> SearchMembersById(string id, bool includeArchived = false)
        {
            List<Members> membersList = new List<Members>();

            try
            {
                DataTable dataTable = membersCrud.SearchById(id, "id", includeArchived);
                membersList = ConvertDataTableToMembersList(dataTable);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error searching members by ID: " + ex.Message);
            }

            return membersList;
        }

        /// <summary>
        /// Search members by department
        /// </summary>
        /// <param name="department">Department to search for</param>
        /// <param name="includeArchived">Include deleted members in search</param>
        /// <returns>List of matching members</returns>
        public static List<Members> SearchMembersByDepartment(string department, bool includeArchived = false)
        {
            List<Members> membersList = new List<Members>();

            try
            {
                DataTable dataTable = membersCrud.SearchByDepartment(department, includeArchived);
                membersList = ConvertDataTableToMembersList(dataTable);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error searching members by department: " + ex.Message);
            }

            return membersList;
        }

        /// <summary>
        /// Advanced search with multiple criteria
        /// </summary>
        /// <param name="name">Name (partial match on first or last name)</param>
        /// <param name="id">Student ID (exact match)</param>
        /// <param name="department">Department (partial match)</param>
        /// <param name="section">Section (partial match)</param>
        /// <param name="role">Role (exact match)</param>
        /// <param name="email">Email (partial match)</param>
        /// <param name="includeArchived">Include deleted members in search</param>
        /// <returns>List of matching members</returns>
        public static List<Members> AdvancedSearchMembers(string name = null, string id = null, string department = null,
            string section = null, string role = null, string email = null, bool includeArchived = false)
        {
            List<Members> membersList = new List<Members>();

            try
            {
                DataTable dataTable = membersCrud.AdvancedSearch(name, id, department, section, role, email, includeArchived);
                membersList = ConvertDataTableToMembersList(dataTable);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error in advanced search: " + ex.Message);
            }

            return membersList;
        }

        // USER-FRIENDLY UPDATE METHODS

        /// <summary>
        /// Update member information by ID
        /// </summary>
        /// <param name="memberId">ID of the member to update</param>
        /// <param name="firstName">New first name (optional)</param>
        /// <param name="lastName">New last name (optional)</param>
        /// <param name="section">New section (optional)</param>
        /// <param name="email">New email (optional)</param>
        /// <param name="department">New department (optional)</param>
        /// <param name="role">New role (optional)</param>
        /// <returns>True if update was successful</returns>
        public static bool UpdateMemberById(string memberId, string firstName = null, string lastName = null,
            string section = null, string email = null, string department = null, string role = null)
        {
            try
            {
                // Build update data dictionary with only non-null values
                var updateData = new Dictionary<string, object>();

                if (!string.IsNullOrWhiteSpace(firstName))
                    updateData.Add("first_name", firstName);

                if (!string.IsNullOrWhiteSpace(lastName))
                    updateData.Add("last_name", lastName);

                if (!string.IsNullOrWhiteSpace(section))
                    updateData.Add("section", section);

                if (!string.IsNullOrWhiteSpace(email))
                    updateData.Add("email", email);

                if (!string.IsNullOrWhiteSpace(department))
                    updateData.Add("department", department);

                if (!string.IsNullOrWhiteSpace(role))
                    updateData.Add("role", role);

                if (updateData.Count == 0)
                {
                    System.Diagnostics.Debug.WriteLine("No data provided for update");
                    return false;
                }

                int rowsAffected = membersCrud.UpdateById(updateData, "id", memberId);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error updating member: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Update member by searching with name
        /// </summary>
        /// <param name="searchName">Name to search for</param>
        /// <param name="updateData">Data to update</param>
        /// <returns>Number of members updated</returns>
        public static int UpdateMemberByName(string searchName, Dictionary<string, object> updateData)
        {
            try
            {
                // Search in both first_name and last_name by using advanced search
                var membersFound = SearchMembersByName(searchName);

                if (membersFound.Count == 0)
                    return 0;

                int totalUpdated = 0;                   
                foreach (var member in membersFound)
                {
                    string idString = member.MemberId.ToString();

                    if (UpdateMemberById(idString,
                        updateData.ContainsKey("first_name") ? updateData["first_name"]?.ToString() : null,
                        updateData.ContainsKey("last_name") ? updateData["last_name"]?.ToString() : null,
                        updateData.ContainsKey("section") ? updateData["section"]?.ToString() : null,
                        updateData.ContainsKey("email") ? updateData["email"]?.ToString() : null,
                        updateData.ContainsKey("department") ? updateData["department"]?.ToString() : null,
                        updateData.ContainsKey("role") ? updateData["role"]?.ToString() : null))
                    {
                        totalUpdated++;
                    }
                }

                return totalUpdated;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error updating member by name: " + ex.Message);
                return 0;
            }
        }

        // USER-FRIENDLY DELETE (ARCHIVE) METHODS

        /// <summary>
        /// Archive (soft delete) a member by ID
        /// </summary>
        /// <param name="memberId">ID of the member to archive</param>
        /// <returns>True if member was archived successfully</returns>
        public static bool DeleteMemberById(string memberId)
        {
            try
            {
                int rowsAffected = membersCrud.DeleteById("id", memberId);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error archiving member: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Archive (soft delete) members by name
        /// </summary>
        /// <param name="name">Name to search for and delete</param>
        /// <returns>Number of members archived</returns>
        public static int DeleteMembersByName(string name)
        {
            try
            {
                // First find members by name
                var membersFound = SearchMembersByName(name);

                int totalDeleted = 0;
                foreach (var member in membersFound)
                {
                    if (DeleteMemberById(member.MemberId.ToString()))
                    {
                        totalDeleted++;
                    }
                }

                return totalDeleted;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error deleting members by name: " + ex.Message);
                return 0;
            }
        }

        /// <summary>
        /// Archive (soft delete) members by department
        /// </summary>
        /// <param name="department">Department to search for and delete</param>
        /// <returns>Number of members archived</returns>
        public static int DeleteMembersByDepartment(string department)
        {
            try
            {
                var searchCriteria = new Dictionary<string, string>
                    {
                        { "department", department }
                    };

                return membersCrud.SearchAndDelete(searchCriteria);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error deleting members by department: " + ex.Message);
                return 0;
            }
        }

        /// <summary>
        /// Restore archived member by ID
        /// </summary>
        /// <param name="memberId">ID of the member to restore</param>
        /// <returns>True if member was restored successfully</returns>
        public static bool RestoreMemberById(string memberId)
        {
            try
            {
                var whereParameters = new Dictionary<string, object>
                    {
                        { "@id", memberId }
                    };

                var updateData = new Dictionary<string, object>
                    {
                        { "deleted_at", null },
                        { "is_deleted", 0 }
                    };

                int rowsAffected = membersCrud.Update(updateData, "id = @id AND is_deleted = 1", whereParameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error restoring member: " + ex.Message);
                return false;
            }
        }

        // EXISTING METHODS (Updated to use new search functionality)

        public static List<Members> GetAllMembersFromDatabase()
        {
            List<Members> membersList = new List<Members>();

            try
            {
                DataTable dataTable = membersCrud.ReadActive();
                membersList = ConvertDataTableToMembersList(dataTable);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error reading members from database: " + ex.Message);
            }

            return membersList;
        }

        public static List<Members> GetMembersByDepartment(string department)
        {
            return SearchMembersByDepartment(department);
        }

        public static Members GetMemberById(string id)
        {
            var members = SearchMembersById(id);
            return members.Count > 0 ? members[0] : null;
        }

        public static List<Members> GetArchivedMembers()
        {
            List<Members> membersList = new List<Members>();

            try
            {
                DataTable dataTable = membersCrud.ReadArchived();
                membersList = ConvertDataTableToMembersList(dataTable);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error reading archived members from database: " + ex.Message);
            }

            return membersList;
        }

        // HELPER METHODS

        /// <summary>
        /// Convert DataTable to List of Members
        /// </summary>
        /// <param name="dataTable">DataTable to convert</param>
        /// <returns>List of Members objects</returns>
        private static List<Members> ConvertDataTableToMembersList(DataTable dataTable)
        {
            List<Members> membersList = new List<Members>();

            if (dataTable == null)
                return membersList;

            foreach (DataRow row in dataTable.Rows)
            {
                Members member = new Members();

                // id -> MemberId
                if (row.Table.Columns.Contains("id") && row["id"] != DBNull.Value)
                {
                    int parsedId;
                    try
                    {
                        parsedId = Convert.ToInt32(row["id"]);
                    }
                    catch
                    {
                        parsedId = 0;
                    }
                    member.MemberId = parsedId;
                }

                if (row.Table.Columns.Contains("student_id") && row["student_id"] != DBNull.Value)
                    member.StudentId = row["student_id"].ToString();

                if (row.Table.Columns.Contains("first_name") && row["first_name"] != DBNull.Value)
                    member.FirstName = row["first_name"].ToString();

                if (row.Table.Columns.Contains("last_name") && row["last_name"] != DBNull.Value)
                    member.LastName = row["last_name"].ToString();

                if (row.Table.Columns.Contains("email") && row["email"] != DBNull.Value)
                    member.Email = row["email"].ToString();

                if (row.Table.Columns.Contains("section") && row["section"] != DBNull.Value)
                    member.Section = row["section"].ToString();

                if (row.Table.Columns.Contains("department") && row["department"] != DBNull.Value)
                    member.Department = row["department"].ToString();

                if (row.Table.Columns.Contains("role") && row["role"] != DBNull.Value)
                    member.Role = row["role"].ToString();

                if (row.Table.Columns.Contains("username") && row["username"] != DBNull.Value)
                    member.Username = row["username"].ToString();

                if (row.Table.Columns.Contains("gender") && row["gender"] != DBNull.Value)
                    member.Gender = row["gender"].ToString();

                if (row.Table.Columns.Contains("birthday") && row["birthday"] != DBNull.Value)
                {
                    DateTime dob;
                    if (DateTime.TryParse(row["birthday"].ToString(), out dob))
                        member.Birthday = dob;
                    else
                        member.Birthday = null;
                }

                if (row.Table.Columns.Contains("profile_image_path") && row["profile_image_path"] != DBNull.Value)
                    member.ProfileImagePath = row["profile_image_path"].ToString();

                membersList.Add(member);
            }

            return membersList;
        }

        /// <summary>
        /// Initialize archiving columns for members table if they don't exist
        /// </summary>
        /// <returns>True if columns exist or were added successfully</returns>
        public static bool InitializeArchiving()
        {
            try
            {
                return membersCrud.AddArchivingColumns();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error initializing archiving columns: " + ex.Message);
                return false;
            }
        }

        private static bool IsValidMember(Members member)
        {
            if (member == null) return false;

            if (string.IsNullOrWhiteSpace(member.FirstName) ||
                string.IsNullOrWhiteSpace(member.LastName) ||
                string.IsNullOrWhiteSpace(member.Email)
               )
            {
                return false;
            }

            if (member.Email == null) return false;

            if (!member.Email.Contains("@") || !member.Email.Contains("."))
            {
                return false;
            }

            return true;
        }
    }
}