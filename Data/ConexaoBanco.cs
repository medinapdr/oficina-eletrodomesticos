using System.Data.SqlClient;

namespace OficinaEletrodomesticos.Data
{
    public class ConexaoBanco
    {
        public static SqlConnection ConectaBanco()
        {
            string connectionString = "Server=tcp:oficinaeletro.database.windows.net,1433;Initial Catalog=Oficina;Persist Security Info=False;User ID=oficinator;Password=DCC603@admin;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            return new SqlConnection(connectionString);
        }
    }
}