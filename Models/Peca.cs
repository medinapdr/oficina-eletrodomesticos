namespace OficinaEletrodomesticos.Models
{
    public class Peca
    {
        public int? Id { get; set; }
        public string Nome { get; set; }
        public decimal? Altura { get; set; }
        public decimal? Largura { get; set; }
        public decimal? Comprimento { get; set; }
        public decimal? Peso { get; set; }
        public string Fabricante { get; set; }
        public decimal Preco { get; set; }
        public int Quantidade { get; set; }

        public Peca() { }
        public Peca(string nome, decimal preco, decimal? altura, decimal? largura, decimal? comprimento, decimal? peso, string fabricante, int quantidade)
        {
            Nome = nome;
            Preco = preco;
            Altura = altura;
            Largura = largura;
            Comprimento = comprimento;
            Peso = peso;
            Fabricante = fabricante;
            Quantidade = quantidade;
        }
    }
}
