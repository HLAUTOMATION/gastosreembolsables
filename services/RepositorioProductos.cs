using Dapper;
using GASTOS_REEMBOLSABLES_VMICA.Models;
using Microsoft.Data.SqlClient;

namespace GASTOS_REEMBOLSABLES_VMICA.services
{

    public interface IRepositorioProductos
    {
		Task Borrar(int id);
		Task<int> ContarProductos();
        Task Crear(Producto producto);
        Task Editar(Producto Producto);
        Task<Producto> GetProductoById(int IdProducto);
        Task<IEnumerable<Producto>> ListarProductos();
        Task<IEnumerable<Producto>> ListarProductosPaginacion(PaginacionViewModel paginacion);
    }
    public class RepositorioProductos : IRepositorioProductos
    {

        private readonly String connectionString;

        public RepositorioProductos(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("defaultConnection");
        }

        public async Task<IEnumerable<Producto>> ListarProductos()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Producto>("ListarProductos",commandType:System.Data.CommandType.StoredProcedure);
        }

        public async Task Crear(Producto producto)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(
                                                     "CrearProducto", 
                                                     new {
                                                     Nombre=producto.Nombre,
                                                     IdCategoria=producto.IdCategoria,
                                                     IdRegla = producto.IdRegla},
                                                     commandType:System.Data.CommandType.StoredProcedure);
            producto.Id = id;
        }

        public async Task<Producto> GetProductoById(int IdProducto)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Producto>(
                "GetProductoById",
                new
                {
                    IdProducto
                },
                 commandType: System.Data.CommandType.StoredProcedure
                );                                                     
        }


        public async Task Editar(Producto producto)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(
                "EditarProducto",
                new
                {
                    producto.Id,
                    producto.Nombre,
                    producto.IdCategoria,
                    producto.IdRegla,
                    producto.Estado

                },
                commandType: System.Data.CommandType.StoredProcedure
                );
        }

        public async Task<IEnumerable<Producto>> ListarProductosPaginacion(PaginacionViewModel paginacion)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Producto>(
                "ListarProductosPaginacion", new
                {
                    paginacion.RecordsASaltar,
                    paginacion.RecordsPorPagina
                },

                commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<int> ContarProductos()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.ExecuteScalarAsync<int>(
                "ContarProductos",

                 commandType: System.Data.CommandType.StoredProcedure
                );
        }

        public async Task Borrar(int Id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(
                "DesactivarProducto",
                new
                {
                   Id, 
                },
                commandType: System.Data.CommandType.StoredProcedure
                );
        }
    }
}
