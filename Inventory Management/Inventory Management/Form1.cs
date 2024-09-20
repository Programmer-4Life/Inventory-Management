using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Windows.Forms;

namespace Inventory_Management
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadProducts();
        }

        private void LoadProducts()
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["InventoryDb"].ConnectionString))
            {
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Products", conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridViewProducts.DataSource = dt;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["InventoryDb"].ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO Products (Name, Quantity, Price) VALUES (@Name, @Quantity, @Price)", conn);
                    cmd.Parameters.AddWithValue("@Name", txtName.Text);
                    cmd.Parameters.AddWithValue("@Quantity", int.Parse(txtQuantity.Text));
                    cmd.Parameters.AddWithValue("@Price", decimal.Parse(txtPrice.Text));
                    cmd.ExecuteNonQuery();
                }
                LoadProducts(); // Refresh the DataGridView
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridViewProducts.SelectedRows.Count > 0)
            {
                int selectedRowIndex = dataGridViewProducts.SelectedRows[0].Index;
                int productId = (int)dataGridViewProducts.Rows[selectedRowIndex].Cells[0].Value; // Assuming Id is the first column

                try
                {
                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["InventoryDB"].ConnectionString))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("UPDATE Products SET Name=@Name, Quantity=@Quantity, Price=@Price WHERE Id=@Id", conn);
                        cmd.Parameters.AddWithValue("@Id", productId);
                        cmd.Parameters.AddWithValue("@Name", txtName.Text);
                        cmd.Parameters.AddWithValue("@Quantity", int.Parse(txtQuantity.Text));
                        cmd.Parameters.AddWithValue("@Price", decimal.Parse(txtPrice.Text));
                        cmd.ExecuteNonQuery();
                    }
                    LoadProducts(); // Refresh the DataGridView
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }


        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewProducts.SelectedRows.Count > 0)
            {
                int selectedRowIndex = dataGridViewProducts.SelectedRows[0].Index;
                int productId = (int)dataGridViewProducts.Rows[selectedRowIndex].Cells[0].Value; // Assuming Id is the first column

                try
                {
                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["InventoryDb"].ConnectionString))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("DELETE FROM Products WHERE Id=@Id", conn);
                        cmd.Parameters.AddWithValue("@Id", productId);
                        cmd.ExecuteNonQuery();
                    }
                    LoadProducts(); // Refresh the DataGridView
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e) // Added this method
        {
            LoadProducts(); // Refresh the product list in the DataGridView
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
