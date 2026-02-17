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
    public partial class ObjectDocumentForm : Form
    {
        private string _connectionString;
        private int? _documentIdToEdit;

        public ObjectDocumentForm(string connectionString)
        {
            InitializeComponent();
            _connectionString = connectionString;
            this.Text = "Добавить документ";
            dateTimePicker1.Value = DateTime.Today; 
            LoadDocumentationTypes();
            LoadConstructionSites();
        }

        public ObjectDocumentForm(string connectionString, int documentId)
        {
            InitializeComponent();
            _connectionString = connectionString;
            _documentIdToEdit = documentId;
            this.Text = "Редактировать документ";

            LoadConstructionSites();  
            LoadDocumentationTypes();
            LoadDocumentData();
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
                        defaultRow["Name"] = "- Выберите объект -";
                        dt.Rows.InsertAt(defaultRow, 0);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки объектов строительства: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDocumentationTypes()
        {
            string query = "SELECT DocTypeID, TypeName FROM DocumentationTypes ORDER BY TypeName";
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

                        comboBox2.DisplayMember = "TypeName";
                        comboBox2.ValueMember = "DocTypeID";
                        comboBox2.DataSource = dt;

                        DataRow defaultRow = dt.NewRow();
                        defaultRow["DocTypeID"] = DBNull.Value;
                        defaultRow["TypeName"] = "- Выберите тип -";
                        dt.Rows.InsertAt(defaultRow, 0);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки типов документации: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void LoadDocumentData()
        {
            if (!_documentIdToEdit.HasValue) return;

            string query = "SELECT ObjectID, DocTypeID, FilePath, UploadDate FROM ObjectDocuments WHERE DocumentID = @DocumentID";
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@DocumentID", _documentIdToEdit.Value);
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                if (reader["ObjectID"] != DBNull.Value) comboBox1.SelectedValue = reader["ObjectID"];
                                if (reader["DocTypeID"] != DBNull.Value) comboBox2.SelectedValue = reader["DocTypeID"];
                                textBox1.Text = reader["FilePath"] != DBNull.Value ? reader["FilePath"].ToString() : string.Empty;
                                if (reader["UploadDate"] != DBNull.Value) dateTimePicker1.Value = Convert.ToDateTime(reader["UploadDate"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных документа: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e) 
        {
            if (comboBox1.SelectedValue == null || comboBox1.SelectedValue == DBNull.Value)
            {
                MessageBox.Show("Необходимо выбрать объект строительства.", "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBox1.Focus();
                return;
            }
            if (comboBox2.SelectedValue == null || comboBox2.SelectedValue == DBNull.Value)
            {
                MessageBox.Show("Необходимо выбрать тип документации.", "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBox2.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Путь/имя файла не может быть пустым.", "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox1.Focus();
                return;
            }


            string query;
            SqlParameter[] parameters;

            if (_documentIdToEdit.HasValue)
            {
                query = @"UPDATE ObjectDocuments
                      SET ObjectID = @ObjectID, DocTypeID = @DocTypeID, FilePath = @FilePath, UploadDate = @UploadDate
                      WHERE DocumentID = @DocumentID";
                parameters = new SqlParameter[]
                {
                new SqlParameter("@ObjectID", comboBox1.SelectedValue),
                new SqlParameter("@DocTypeID", comboBox2.SelectedValue),
                new SqlParameter("@FilePath", textBox1.Text),
                new SqlParameter("@UploadDate", (object)dateTimePicker1.Value.Date),
                new SqlParameter("@DocumentID", _documentIdToEdit.Value)
                };
            }
            else
            {
                query = @"INSERT INTO ObjectDocuments (ObjectID, DocTypeID, FilePath, UploadDate)
                      VALUES (@ObjectID, @DocTypeID, @FilePath, @UploadDate)";
                parameters = new SqlParameter[]
                {
                new SqlParameter("@ObjectID", comboBox1.SelectedValue),
                new SqlParameter("@DocTypeID", comboBox2.SelectedValue),
                new SqlParameter("@FilePath", textBox1.Text),
                new SqlParameter("@UploadDate", (object)dateTimePicker1.Value.Date)
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
