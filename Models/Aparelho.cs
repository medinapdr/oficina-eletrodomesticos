namespace OficinaEletrodomesticos.Models
{
    public class Aparelho
    {
        public string? Id { get; set; }
        public Cliente ClienteAssociado { get; set; }
        public string Tipo { get; set; }
        public string Marca { get; set; }
    }
}
