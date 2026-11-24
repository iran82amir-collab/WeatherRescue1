using System;
using System.Data.SqlClient;
using System.Windows;

namespace WeatherRescue
{
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            string fullName = FullNameTextBox.Text.Trim();
            string phone = PhoneTextBox.Text.Trim();
            string password = PasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

            if (string.IsNullOrWhiteSpace(fullName) ||
                string.IsNullOrWhiteSpace(phone) ||
                string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please fill in all required fields.");
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.");
                return;
            }

            if (password.Length < 8)
            {
                MessageBox.Show("Password must be at least 8 characters.");
                return;
            }

            try
            {
                DatabaseConnection db = new DatabaseConnection();

                using (SqlConnection connection = db.GetConnection())
                {
                    connection.Open();

                    string checkQuery =
                        "SELECT COUNT(*) FROM Users WHERE PhoneNumber = @Phone";

                    using (SqlCommand checkCommand =
                        new SqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@Phone", phone);

                        int count = (int)checkCommand.ExecuteScalar();

                        if (count > 0)
                        {
                            MessageBox.Show("This phone number is already registered.");
                            return;
                        }
                    }

                    string passwordHash =
                        PasswordHasher.HashPassword(password);

                    string insertQuery = @"
                        INSERT INTO Users
                        (FullName, PhoneNumber, PasswordHash, Role)
                        VALUES
                        (@FullName, @Phone, @PasswordHash, 'User')";

                    using (SqlCommand command =
                        new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@FullName", fullName);
                        command.Parameters.AddWithValue("@Phone", phone);
                        command.Parameters.AddWithValue("@PasswordHash", passwordHash);

                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show(
                    "Registration completed successfully!",
                    "WeatherRescue");

                LoginWindow login = new LoginWindow();
                login.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Registration Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void BackToLogin_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();
            login.Show();
            this.Close();
        }
    }
}