using Npgsql;

namespace ControleEstoque.Data
{
    public static class DatabaseConfig
    {
        // ALTERE AQUI: suas credenciais do PostgreSQL
        private static readonly string connectionString =
            "Host=localhost;Port=5432;Database=estoque_db;Username=postgres;Password=bancodedados";

        public static NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(connectionString);
        }

        public static void TestConnection()
        {
            try
            {
                using var connection = GetConnection();
                connection.Open();
                Console.WriteLine("✓ Conexão com o banco de dados estabelecida com sucesso!");
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Erro ao conectar com o banco: {ex.Message}");
            }
        }
    }
}