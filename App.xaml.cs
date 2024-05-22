using OficinaEletrodomesticos.Data;
using System.Windows;

namespace OficinaEletrodomesticos
{
    public class Usuario
    {
        public string NomeUsuario { get; set; }
        public string Senha { get; set; }
        public Pessoa PessoaAssociada { get; set; }
    }

    public enum Cargo
    {
        Vendedor,
        Técnico,
        Gerente,
        Administrador
    }

    public enum Departamento
    {
        Vendas,
        Serviços,
        Gerência
    }

    public enum StatusServico
    {
        Parado = 0,
        Iniciado = 1,
        EmAndamento = 2,
        EmTeste = 3,
        Testado = 4,
        Realizado = 5
    }

    public class Pessoa
    {
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Telefone { get; set; }
        public string Endereco { get; set; }
        public string TipoPessoa { get; set; }
    }

    public class Cliente : Pessoa
    {
        public SolicitacaoOrcamento solicitarOrcamento(Aparelho aparelho, string defeito)
        {
            return new SolicitacaoOrcamento(aparelho, this, defeito);
        }
    }

    public class Funcionario : Pessoa
    {
        public Cargo Cargo { get; set; }
        public decimal Salario { get; set; }
        public Departamento Departamento { get; set; }
    }

    public class Tecnico : Funcionario
    {
        public List<Servico> servicosAtribuidos { get; } = new List<Servico>();

        public void RealizarServico(Servico servico, StatusServico status)
        {
            if (servico.TecnicoResponsavel != this)
            {
                throw new InvalidOperationException("Você não é o técnico responsável por este serviço.");
            }

            MessageBox.Show($"Realizando serviço com status {status}");
            servico.Status = status;
        }
    }

    public class Vendedor : Funcionario
    {
        public Orcamento CriarOrcamento(SolicitacaoOrcamento solicitacao)
        {
            var orcamento = new Orcamento();
            return orcamento;
        }
    }

    public class Gerente : Funcionario
    {
    }
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

    public class Aparelho
    {
        public string Tipo { get; set; }
        public string Marca { get; set; }
        public DateTime DataEntrada { get; set; }
        public Cliente ClienteAssociado { get; set; }

        public void AssociarCliente(Cliente cliente)
        {
            ClienteAssociado = cliente;
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
            ValorTotal = (ValorPecas * 1.5m) + ValorLogaritmico;
        }

        public void CalcularPrazoEntrega()
        {
            double diasNecessarios = Math.Sqrt((PecasNecessarias.Sum(p => p.Item2) * 2));
            PrazoEntrega = TimeSpan.FromDays(diasNecessarios);
        }
    }

    public class Pedido(List<(Peca, int)> pecasQuantidade, decimal valorTotal, string fornecedor)
    {
        public List<(Peca, int)> PecasQuantidade { get; } = pecasQuantidade;
        public decimal ValorTotal { get; } = valorTotal;
        public string Fornecedor { get; } = fornecedor;
    }

    public class Peca
    {
        public int? Id { get; set; } 
        public string Nome { get; set; }
        public decimal Altura { get; set; }
        public decimal Largura { get; set; }
        public decimal Comprimento  { get; set; }

        public decimal Peso { get; set; }
        public string Fabricante { get; set; }
        public decimal Preco { get; set; }
        public int Quantidade { get; set; }

        public Peca() { }
        public Peca(string nome, decimal preco, decimal altura, decimal largura, decimal comprimento, decimal peso, string fabricante, int quantidade)
        {
            this.Nome = nome;
            this.Preco = preco;
            this.Altura = altura;
            this.Largura = largura; 
            this.Comprimento = comprimento;
            this.Peso = peso;
            this.Fabricante = fabricante;
            this.Quantidade = quantidade;
        }
    }

    public class Servico
    {
        public Tecnico TecnicoResponsavel { get; set; }
        public Aparelho Aparelho { get; set; }
        public Orcamento OrcamentoAssociado { get; set; }
        public string Descricao { get; set; }
        public double? ValorPagamento { get; set; }
        public DateTime? DataPagamento { get; set; }
        public StatusServico Status { get; set; }

        public Servico(Orcamento orcamento, Tecnico tecnico)
        {
            OrcamentoAssociado = orcamento;
            Aparelho = orcamento.AparelhoRelacionado;
            TecnicoResponsavel = tecnico;

            orcamento.Autorizado = true;
        }
    }
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

        public void AdicionarPedido(List<(Peca, int)> pecasQuantidade, string fornecedor)
        {
            decimal valorTotal = 0;

            foreach (var (peca, quantidade) in pecasQuantidade)
            {
                valorTotal += peca.Preco * quantidade;
            }

            var pedido = new Pedido(pecasQuantidade, valorTotal, fornecedor);
            PedidosPendentes.Add(pedido);

            MessageBox.Show($"Pedido de valor total {valorTotal} feito ao fornecedor {fornecedor}.");
        }
    }

    public class Historico
    {
        public List<SolicitacaoOrcamento> Solicitacoes { get; } = [];
        public List<Orcamento> Orcamentos { get; } = [];
        public List<Servico> Servicos { get; } = [];

        public void AdicionarSolicitacao(SolicitacaoOrcamento solicitacao)
        {
            Solicitacoes.Add(solicitacao);
        }

        public void AdicionarOrcamento(Orcamento orcamento)
        {
            Orcamentos.Add(orcamento);
        }

        public void AdicionarServico(Servico servico)
        {
            Servicos.Add(servico);
        }
    }
}
