    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using static System.Windows.Forms.VisualStyles.VisualStyleElement;

    namespace ПП._2
    {
        public partial class ContractorForm : Form
        {
            private string _connectionString;
            private int? _contractorIdToEdit; 
            public ContractorForm(string connectionString)
            {
                InitializeComponent();
                _connectionString = connectionString;
                this.Text = "Добавить подрядчика";
            }
            public ContractorForm(string connectionString, int contractorId)
            {
                InitializeComponent();
                _connectionString = connectionString;
                _contractorIdToEdit = contractorId;
                this.Text = "Редактировать подрядчика";
                LoadContractorData();
            }
            private void LoadContractorData()
            {
                if (!_contractorIdToEdit.HasValue) return;
                string query = "SELECT CompanyName, INN, ContactPerson, Phone, Email FROM Contractors WHERE ContractorID = @ContractorID";
                try
                {
                    using (SqlConnection con = new SqlConnection(_connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@ContractorID", _contractorIdToEdit.Value);
                            con.Open();
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    textBox1.Text = reader["CompanyName"].ToString();
                                    textBox2.Text = reader["INN"] != DBNull.Value ? reader["INN"].ToString() : string.Empty;
                                    textBox3.Text = reader["ContactPerson"] != DBNull.Value ? reader["ContactPerson"].ToString() : string.Empty;
                                    textBox4.Text = reader["Phone"] != DBNull.Value ? reader["Phone"].ToString() : string.Empty;
                                    textBox5.Text = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : string.Empty;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки данных подрядчика: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            private void button1_Click(object sender, EventArgs e)
            {
                if (string.IsNullOrWhiteSpace(textBox1.Text))
                {
                    MessageBox.Show("Наименование компании не может быть пустым.", "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBox1.Focus();
                    return;
                }
                string query;
                SqlParameter[] parameters;
                if (_contractorIdToEdit.HasValue)
                {
                    query = @"UPDATE Contractors
                          SET CompanyName = @CompanyName, INN = @INN, ContactPerson = @ContactPerson,
                              Phone = @Phone, Email = @Email
                          WHERE ContractorID = @ContractorID";
                    parameters = new SqlParameter[]
                    {
                        new SqlParameter("@CompanyName", textBox1.Text),
                        new SqlParameter("@INN", string.IsNullOrWhiteSpace(textBox2.Text) ? (object)DBNull.Value : textBox2.Text),
                        new SqlParameter("@ContactPerson", string.IsNullOrWhiteSpace(textBox3.Text) ? (object)DBNull.Value : textBox3.Text),
                        new SqlParameter("@Phone", string.IsNullOrWhiteSpace(textBox4.Text) ? (object)DBNull.Value : textBox4.Text),
                        new SqlParameter("@Email", string.IsNullOrWhiteSpace(textBox5.Text) ? (object)DBNull.Value : textBox5.Text),
                    new SqlParameter("@ContractorID", _contractorIdToEdit.Value)
                    };
                }
                else
                {
                    query = @"INSERT INTO Contractors (CompanyName, INN, ContactPerson, Phone, Email)
                          VALUES (@CompanyName, @INN, @ContactPerson, @Phone, @Email)";
                    parameters = new SqlParameter[]
                    {
                        new SqlParameter("@CompanyName", textBox1.Text),
                        new SqlParameter("@INN", string.IsNullOrWhiteSpace(textBox2.Text) ? (object)DBNull.Value : textBox2.Text),
                        new SqlParameter("@ContactPerson", string.IsNullOrWhiteSpace(textBox3.Text) ? (object)DBNull.Value : textBox3.Text),
                        new SqlParameter("@Phone", string.IsNullOrWhiteSpace(textBox4.Text) ? (object)DBNull.Value : textBox4.Text),
                    new SqlParameter("@Email", string.IsNullOrWhiteSpace(textBox5.Text) ? (object)DBNull.Value : textBox5.Text)
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
                    MessageBox.Show($"Ошибка сохранения данных: {ex.Message}", "Ошибка базы данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            private void button2_Click(object sender, EventArgs e)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }
    }
