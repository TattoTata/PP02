using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;



namespace ПП._2
{
    public partial class ObjectWorkForm : Form
    {
        private string _connectionString;
        private int? _objectWorkIdToEdit;

        public ObjectWorkForm(string connectionString)
        {
            InitializeComponent();
            _connectionString = connectionString;
            this.Text = "Добавить работу по объекту";
            dateTimePicker1.Value = DateTime.Today;
            dateTimePicker2.ShowCheckBox = true;
            dateTimePicker2.Checked = false;
            LoadConstructionSites();
            LoadWorkTypes();
            LoadContractors();
            LoadObjectWorkData(); 
        }
        public ObjectWorkForm(string connectionString, int objectWorkId)
        {
            InitializeComponent();
            _connectionString = connectionString;
            _objectWorkIdToEdit = objectWorkId;
            this.Text = "Редактировать работу по объекту";
            dateTimePicker2.ShowCheckBox = true;
            LoadConstructionSites();
            LoadWorkTypes();
            LoadContractors();
            LoadObjectWorkData();
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
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки объектов строительства: {ex.Message}");
            }
        }
        private void LoadWorkTypes()
        {
            string query = "SELECT WorkTypeID, Name FROM WorkTypes ORDER BY Name";
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

                        comboBox2.DisplayMember = "Name";
                        comboBox2.ValueMember = "WorkTypeID";
                        comboBox2.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки типов работ: {ex.Message}");
            }
        }
        private void LoadContractors()
        {
            string query = "SELECT ContractorID, CompanyName FROM Contractors ORDER BY CompanyName";
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

                        comboBox3.DisplayMember = "CompanyName";
                        comboBox3.ValueMember = "ContractorID";
                        comboBox3.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки подрядчиков: {ex.Message}");
            }
        }
        private void LoadObjectWorkData()
        {
            if (!_objectWorkIdToEdit.HasValue) return;

            string query = "SELECT ObjectID, WorkTypeID, ContractorID, StartDate, EndDate, Cost FROM ObjectWorks WHERE ObjectWorkID = @ObjectWorkID";
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ObjectWorkID", _objectWorkIdToEdit.Value);
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                if (reader["ObjectID"] != DBNull.Value) comboBox1.SelectedValue = reader["ObjectID"];
                                if (reader["WorkTypeID"] != DBNull.Value) comboBox2.SelectedValue = reader["WorkTypeID"];
                                if (reader["ContractorID"] != DBNull.Value) comboBox3.SelectedValue = reader["ContractorID"];

                                if (reader["StartDate"] != DBNull.Value) dateTimePicker1.Value = Convert.ToDateTime(reader["StartDate"]);

                                if (reader["EndDate"] != DBNull.Value)
                                {
                                    dateTimePicker2.Value = Convert.ToDateTime(reader["EndDate"]);
                                    dateTimePicker2.Checked = true;
                                }
                                else
                                {
                                    dateTimePicker2.Checked = false;
                                }
                                if (reader["Cost"] != DBNull.Value) textBox1.Text = reader["Cost"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных работы по объекту: {ex.Message}");
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue == null || comboBox1.SelectedValue == DBNull.Value)
            {
                MessageBox.Show("Необходимо выбрать объект строительства.");
                comboBox1.Focus();
                return;
            }
            if (comboBox2.SelectedValue == null || comboBox2.SelectedValue == DBNull.Value)
            {
                MessageBox.Show("Необходимо выбрать тип работы.");
                comboBox2.Focus();
                return;
            }
            if (comboBox3.SelectedValue == null || comboBox3.SelectedValue == DBNull.Value)
            {
                MessageBox.Show("Необходимо выбрать подрядчика.");
                comboBox3.Focus();
                return;
            }
            decimal cost = 0;
            if (!string.IsNullOrWhiteSpace(textBox1.Text))
            {
                if (!decimal.TryParse(textBox1.Text, out cost))
                {
                    MessageBox.Show("Введите корректное значение для стоимости.");
                    textBox1.Focus();
                    return;
                }
            }
            string query;
            SqlParameter[] parameters;

            if (_objectWorkIdToEdit.HasValue) 
            {
                query = @"UPDATE ObjectWorks
                      SET ObjectID = @ObjectID, WorkTypeID = @WorkTypeID, ContractorID = @ContractorID,
                          StartDate = @StartDate, EndDate = @EndDate, Cost = @Cost
                      WHERE ObjectWorkID = @ObjectWorkID";
                parameters = new SqlParameter[]
                {
                new SqlParameter("@ObjectID", comboBox1.SelectedValue),
                new SqlParameter("@WorkTypeID", comboBox2.SelectedValue),
                new SqlParameter("@ContractorID", comboBox3.SelectedValue),
                new SqlParameter("@StartDate", (object)dateTimePicker1.Value.Date),
                new SqlParameter("@EndDate", dateTimePicker2.Checked ? (object)dateTimePicker2.Value.Date : DBNull.Value),
                new SqlParameter("@Cost", string.IsNullOrWhiteSpace(textBox1.Text) ? (object)DBNull.Value : cost),
                new SqlParameter("@ObjectWorkID", _objectWorkIdToEdit.Value)
                };
            }
            else
            {
                query = @"INSERT INTO ObjectWorks (ObjectID, WorkTypeID, ContractorID, StartDate, EndDate, Cost)
                      VALUES (@ObjectID, @WorkTypeID, @ContractorID, @StartDate, @EndDate, @Cost)";
                parameters = new SqlParameter[]
                {
                new SqlParameter("@ObjectID", comboBox1.SelectedValue),
                new SqlParameter("@WorkTypeID", comboBox2.SelectedValue),
                new SqlParameter("@ContractorID", comboBox3.SelectedValue),
                new SqlParameter("@StartDate", (object)dateTimePicker1.Value.Date),
                new SqlParameter("@EndDate", dateTimePicker2.Checked ? (object)dateTimePicker2.Value.Date : DBNull.Value),
                new SqlParameter("@Cost", string.IsNullOrWhiteSpace(textBox1.Text) ? (object)DBNull.Value : cost)
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
        private void btnCreateContract_Click(object sender, EventArgs e)
        {
            // Проверяем, что работа сохранена (режим редактирования или ID не пустой)
            int workId;
            if (_objectWorkIdToEdit.HasValue)
            {
                workId = _objectWorkIdToEdit.Value;
            }
            else
            {
                MessageBox.Show("Сначала сохраните работу, чтобы создать договор.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Запрос пути сохранения
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Word Documents (*.docx)|*.docx";
            saveFileDialog.FileName = $"Договор_{workId}_{DateTime.Now:yyyyMMdd}.docx";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                DocumentGenerator generator = new DocumentGenerator(_connectionString);
                generator.GenerateContract(workId, saveFileDialog.FileName);
            }
        }
    }
}