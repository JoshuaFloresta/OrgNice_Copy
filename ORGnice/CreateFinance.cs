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
    public partial class CreateFinance : Form
    {
  private readonly Action _refreshFinances;

  public CreateFinance(Action refreshFinances)
        {
   InitializeComponent();
 _refreshFinances = refreshFinances;
   this.StartPosition = FormStartPosition.CenterScreen;

   // Wire up event handlers
    this.btnSubmit.Click += btnSubmit_Click;
this.btnClear.Click += btnClear_Click;
    this.btnClose.Click += btnClose_Click;
       this.cbDepartment.SelectedIndexChanged += cbDepartment_SelectedIndexChanged;
      this.txtTargetAmount.TextChanged += txtTargetAmount_TextChanged;

   // Load departments into combo box
          LoadDepartments();

            // Set default values
      cbCurrency.SelectedIndex = 0; // Default to PHP
        cbPaymentStatus.SelectedIndex = 0; // Default to InProgress
   cbMemberStatus.SelectedIndex = 0; // Default to Not Paid

      // Hide collected amount and expenses controls as they will be auto-calculated
   if (txtCollectedAmount != null && label5 != null)
   {
       txtCollectedAmount.Visible = false;
        label5.Visible = false;
    }

    // Hide expenses amount controls for now
        if (txtExpensesAmount != null && label6 != null)
  {
      txtExpensesAmount.Visible = false;
       label6.Visible = false;
            }

     // Add information label
    AddInformationLabel();
        }

  private void AddInformationLabel()
   {
try
    {
       // Create an information panel with creation-specific styling
   Panel infoPanel = new Panel();
       infoPanel.BackColor = Color.LightGreen; // Different color to show it's for creation
   infoPanel.BorderStyle = BorderStyle.FixedSingle;
      infoPanel.Size = new Size(400, 100);
 infoPanel.Location = new Point(20, 350);

       Label infoLabel = new Label();
  infoLabel.Text = "?? CREATING NEW FINANCE GOAL:\n" +
            "• Select department to see member count\n" +
         "• Target amount will be divided equally among all department members\n" +
       "• Individual payment tracking starts automatically after creation\n" +
   "• Members will be shown in Finance Management tab for payment updates";
            infoLabel.ForeColor = Color.DarkGreen;
    infoLabel.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
        infoLabel.AutoSize = false;
       infoLabel.Size = new Size(380, 90);
    infoLabel.Location = new Point(10, 5);

  infoPanel.Controls.Add(infoLabel);
      this.Controls.Add(infoPanel);

    // Add creation step indicator
     Label stepLabel = new Label();
    stepLabel.Text = "STEP 1: Create Goal ? STEP 2: Manage Payments in Finance Tab";
    stepLabel.ForeColor = Color.Blue;
  stepLabel.Font = new Font("Microsoft Sans Serif", 8F, FontStyle.Italic);
         stepLabel.AutoSize = true;
 stepLabel.Location = new Point(20, 460);
         this.Controls.Add(stepLabel);
            }
          catch
  {
      // If adding info panel fails, continue without it
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

        private void cbDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
       UpdateMemberCountInfo();
   }

   private void txtTargetAmount_TextChanged(object sender, EventArgs e)
      {
     UpdateMemberCountInfo();
        }

        private void UpdateMemberCountInfo()
        {
     try
     {
  if (cbDepartment.SelectedItem == null) return;

      string selectedDepartment = cbDepartment.SelectedItem.ToString();
      var departmentMembers = Members.SearchMembersByDepartment(selectedDepartment, false);
      int memberCount = departmentMembers.Count;

   // Update label to show member count and individual amount
      decimal targetAmount = 0;
       decimal.TryParse(txtTargetAmount.Text, out targetAmount);

        decimal individualAmount = memberCount > 0 ? targetAmount / memberCount : 0;

// Find or create member info label
      Label memberInfoLabel = this.Controls.OfType<Label>().FirstOrDefault(l => l.Name == "lblMemberInfo");
     if (memberInfoLabel == null)
            {
      memberInfoLabel = new Label();
    memberInfoLabel.Name = "lblMemberInfo";
          memberInfoLabel.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
   memberInfoLabel.ForeColor = Color.DarkGreen;
      memberInfoLabel.AutoSize = false;
  memberInfoLabel.Size = new Size(350, 40);
      memberInfoLabel.Location = new Point(150, 270);
            this.Controls.Add(memberInfoLabel);
       }

 if (memberCount > 0)
    {
    memberInfoLabel.Text = $"?? {memberCount} members in {selectedDepartment}\n" +
 $"?? Individual amount: {individualAmount:C} per member";
 memberInfoLabel.ForeColor = Color.DarkGreen;
     }
      else
     {
     memberInfoLabel.Text = "?? No members found in selected department";
      memberInfoLabel.ForeColor = Color.Red;
}
   }
        catch (Exception ex)
            {
   System.Diagnostics.Debug.WriteLine($"Error updating member count info: {ex.Message}");
          }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
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

      if (cbDepartment.SelectedItem == null)
    {
 MessageBox.Show("Please select a department.", "Validation Error",
       MessageBoxButtons.OK, MessageBoxIcon.Warning);
        cbDepartment.Focus();
       return;
        }

      // Parse numeric values
      decimal targetAmount = 0;
     if (!decimal.TryParse(txtTargetAmount.Text, out targetAmount) || targetAmount <= 0)
      {
   MessageBox.Show("Please enter a valid target amount (greater than 0).", "Validation Error",
MessageBoxButtons.OK, MessageBoxIcon.Warning);
      txtTargetAmount.Focus();
   return;
     }

 // Check if department has members
       string selectedDepartment = cbDepartment.SelectedItem.ToString();
      var departmentMembers = Members.SearchMembersByDepartment(selectedDepartment, false);
            if (departmentMembers.Count == 0)
    {
MessageBox.Show($"No members found in department '{selectedDepartment}'. Please select a different department or add members first.", 
   "No Members Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
         return;
     }

      // Show confirmation with calculation details
      decimal individualAmount = targetAmount / departmentMembers.Count;
       string confirmMessage = $"Create Finance Goal:\n\n" +
     $"Goal Name: {txtGoalName.Text}\n" +
      $"Department: {selectedDepartment}\n" +
     $"Target Amount: {targetAmount:C}\n" +
            $"Members: {departmentMembers.Count}\n" +
       $"Individual Amount: {individualAmount:C} per member\n\n" +
   $"Do you want to create this goal?";

       DialogResult result = MessageBox.Show(confirmMessage, "Confirm Finance Goal Creation", 
          MessageBoxButtons.YesNo, MessageBoxIcon.Question);

    if (result != DialogResult.Yes) return;

        // Create finance object with 0 collected amount (will be calculated based on payments)
     var finance = new FinanceClass(
      eventId: 1, // Default event ID
   goalName: txtGoalName.Text.Trim(),
       description: BuildDescription(selectedDepartment),
       targetAmount: targetAmount,
      collectedAmount: 0, // Start with 0, will be calculated from member payments
    expensesAmount: 0, // Not implemented yet
   currency: cbCurrency.Text,
    dueDate: dtDueDate.Checked ? (DateTime?)dtDueDate.Value.Date : null,
      paymentStatus: "InProgress", // Always start as InProgress
        memberStatus: "Not Paid" // Default member status
    );

      // Save to database
    bool success = FinanceClass.AddFinanceToDatabase(finance);

        if (success)
  {
   System.Diagnostics.Debug.WriteLine("btnSubmit_Click: Finance goal created successfully");
    
   // EMERGENCY: Force immediate refresh of parent FinanceTab
    if (this.Owner is Form parentForm)
     {
  var financeTab = parentForm.Controls.OfType<FinanceTab>().FirstOrDefault();
       if (financeTab != null)
   {
        System.Diagnostics.Debug.WriteLine("EMERGENCY: Found FinanceTab, forcing refresh");
      financeTab.LoadFinanceGoals();
 financeTab.RefreshFinanceData();
      }
 }
    
    MessageBox.Show($"Finance goal created successfully!\n\n" +
      $"• Target amount of {targetAmount:C} has been divided among {departmentMembers.Count} members\n" +
        $"• Each member needs to pay {individualAmount:C}\n" +
       $"• You can now track individual payments in the Finance Management tab", 
         "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

   // Call refresh callback before closing
      System.Diagnostics.Debug.WriteLine("btnSubmit_Click: Calling refresh callback");
    _refreshFinances?.Invoke();
     
     System.Diagnostics.Debug.WriteLine("btnSubmit_Click: Clearing form and closing");
     ClearFormFields();
        this.Close();
   }
        else
   {
       System.Diagnostics.Debug.WriteLine("btnSubmit_Click: Failed to create finance goal");
    MessageBox.Show("Failed to create finance goal. Please check your input and try again.",
      "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
}
   }
       catch (Exception ex)
      {
      MessageBox.Show($"An error occurred: {ex.Message}", "Error",
     MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
        }

   private string BuildDescription(string department)
        {
  string description = txtDescription.Text.Trim();

 // Always include department information
      string departmentInfo = $"Department: {department}";

     if (string.IsNullOrWhiteSpace(description))
{
   return departmentInfo;
      }
   else if (!description.Contains(department))
            {
       return $"{description} | {departmentInfo}";
          }
        else
        {
       return description;
   }
     }

     private void btnClear_Click(object sender, EventArgs e)
        {
 ClearFormFields();
        }

     private void btnClose_Click(object sender, EventArgs e)
        {
   this.Close();
        }

        private void ClearFormFields()
        {
     txtGoalName.Clear();
      txtDescription.Clear();
      txtTargetAmount.Clear();

  cbDepartment.SelectedIndex = -1;
   cbCurrency.SelectedIndex = 0; // Default to PHP
            cbPaymentStatus.SelectedIndex = 0; // Default to InProgress
  cbMemberStatus.SelectedIndex = 0; // Default to Not Paid

          dtDueDate.Checked = false;
      dtDueDate.Value = DateTime.Today;

// Clear member info label
      Label memberInfoLabel = this.Controls.OfType<Label>().FirstOrDefault(l => l.Name == "lblMemberInfo");
       if (memberInfoLabel != null)
        {
     memberInfoLabel.Text = "";
      }
      }
  }
}