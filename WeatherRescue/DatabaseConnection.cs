using System.Data.SqlClient;

namespace WeatherRescue
{
    public class DatabaseConnection
    {
        private readonly string connectionString =
            @"Server=localhost\SQL2019;Database=WeatherRescueDB;Integrated Security=True;TrustServerCertificate=True;";

        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}