using ControleEstoque.Models;
using ControleEstoque.Data;

namespace ControleEstoque
{
    public class Program
    {
        private static readonly ProdutoRepository repository = new ProdutoRepository();

        public static void Main(string[] args)
        {
            Console.WriteLine("=== SISTEMA DE CONTROLE DE ESTOQUE ===\n");

            // Testa a conexão com o banco
            DatabaseConfig.TestConnection();
            Console.WriteLine();

            bool continuar = true;

            while (continuar)
            {
                ExibirMenu();
                string opcao = Console.ReadLine() ?? "";

                switch (opcao)
                {
                    case "1":
                        CadastrarProduto();
                        break;
                    case "2":
                        ListarProdutos();
                        break;
                    case "3":
                        IncrementarEstoque();
                        break;
                    case "4":
                        DecrementarEstoque();
                        break;
                    case "5":
                        BuscarProduto();
                        break;
                    case "0":
                        continuar = false;
                        Console.WriteLine("Encerrando o sistema...");
                        break;
                    default:
                        Console.WriteLine("Opção inválida! Tente novamente.\n");
                        break;
                }

                if (continuar)
                {
                    Console.WriteLine("\nPressione qualquer tecla para continuar...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }

        private static void ExibirMenu()
        {
            Console.WriteLine("=== MENU PRINCIPAL ===");
            Console.WriteLine("1 - Cadastrar Produto");
            Console.WriteLine("2 - Listar Produtos");
            Console.WriteLine("3 - Incrementar Estoque");
            Console.WriteLine("4 - Decrementar Estoque");
            Console.WriteLine("5 - Buscar Produto");
            Console.WriteLine("0 - Sair");
            Console.Write("Escolha uma opção: ");
        }

        private static void CadastrarProduto()
        {
            Console.WriteLine("\n=== CADASTRAR PRODUTO ===");

            Console.Write("Nome do produto: ");
            string nome = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(nome))
            {
                Console.WriteLine("Nome é obrigatório!");
                return;
            }

            Console.Write("Preço: R$ ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal preco) || preco < 0)
            {
                Console.WriteLine("Preço inválido!");
                return;
            }

            Console.Write("Quantidade inicial (Enter para 0): ");
            string quantidadeInput = Console.ReadLine() ?? "";
            int quantidade = 0;

            if (!string.IsNullOrWhiteSpace(quantidadeInput))
            {
                if (!int.TryParse(quantidadeInput, out quantidade) || quantidade < 0)
                {
                    Console.WriteLine("Quantidade inválida!");
                    return;
                }
            }

            var produto = new Produto(nome, preco, quantidade);

            if (repository.CadastrarProduto(produto))
            {
                Console.WriteLine("✓ Produto cadastrado com sucesso!");
            }
            else
            {
                Console.WriteLine("✗ Erro ao cadastrar produto!");
            }
        }

        private static void ListarProdutos()
        {
            Console.WriteLine("\n=== LISTA DE PRODUTOS ===");

            var produtos = repository.ListarTodosProdutos();

            if (!produtos.Any())
            {
                Console.WriteLine("Nenhum produto cadastrado.");
                return;
            }

            Console.WriteLine($"{"ID",-4} {"Nome",-20} {"Preço",-12} {"Quantidade",-10}");
            Console.WriteLine(new string('-', 50));

            foreach (var produto in produtos)
            {
                Console.WriteLine($"{produto.IdProduto,-4} {produto.Nome,-20} R$ {produto.Preco,-8:F2} {produto.Quantidade,-10}");
            }

            Console.WriteLine($"\nTotal de produtos: {produtos.Count}");
        }

        private static void IncrementarEstoque()
        {
            Console.WriteLine("\n=== INCREMENTAR ESTOQUE ===");

            Console.Write("ID do produto: ");
            if (!int.TryParse(Console.ReadLine(), out int id) || id <= 0)
            {
                Console.WriteLine("ID inválido!");
                return;
            }

            var produto = repository.BuscarPorId(id);
            if (produto == null)
            {
                Console.WriteLine("Produto não encontrado!");
                return;
            }

            Console.WriteLine($"Produto: {produto.Nome}");
            Console.WriteLine($"Quantidade atual: {produto.Quantidade}");

            Console.Write("Quantidade a adicionar: ");
            if (!int.TryParse(Console.ReadLine(), out int quantidade) || quantidade <= 0)
            {
                Console.WriteLine("Quantidade inválida!");
                return;
            }

            if (repository.IncrementarQuantidade(id, quantidade))
            {
                Console.WriteLine($"✓ Estoque incrementado! Nova quantidade: {produto.Quantidade + quantidade}");
            }
            else
            {
                Console.WriteLine("✗ Erro ao incrementar estoque!");
            }
        }

        private static void DecrementarEstoque()
        {
            Console.WriteLine("\n=== DECREMENTAR ESTOQUE ===");

            Console.Write("ID do produto: ");
            if (!int.TryParse(Console.ReadLine(), out int id) || id <= 0)
            {
                Console.WriteLine("ID inválido!");
                return;
            }

            var produto = repository.BuscarPorId(id);
            if (produto == null)
            {
                Console.WriteLine("Produto não encontrado!");
                return;
            }

            Console.WriteLine($"Produto: {produto.Nome}");
            Console.WriteLine($"Quantidade atual: {produto.Quantidade}");

            Console.Write("Quantidade a remover: ");
            if (!int.TryParse(Console.ReadLine(), out int quantidade) || quantidade <= 0)
            {
                Console.WriteLine("Quantidade inválida!");
                return;
            }

            if (repository.DecrementarQuantidade(id, quantidade))
            {
                Console.WriteLine($"✓ Estoque decrementado! Nova quantidade: {produto.Quantidade - quantidade}");
            }
            else
            {
                Console.WriteLine("✗ Erro ao decrementar estoque ou quantidade insuficiente!");
            }
        }

        private static void BuscarProduto()
        {
            Console.WriteLine("\n=== BUSCAR PRODUTO ===");

            Console.Write("ID do produto: ");
            if (!int.TryParse(Console.ReadLine(), out int id) || id <= 0)
            {
                Console.WriteLine("ID inválido!");
                return;
            }

            var produto = repository.BuscarPorId(id);
            if (produto == null)
            {
                Console.WriteLine("Produto não encontrado!");
                return;
            }

            Console.WriteLine("\n=== DADOS DO PRODUTO ===");
            Console.WriteLine($"ID: {produto.IdProduto}");
            Console.WriteLine($"Nome: {produto.Nome}");
            Console.WriteLine($"Preço: R$ {produto.Preco:F2}");
            Console.WriteLine($"Quantidade em estoque: {produto.Quantidade}");
        }
    }
}