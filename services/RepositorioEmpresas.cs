
using Dapper;
using GASTOS_REEMBOLSABLES_VMICA.Models;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace GASTOS_REEMBOLSABLES_VMICA.services
{
    public interface IRepositorioEmpresas
    {
		Task Borrar(int id);
		Task Crear(Empresa empresa);
        Task Editar(Empresa empresa);
        Task<Empresa> GetEmpresaById(int IdEmpresa);
        Task<IEnumerable<Empresa>> ListarEmpresas();
    }
    public class RepositorioEmpresas : IRepositorioEmpresas
    {
        private readonly String connectionString;

        public RepositorioEmpresas(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("defaultConnection");
        }

        public async Task<IEnumerable<Empresa>> ListarEmpresas()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Empresa>("ListarEmpresas",commandType:System.Data.CommandType.StoredProcedure);

        }


        public async Task Crear(Empresa empresa)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(
                                                     "CrearEmpresa",
                                                     new
                                                     {
                                                         empresa.Nombre,
                                                         empresa.Sector,
                                                         empresa.Direccion
                                                     },
                                                     commandType: System.Data.CommandType.StoredProcedure);
            empresa.Id = id;

        }
        public async Task<Empresa> GetEmpresaById(int Id)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Empresa>(
                "GetEmpresaById",
                new
                {
                    Id
                },
                 commandType: System.Data.CommandType.StoredProcedure
                );
        }

        public async Task Editar(Empresa empresa)
        {
            using var connection=new SqlConnection(connectionString);
            await connection.ExecuteAsync(
                "EditarEmpresa",
                new
                {
                    empresa.Id,
                    empresa.Nombre,
                    empresa.Sector,
                    empresa.Direccion,
                },
                commandType: System.Data.CommandType.StoredProcedure
                );
        }

        public async Task Borrar(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(
                "DesactivarEmpresa",
                new
                {
                    id
                },
                commandType: System.Data.CommandType.StoredProcedure
                );
        }
    }
}
    

