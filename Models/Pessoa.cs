using System.Windows;

namespace OficinaEletrodomesticos.Models
{
    public class Pessoa
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string? Telefone { get; set; }
        public string? Endereco { get; set; }
        public string TipoPessoa { get; set; }
    }

    public class Cliente : Pessoa
    {
    }

    public class Funcionario : Pessoa
    {
        public Cargo Cargo { get; set; }
        public decimal Salario { get; set; }
        public Departamento Departamento { get; set; }
    }

    public class Tecnico : Funcionario
    {
    }

    public class Vendedor : Funcionario
    {
    }

    public class Gerente : Funcionario
    {
    }
}
