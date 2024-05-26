using System.Data.SqlClient;

namespace OficinaEletrodomesticos.Data
{
    public class ConexaoBanco
    {
        private readonly string _connectionString;

        public ConexaoBanco()
        {
            _connectionString = "Server=tcp:oficinaeletro.database.windows.net,1433;Initial Catalog=Oficina;Persist Security Info=False;User ID=oficinator;Password=DCC603@admin;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        }

        public SqlConnection ConectaBanco()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
