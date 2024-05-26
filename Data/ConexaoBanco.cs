using System.Configuration;
using System.Data.SqlClient;

namespace OficinaEletrodomesticos.Data
{
    public class ConexaoBanco
    {
        public static SqlConnection ConectaBanco()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;

            return new SqlConnection(connectionString);
        }
    }
}