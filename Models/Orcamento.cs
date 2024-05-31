using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace OficinaEletrodomesticos.Models
{
    public class SolicitacaoOrcamento
    {
        public int Id { get; set; }
        public Aparelho Aparelho { get; set; }
        public Cliente Cliente { get; set; }
        public DateTime DataSolicitacao { get; set; }
        public string? Descricao { get; set; }
    }

    public class Orcamento
    {
        public int Id { get; set; }
        public SolicitacaoOrcamento Solicitacao { get; set; }
        public DateTime DataOrcamento { get; set; }
        public List<(Peca, int)>? PecasNecessarias { get; set; } = [];
        public decimal ValorTotal { get; set; }
        public DateTime PrazoEntrega { get; set; }
        public bool Autorizado { get; set; }
    }
}
