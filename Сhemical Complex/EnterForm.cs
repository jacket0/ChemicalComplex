using Microsoft.Data.Sqlite;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace Сhemical_Complex
{
    public partial class EnterForm : Form
    {
        private static int _loginAttempts = 0;

        private string _connectionString;

        public string UserRole { get; private set; }

        public EnterForm()
        {
            InitializeComponent();
            _connectionString = @"Data Source=" + Application.StartupPath + @"\ComplexDB.db";
            this.CenterToScreen();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (_loginAttempts >= 3)
            {
                MessageBox.Show("Превышено количество попыток. Обратитесь к администратору для получения данных для входа.");
                return;
            }

            _loginAttempts++;

            string username = textBoxLogin.Text;
            string password = textBoxPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Все поля должны быть заполнены!");
                return;
            }

            try
            {
                using (var connection = new SqliteConnection(_connectionString))
                {
                    connection.Open();

                    string query = @"SELECT Users.Password, Roles.Name as Role 
                               FROM Users 
                               JOIN Roles ON Users.RoleId = Roles.Id 
                               WHERE Users.Name = @username";

                    using (var cmd = new SqliteCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@username", username);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                MessageBox.Show("Пользователь не найден");
                                return;
                            }

                            string storedHash = reader["Password"].ToString();
                            UserRole = reader["Role"].ToString();
                            string inputHash = PasswordManage.GetHash(password);

                            if (storedHash == inputHash)
                            {
                                DialogResult = DialogResult.OK;
                                Close();
                            }
                            else
                            {
                                MessageBox.Show("Неверный пароль");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка авторизации: {ex.Message}");
            }
        }
    }
}
