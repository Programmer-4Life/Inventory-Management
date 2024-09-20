using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace Inventory_Management
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            // Any initialization code can go here
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["InventoryDB"].ConnectionString))
            {
                conn.Open();
                string hashedPassword = HashPassword(txtPassword.Text);

                var cmd = new SqlCommand("SELECT COUNT(*) FROM Users WHERE Username = @Username AND PasswordHash = @PasswordHash", conn);
                cmd.Parameters.AddWithValue("@Username", txtUsername.Text);
                cmd.Parameters.AddWithValue("@PasswordHash", hashedPassword);

                int userCount = (int)cmd.ExecuteScalar();

                if (userCount > 0)
                {
                    MessageBox.Show("Login successful!");
                    Form1 mainForm = new Form1(); // Create an instance of Form1
                    mainForm.Show(); // Show Form1
                    this.Hide(); // Hide the login form
                }
                else
                {
                    MessageBox.Show("Invalid username or password.");
                }
            }
        }

        private void btnOpenRegister_Click(object sender, EventArgs e)
        {
            FormRegister registerForm = new FormRegister();
            registerForm.Show();
            this.Hide(); // Optionally hide the login form
        }
    }
}
