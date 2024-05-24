using OficinaEletrodomesticos.Data;
using System.Windows;

namespace OficinaEletrodomesticos.Models
{
    public class Estoque
    {
        private readonly ConexaoBanco? _ConexaoBanco = new();
        public List<Peca> Pecas { get; set; } = new List<Peca>();
        public List<Pedido> Pedidos { get; set;  } = new List<Pedido>();

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
        public List<Pedido> ConsultarPedidos()
        {
            ConexaoBanco conexaoBanco = new ConexaoBanco();
            return conexaoBanco.ConsultarPedidos();
        }

        public bool AdicionarPedido(Pedido pedido)
        {
            var retorno = _ConexaoBanco.AdicionarPedido(pedido);
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
                var retorno = _ConexaoBanco.ConfirmarRecebimentoPedido(pedido.Id, DateTime.Now);
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
