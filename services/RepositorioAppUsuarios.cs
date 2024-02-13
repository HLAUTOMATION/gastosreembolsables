using Dapper;
using DocumentFormat.OpenXml.Spreadsheet;
using GASTOS_REEMBOLSABLES_VMICA.Models;
using Microsoft.Data.SqlClient;

namespace GASTOS_REEMBOLSABLES_VMICA.services
{

    public interface IRepositorioAppUsuarios
    {
		Task Borrar(int id);
		Task<int> ContarUsuarios();
        Task Crear(AppUsuario appUsuario);
        Task Editar(AppUsuario appUsuario);
        Task<AppUsuario> GetAppUsuarioByEmail(string Email);
        Task<AppUsuario> GetAppUsuarioById(int id);
        Task<IEnumerable<AppUsuario>> ListarAppUsuarios();
        Task<IEnumerable<AppUsuario>> ListarAppUsuariosPaginacion(PaginacionViewModel paginacionViewModel);
    }
    public class RepositorioAppUsuarios: IRepositorioAppUsuarios

    {
        private readonly String connectionString;
        public RepositorioAppUsuarios(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("defaultConnection");
        }

        public async Task<IEnumerable<AppUsuario>> ListarAppUsuarios()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<AppUsuario>(
                "ListarAppUsuarios", 
                 commandType: System.Data.CommandType.StoredProcedure
                );
        }

        public async Task<AppUsuario> GetAppUsuarioByEmail(string Email)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<AppUsuario>(
                "GetAppUsuarioByEmail",
                new
                {
                    Email
                },
                 commandType: System.Data.CommandType.StoredProcedure
                );
        }

        public async Task Crear (AppUsuario appUsuario)
        {
            using var connection=new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(
                "CrearAspUsuario",
                new
                {
                    appUsuario.EmailUsuario,
                    appUsuario.IdPerfile,
                    appUsuario.FechaContrato,
                    appUsuario.IdProyecto,
                    appUsuario.TipoContrato,
                    appUsuario.Rut,
                    appUsuario.FechaNacimiento,
                    appUsuario.FechaCreacion
                },
                commandType: System.Data.CommandType.StoredProcedure
                );
            appUsuario.Id = id;
        }

        public async Task Editar(AppUsuario appUsuario)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(
                "EditarAppUsuario",
                new
                {
                    appUsuario.Id,
                    appUsuario.Email,
                    appUsuario.Rut,
                    appUsuario.FechaNacimiento,
                    appUsuario.FechaContrato,
                    appUsuario.TipoContrato,
                    appUsuario.IdProyecto
                },
                commandType: System.Data.CommandType.StoredProcedure
                );
        }

        public async Task<IEnumerable<AppUsuario>> ListarAppUsuariosPaginacion(PaginacionViewModel paginacionViewModel)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<AppUsuario>(
                "ListarAppUsuariosPaginacion", new
                {
                    paginacionViewModel.RecordsASaltar,
                    paginacionViewModel.RecordsPorPagina
                },
                commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<int> ContarUsuarios()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.ExecuteScalarAsync<int>(
                "ContarUsuarios",
                commandType: System.Data.CommandType.StoredProcedure
                );
        }

        public async Task<AppUsuario> GetAppUsuarioById(int Id)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<AppUsuario>(
                "GetAppUsuarioById",
            new
                {
                    Id
                },
                 commandType: System.Data.CommandType.StoredProcedure
                );
        }

        public async Task Borrar(int Id)
        {
            using var connection=new SqlConnection(connectionString);
            await connection.QueryAsync<AppUsuario>(
                "DesactivarAppUsuario",
                new
                {
                    Id
                },
                commandType: System.Data.CommandType.StoredProcedure
                );
        }
    }
}
