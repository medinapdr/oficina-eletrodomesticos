using System.Windows;

namespace OficinaEletrodomesticos.Models
{
    public class SolicitacaoOrcamento
    {
        public Aparelho AparelhoAssociado { get; set; }
        public Cliente ClienteAssociado { get; set; }
        public string DescricaoDefeito { get; set; }

        public SolicitacaoOrcamento(Aparelho aparelho, Cliente cliente, string defeito)
        {
            AparelhoAssociado = aparelho;
            ClienteAssociado = cliente;
            DescricaoDefeito = defeito;
        }
    }

    public class Orcamento
    {
        public DateTime DataOrcamento { get; set; }
        public decimal ValorTotal { get; set; }
        public bool Autorizado { get; set; }
        public List<(Peca, int)> PecasNecessarias { get; set; }
        public Aparelho AparelhoRelacionado { get; set; }
        public string Descricao { get; set; }
        public TimeSpan PrazoEntrega { get; set; }
        public SolicitacaoOrcamento Solicitacao { get; set; }

        public Orcamento()
        {
            DataOrcamento = DateTime.Now;
            Descricao = "Orçamento padrão";
        }

        public Orcamento(SolicitacaoOrcamento solicitacaoOrcamento, string descricaoOrcamento)
        {
            DataOrcamento = DateTime.Now;
            Descricao = descricaoOrcamento;
            Solicitacao = solicitacaoOrcamento;
            AparelhoRelacionado = solicitacaoOrcamento.AparelhoAssociado;
        }

        public void AdicionarPeca((Peca, int) pecaQuantidade)
        {
            var (peca, quantidade) = pecaQuantidade;
            var item = PecasNecessarias.FirstOrDefault(p => p.Item1 == peca);
            if (item != default)
            {
                PecasNecessarias.Remove(item);
                PecasNecessarias.Add((item.Item1, item.Item2 + quantidade));
                MessageBox.Show($"Peça {peca.Nome} adicionada ao orçamento.");
            }
            else
            {
                PecasNecessarias.Add(pecaQuantidade);
                MessageBox.Show($"Peça {peca.Nome} adicionada ao orçamento.");
            }
        }

        public void RemoverPeca((Peca, int) pecaQuantidade)
        {
            var item = PecasNecessarias.FirstOrDefault(p => p.Item1 == pecaQuantidade.Item1);
            if (item != default)
            {
                if (pecaQuantidade.Item2 == 0)
                {
                    item = (item.Item1, 0);
                }
                else if (item.Item2 >= pecaQuantidade.Item2)
                {
                    item = (item.Item1, item.Item2 - pecaQuantidade.Item2);
                    MessageBox.Show($"Removidas {pecaQuantidade.Item2} peças de {item.Item1.Nome} do orçamento.");
                    if (item.Item2 == 0)
                        PecasNecessarias.Remove(item);
                }
                else
                {
                    MessageBox.Show($"Quantidade insuficiente de {item.Item1.Nome} no orçamento.");
                }
            }
            else
            {
                MessageBox.Show($"Peça {pecaQuantidade.Item1.Nome} não encontrada no orçamento.");
            }
        }

        public void CalcularValorTotal()
        {
            decimal ValorPecas = PecasNecessarias.Sum(pecaQuantidade => pecaQuantidade.Item1.Preco * pecaQuantidade.Item2);
            double PrazoEntregaDias = PrazoEntrega.TotalDays;
            decimal ValorLogaritmico = (decimal)Math.Log(PrazoEntregaDias + 1);
            ValorTotal = ValorPecas * 1.5m + ValorLogaritmico;
        }

        public void CalcularPrazoEntrega()
        {
            double diasNecessarios = Math.Sqrt(PecasNecessarias.Sum(p => p.Item2) * 2);
            PrazoEntrega = TimeSpan.FromDays(diasNecessarios);
        }
    }
}
