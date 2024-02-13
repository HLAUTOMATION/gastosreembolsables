using Dapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using GASTOS_REEMBOLSABLES_VMICA.Models;
using Microsoft.Data.SqlClient;

namespace GASTOS_REEMBOLSABLES_VMICA.services
{

    public interface IRepositorioReglas
    {
        Task<int> ContarReglas();
        Task Crear(Regla regla);
        Task Editar(Regla regla);
        Task<Regla> GetReglaById(int IdRegla);
		Task<Regla> GetReglaByIdProducto(int id);
		Task<IEnumerable<Regla>> ListarReglasPaginacion(PaginacionViewModel paginacion);
        Task<IEnumerable<Regla>> ListarReglas();
    }
    public class RepositorioReglas : IRepositorioReglas
    {
        private readonly string connectionString;

        public RepositorioReglas(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("defaultConnection");
        }

        

        public async Task<IEnumerable<Regla>> ListarReglas()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Regla>(
                "ListarReglas", 

                commandType: System.Data.CommandType.StoredProcedure);
        }


        public async Task Crear(Regla regla)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(
                                                     "CrearRegla",
                                                     new
                                                     {
                                                         regla.Nombre,
                                                         regla.Antiguedad,
                                                         regla.Veces,
                                                         regla.Periodo,
                                                         regla.Porcentaje,
                                                         regla.Tope,
                                                         regla.Descripcion

                                                     },
                                                     commandType: System.Data.CommandType.StoredProcedure);
             regla.Id = id;

        }


        public async Task<Regla> GetReglaById(int Id)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Regla>(
                "GetReglaById",
                new
                {
                    Id
                },
                 commandType: System.Data.CommandType.StoredProcedure
                );
        }

        public async Task Editar(Regla regla)
        {
            var connection = new SqlConnection(connectionString);
            await connection.QueryAsync<Regla>(
                "EditarRegla",
                new
                {
                    regla.Id,
                    regla.Nombre,
                    regla.Antiguedad,
                    regla.Veces,
                    regla.Periodo,
                    regla.Porcentaje,
                    regla.Tope,
                    regla.Descripcion
                },
                commandType: System.Data.CommandType.StoredProcedure
                );
        }

        public async Task<Regla> GetReglaByIdProducto(int Id)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Regla>(
                "GetReglaByIdProducto",
                new
                {
                    Id
                },
                 commandType: System.Data.CommandType.StoredProcedure
                );
        }

        public async Task<IEnumerable<Regla>> ListarReglasPaginacion(PaginacionViewModel paginacion)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Regla>(
                "ListarReglasPaginacion", new
                {
                    paginacion.RecordsASaltar,
                    paginacion.RecordsPorPagina
                },

                commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<int> ContarReglas()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.ExecuteScalarAsync<int>(
                "ContarReglas",
                
                 commandType: System.Data.CommandType.StoredProcedure
                );
        }
    }
}
