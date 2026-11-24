using System;
using System.Data.SqlClient;
using System.Windows;

namespace WeatherRescue
{
    public partial class DatabaseTest : Window
    {
        public DatabaseTest()
        {
            InitializeComponent();
        }

        private void TestConnection_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DatabaseConnection db = new DatabaseConnection();

                using (SqlConnection connection = db.GetConnection())
                {
                    connection.Open();

                    using (SqlCommand command =
                        new SqlCommand("SELECT DB_NAME()", connection))
                    {
                        string databaseName = command.ExecuteScalar().ToString();

                        StatusText.Text = "Connected successfully!\nDatabase: " + databaseName;
                    }
                }
            }
            catch (Exception ex)
            {
                StatusText.Text = "Connection failed!";
                MessageBox.Show(ex.Message, "Database Error");
            }
        }
    }
}