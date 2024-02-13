using Dapper;
using GASTOS_REEMBOLSABLES_VMICA.Models;
using Microsoft.Data.SqlClient;

namespace GASTOS_REEMBOLSABLES_VMICA.services
{

    public interface IRepositorioProyectos
    {
        Task Crear(Proyecto proyecto);
        Task<IEnumerable<Proyecto>> ListarProyectos();
        Task<IEnumerable<Proyecto>> GetProyectosByIdEmpresa(EmpresaEnum IdEmpresa);
        Task Editar(Proyecto proyecto);
        Task<Proyecto> GetProyectoById(int Id);
        Task<IEnumerable<Proyecto>> ListarProyectosPaginacion(PaginacionViewModel paginacionViewModel);
        Task<int> ContarProyectos();
		Task Borrar(int id);
	}
    public class RepositorioProyectos : IRepositorioProyectos
    {
        private readonly String connectionString;

        public RepositorioProyectos(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("defaultConnection");
                
        }

        public async Task<IEnumerable<Proyecto>> ListarProyectos()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Proyecto>("ListarProyectos",commandType:System.Data.CommandType.StoredProcedure);
        }

        public async Task Crear(Proyecto proyecto)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(
                                                     "CrearProyecto",
                                                     new
                                                     {
                                                         proyecto.IdEmpresa,
                                                         proyecto.Nombre,
                                                         proyecto.Descripcion,
                                                     },
                                                     commandType: System.Data.CommandType.StoredProcedure);
            proyecto.Id = id;

        }

        public async Task<IEnumerable<Proyecto>> GetProyectosByIdEmpresa(EmpresaEnum IdEmpresa)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Proyecto>(
                "GetProyectosByIdEmpresa", new
                {
                    IdEmpresa
                },
                commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<Proyecto> GetProyectoById(int Id)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Proyecto>(
                "GetProyectoById", 
                new
                {
                    Id
                },
                commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task Editar(Proyecto proyecto)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.QueryAsync<Proyecto>(
                
                "EditarProyecto",
                new
                {
                    proyecto.Id,
                    proyecto.IdEmpresa,
                    proyecto.Nombre,
                    proyecto.Descripcion
                },
                commandType: System.Data.CommandType.StoredProcedure);
                
        }

        public async Task<IEnumerable<Proyecto>> ListarProyectosPaginacion(PaginacionViewModel paginacionViewModel)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Proyecto>(
                "ListarProyectosPaginacion", new
                {
                    paginacionViewModel.RecordsASaltar,
                    paginacionViewModel.RecordsPorPagina
                },
                commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<int> ContarProyectos()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.ExecuteScalarAsync<int>(
                "ContarProyectos",
                commandType: System.Data.CommandType.StoredProcedure
                );
        }

        public async Task Borrar(int Id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.QueryAsync<Proyecto>(

                "DesactivarProyecto",
                new
                {
                   Id
                },
                commandType: System.Data.CommandType.StoredProcedure);
        }
    }

    
}
