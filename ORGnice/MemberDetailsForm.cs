using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace ORGnice
{
    public partial class MemberDetailsForm : Form
    {
        private readonly int _memberId;
        private readonly string _connString =
            "server=localhost;port=3306;database=orgdb;uid=root;pwd=legorocket3368.;";
        public MemberDetailsForm(int memberId)
        {
            InitializeComponent();
            _memberId = memberId;
            this.StartPosition = FormStartPosition.CenterParent;

            LoadMember();

            if (imageUploadLbl != null)
            {
                imageUploadLbl.Visible = false;
            }

        }

        private void LoadMember()
        {
            using (var conn = new MySqlConnection(_connString))
            {
                conn.Open();

                string sql = @"SELECT member_id, student_id, first_name, last_name,
                                      gender, birthday, section, email,
                                      department, role, username, password,
                                      profile_image_path
                               FROM members
                               WHERE member_id = @id";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", _memberId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Basic text fields
                            txtMemberId.Text = reader["member_id"]?.ToString();
                            txtStudentId.Text = reader["student_id"]?.ToString();
                            txtFirstName.Text = reader["first_name"]?.ToString();
                            txtLastName.Text = reader["last_name"]?.ToString();
                            txtSection.Text = reader["section"]?.ToString();
                            txtEmail.Text = reader["email"]?.ToString();
                            txtDepartment.Text = reader["department"]?.ToString();
                            cbRole.Text = reader["role"]?.ToString();
                            txtUsername.Text = reader["username"]?.ToString();
                            txtPassword.Text = reader["password"]?.ToString();

                            // Gender -> radio buttons
                            var gender = reader["gender"] == DBNull.Value ? string.Empty : reader["gender"].ToString();
                            rbMale.Checked = string.Equals(gender, "Male", StringComparison.OrdinalIgnoreCase);
                            rbFem.Checked = string.Equals(gender, "Female", StringComparison.OrdinalIgnoreCase);
                            rbOthers.Checked = string.Equals(gender, "Other", StringComparison.OrdinalIgnoreCase);

                            // Birthday -> DateTimePicker (supports nullable via Checked)
                            if (reader["birthday"] != DBNull.Value)
                            {
                                DateTime parsed;
                                if (DateTime.TryParse(reader["birthday"].ToString(), out parsed))
                                {
                                    dtBirthday.Value = parsed;
                                    dtBirthday.Checked = true;
                                }
                                else
                                {
                                    dtBirthday.Checked = false;
                                }
                            }
                            else
                            {
                                dtBirthday.Checked = false;
                            }

                            // Profile image path and preview
                            string imgPath = reader["profile_image_path"] == DBNull.Value
                                ? string.Empty
                                : reader["profile_image_path"].ToString();

                            picProfile.Text = imgPath;

                            // Load image safely (copy into memory to avoid file lock)
                            if (!string.IsNullOrWhiteSpace(imgPath) && File.Exists(imgPath))
                            {
                                try
                                {
                                    using (var fs = File.OpenRead(imgPath))
                                    using (var tmpImg = Image.FromStream(fs))
                                    {
                                        // Dispose previous image to release resources
                                        if (picProfile.Image != null)
                                        {
                                            var old = picProfile.Image;
                                            picProfile.Image = null;
                                            old.Dispose();
                                        }

                                        picProfile.Image = new Bitmap(tmpImg);
                                    }
                                }
                                catch
                                {
                                    // failed to load image -> clear preview
                                    if (picProfile.Image != null)
                                    {
                                        var old = picProfile.Image;
                                        picProfile.Image = null;
                                        old.Dispose();
                                    }
                                }
                            }
                            else
                            {
                                if (picProfile.Image != null)
                                {
                                    var old = picProfile.Image;
                                    picProfile.Image = null;
                                    old.Dispose();
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Member not found.");
                            DialogResult = DialogResult.Cancel;
                            Close();
                        }
                    }
                }
            }
        }


        private void close_btn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void roundedButton3_Click(object sender, EventArgs e)
        {

            using (var ofd = new OpenFileDialog())
            {
           
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    // store path in the dedicated textbox and preview image
                    picProfile.Text = ofd.FileName;

                    try
                    {
                        using (var fs = File.OpenRead(ofd.FileName))
                        using (var tmpImg = Image.FromStream(fs))
                        {
                            if (picProfile.Image != null)
                            {
                                var old = picProfile.Image;
                                picProfile.Image = null;
                                old.Dispose();
                            }

                            picProfile.Image = new Bitmap(tmpImg);
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Unable to load selected image.", "Image error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void roundedButton1_Click(object sender, EventArgs e)
        {
            using (var conn = new MySqlConnection(_connString))
            {
                conn.Open();

                string sql = @"
                UPDATE members SET
                    student_id = @studentId,
                    first_name = @firstName,
                    last_name = @lastName,
                    gender = @gender,
                    birthday = @birthday,
                    section = @section,
                    email = @email,
                    department = @department,
                    role = @role,
                    username = @username,
                    password = @password,
                    profile_image_path = @profileImagePath
                WHERE member_id = @id";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", _memberId);
                    cmd.Parameters.AddWithValue("@studentId", txtStudentId.Text);
                    cmd.Parameters.AddWithValue("@firstName", txtFirstName.Text);
                    cmd.Parameters.AddWithValue("@lastName", txtLastName.Text);

                    string gender = null;
                    if (rbMale.Checked) gender = "Male";
                    else if (rbFem.Checked) gender = "Female";
                    else if (rbOthers.Checked) gender = "Other";

                    cmd.Parameters.AddWithValue("@gender",
                        string.IsNullOrWhiteSpace(gender) ? (object)DBNull.Value : gender);

                    cmd.Parameters.AddWithValue("@birthday",
                        dtBirthday.Checked ? (object)dtBirthday.Value.Date : DBNull.Value);

                    cmd.Parameters.AddWithValue("@section", txtSection.Text);
                    cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@department", txtDepartment.Text);
                    cmd.Parameters.AddWithValue("@role", cbRole.Text);
                    cmd.Parameters.AddWithValue("@username",
                        string.IsNullOrWhiteSpace(txtUsername.Text) ? (object)DBNull.Value : txtUsername.Text);
                    cmd.Parameters.AddWithValue("@password",
                        string.IsNullOrWhiteSpace(txtPassword.Text) ? (object)DBNull.Value : txtPassword.Text);
                    cmd.Parameters.AddWithValue("@profileImagePath",
                        string.IsNullOrWhiteSpace(picProfile.Text) ? (object)DBNull.Value : picProfile.Text);

                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        MessageBox.Show("Member updated successfully.");
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("No changes were saved.");
                    }
                }
            }

            }




        private void Clr_btn_Click(object sender, EventArgs e)
        {
            txtFirstName.Clear();
            txtLastName.Clear();
            txtSection.Clear();
            txtEmail.Clear();

            // Student number and username textboxes
            if (txtStudentId != null) txtStudentId.Clear();
            if (txtUsername != null) txtUsername.Clear();

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


            // Reset birthday to today (adjust if you prefer another default)
            if (dtBirthday != null) dtBirthday.Value = DateTime.Today;

            // clear password if present
            if (txtPassword != null) txtPassword.Clear();

            // clear profile preview
            if (picProfile != null)
            {
                picProfile.Image = null;
                picProfile.Tag = null;
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
    }
}
