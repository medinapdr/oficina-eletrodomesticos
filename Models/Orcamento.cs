using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace OficinaEletrodomesticos.Models
{
    public class SolicitacaoOrcamento
    {
        public string? Id { get; set; }
        public Aparelho Aparelho { get; set; }
        public Cliente Cliente { get; set; }
        public string? Descricao { get; set; }
    }

    public class Orcamento
    {
        public string? Id { get; set; }
        public SolicitacaoOrcamento Solicitacao { get; set; }
        public DateTime DataOrcamento { get; set; }
        public List<(Peca, int)>? PecasNecessarias { get; set; } = [];
        public decimal ValorTotal { get; set; }
        public TimeSpan PrazoEntrega { get; set; }
        public bool Autorizado { get; set; }

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
            if (PecasNecessarias == null || PecasNecessarias.Count == 0)
            {
                ValorTotal = 0;
                return;
            }

            decimal valorPecas = PecasNecessarias.Sum(pecaQuantidade => pecaQuantidade.Item1.Preco * pecaQuantidade.Item2);
            double prazoEntregaDias = PrazoEntrega.TotalDays;
            decimal valorLogaritmico = (decimal)Math.Log(prazoEntregaDias + 1);
            ValorTotal = valorPecas * 1.5m + valorLogaritmico;
        }

        public void CalcularValorTotal(decimal valorPadrao) 
        {
            if (PecasNecessarias == null || PecasNecessarias.Count == 0)
            {
                ValorTotal = valorPadrao;
                return;
            }

            ValorTotal = valorPadrao;
        }

        public void CalcularPrazoEntrega()
        {
            if (PecasNecessarias == null || PecasNecessarias.Count == 0)
            {
                PrazoEntrega = TimeSpan.Zero;
                return;
            }

            double diasNecessarios = Math.Sqrt(PecasNecessarias.Sum(p => p.Item2) * 2);
            PrazoEntrega = TimeSpan.FromDays(diasNecessarios);
        }

        public void CalcularPrazoEntrega(TimeSpan prazoPadrao)
        {
            if (PecasNecessarias == null || PecasNecessarias.Count == 0)
            {
                PrazoEntrega = prazoPadrao;
                return;
            }

            PrazoEntrega = prazoPadrao;
        }
    }
}
