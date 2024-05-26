using System.Windows;

namespace OficinaEletrodomesticos.Models
{
    public class Pessoa
    {
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string? Telefone { get; set; }
        public string? Endereco { get; set; }
        public string TipoPessoa { get; set; }
    }

    public class Cliente : Pessoa
    {
        public void SolicitarOrcamento(Aparelho aparelho, string defeito)
        {
            
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
}
