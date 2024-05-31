namespace OficinaEletrodomesticos.Models
{
    public class Peca
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public decimal? Altura { get; set; }
        public decimal? Largura { get; set; }
        public decimal? Comprimento { get; set; }
        public decimal? Peso { get; set; }
        public string Fabricante { get; set; }
        public decimal Preco { get; set; }
        public int Quantidade { get; set; }

    }
}
