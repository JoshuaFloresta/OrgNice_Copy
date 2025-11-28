using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

namespace ORGnice
{
    public class Events
    {
        public int EventId { get; set; }                 // event_id
        public string EventName { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string Department { get; set; }           // participants from members.department
        public int? OrganizerMemberId { get; set; }      // FK to members
        public string Venue { get; set; }
        public string Status { get; set; }               // Planned/Ongoing/Completed/Cancelled
        public string Notes { get; set; }
        public bool IsArchived { get; set; }
        public DateTime? ArchivedAt { get; set; }

        private static Crud eventsCrud = new Crud("events");

        public Events() { }

        public Events(string eventName,
                      DateTime start,
                      DateTime end,
                      string department,
                      int? organizerMemberId,
                      string venue,
                      string status,
                      string notes)
        {
            EventName = eventName;
            StartDateTime = start;
            EndDateTime = end;
            Department = department;
            OrganizerMemberId = organizerMemberId;
            Venue = venue;
            Status = status;
            Notes = notes;
        }

        // ADD

        public static bool AddEventToDatabase(Events ev)
        {
            if (!IsValidEvent(ev))
                return false;

            string connectionString =
                "server=localhost;port=3306;database=orgdb;uid=root;pwd=Joshua@2004;";

            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string sql = @"
                    INSERT INTO events
                        (event_name, start_datetime, end_datetime,
                         department, organizer_member_id, venue,
                         status, notes)
                    VALUES
                        (@name, @start, @end,
                         @department, @organizerId, @venue,
                         @status, @notes);";

                    using (var cmd = new MySqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@name", ev.EventName);
                        cmd.Parameters.AddWithValue("@start", ev.StartDateTime);
                        cmd.Parameters.AddWithValue("@end", ev.EndDateTime);
                        cmd.Parameters.AddWithValue("@department", ev.Department);
                        cmd.Parameters.AddWithValue("@organizerId",
                            ev.OrganizerMemberId.HasValue ? (object)ev.OrganizerMemberId.Value : DBNull.Value);
                        cmd.Parameters.AddWithValue("@venue",
                            string.IsNullOrWhiteSpace(ev.Venue) ? (object)DBNull.Value : ev.Venue);
                        cmd.Parameters.AddWithValue("@status",
                            string.IsNullOrWhiteSpace(ev.Status) ? "Planned" : ev.Status);
                        cmd.Parameters.AddWithValue("@notes",
                            string.IsNullOrWhiteSpace(ev.Notes) ? (object)DBNull.Value : ev.Notes);

                        int rows = cmd.ExecuteNonQuery();
                        return rows > 0;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error adding event: " + ex.Message);
                    return false;
                }
            }
        }

        // SEARCH METHODS

        public static List<Events> SearchEventsByName(string name, bool includeArchived = false)
        {
            List<Events> list = new List<Events>();
            try
            {
                DataTable dt = eventsCrud.SearchByName(name, includeArchived);
                list = ConvertDataTableToEventsList(dt);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error searching events by name: " + ex.Message);
            }
            return list;
        }

        public static List<Events> SearchEventsByDepartment(string department, bool includeArchived = false)
        {
            List<Events> list = new List<Events>();
            try
            {
                DataTable dt = eventsCrud.SearchByDepartment(department, includeArchived);
                list = ConvertDataTableToEventsList(dt);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error searching events by department: " + ex.Message);
            }
            return list;
        }

        public static Events GetEventById(string id)
        {
            List<Events> list = new List<Events>();
            try
            {
                DataTable dt = eventsCrud.SearchById(id, "event_id", includeArchived: true);
                list = ConvertDataTableToEventsList(dt);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error getting event by id: " + ex.Message);
            }
            return list.Count > 0 ? list[0] : null;
        }

        public static List<Events> GetAllActiveEvents()
        {
            List<Events> list = new List<Events>();
            try
            {
                DataTable dt = eventsCrud.ReadActive();
                list = ConvertDataTableToEventsList(dt);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error reading active events: " + ex.Message);
            }
            return list;
        }

        public static List<Events> GetArchivedEvents()
        {
            List<Events> list = new List<Events>();
            try
            {
                DataTable dt = eventsCrud.ReadArchived();
                list = ConvertDataTableToEventsList(dt);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error reading archived events: " + ex.Message);
            }
            return list;
        }

        // UPDATE

        public static bool UpdateEventById(string eventId,
            string eventName = null,
            DateTime? start = null,
            DateTime? end = null,
            string department = null,
            int? organizerMemberId = null,
            string venue = null,
            string status = null,
            string notes = null)
        {
            try
            {
                var updateData = new Dictionary<string, object>();

                if (!string.IsNullOrWhiteSpace(eventName))
                    updateData.Add("event_name", eventName);
                if (start.HasValue)
                    updateData.Add("start_datetime", start.Value);
                if (end.HasValue)
                    updateData.Add("end_datetime", end.Value);
                if (!string.IsNullOrWhiteSpace(department))
                    updateData.Add("department", department);
                if (organizerMemberId.HasValue)
                    updateData.Add("organizer_member_id", organizerMemberId.Value);
                if (!string.IsNullOrWhiteSpace(venue))
                    updateData.Add("venue", venue);
                if (!string.IsNullOrWhiteSpace(status))
                    updateData.Add("status", status);
                if (notes != null)
                    updateData.Add("notes", string.IsNullOrWhiteSpace(notes) ? (object)DBNull.Value : notes);

                if (updateData.Count == 0)
                    return false;

                int rows = eventsCrud.UpdateById(updateData, "event_id", eventId);
                return rows > 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error updating event: " + ex.Message);
                return false;
            }
        }

        // ARCHIVE / RESTORE

        public static bool ArchiveEventById(string eventId)
        {
            try
            {
                int rows = eventsCrud.DeleteById("event_id", eventId); // your Crud should mark is_archived=1
                return rows > 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error archiving event: " + ex.Message);
                return false;
            }
        }

        public static bool RestoreEventById(string eventId)
        {
            try
            {
                var whereParameters = new Dictionary<string, object>
                {
                    { "@id", eventId }
                };

                var updateData = new Dictionary<string, object>
                {
                    { "is_archived", 0 },
                    { "archived_at", null }
                };

                int rows = eventsCrud.Update(updateData, "event_id = @id AND is_archived = 1", whereParameters);
                return rows > 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error restoring event: " + ex.Message);
                return false;
            }
        }

        // HELPERS

        private static List<Events> ConvertDataTableToEventsList(DataTable table)
        {
            List<Events> list = new List<Events>();
            if (table == null) return list;

            foreach (DataRow row in table.Rows)
            {
                var ev = new Events();

                if (table.Columns.Contains("event_id") && row["event_id"] != DBNull.Value)
                    ev.EventId = Convert.ToInt32(row["event_id"]);

                if (table.Columns.Contains("event_name") && row["event_name"] != DBNull.Value)
                    ev.EventName = row["event_name"].ToString();

                if (table.Columns.Contains("start_datetime") && row["start_datetime"] != DBNull.Value)
                    ev.StartDateTime = Convert.ToDateTime(row["start_datetime"]);

                if (table.Columns.Contains("end_datetime") && row["end_datetime"] != DBNull.Value)
                    ev.EndDateTime = Convert.ToDateTime(row["end_datetime"]);

                if (table.Columns.Contains("department") && row["department"] != DBNull.Value)
                    ev.Department = row["department"].ToString();

                if (table.Columns.Contains("organizer_member_id") && row["organizer_member_id"] != DBNull.Value)
                    ev.OrganizerMemberId = Convert.ToInt32(row["organizer_member_id"]);

                if (table.Columns.Contains("venue") && row["venue"] != DBNull.Value)
                    ev.Venue = row["venue"].ToString();

                if (table.Columns.Contains("status") && row["status"] != DBNull.Value)
                    ev.Status = row["status"].ToString();

                if (table.Columns.Contains("notes") && row["notes"] != DBNull.Value)
                    ev.Notes = row["notes"].ToString();

                if (table.Columns.Contains("is_archived") && row["is_archived"] != DBNull.Value)
                    ev.IsArchived = Convert.ToBoolean(row["is_archived"]);

                if (table.Columns.Contains("archived_at") && row["archived_at"] != DBNull.Value)
                    ev.ArchivedAt = Convert.ToDateTime(row["archived_at"]);

                list.Add(ev);
            }

            return list;
        }

        public static bool InitializeArchiving()
        {
            try
            {
                return eventsCrud.AddArchivingColumns();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error initializing event archiving: " + ex.Message);
                return false;
            }
        }

        private static bool IsValidEvent(Events ev)
        {
            if (ev == null) return false;
            if (string.IsNullOrWhiteSpace(ev.EventName)) return false;
            if (ev.EndDateTime < ev.StartDateTime) return false;
            return true;
        }
    }
}
