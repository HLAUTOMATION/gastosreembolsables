using Dapper;
using GASTOS_REEMBOLSABLES_VMICA.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace GASTOS_REEMBOLSABLES_VMICA.services
{

    public interface IRepositorioClientes
    {
		Task Borrar(int id);
		Task Crear(Cliente cliente);
        Task Editar(Cliente cliente);
        Task<Cliente> GetClienteById(int Id);
        Task<IEnumerable<Cliente>> ListarClientes();
    }
    public class RepositorioClientes : IRepositorioClientes
    {


        private readonly String connectionString;
        public RepositorioClientes(IConfiguration configuration)          
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Cliente>> ListarClientes()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Cliente>("ListarClientes",commandType:System.Data.CommandType.StoredProcedure);
        }

        public async Task Crear(Cliente cliente)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(
                                                     "CrearCliente",
                                                     new
                                                     {
                                                         cliente.Nombre,
                                                         cliente.Correo,
                                                         cliente.IdEmpresa,
                                                         cliente.Titulo,

                                                     },
                                                     commandType: System.Data.CommandType.StoredProcedure);
            cliente.Id = id;

        }

        public async Task<Cliente> GetClienteById(int Id)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Cliente>(
                "GetClienteById",
                new
                {
                    Id
                },
                 commandType: System.Data.CommandType.StoredProcedure
                );
        }

        public async Task Editar(Cliente cliente)
        {
            var connection=new SqlConnection(connectionString);
            await connection.QueryAsync<Cliente>(
                "EditarCliente",
                new
                {
                    cliente.Id,
                    cliente.Nombre,
                    cliente.Correo,
                    cliente.IdEmpresa,
                    cliente.Titulo
                },
                commandType: System.Data.CommandType.StoredProcedure
                );
        }

        public async Task Borrar(int Id)
        {
            var connection = new SqlConnection(connectionString);
            await connection.QueryAsync<Cliente>(
                "DesactivarCliente",
                new
                {
                    Id
                    
                },
                commandType: System.Data.CommandType.StoredProcedure
                );
        }
    }
}
