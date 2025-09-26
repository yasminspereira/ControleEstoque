namespace ControleEstoque.Models
{
    public class Produto
    {
        public int IdProduto { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public int Quantidade { get; set; }

        public Produto() { }

        public Produto(string nome, decimal preco, int quantidade = 0)
        {
            Nome = nome;
            Preco = preco;
            Quantidade = quantidade;
        }

        public override string ToString()
        {
            return $"ID: {IdProduto} | Nome: {Nome} | Preço: R$ {Preco:F2} | Quantidade: {Quantidade}";
        }
    }
}