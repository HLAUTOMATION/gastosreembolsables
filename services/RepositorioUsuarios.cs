using Dapper;
using GASTOS_REEMBOLSABLES_VMICA.Models;
using Microsoft.Data.SqlClient;

namespace GASTOS_REEMBOLSABLES_VMICA.services
{
    public interface IRepositorioUsuarios
    {
        Task<int> Crear(AppUsuario usuario);
        Task<AppUsuario> GetUsuarioByEmail(String emailNormalizado);
        Task<AppUsuario> GetUsuarioById(string Id);
    }
    public class RepositorioUsuarios : IRepositorioUsuarios
    {
        private readonly String connectionString;
        public RepositorioUsuarios(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<int> Crear(AppUsuario usuario)
        {
            using var connection = new SqlConnection(connectionString);
       
            var id=await connection.QuerySingleAsync<int>(
                "CrearUsuario",
                new
                {
                    usuario.Email,
                    usuario.NormalizedEmail,
                    usuario.PasswordHash
                },
                commandType: System.Data.CommandType.StoredProcedure);

            return id;

        }

        public async Task<AppUsuario> GetUsuarioByEmail(String emailNormalizado)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QuerySingleOrDefaultAsync<AppUsuario>(

                "GetUsuarioByEmail",
                new
                {
                    emailNormalizado
                },
                commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<AppUsuario> GetUsuarioById(string Id)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QuerySingleOrDefaultAsync<AppUsuario>(

                "GetUsuarioById",
                new
                {
                    Id
                },
                commandType: System.Data.CommandType.StoredProcedure);
        }
    }
}
