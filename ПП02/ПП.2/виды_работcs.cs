using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ПП
{
    public partial class WorkTypeForm : Form
    {
        private string _connectionString;
        private int? _workTypeIdToEdit;
        public WorkTypeForm(string connectionString)
        {
            InitializeComponent();
            _connectionString = connectionString;
            this.Text = "Добавить тип работы";
        }
        public WorkTypeForm(string connectionString, int workTypeId)
        {
            InitializeComponent(); 
            _connectionString = connectionString;
            _workTypeIdToEdit = workTypeId;
            this.Text = "Редактировать тип работы";
            LoadWorkTypeData();
        }
        private void LoadWorkTypeData()
        {
            if (!_workTypeIdToEdit.HasValue) return;

            string query = "SELECT Name, Description FROM WorkTypes WHERE WorkTypeID = @WorkTypeID";
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@WorkTypeID", _workTypeIdToEdit.Value);
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                textBox1.Text = reader["Name"].ToString();
                                textBox2.Text = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : string.Empty;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных типа работы: {ex.Message}");
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Наименование типа работы не может быть пустым.");
                textBox1.Focus();
                return;
            }

            string query;
            SqlParameter[] parameters;

            if (_workTypeIdToEdit.HasValue) 
            {
                query = @"UPDATE WorkTypes
                          SET Name = @Name, Description = @Description
                          WHERE WorkTypeID = @WorkTypeID";
                parameters = new SqlParameter[]
                {
                    new SqlParameter("@Name", textBox1.Text),
                    new SqlParameter("@Description", string.IsNullOrWhiteSpace(textBox2.Text) ? (object)DBNull.Value : textBox2.Text),
                    new SqlParameter("@WorkTypeID", _workTypeIdToEdit.Value)
                };
            }
            else 
            {
                query = @"INSERT INTO WorkTypes (Name, Description)
                          VALUES (@Name, @Description)";
                parameters = new SqlParameter[]
                {
                    new SqlParameter("@Name", textBox1.Text),
                    new SqlParameter("@Description", string.IsNullOrWhiteSpace(textBox2.Text) ? (object)DBNull.Value : textBox2.Text)
                };
            }

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddRange(parameters);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения данных: {ex.Message}");
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}