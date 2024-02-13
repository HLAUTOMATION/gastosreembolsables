using Dapper;
using GASTOS_REEMBOLSABLES_VMICA.Models;
using Microsoft.Data.SqlClient;

namespace GASTOS_REEMBOLSABLES_VMICA.services
{
    public interface IRepositorioColaboradores
    {
        Task Crear(Colaborador colaborador);
        Task Editar(Colaborador colaborador);
        Task<Colaborador> GetColaboradorById(int Id);
        Task<IEnumerable<Colaborador>> ListarColaboradores();
    }
    public class RepositorioColaboradores : IRepositorioColaboradores
    {
        private readonly String connectionString;

        public RepositorioColaboradores(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Colaborador>> ListarColaboradores()
        {

            using var connection=new SqlConnection(connectionString);
            return await connection.QueryAsync<Colaborador>(
                "ListarColaboradores",
                commandType:System.Data.CommandType.StoredProcedure);
        }

        public async Task<Colaborador> GetColaboradorById(int Id)
        {
            using var connection=new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Colaborador>(
                "GetColaboradorById",
                new
                {
                    Id
                },
                commandType:System.Data.CommandType.StoredProcedure);
        }

        public async Task Crear(Colaborador colaborador)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync(
                "CrearColaborador",
                new
                {
                    colaborador.Nombre,
                    colaborador.IdPerfile,
                    colaborador.Correo,
                    colaborador.FechaContrato,
                    colaborador.IdTipoContrato
                    

                },
                commandType: System.Data.CommandType.StoredProcedure
                );
        }

        public async Task Editar(Colaborador colaborador)
        {
            var connection = new SqlConnection(connectionString);
            await connection.QueryAsync<Colaborador>(
                "EditarColaborador",
                new
                {
                    colaborador.Id,
                    colaborador.Nombre,
                    colaborador.IdPerfile,
                    colaborador.Correo,
                    colaborador.FechaContrato,
                    colaborador.IdTipoContrato
                },
                commandType: System.Data.CommandType.StoredProcedure
                );
        }
    }
}
