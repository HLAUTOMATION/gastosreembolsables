using Dapper;
using GASTOS_REEMBOLSABLES_VMICA.Models;
using Microsoft.Data.SqlClient;

namespace GASTOS_REEMBOLSABLES_VMICA.services
{

    public interface IRepositorioCuentas
    {
        Task<IEnumerable<Cuenta>> GetCuentasByEmailAppUsuario(string Email);
        Task<IEnumerable<Cuenta>> GetCuentasByIdUsuario(int idUsuario);
        Task<IEnumerable<Cuenta>> ListarCuentas();
    }
    public class RepositorioCuentas : IRepositorioCuentas
    {
        private readonly String connectionString;

        public RepositorioCuentas(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("defaultConnection");
        }

        public async Task<IEnumerable<Cuenta>> ListarCuentas()
        {

            using var connection=new SqlConnection(connectionString);
            return await connection.QueryAsync<Cuenta>("ListarCuentas",commandType:System.Data.CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Cuenta>> GetCuentasByIdUsuario(int Id)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Cuenta>("GetCuentasByIdColaborador", 
                commandType: System.Data.CommandType.StoredProcedure);

        }

        public async Task<IEnumerable<Cuenta>> GetCuentasByEmailAppUsuario(string Email)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Cuenta>("GetCuentasByEmailAppUsuario",
                new
                {
                    Email
                },
                commandType: System.Data.CommandType.StoredProcedure);

        }
    }
}
