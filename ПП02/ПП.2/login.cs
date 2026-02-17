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
    public partial class LoginForm : Form
    {
        // Строка подключения к БД (измените под вашу БД)
        private const string ConnectionString = @"Data Source=localhost\MSSQLSERVER01;Initial Catalog=ПП.2;Integrated Security=True";

        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    // Получаем несколько полей пользователя
                    string query = "SELECT Role, Email FROM Users WHERE Login = @Login AND Password = @Password";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Login", txtLogin.Text);
                        command.Parameters.AddWithValue("@Password", txtPassword.Text);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string role = reader["Role"].ToString();
                                string email = reader["Email"].ToString();

                                MessageBox.Show($"Добро пожаловать, {txtLogin.Text}!");

                                this.Hide();
                                Form roleForm = null;

                                switch (role.ToLower())
                                {
                                    case "менеджер":
                                    case "manager":
                                        roleForm = new Менеджер(txtLogin.Text, role, email);
                                        break;

                                    case "директор":
                                    case "director":
                                        roleForm = new Директор(txtLogin.Text, role, email);
                                        break;

                                }

                                if (roleForm != null)
                                {
                                    roleForm.FormClosed += (s, args) => this.Close();
                                    roleForm.Show();
                                }
                            }
                            else
                            {
                                MessageBox.Show("Неверный логин или пароль");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            Registrationcs registerForm = new Registrationcs();
            registerForm.ShowDialog();
        }
    }
}
