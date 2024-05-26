namespace OficinaEletrodomesticos.Models
{
    public class Servico
    {
        public Tecnico TecnicoResponsavel { get; set; }
        public Orcamento Orcamento { get; set; }
        public string Descricao { get; set; }
        public double? ValorPagamento { get; set; }
        public DateTime? DataPagamento { get; set; }
        public StatusServico Status { get; set; } = StatusServico.Parado;

    }
}
