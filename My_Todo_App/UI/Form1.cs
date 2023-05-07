using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Threading.Tasks;
//using System.Threading.Task;

namespace My_Todo_App
{
    public partial class Form1 : Form
    {

        private readonly Db _db;
        




        public Form1()
        {
            _db = new Db();
            InitializeComponent();
            LoadDataAsync();
            
        }
        //LOAD DATA FROM DATABASE

        public async Task LoadDataAsync()
        {
            try
            {
        DataTable dataTable = new DataTable();
        string searchTerm = textBox2.Text.Trim();

                using (SqlConnection connection = new SqlConnection(_db.connectionString))
                {
                    // Load all data from the TodoTable
                    using (SqlCommand command = new SqlCommand("SELECT * FROM TodoApp_1", connection))
                    {
                        await connection.OpenAsync();

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            dataTable.Clear();
                            dataTable.Load(reader);
                        }

                        connection.Close();
                    }

                    // Filter the data based on the search term
                    if (!string.IsNullOrEmpty(searchTerm))
                    {
                        dataTable.DefaultView.RowFilter = $"TodoItem LIKE '{searchTerm}%'"; // filter by TodoItem column
                        dataGridView1.DataSource = dataTable.DefaultView; // no need to wrap in Task.Run since it's not an expensive operation
                    }
                    else
                    {
                        dataGridView1.DataSource = dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            
        }

    

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private async void dataGridView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
                int id = Convert.ToInt32(e.Row.Cells["Id"].Value);

                using (SqlConnection connection = new SqlConnection(_db.connectionString))
                {
                    using (SqlCommand command = new SqlCommand("pTodoItem", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id", id);
                        command.Parameters.AddWithValue("@Action", "Delete");

                       await connection.OpenAsync();
                       await command.ExecuteNonQueryAsync();
                        connection.Close();
                    }
            }
        }
    
    
    

    private void label3_Click(object sender, EventArgs e)
        {

        }


        //ADD ITEMS
        private async void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string todoItem = textBox1.Text.Trim();

                if (!string.IsNullOrEmpty(todoItem))
                {
                    using (SqlConnection connection = new SqlConnection(_db.connectionString))
                    {
                        using (SqlCommand command = new SqlCommand("pTodoItem", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@Id", 0);
                            command.Parameters.AddWithValue("@TodoItem", todoItem);
                            command.Parameters.AddWithValue("@Completed", false);
                            command.Parameters.AddWithValue("@Action", "Insert");

                            await connection.OpenAsync();

                            await command.ExecuteNonQueryAsync();

                            connection.Close();
                        }
                    }

                   await  LoadDataAsync();
                }
            }
            catch (Exception ex)
            {
                // Handle the exception
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
        private void dataGridView1_CellContentClick1(object sender, DataGridViewCellEventArgs e)
        {/*
            if (e.ColumnIndex == dataGridView1.Columns["Delete"].Index && e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                dataGridView_UserDeletingRow(sender, new DataGridViewRowCancelEventArgs(row));
            }*/
        }


        //DELETE ITEMS

        private async void dataGridView1_CellContentClick_2(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == dataGridView1.Columns["Delete"].Index && e.RowIndex >= 0)
                {
                    int id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["Id"].Value);

                    using (SqlConnection connection = new SqlConnection(_db.connectionString))
                    {
                        using (SqlCommand command = new SqlCommand("pTodoItem", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@Id", id);
                            command.Parameters.AddWithValue("@Action", "Delete");

                            await connection.OpenAsync();

                            await command.ExecuteNonQueryAsync();

                            connection.Close();
                        }
                    }

                    dataGridView1.Rows.RemoveAt(e.RowIndex);
                }
            }
            catch (Exception ex)
            {
                // Handle the exception
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //SEARCH
        private async void textBox2_TextChanged_1(object sender, EventArgs e)
        {
         await LoadDataAsync();
        }

        //MAKE WHATEVER YOU WANT TO EDIT POP UP IN TEXTBOX1

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];

                // Display the selected TodoItem in a textbox for editing
                textBox1.Text = row.Cells["TodoItem"].Value.ToString();
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {

            try
            {
                string todoItem = textBox1.Text.Trim();

                if (!string.IsNullOrEmpty(todoItem))
                {
                    using (SqlConnection connection = new SqlConnection(_db.connectionString))
                    {
                        using (SqlCommand command = new SqlCommand("pTodoItem", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@Id", 0);
                            command.Parameters.AddWithValue("@TodoItem", todoItem);
                            command.Parameters.AddWithValue("@Completed", false);
                            command.Parameters.AddWithValue("@Action", "Insert");

                            await connection.OpenAsync();

                            await command.ExecuteNonQueryAsync();

                            connection.Close();
                        }
                    }

                     await  LoadDataAsync();
                }
            }
            catch (Exception ex)
            {
                // Handle the exception
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

       }
    }

       
  
   

