using ControleEstoque.Models;
using Npgsql;
using System.Data;

namespace ControleEstoque.Data
{
    public class ProdutoRepository
    {
        public bool CadastrarProduto(Produto produto)
        {
            try
            {
                using var connection = DatabaseConfig.GetConnection();
                connection.Open();

                string sql = @"INSERT INTO produto (nome, preco, quantidade) 
                              VALUES (@nome, @preco, @quantidade)";

                using var command = new NpgsqlCommand(sql, connection);
                command.Parameters.AddWithValue("@nome", produto.Nome);
                command.Parameters.AddWithValue("@preco", produto.Preco);
                command.Parameters.AddWithValue("@quantidade", produto.Quantidade);

                int result = command.ExecuteNonQuery();
                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao cadastrar produto: {ex.Message}");
                return false;
            }
        }

        public List<Produto> ListarTodosProdutos()
        {
            var produtos = new List<Produto>();

            try
            {
                using var connection = DatabaseConfig.GetConnection();
                connection.Open();

                string sql = "SELECT id_produto, nome, preco, quantidade FROM produto ORDER BY nome";

                using var command = new NpgsqlCommand(sql, connection);
                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    produtos.Add(new Produto
                    {
                        IdProduto = reader.GetInt32("id_produto"),
                        Nome = reader.GetString("nome"),
                        Preco = reader.GetDecimal("preco"),
                        Quantidade = reader.GetInt32("quantidade")
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao listar produtos: {ex.Message}");
            }

            return produtos;
        }

        public Produto? BuscarPorId(int id)
        {
            try
            {
                using var connection = DatabaseConfig.GetConnection();
                connection.Open();

                string sql = "SELECT id_produto, nome, preco, quantidade FROM produto WHERE id_produto = @id";

                using var command = new NpgsqlCommand(sql, connection);
                command.Parameters.AddWithValue("@id", id);

                using var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new Produto
                    {
                        IdProduto = reader.GetInt32("id_produto"),
                        Nome = reader.GetString("nome"),
                        Preco = reader.GetDecimal("preco"),
                        Quantidade = reader.GetInt32("quantidade")
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar produto: {ex.Message}");
            }

            return null;
        }

        public bool IncrementarQuantidade(int idProduto, int quantidade)
        {
            try
            {
                using var connection = DatabaseConfig.GetConnection();
                connection.Open();

                string sql = "UPDATE produto SET quantidade = quantidade + @quantidade WHERE id_produto = @id";

                using var command = new NpgsqlCommand(sql, connection);
                command.Parameters.AddWithValue("@quantidade", quantidade);
                command.Parameters.AddWithValue("@id", idProduto);

                int result = command.ExecuteNonQuery();
                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao incrementar quantidade: {ex.Message}");
                return false;
            }
        }

        public bool DecrementarQuantidade(int idProduto, int quantidade)
        {
            try
            {
                using var connection = DatabaseConfig.GetConnection();
                connection.Open();

                // Primeiro verifica se há quantidade suficiente
                var produto = BuscarPorId(idProduto);
                if (produto == null)
                {
                    Console.WriteLine("Produto não encontrado!");
                    return false;
                }

                if (produto.Quantidade < quantidade)
                {
                    Console.WriteLine($"Quantidade insuficiente! Disponível: {produto.Quantidade}");
                    return false;
                }

                string sql = "UPDATE produto SET quantidade = quantidade - @quantidade WHERE id_produto = @id";

                using var command = new NpgsqlCommand(sql, connection);
                command.Parameters.AddWithValue("@quantidade", quantidade);
                command.Parameters.AddWithValue("@id", idProduto);

                int result = command.ExecuteNonQuery();
                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao decrementar quantidade: {ex.Message}");
                return false;
            }
        }
    }
}