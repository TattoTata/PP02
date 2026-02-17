using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ПП._2
{
    public partial class SiteManagerForm : Form
    {
        private string _connectionString;
        private int? _managerIdToEdit; 
        public SiteManagerForm(string connectionString)
        {
            InitializeComponent();
            _connectionString = connectionString;
            this.Text = "Добавить руокводителя";
             LoadConstructionSites();
        }
        public SiteManagerForm(string connectionString, int managerId)
        {
            InitializeComponent();
            _connectionString = connectionString;
            _managerIdToEdit = managerId;
            this.Text = "Редактировать руководителя";
            LoadManagerData();
            LoadConstructionSites();
        }
        private void LoadConstructionSites()
        {
            string query = "SELECT ObjectID, Name FROM ConstructionSite ORDER BY Name";
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        con.Open();
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);

                        comboBox1.DisplayMember = "Name";
                        comboBox1.ValueMember = "ObjectID"; 
                        comboBox1.DataSource = dt;
                        DataRow defaultRow = dt.NewRow();
                        defaultRow["ObjectID"] = DBNull.Value;
                        defaultRow["Name"] = "- Нет объекта -";
                        dt.Rows.InsertAt(defaultRow, 0);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки объектов строительства: {ex.Message}");
            }
        }

        private void LoadManagerData()
        {
            if (!_managerIdToEdit.HasValue) return;

            string query = "SELECT FullName, Phone, Email, ObjectID FROM SiteManagers WHERE ManagerID = @ManagerID";
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ManagerID", _managerIdToEdit.Value);
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                textBox1.Text = reader["FullName"].ToString();
                                textBox2.Text = reader["Phone"] != DBNull.Value ? reader["Phone"].ToString() : string.Empty;
                                textBox3.Text = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : string.Empty;

                                if (reader["ObjectID"] != DBNull.Value) comboBox1.SelectedValue = reader["ObjectID"];
                                else comboBox1.SelectedValue = DBNull.Value;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных ответственного: {ex.Message}");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Полное имя ответственного не может быть пустым.");
                textBox1.Focus();
                return;
            }
            string query;
            SqlParameter[] parameters;
            object selectedObjectId = comboBox1.SelectedValue;
            object objectIdParam = (selectedObjectId != null && selectedObjectId != DBNull.Value) ? selectedObjectId : (object)DBNull.Value;
            if (_managerIdToEdit.HasValue)
            {
                query = @"UPDATE SiteManagers
                      SET FullName = @FullName, Phone = @Phone, Email = @Email, ObjectID = @ObjectID
                      WHERE ManagerID = @ManagerID";
                parameters = new SqlParameter[]
                {
                    new SqlParameter("@FullName", textBox1.Text),
                    new SqlParameter("@Phone", string.IsNullOrWhiteSpace(textBox2.Text) ? (object)DBNull.Value : textBox2.Text),
                    new SqlParameter("@Email", string.IsNullOrWhiteSpace(textBox3.Text) ? (object)DBNull.Value : textBox3.Text),
                new SqlParameter("@ObjectID", objectIdParam),
                new SqlParameter("@ManagerID", _managerIdToEdit.Value)
                };
            }
            else
            {
                query = @"INSERT INTO SiteManagers (FullName, Phone, Email, ObjectID)
                      VALUES (@FullName, @Phone, @Email, @ObjectID)";
                parameters = new SqlParameter[]
                {
                    new SqlParameter("@FullName", textBox1.Text),
                    new SqlParameter("@Phone", string.IsNullOrWhiteSpace(textBox2.Text) ? (object)DBNull.Value : textBox2.Text),
                    new SqlParameter("@Email", string.IsNullOrWhiteSpace(textBox3.Text) ? (object)DBNull.Value : textBox3.Text),
                new SqlParameter("@ObjectID", objectIdParam)
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

