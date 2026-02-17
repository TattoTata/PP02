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
    public partial class Registrationcs : Form
    {
        private const string ConnectionString = @"Data Source=localhost\MSSQLSERVER01;Initial Catalog=ПП.2;Integrated Security=True";

        public Registrationcs()
        {
            InitializeComponent();
            cmbRole.Items.AddRange(new object[] { "Директор", "Менеджер" });
            cmbRole.SelectedIndex = 0;
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
                using (SqlConnection connection = new SqlConnection(ConnectionString))
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
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка регистрации: {ex.Message}");
            }
        }
    }
}