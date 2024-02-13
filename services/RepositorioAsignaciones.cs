using Dapper;
using GASTOS_REEMBOLSABLES_VMICA.Models;
using Microsoft.Data.SqlClient;

namespace GASTOS_REEMBOLSABLES_VMICA.services
{

    public interface IRepositorioAsignaciones
    {
        Task Crear(Asignacion asignacion);
        Task Editar(Asignacion asignacion);
        Task<Asignacion> GetAsignacionById(int Id);
        Task<IEnumerable<Asignacion>> ListarAsignaciones();
    }
    public class RepositorioAsignaciones : IRepositorioAsignaciones
    {
        private readonly string connectionString;

        public RepositorioAsignaciones(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("defaultConnection");
        }

        public async Task<IEnumerable<Asignacion>> ListarAsignaciones()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Asignacion>("ListarAsignaciones", commandType: System.Data.CommandType.StoredProcedure);

        }

        public async Task Crear(Asignacion asignacion)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(
                "CrearAsignacion",
                new
                {
                    asignacion.IdColaborador,
                    asignacion.IdEmpresa,
                    asignacion.IdProyecto
                },
                commandType: System.Data.CommandType.StoredProcedure
                );

            asignacion.Id = id;
        }

        public async Task<Asignacion> GetAsignacionById(int Id)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Asignacion>(
                "GetAsignacionById",
                new
                {
                    Id
                },
                 commandType: System.Data.CommandType.StoredProcedure
                );
        }

      

        public async Task Editar(Asignacion asignacion)
        {
            var connection = new SqlConnection(connectionString);
            await connection.QueryAsync<Asignacion>(
                "EditarAsignacion",
                new
                {
                    asignacion.Id,
                    asignacion.IdColaborador,
                    asignacion.IdEmpresa,
                    asignacion.IdProyecto
                },
                commandType: System.Data.CommandType.StoredProcedure
                );
        }
    }
}
