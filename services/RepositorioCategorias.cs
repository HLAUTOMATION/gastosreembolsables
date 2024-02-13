using Dapper;
using GASTOS_REEMBOLSABLES_VMICA.Models;
using Microsoft.Data.SqlClient;

namespace GASTOS_REEMBOLSABLES_VMICA.services
{
    public interface IRepositorioCategorias
    {
		Task Borrar(int id);
		Task Crear(Categoria categoria);
        Task Editar(Categoria categoria);
        Task<Categoria> GetCategoriaById(int Id);
        Task<IEnumerable<Categoria>> ListarCategorias();
    }

    public class RepositorioCategorias : IRepositorioCategorias
    {

        private readonly String connectionString;
        public RepositorioCategorias(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("defaultConnection");
        }

        public async Task<IEnumerable<Categoria>> ListarCategorias()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Categoria>("ListarCategorias",commandType:System.Data.CommandType.StoredProcedure );

        }

        public async Task Crear(Categoria categoria)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(
                                                     "CrearCategoria",
                                                     new
                                                     {
                                                         categoria.Nombre,                                           
                                                     },
                                                     commandType: System.Data.CommandType.StoredProcedure);
            categoria.Id = id;

        }

        public async Task<Categoria> GetCategoriaById(int Id)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Categoria>(
                "GetCategoriaById",
                new
                {
                Id
                },
                 commandType: System.Data.CommandType.StoredProcedure
                );
        }

    
        public async Task Editar(Categoria categoria)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(
                "EditarCategoria",
                new
                {
                    categoria.Id,
                    categoria.Nombre  
                },
                commandType: System.Data.CommandType.StoredProcedure
                );
        }

        public async Task Borrar(int id)
        {
            using var connection=new SqlConnection(connectionString);
            await connection.ExecuteAsync(
                "DesactivarCategoria",
                new
                {
                    id
                },
                commandType: System.Data.CommandType.StoredProcedure
                );
        }
    }
}
