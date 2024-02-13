using AutoMapper;
using Dapper;
using GASTOS_REEMBOLSABLES_VMICA.Models;
using Microsoft.Data.SqlClient;

namespace GASTOS_REEMBOLSABLES_VMICA.services
{

    public interface IRepositorioPerfiles
    {
		Task Borrar(int id);
		Task<int> ContarPerfiles();
        Task Crear(Perfile perfile);
        Task Editar(Perfile perfile);
        Task<Perfile> GetPerfileById(int Id);
        Task<IEnumerable<Perfile>> ListarPerfiles();
        Task<IEnumerable<Perfile>> ListarPerfilesPaginacion(PaginacionViewModel paginacion);
    }
    public class RepositorioPerfiles : IRepositorioPerfiles
    {

        private readonly String connectionString;

        public RepositorioPerfiles(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("defaultConnection");
        }

        public async Task<IEnumerable<Perfile>> ListarPerfiles()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Perfile>("ListarPerfiles",commandType:System.Data.CommandType.StoredProcedure);
        }


        public async Task Crear(Perfile perfile)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(
                                                     "CrearPerfile",
                                                     new
                                                     {
                                                         Nombre = perfile.Nombre,
                                                     },
                                                     commandType: System.Data.CommandType.StoredProcedure);
            perfile.Id = id;

        }

        public async Task<Perfile> GetPerfileById(int Id)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Perfile>(
                "GetPerfileById",
                new
                {
                    Id
                },
                commandType: System.Data.CommandType.StoredProcedure
                );
        }

        public async Task Editar(Perfile perfile)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(
                "EditarPerfile",
                new
                {
                    perfile.Id,
                    perfile.Nombre
                    

                },
                commandType: System.Data.CommandType.StoredProcedure
                );
        }
        public async Task<IEnumerable<Perfile>> ListarPerfilesPaginacion(PaginacionViewModel paginacion)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Perfile>(
                "ListarPerfilesPaginacion", new
                {
                    paginacion.RecordsASaltar,
                    paginacion.RecordsPorPagina
                },

                commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<int> ContarPerfiles()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.ExecuteScalarAsync<int>(
                "ContarPerfiles",

                 commandType: System.Data.CommandType.StoredProcedure
                );
        }

        public async Task Borrar(int Id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(
                "DesactivarPerfile",
                new
                {
                    Id
                },
                commandType: System.Data.CommandType.StoredProcedure
                );
        }
    }
}
