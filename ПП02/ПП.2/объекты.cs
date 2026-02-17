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

namespace ПП
{
    public partial class ConstructionSiteForm : Form
    {
        private string _connectionString;
        private int? _objectIdToEdit;
        private string _userRole;

        private void InitializeStatusComboBox()
        {
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(new string[] { "Проект", "В процессе", "Приостановлен", "Завершен" });
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.SelectedIndex = 0;
        }

        public ConstructionSiteForm(string connectionString)
        {
            InitializeComponent();
            _connectionString = connectionString;
            this.Text = "Добавить объект";
            dateTimePicker1.Value = DateTime.Today;
            dateTimePicker2.ShowCheckBox = true;
            dateTimePicker2.Checked = false;
            InitializeStatusComboBox();
        }

        public ConstructionSiteForm(string connectionString, int objectId, string userRole)
        {
            InitializeComponent();
            _connectionString = connectionString;
            _objectIdToEdit = objectId;
            _userRole = userRole;

            this.Text = "Редактировать объект";
            dateTimePicker2.ShowCheckBox = true;
            InitializeStatusComboBox();

            if (_userRole != "Директор")
            {
                comboBox1.Enabled = false;
            }

            LoadObjectData();
        }

        private void LoadObjectData()
        {
            if (!_objectIdToEdit.HasValue) return;
            string query = "SELECT Name, Address, StartDate, EndDate, Budget, Status FROM ConstructionSiteN WHERE ObjectID = @ObjectID";
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ObjectID", _objectIdToEdit.Value);
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                textBox1.Text = reader["Name"].ToString();
                                textBox2.Text = reader["Address"] != DBNull.Value ? reader["Address"].ToString() : string.Empty;

                                if (reader["Status"] != DBNull.Value)
                                {
                                    comboBox1.SelectedItem = reader["Status"].ToString();
                                }
                                else
                                {
                                    comboBox1.SelectedIndex = 0;
                                }

                                if (reader["StartDate"] != DBNull.Value)
                                {
                                    dateTimePicker1.Value = Convert.ToDateTime(reader["StartDate"]);
                                }
                                else
                                {
                                    dateTimePicker1.Value = DateTime.Today;
                                }

                                if (reader["EndDate"] != DBNull.Value)
                                {
                                    dateTimePicker2.Value = Convert.ToDateTime(reader["EndDate"]);
                                    dateTimePicker2.Checked = true;
                                }
                                else
                                {
                                    dateTimePicker2.Checked = false;
                                }

                                if (reader["Budget"] != DBNull.Value) textBox3.Text = reader["Budget"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных объекта: {ex.Message}");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Наименование объекта не может быть пустым.");
                textBox1.Focus();
                return;
            }

            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите статус.");
                comboBox1.Focus();
                return;
            }

            string query;
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("@Name", textBox1.Text));
            parameters.Add(new SqlParameter("@Address", string.IsNullOrWhiteSpace(textBox2.Text) ? (object)DBNull.Value : textBox2.Text));
            parameters.Add(new SqlParameter("@StartDate", dateTimePicker1.Value.Date));
            parameters.Add(new SqlParameter("@EndDate", dateTimePicker2.Checked ? (object)dateTimePicker2.Value.Date : DBNull.Value));
            parameters.Add(new SqlParameter("@Budget", textBox3.Text));
            parameters.Add(new SqlParameter("@Status", comboBox1.SelectedItem.ToString()));

            if (_objectIdToEdit.HasValue)
            {
                query = @"UPDATE ConstructionSiteN
                          SET Name = @Name, Address = @Address, StartDate = @StartDate,
                              EndDate = @EndDate, Budget = @Budget, Status = @Status
                          WHERE ObjectID = @ObjectID";
                parameters.Add(new SqlParameter("@ObjectID", _objectIdToEdit.Value));
            }
            else
            {
                query = @"INSERT INTO ConstructionSiteN (Name, Address, StartDate, EndDate, Budget, Status)
                          VALUES (@Name, @Address, @StartDate, @EndDate, @Budget, @Status)";
            }

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddRange(parameters.ToArray());
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

        private string GetCurrentStatusFromDatabase()
        {
            string currentStatus = "";
            if (!_objectIdToEdit.HasValue) return comboBox1.Items[0].ToString();

            string query = "SELECT Status FROM ConstructionSiteN WHERE ObjectID = @ObjectID";
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ObjectID", _objectIdToEdit.Value);
                        con.Open();
                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            currentStatus = result.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка получения текущего статуса: {ex.Message}");
            }

            return string.IsNullOrWhiteSpace(currentStatus) ? comboBox1.Items[0].ToString() : currentStatus;
        }
    }
}