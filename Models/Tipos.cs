namespace OficinaEletrodomesticos.Models
{
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
        EmAndamento = 1,
        Testado = 2,
        Realizado = 3
    }
}
