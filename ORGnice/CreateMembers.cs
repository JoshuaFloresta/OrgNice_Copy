using System;
using System.Drawing;
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
            this.StartPosition = FormStartPosition.CenterScreen;

            cbRole.SelectedIndexChanged += cbRole_SelectedIndexChanged;
            roundedButton1.Click += roundedButton1_Click;   // Save
            roundedButton2.Click += RoundedButton2_Click;   // Clear

            if (imageUploadLbl != null)
                imageUploadLbl.Visible = false;
        }

        private void cbRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool isAdmin = string.Equals(cbRole.Text, "Admin", StringComparison.OrdinalIgnoreCase);

            lblPassword.Visible = isAdmin;
            txtPassword.Visible = isAdmin;

            if (!isAdmin)
                txtPassword.Clear();
        }

        private void roundedButton1_Click(object sender, EventArgs e)
        {
            string firstname = txtFirstName.Text.Trim();
            string lastname = txtLastName.Text.Trim();
            string section = txtSection.Text.Trim();
            string email = txtEmail.Text.Trim();
            string department = txtDepartment.Text;
            string role = cbRole.Text;

            string studentNo = textBox2?.Text.Trim();
            string username = textBox1?.Text.Trim();

            string gender = null;
            if (MaleRadio?.Checked == true) gender = "Male";
            else if (FemRadio?.Checked == true) gender = "Female";
            else if (OthersRadio?.Checked == true) gender = "Other";

            DateTime? birthday = dateTimePicker1?.Value.Date;

            string password = null;
            if (txtPassword.Visible && !string.IsNullOrWhiteSpace(txtPassword.Text))
                password = txtPassword.Text;

            string profileImagePath = null;
            if (txtImagePath != null && txtImagePath.Tag != null)
                profileImagePath = txtImagePath.Tag.ToString();

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
                MessageBox.Show("Member added successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                _refreshMembers?.Invoke();
                ClearFormFields();
            }
            else
            {
                MessageBox.Show("Failed to add member. Please check your input and try again.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearFormFields()
        {
            txtFirstName.Clear();
            txtLastName.Clear();
            txtSection.Clear();
            txtEmail.Clear();

            textBox2?.Clear();
            textBox1?.Clear();

            try { txtDepartment.SelectedIndex = -1; }
            catch { txtDepartment.Text = string.Empty; }

            cbRole.SelectedIndex = -1;

            if (MaleRadio != null) MaleRadio.Checked = false;
            if (FemRadio != null) FemRadio.Checked = false;
            if (OthersRadio != null) OthersRadio.Checked = false;

            if (dateTimePicker1 != null) dateTimePicker1.Value = DateTime.Today;

            txtPassword?.Clear();

            if (txtImagePath != null)
            {
                txtImagePath.Image = null;
                txtImagePath.Tag = null;
            }
        }

        private void RoundedButton2_Click(object sender, EventArgs e)
        {
            ClearFormFields();
        }

        private void roundedButton3_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtImagePath.Image = Image.FromFile(ofd.FileName);
                    txtImagePath.Tag = ofd.FileName;
                }
            }
        }

        private void close_btn_Click_1(object sender, EventArgs e)
        {
            Close();
        }
    }
}
