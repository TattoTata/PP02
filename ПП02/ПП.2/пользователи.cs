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

namespace ПП._2
{
    public partial class UserForm : Form
    {
        private string connectionString;
        public UserForm()
        {
            InitializeComponent();
            this.connectionString = @"Data Source=localhost;Initial Catalog=ПП.2;Integrated Security=True"; 
            cmbRole.Items.AddRange(new object[] { "Директор", "Менеджер" });
            cmbRole.SelectedIndex = 0;
        }
        public UserForm(string connectionString) : this() 
        {
            this.connectionString = connectionString;
        }
        public UserForm(string connectionString, int userId) : this(connectionString) 
        {
            LoadUserData(userId);
        }

        private void LoadUserData(int userId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT Login, Password, Email, Role FROM Users WHERE ID = @ID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ID", userId);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtRegLogin.Text = reader["Login"].ToString();
                                txtRegPassword.Text = reader["Password"].ToString(); 
                                txtRegEmail.Text = reader["Email"].ToString();
                                cmbRole.SelectedItem = reader["Role"].ToString();
                            }
                            else
                            {
                                MessageBox.Show("Пользователь не найден.");
                                this.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных пользователя: {ex.Message}");
            }
        }

        private void btnRegisterUser_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRegLogin.Text) ||
                string.IsNullOrWhiteSpace(txtRegPassword.Text) ||
                string.IsNullOrWhiteSpace(txtRegEmail.Text))
            {
                MessageBox.Show("Все поля обязательны для заполнения!");
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString)) 
                {
                    connection.Open();
                    string query = @"INSERT INTO Users (Login, Password, Email, Role)
                                     VALUES (@Login, @Password, @Email, @Role)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Login", txtRegLogin.Text);
                        command.Parameters.AddWithValue("@Password", txtRegPassword.Text);
                        command.Parameters.AddWithValue("@Email", txtRegEmail.Text);
                        command.Parameters.AddWithValue("@Role", cmbRole.SelectedItem.ToString());

                        command.ExecuteNonQuery();
                        MessageBox.Show("Регистрация прошла успешно!");
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка регистрации: {ex.Message}");
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}