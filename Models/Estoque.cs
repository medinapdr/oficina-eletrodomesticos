using System.Windows;
using OficinaEletrodomesticos.Data;

namespace OficinaEletrodomesticos.Models
{
    public class Estoque()
    {
        public List<Peca> Pecas { get; set; } = [];
        public List<Pedido> Pedidos { get; set; } = [];

        public void AdicionarPeca(Peca peca)
        {
            var retorno = PecaRepository.AdicionarPeca(peca);
            MessageBox.Show(retorno ? $"Peça {peca.Nome} adicionada ao estoque." : "Falha ao adicionar peça ao estoque.");
        }

        public void RemoverPeca(Peca peca)
        {
            var retorno = PecaRepository.RemoverPeca(peca);
            MessageBox.Show(retorno ? $"Peça {peca.Nome} removida do estoque." : $"Falha ao remover peça {peca.Nome} do estoque.");
        }

        public List<Peca> ConsultarEstoque()
        {
            return PecaRepository.ConsultarEstoque();
        }

        public void AtualizarPeca(Peca peca)
        {
            var retorno = PecaRepository.AtualizarPeca(peca);
            MessageBox.Show(retorno ? $"Peça {peca.Nome} atualizada no estoque." : $"Falha ao atualizar peça {peca.Nome}.");
        }
        public List<Pedido> ConsultarPedidos()
        {
            return PedidoRepository.ConsultarPedidos();
        }

        public bool AdicionarPedido(Pedido pedido)
        {
            var retorno = PedidoRepository.AdicionarPedido(pedido);
            if (retorno)
            {
                Pedidos.Add(pedido);
                MessageBox.Show("Pedido adicionado com sucesso.");
                return true;
            }
            else
            {
                MessageBox.Show("Falha ao adicionar pedido.");
                return false;
            }
        }

        public bool ConfirmarRecebimentoPedido(Pedido pedido)
        {
            if (pedido.DataRecebimento == null)
            {
                var retorno = PedidoRepository.ConfirmarRecebimentoPedido(pedido.Id, DateTime.Now);
                if (retorno)
                {
                    pedido.DataRecebimento = DateTime.Now;
                    AdicionarPecaAoEstoque(pedido.Peca);
                    MessageBox.Show($"Pedido recebido em {pedido.DataRecebimento} e peça adicionada ao estoque.");
                    return true;
                }
                else
                {
                    MessageBox.Show("Falha ao confirmar o recebimento do pedido.");
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private void AdicionarPecaAoEstoque(Peca peca)
        {
            var pecaExistente = Pecas.FirstOrDefault(p => p.Nome == peca.Nome && p.Fabricante == peca.Fabricante);
            if (pecaExistente != null)
            {
                pecaExistente.Quantidade += peca.Quantidade;
            }
            else
            {
                Pecas.Add(peca);
            }
        }
    }
}
