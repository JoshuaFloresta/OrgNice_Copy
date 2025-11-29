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
    public partial class FinanceDetail : Form
    {
        private readonly int _goalId;
 private readonly Action _refreshFinances;
        private FinanceClass _currentFinance;

  public FinanceDetail(int goalId, Action refreshFinances)
        {
 InitializeComponent();
   _goalId = goalId;
    _refreshFinances = refreshFinances;
      this.StartPosition = FormStartPosition.CenterScreen;

            // Wire up event handlers
            this.btnUpdate.Click += btnUpdate_Click;
    this.btnArchive.Click += btnArchive_Click;
       this.btnClose.Click += btnClose_Click;

      // Load departments into combo box
            LoadDepartments();

      // Load finance data
    LoadFinanceData();

            // Hide collected amount and expenses controls as they are auto-calculated
 HideAutoCalculatedControls();
        }

        private void HideAutoCalculatedControls()
        {
     try
            {
       // Add editing context information at the top
      Label editingContextLabel = new Label();
   editingContextLabel.Text = "?? EDITING EXISTING FINANCE GOAL - Real-time data from database";
     editingContextLabel.ForeColor = Color.DarkBlue;
      editingContextLabel.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
         editingContextLabel.AutoSize = false;
     editingContextLabel.Size = new Size(600, 25);
      editingContextLabel.Location = new Point(30, 55);
    this.Controls.Add(editingContextLabel);

      // Hide collected amount controls (auto-calculated from member payments)
          if (txtCollectedAmount != null && label5 != null)
       {
   txtCollectedAmount.Visible = false;
    label5.Visible = false;
  }

      // Hide expenses amount controls (not implemented yet)
      if (txtExpensesAmount != null && label6 != null)
      {
        txtExpensesAmount.Visible = false;
      label6.Visible = false;
      }

        // Add a new button to view member payments
       Button btnViewPayments = new Button();
    btnViewPayments.Name = "btnViewMemberPayments";
       btnViewPayments.Text = "?? View Member Payments";
        btnViewPayments.Size = new Size(180, 35);
     btnViewPayments.Location = new Point(250, 330);
   btnViewPayments.BackColor = Color.FromArgb(40, 167, 69);
    btnViewPayments.ForeColor = Color.White;
     btnViewPayments.FlatStyle = FlatStyle.Flat;
        btnViewPayments.FlatAppearance.BorderSize = 0;
      btnViewPayments.Click += btnViewMemberPayments_Click;
      this.Controls.Add(btnViewPayments);

    // Add information label specific to editing
   Label lblInfo = new Label();
        lblInfo.Name = "lblPaymentInfo";
    lblInfo.Text = "?? EDITING MODE:\n" +
    "• Collected amount shows real-time member payments\n" +
        "• Use 'View Member Payments' for detailed payment management\n" +
         "• Changes to target amount will reset all member payment statuses";
      lblInfo.ForeColor = Color.Purple;
        lblInfo.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular);
         lblInfo.AutoSize = false;
lblInfo.Size = new Size(400, 60);
   lblInfo.Location = new Point(150, 260);
   this.Controls.Add(lblInfo);
     }
    catch (Exception ex)
      {
System.Diagnostics.Debug.WriteLine($"Error hiding auto-calculated controls: {ex.Message}");
   }
        }

        private void LoadDepartments()
{
            try
    {
   // Get unique departments from members table
     var members = Members.GetAllMembersFromDatabase();
    var departments = members.Select(m => m.Department)
  .Where(d => !string.IsNullOrWhiteSpace(d))
      .Distinct()
         .OrderBy(d => d)
    .ToList();

      cbDepartment.Items.Clear();
      foreach (string dept in departments)
    {
            cbDepartment.Items.Add(dept);
             }

                // Add default departments if none exist
         if (cbDepartment.Items.Count == 0)
     {
    cbDepartment.Items.AddRange(new string[]
      {
      "Singing Group",
     "Dance Group",
  "Band Group"
          });
                }
   }
       catch (Exception ex)
            {
                // Add fallback departments
           cbDepartment.Items.AddRange(new string[]
        {
   "Singing Group",
 "Dance Group",
    "Band Group"
});
         System.Diagnostics.Debug.WriteLine($"Error loading departments: {ex.Message}");
            }
     }

        private void LoadFinanceData()
        {
            try
   {
     _currentFinance = FinanceClass.GetFinanceById(_goalId.ToString());

    if (_currentFinance == null)
  {
          MessageBox.Show("Finance record not found.", "Error",
      MessageBoxButtons.OK, MessageBoxIcon.Error);
        this.Close();
   return;
          }

    // Populate form fields
      txtGoalName.Text = _currentFinance.GoalName;
      txtDescription.Text = _currentFinance.Description;
                txtTargetAmount.Text = _currentFinance.TargetAmount.ToString("F2");

   // Show calculated collected amount (read-only)
  ShowCalculatedAmounts();

                // Set combo box values
  cbCurrency.Text = _currentFinance.Currency ?? "PHP";
          cbPaymentStatus.Text = _currentFinance.PaymentStatus ?? "InProgress";
         cbMemberStatus.Text = _currentFinance.MemberStatus ?? "Not Paid";

             // Set due date
                if (_currentFinance.DueDate.HasValue)
          {
      dtDueDate.Checked = true;
  dtDueDate.Value = _currentFinance.DueDate.Value;
         }
         else
       {
            dtDueDate.Checked = false;
       }

      // Try to extract department from description (if available)
          ExtractDepartmentFromDescription();

              // Show member count information
            ShowDepartmentInfo();
  }
            catch (Exception ex)
          {
       MessageBox.Show($"Error loading finance data: {ex.Message}", "Error",
   MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowCalculatedAmounts()
      {
     try
  {
    // Find or create labels to show calculated amounts
            Label lblCalculatedInfo = this.Controls.OfType<Label>().FirstOrDefault(l => l.Name == "lblCalculatedInfo");
     if (lblCalculatedInfo == null)
    {
      lblCalculatedInfo = new Label();
   lblCalculatedInfo.Name = "lblCalculatedInfo";
  lblCalculatedInfo.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
     lblCalculatedInfo.ForeColor = Color.DarkGreen;
     lblCalculatedInfo.AutoSize = false;
       lblCalculatedInfo.Size = new Size(300, 50);
   lblCalculatedInfo.Location = new Point(380, 187);
       this.Controls.Add(lblCalculatedInfo);
 }

      lblCalculatedInfo.Text = $"?? Collected Amount: {_currentFinance.CollectedAmount:C}\n" +
        $"?? Progress: {(_currentFinance.TargetAmount > 0 ? (_currentFinance.CollectedAmount / _currentFinance.TargetAmount * 100) : 0):F1}%";
     }
            catch (Exception ex)
            {
  System.Diagnostics.Debug.WriteLine($"Error showing calculated amounts: {ex.Message}");
 }
        }

      private void ShowDepartmentInfo()
    {
      try
            {
     string department = ExtractDepartmentFromDescription();
        if (!string.IsNullOrEmpty(department))
         {
  var departmentMembers = Members.SearchMembersByDepartment(department, false);
    int memberCount = departmentMembers.Count;
         decimal individualAmount = memberCount > 0 ? _currentFinance.TargetAmount / memberCount : 0;

        Label lblDeptInfo = this.Controls.OfType<Label>().FirstOrDefault(l => l.Name == "lblDeptInfo");
       if (lblDeptInfo == null)
      {
             lblDeptInfo = new Label();
lblDeptInfo.Name = "lblDeptInfo";
   lblDeptInfo.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular);
               lblDeptInfo.ForeColor = Color.Blue;
         lblDeptInfo.AutoSize = false;
       lblDeptInfo.Size = new Size(300, 40);
    lblDeptInfo.Location = new Point(380, 107);
  this.Controls.Add(lblDeptInfo);
       }

        lblDeptInfo.Text = $"?? {memberCount} members in {department}\n?? {individualAmount:C} per member";
    }
       }
            catch (Exception ex)
            {
           System.Diagnostics.Debug.WriteLine($"Error showing department info: {ex.Message}");
  }
        }

  private string ExtractDepartmentFromDescription()
        {
  if (!string.IsNullOrWhiteSpace(_currentFinance.Description) && _currentFinance.Description.Contains("Department:"))
            {
   string desc = _currentFinance.Description;
        int startIndex = desc.IndexOf("Department:") + "Department:".Length;
      int endIndex = desc.IndexOf("|", startIndex);
      if (endIndex == -1) endIndex = desc.Length;

     string department = desc.Substring(startIndex, endIndex - startIndex).Trim();

    foreach (string item in cbDepartment.Items)
     {
              if (item.Equals(department, StringComparison.OrdinalIgnoreCase))
    {
  cbDepartment.Text = item;
     return department;
  }
   }
         return department;
            }
          return string.Empty;
   }

        private void btnUpdate_Click(object sender, EventArgs e)
      {
     try
     {
        // Validate required fields
      if (string.IsNullOrWhiteSpace(txtGoalName.Text))
  {
  MessageBox.Show("Goal name is required.", "Validation Error",
      MessageBoxButtons.OK, MessageBoxIcon.Warning);
        txtGoalName.Focus();
        return;
     }

      // Parse numeric values
        decimal targetAmount = 0;
            if (!decimal.TryParse(txtTargetAmount.Text, out targetAmount) || targetAmount < 0)
       {
      MessageBox.Show("Please enter a valid target amount.", "Validation Error",
                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
           txtTargetAmount.Focus();
        return;
         }

           // Check if target amount changed
        bool targetAmountChanged = targetAmount != _currentFinance.TargetAmount;
      if (targetAmountChanged)
      {
     string department = ExtractDepartmentFromDescription();
            if (!string.IsNullOrEmpty(department))
         {
              var departmentMembers = Members.SearchMembersByDepartment(department, false);
       decimal newIndividualAmount = departmentMembers.Count > 0 ? targetAmount / departmentMembers.Count : 0;

   DialogResult result = MessageBox.Show(
              $"Changing the target amount will affect individual member amounts:\n\n" +
               $"New individual amount: {newIndividualAmount:C} per member\n" +
     $"This will reset all member payment statuses to 'Not Paid'.\n\n" +
       $"Do you want to continue?", "Target Amount Change",
     MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

              if (result != DialogResult.Yes) return;

    // Reset member payments if target amount changed
     ResetMemberPayments(_goalId, departmentMembers);
           }
      }

        // Update finance record (collected amount will be recalculated)
  bool success = FinanceClass.UpdateFinanceById(
           _goalId.ToString(),
            goalName: txtGoalName.Text.Trim(),
       description: GetDescriptionWithDepartment(),
         targetAmount: targetAmount,
                collectedAmount: targetAmountChanged ? 0 : _currentFinance.CollectedAmount, // Reset if target changed
 memberStatus: cbMemberStatus.Text,
        dueDate: dtDueDate.Checked ? (DateTime?)dtDueDate.Value.Date : null
       );

if (success)
                {
           MessageBox.Show("Finance record updated successfully!", "Success",
   MessageBoxButtons.OK, MessageBoxIcon.Information);
         _refreshFinances?.Invoke();
        this.Close();
                }
            else
       {
             MessageBox.Show("Failed to update finance record. Please try again.",
     "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
   }
         catch (Exception ex)
  {
       MessageBox.Show($"An error occurred: {ex.Message}", "Error",
        MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
     }

        private void ResetMemberPayments(int goalId, List<Members> members)
        {
    try
        {
     string connectionString = "server=localhost;port=3306;database=orgdb;uid=root;pwd=legorocket3368.;";

          using (var connection = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
                {
        connection.Open();

             // Reset all member payments for this goal
    string resetSql = "UPDATE member_payments SET payment_status = 'Not Paid', paid_amount = 0 WHERE goal_id = @goalId";
  using (var cmd = new MySql.Data.MySqlClient.MySqlCommand(resetSql, connection))
          {
 cmd.Parameters.AddWithValue("@goalId", goalId);
          cmd.ExecuteNonQuery();
           }
         }

     System.Diagnostics.Debug.WriteLine($"Reset member payments for goal {goalId}");
  }
   catch (Exception ex)
            {
System.Diagnostics.Debug.WriteLine($"Error resetting member payments: {ex.Message}");
 }
    }

        private string GetDescriptionWithDepartment()
        {
          string description = txtDescription.Text.Trim();
   string department = cbDepartment.Text;

    // Remove existing department info from description
            if (description.Contains("Department:"))
        {
                int startIndex = description.IndexOf("Department:");
    int endIndex = description.IndexOf("|", startIndex);
     if (endIndex != -1)
   {
      description = description.Substring(0, startIndex) + description.Substring(endIndex + 1);
      }
          else
                {
 description = description.Substring(0, startIndex);
    }
                description = description.Trim().TrimEnd('|').Trim();
        }

            // Add new department info
            if (!string.IsNullOrWhiteSpace(department))
      {
                if (string.IsNullOrWhiteSpace(description))
         {
              description = $"Department: {department}";
       }
 else
    {
     description += $" | Department: {department}";
        }
  }

            return description;
  }

        private void btnViewMemberPayments_Click(object sender, EventArgs e)
        {
        try
  {
              // Close this form and let user go to Finance tab manually
MessageBox.Show("Please go to the Finance Management tab to view and manage member payments for this goal.", 
            "Member Payments", MessageBoxButtons.OK, MessageBoxIcon.Information);
     this.Close();
   }
       catch (Exception ex)
       {
          MessageBox.Show($"Error opening member payments: {ex.Message}", "Error", 
 MessageBoxButtons.OK, MessageBoxIcon.Error);
       }
        }

        private void btnArchive_Click(object sender, EventArgs e)
        {
      try
{
      DialogResult result = MessageBox.Show(
     "Are you sure you want to archive this finance record? This action will move it to the archive.",
                  "Confirm Archive",
      MessageBoxButtons.YesNo,
     MessageBoxIcon.Question);

     if (result == DialogResult.Yes)
                {
              bool success = FinanceClass.ArchiveFinanceById(_goalId.ToString());

     if (success)
       {
        MessageBox.Show("Finance record archived successfully!", "Success",
   MessageBoxButtons.OK, MessageBoxIcon.Information);
         _refreshFinances?.Invoke();
      this.Close();
       }
               else
           {
            MessageBox.Show("Failed to archive finance record. Please try again.",
  "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
     }
    }
     }
   catch (Exception ex)
       {
          MessageBox.Show($"An error occurred: {ex.Message}", "Error",
         MessageBoxButtons.OK, MessageBoxIcon.Error);
       }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
      this.Close();
        }
    }
}