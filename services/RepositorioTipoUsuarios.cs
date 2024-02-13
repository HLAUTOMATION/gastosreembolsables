using Dapper;
using GASTOS_REEMBOLSABLES_VMICA.Models;
using Microsoft.Data.SqlClient;

namespace GASTOS_REEMBOLSABLES_VMICA.services
{
    public interface IRepositorioTipoUsuario
    {
        Task Crear(TipoUsuario tipoUsuario);
        Task Editar(TipoUsuario tipoUsuario);
        Task<TipoUsuario> GetTipoUsuarioById(int Id);
        Task<IEnumerable<TipoUsuario>> ListarTipoUsuario();
    }

    public class RepositorioTipoUsuario : IRepositorioTipoUsuario
    {

        private readonly String connectionString;
        public RepositorioTipoUsuario(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("defaultConnection");
        }

        public async Task<IEnumerable<TipoUsuario>> ListarTipoUsuario()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<TipoUsuario>(
                "ListarTipoUsuario",
                commandType:System.Data.CommandType.StoredProcedure );

        }

        public async Task Crear(TipoUsuario tipoUsuario)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(
                                                     "CrearTipoUsuario",
                                                     new
                                                     {
                                                         tipoUsuario.Nombre,                                           
                                                     },
                                                     commandType: System.Data.CommandType.StoredProcedure);
            tipoUsuario.Id = id;

        }

        public async Task<TipoUsuario> GetTipoUsuarioById(int Id)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<TipoUsuario>(
                "GetTipoUsuarioById",
                new
                {
                Id
                },
                 commandType: System.Data.CommandType.StoredProcedure
                );
        }

    
        public async Task Editar(TipoUsuario tipoUsuario)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(
                "EditarTipoUsuario",
                new
                {
                    tipoUsuario.Id,
                    tipoUsuario.Nombre  
                },
                commandType: System.Data.CommandType.StoredProcedure
                );
        }
    }
}
