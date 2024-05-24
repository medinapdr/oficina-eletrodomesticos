using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        Iniciado = 1,
        EmAndamento = 2,
        EmTeste = 3,
        Testado = 4,
        Realizado = 5
    }
}
