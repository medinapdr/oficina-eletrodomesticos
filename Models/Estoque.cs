using OficinaEletrodomesticos.Data;
using System.Windows;

namespace OficinaEletrodomesticos.Models
{
    public class Estoque
    {
        private readonly ConexaoBanco? _ConexaoBanco = new();
        public List<Peca> Pecas { get; set; } = new List<Peca>();
        public List<Pedido> PedidosPendentes { get; } = new List<Pedido>();

        public void AdicionarPeca(Peca peca)
        {
            var retorno = _ConexaoBanco.AdicionarPeca(peca);
            MessageBox.Show(retorno ? $"Peça {peca.Nome} adicionada ao estoque." : "Falha ao adicionar peça ao estoque.");
        }

        public void RemoverPeca(Peca peca)
        {
            var retorno = _ConexaoBanco.RemoverPeca(peca);
            MessageBox.Show(retorno ? $"Peça {peca.Nome} removida do estoque." : $"Falha ao remover peça {peca.Nome} do estoque.");
        }

        public List<Peca> ConsultarEstoque()
        {
            return _ConexaoBanco.ConsultarEstoque();
        }

        public void AtualizarPeca(Peca peca)
        {
            var retorno = _ConexaoBanco.AtualizarPeca(peca);
            MessageBox.Show(retorno ? $"Peça {peca.Nome} atualizada no estoque." : $"Falha ao atualizar peça {peca.Nome}.");
        }

        public void AdicionarPedido(List<Peca> pecas, string fornecedor)
        {
            decimal valorTotal = 0;

            foreach (var peca in pecas)
            {
                valorTotal += peca.Preco * peca.Quantidade;
            }

            var pedido = new Pedido(pecas, valorTotal, fornecedor);
            PedidosPendentes.Add(pedido);

            MessageBox.Show($"Pedido de valor total {valorTotal} feito ao fornecedor {fornecedor}.");
        }
    }
}
