namespace OficinaEletrodomesticos.Models
{
    public class Servico
    {
        public int Id { get; set; }
        public Funcionario TecnicoResponsavel { get; set; }
        public string NomeTecnico { get; set; }
        public Orcamento Orcamento { get; set; }
        public string Descricao { get; set; }
        public double? ValorPagamento { get; set; }
        public DateTime? DataPagamento { get; set; }
        public StatusServico Status { get; set; } = StatusServico.Parado;

    }
}
    