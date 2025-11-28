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
    public partial class CreateMembers : Form
    {
        private readonly Action _refreshMembers;

        public CreateMembers(Action refreshMembers)
        {
            InitializeComponent();
            _refreshMembers = refreshMembers;
            cbRole.SelectedIndexChanged += cbRole_SelectedIndexChanged;
            this.StartPosition = FormStartPosition.CenterScreen;

            // Ensure buttons are wired (Designer may not have set these)
            this.roundedButton1.Click += roundedButton1_Click;
            this.roundedButton2.Click += RoundedButton2_Click;

            //check if theres an image upload label to hide
            if (imageUploadLbl != null)
            {
                imageUploadLbl.Visible = false;
            }
        }

        private void cbRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Adjust the comparison string to exactly match your "Admin" item text
            bool isAdmin = string.Equals(cbRole.Text, "Admin", StringComparison.OrdinalIgnoreCase);

            lblPassword.Visible = isAdmin;
            txtPassword.Visible = isAdmin;

            if (!isAdmin)
            {
                txtPassword.Clear(); // optional: clear when not admin
            }
        }

            
        private void roundedButton1_Click(object sender, EventArgs e)
        {
            // Collect input values
            string firstname = txtFirstName.Text.Trim();
            string lastname = txtLastName.Text.Trim();
            string section = txtSection.Text.Trim();
            string email = txtEmail.Text.Trim();
            string department = txtDepartment.Text;
            string role = cbRole.Text;

            string studentNo = string.Empty;
            if (textBox2 != null) studentNo = textBox2.Text.Trim();
            string username = string.Empty;
            if (textBox1 != null) username = textBox1.Text.Trim();

            string gender = null;
            if (MaleRadio != null && MaleRadio.Checked) gender = "Male";
            else if (FemRadio != null && FemRadio.Checked) gender = "Female";
            else if (OthersRadio != null && OthersRadio.Checked) gender = "Other";

            DateTime? birthday = null;
            if (dateTimePicker1 != null)
            {
                // Use the selected date
                birthday = dateTimePicker1.Value.Date;
            }

            // Only include password when visible and filled
            string password = null;
            if (txtPassword.Visible && !string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                password = txtPassword.Text;
            }

            // Profile image path if user picked one (picProfile.Tag used in designer code)
            string profileImagePath = null;
            if (txtImagePath != null && txtImagePath.Tag != null)
            {
                profileImagePath = txtImagePath.Tag.ToString();
            }

            // Create member object using property initializers
            var member = new Members
            {
                StudentId = string.IsNullOrWhiteSpace(studentNo) ? null : studentNo,
                FirstName = firstname,
                LastName = lastname,
                Gender = gender,
                Birthday = birthday,
                Section = section,
                Email = email,
                Department = department,
                Role = role,
                Username = string.IsNullOrWhiteSpace(username) ? null : username,
                Password = password,
                ProfileImagePath = profileImagePath
            };

            bool success = Members.AddMemberToDatabase(member);

            if (success)
            {
                MessageBox.Show("Member added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _refreshMembers?.Invoke();
                ClearFormFields();
            }
            else
            {
                MessageBox.Show("Failed to add member. Please check your input and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearFormFields()
        {
            txtFirstName.Clear();
            txtLastName.Clear();
            txtSection.Clear();
            txtEmail.Clear();

            // Student number and username textboxes
            if (textBox2 != null) textBox2.Clear();
            if (textBox1 != null) textBox1.Clear();

            // handle department control (ComboBox) and role ComboBox
            try
            {
                txtDepartment.SelectedIndex = -1;
            }
            catch
            {
                txtDepartment.Text = string.Empty;
            }

            cbRole.SelectedIndex = -1;

            // Uncheck gender radios
            if (MaleRadio != null) MaleRadio.Checked = false;
            if (FemRadio != null) FemRadio.Checked = false;
            if (OthersRadio != null) OthersRadio.Checked = false;

            // Reset birthday to today (adjust if you prefer another default)
            if (dateTimePicker1 != null) dateTimePicker1.Value = DateTime.Today;

            // clear password if present
            if (txtPassword != null) txtPassword.Clear();

            // clear profile preview
            if (txtImagePath != null)
            {
                txtImagePath.Image = null;
                txtImagePath.Tag = null;
            }
        }

        private void RoundedButton2_Click(object sender, EventArgs e)
        {
            // Clear button pressed
            ClearFormFields();
        }

        private void roundedButton3_Click(object sender, EventArgs e)
        {

            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtImagePath.Image = Image.FromFile(ofd.FileName);   // preview
                    txtImagePath.Tag = ofd.FileName;                     // store path for saving
                }
            }
        }

        private void close_btn_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
