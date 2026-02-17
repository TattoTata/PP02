using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ПП
{
    public partial class DocumentationTypeForm : Form
    {
        private string _connectionString;
        private int? _docTypeIdToEdit; 

        public DocumentationTypeForm(string connectionString)
        {
            InitializeComponent();
            _connectionString = connectionString;
            this.Text = "Добавить тип документации";
            LoadDocTypeData();
        }

        public DocumentationTypeForm(string connectionString, int docTypeId)
        {
            InitializeComponent();
            _connectionString = connectionString;
            _docTypeIdToEdit = docTypeId;
            this.Text = "Редактировать тип документации";
            LoadDocTypeData();
        }

        private void LoadDocTypeData()
        {
            if (!_docTypeIdToEdit.HasValue) return;

            string query = "SELECT TypeName FROM DocumentationTypes WHERE DocTypeID = @DocTypeID";
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@DocTypeID", _docTypeIdToEdit.Value);
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                textBox1.Text = reader["TypeName"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных типа документации: {ex.Message}");
            }
        }

        private void button1_Click(object sender, EventArgs e) 
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Наименование типа документации не может быть пустым.");
                textBox1.Focus();
                return;
            }

            string query;
            SqlParameter parameter = new SqlParameter("@TypeName", textBox1.Text);

            if (_docTypeIdToEdit.HasValue) 
            {
                query = "UPDATE DocumentationTypes SET TypeName = @TypeName WHERE DocTypeID = @DocTypeID";
                SqlParameter idParameter = new SqlParameter("@DocTypeID", _docTypeIdToEdit.Value);
                parameters = new SqlParameter[] { parameter, idParameter };
            }
            else 
            {
                query = "INSERT INTO DocumentationTypes (TypeName) VALUES (@TypeName)";
                parameters = new SqlParameter[] { parameter };
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
        private SqlParameter[] parameters; 

        private void button2_Click(object sender, EventArgs e) 
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

    }

}
