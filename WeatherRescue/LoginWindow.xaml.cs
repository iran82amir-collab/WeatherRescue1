using System.Windows;

using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace WeatherRescue
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string phone = PhoneTextBox.Text.Trim();
            string password = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(phone) ||
                string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter phone number and password.");
                return;
            }

            try
            {
                DatabaseConnection db = new DatabaseConnection();

                using (SqlConnection connection = db.GetConnection())
                {
                    connection.Open();

                    string query = @"
                SELECT FullName, PasswordHash, Role
                FROM Users
                WHERE PhoneNumber = @Phone";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Phone", phone);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                MessageBox.Show("Phone number or password is incorrect.");
                                return;
                            }

                            string fullName = reader["FullName"].ToString();
                            string storedHash = reader["PasswordHash"].ToString();
                            string role = reader["Role"].ToString();

                            if (!PasswordHasher.VerifyPassword(password, storedHash))
                            {
                                MessageBox.Show("Phone number or password is incorrect.");
                                return;
                            }

                            if (role == "Admin")
                            {
                                AdminDashboardWindow adminDashboard =
                                    new AdminDashboardWindow();

                                adminDashboard.Show();
                            }
                            else
                            {
                                DashboardWindow dashboard =
                                    new DashboardWindow(fullName, role);

                                dashboard.Show();
                            }

                            this.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Login Error");
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow register = new RegisterWindow();
            register.Show();
            this.Close();
        }
    }
}