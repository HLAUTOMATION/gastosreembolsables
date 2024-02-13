using Dapper;
using GASTOS_REEMBOLSABLES_VMICA.Models;
using Microsoft.Data.SqlClient;

namespace GASTOS_REEMBOLSABLES_VMICA.services
{
    public interface IRepositorioTipoContratos
    {
        Task<TipoContrato> GetTipoContratoById(int Id);
        Task<IEnumerable<TipoContrato>> ListarTipoContratos();
    }

    public class RepositorioTipoContratos : IRepositorioTipoContratos
    {

        private readonly String connectionString;
        public RepositorioTipoContratos(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("defaultConnection");
        }

        public async Task<IEnumerable<TipoContrato>> ListarTipoContratos()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<TipoContrato>(
                "ListarTipoContratos",
                commandType:System.Data.CommandType.StoredProcedure );

        }

        public async Task<TipoContrato> GetTipoContratoById(int Id)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<TipoContrato>(
                "GetTipoContratoById",
                new
                {
                    Id
                },
                 commandType: System.Data.CommandType.StoredProcedure
                );
        }



    }
}
