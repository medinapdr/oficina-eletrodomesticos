namespace OficinaEletrodomesticos.Models
{
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
}
