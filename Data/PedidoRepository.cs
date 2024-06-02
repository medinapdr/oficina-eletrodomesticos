using System.Data.SqlClient;
using OficinaEletrodomesticos.Models;

namespace OficinaEletrodomesticos.Data
{
    public class PedidoRepository
    {
        public static List<Pedido> ConsultarPedidos()
        {
            const string query = @"SELECT Id, PecaId, NomePeca, Quantidade, ValorTotal, Fornecedor, DataCriacao, DataRecebimento, ValorUnitario FROM Pedido";

            var pedidos = new List<Pedido>();

            using var conexao = ConexaoBanco.ConectaBanco();
            conexao.Open();

            using var cmd = new SqlCommand(query, conexao);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                pedidos.Add(new Pedido
                {
                    Id = reader.GetInt32(0),
                    Peca = new Peca { Id = reader.GetInt32(1), Nome = reader.GetString(2) },
                    Quantidade = reader.GetInt32(3),
                    ValorTotal = reader.GetDecimal(4),
                    Fornecedor = reader.GetString(5),
                    DataCriacao = reader.GetDateTime(6),
                    DataRecebimento = reader.IsDBNull(7) ? (DateTime?)null : reader.GetDateTime(7),
                    ValorUnitario = reader.GetDecimal(8)
                });
            }
            return pedidos;
        }

        public static bool AdicionarPedido(Pedido pedido)
        {
            const string query = @"
            INSERT INTO Pedido (PecaId, NomePeca, Quantidade, ValorTotal, Fornecedor, DataCriacao, ValorUnitario) 
            VALUES (@PecaId, @NomePeca, @Quantidade, @ValorTotal, @Fornecedor, @DataCriacao, @ValorUnitario);
            SELECT SCOPE_IDENTITY();";

            using var conexao = ConexaoBanco.ConectaBanco();

            using var cmd = new SqlCommand(query, conexao);
            cmd.Parameters.AddWithValue("@PecaId", pedido.Peca.Id);
            cmd.Parameters.AddWithValue("@NomePeca", pedido.Peca.Nome);
            cmd.Parameters.AddWithValue("@Quantidade", pedido.Quantidade);
            cmd.Parameters.AddWithValue("@ValorTotal", pedido.ValorTotal);
            cmd.Parameters.AddWithValue("@Fornecedor", pedido.Fornecedor);
            cmd.Parameters.AddWithValue("@DataCriacao", pedido.DataCriacao);
            cmd.Parameters.AddWithValue("@ValorUnitario", pedido.ValorUnitario);

            try
            {
                conexao.Open();
                var result = cmd.ExecuteScalar();
                if (result != null)
                {
                    // Define o ID do pedido adicionado.
                    pedido.Id = Convert.ToInt32(result);
                    return true;
                }
                return false;
            }
            catch (SqlException)
            {
                return false;
            }
        }

        public static bool ConfirmarRecebimentoPedido(int pedidoId, DateTime dataRecebimento)
        {
            const string queryPedido = "SELECT PecaId, Quantidade FROM Pedido WHERE Id = @PedidoId";
            const string queryUpdatePedido = "UPDATE Pedido SET DataRecebimento = @DataRecebimento WHERE Id = @PedidoId";
            const string queryUpdatePeca = "UPDATE Peca SET Quantidade = Quantidade + @Quantidade WHERE Id = @PecaId";

            using var conexao = ConexaoBanco.ConectaBanco();
            conexao.Open();

            using var cmdPedido = new SqlCommand(queryPedido, conexao);
            cmdPedido.Parameters.AddWithValue("@PedidoId", pedidoId);

            using var reader = cmdPedido.ExecuteReader();
            if (!reader.Read()) return false; // Verifica se o pedido existe.
            int pecaId = reader.GetInt32(0);
            int quantidade = reader.GetInt32(1);
            reader.Close();

            using var transaction = conexao.BeginTransaction();
            using var cmdUpdatePedido = new SqlCommand(queryUpdatePedido, conexao, transaction);
            using var cmdUpdatePeca = new SqlCommand(queryUpdatePeca, conexao, transaction);
            cmdUpdatePedido.Parameters.AddWithValue("@PedidoId", pedidoId);
            cmdUpdatePedido.Parameters.AddWithValue("@DataRecebimento", dataRecebimento);
            cmdUpdatePeca.Parameters.AddWithValue("@PecaId", pecaId);
            cmdUpdatePeca.Parameters.AddWithValue("@Quantidade", quantidade);

            try
            {
                int rowsAffectedPedido = cmdUpdatePedido.ExecuteNonQuery();
                int rowsAffectedPeca = cmdUpdatePeca.ExecuteNonQuery();

                if (rowsAffectedPedido > 0 && rowsAffectedPeca > 0)
                {
                    transaction.Commit();
                    return true;
                }
                transaction.Rollback();
                return false;
            }
            catch (SqlException)
            {
                transaction.Rollback();
                return false;
            }
        }
    }
}
