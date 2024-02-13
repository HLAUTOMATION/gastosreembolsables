using Azure;
using Dapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using GASTOS_REEMBOLSABLES_VMICA.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GASTOS_REEMBOLSABLES_VMICA.services
{
    public interface IRepositorioOperaciones
    {
        Task<IEnumerable<Operacion>> ListarOperaciones();
        Task<IEnumerable<Operacion>> ListarOperacionesHistoricas();
        Task<IEnumerable<Operacion>> GetOperacionesByIdUsuario(string Id);
        Task<int> Crear(Operacion operacion);
        Task Editar(Operacion operacion);
        Task<Operacion> GetOperacionById(int Id);
        Task<Operacion> GetLastOperacionByCorreoUsuarioAndIdProducto(string CorreoUsuario, int IdProducto);
		Task Aprobar(Operacion operacionNew);
        Task Rechazar(Operacion operacion);
        Task<IEnumerable<Operacion>> GetOperacionesHistoricasByIdAppUsuario(string EmailAppUsuario);
        Task<IEnumerable<Operacion>> GetOperacionesByCorreoUsuario(string CorreoUsuario);
        Task<IEnumerable<Operacion>> GetOperacionesByCorreoUsuarioPaginacion(string CorreoUsuario, PaginacionViewModel paginacionViewModel);
        Task OnHold(OperacionCreacionViewModel operacion);
        Task CrearOperacionReactivada(OperacionCreacionViewModel operacionNew);
        Task<IEnumerable<Operacion>> ListarOperacionesReactivadas();
        Task<IEnumerable<Operacion>> GetOperacionesReactivadasByCorreoUsuario(string CorreoUsuario);
        Task<IEnumerable<Operacion>> GetOperacionesReactivadasByIdOperacion(int idOperacion);
        Task Reactivar(Operacion operacion);
        Task<Operacion> GetOperacionReactivadaByIdOperacion(int id);
        Task<Operacion> GetOperacionReactivadaById(int Id);
        Task EditarOperacionReactivada(Operacion operacionNew);
		Task AprobarOperacionReactivada(Operacion operacionToAprobar);
        Task Borrar(int id);
        Task<int> GetCurrentMonthTotalDeOperacionesConsumoOficinaDelColaborador(string correoUsuario,int idProducto, int idRegla);
        Task<IEnumerable<Sumatorio>> GetSumatorioOperacionesPenditesByCorreoUsuario(string CorreoUsuario);
        Task<int> ContarSumatorioOperacionesPenditesByCorreoUsuario(string CorreoUsuario);
        Task<IEnumerable<Sumatorio>> GetSumatorioOperacionesProcesadasByCorreoUsuario(string CorreoUsuario);

        Task<int> ContarSumatorioOperacionesProcesadasByCorreoUsuario(string CorreoUsuario);
        Task EditarSumatorioOperacionesDelUsuarioOnNuevaSolicitud(Operacion operacion);
        Task EditarSumatorioOperacionesDelUsuarioOnBorrarSolicitud(Operacion operacion);
        Task EditarSumatorioOperacionesDelUsuarioOnHold(Operacion operacion);
        Task EditarSumatorioOperacionesDelUsuarioOnRechazar(Operacion operacion);
        Task EditarSumatorioOperacionesDelUsuarioOnAprobar(Operacion operacion);
        Task RechazarOperacionReactivada(Operacion operacion);
        Task<IEnumerable<Operacion>> GetOperacionesPendientesAprobadasOnHoldReactivadasByCorreoUsuario(string CorreoUsuario);
        Task<IEnumerable<Sumatorio>> GetSumatorioOperacionesPendites();
        Task<IEnumerable<Sumatorio>> GetSumatorioOperacionesProcesadas();
        Task EditarSumatorioOperacionesDelUsuarioOnSolicitudreactivada(Operacion operacion);
        Task<double> GetTotalConsomoOficinaAprobadoByMonthByUsuario(Operacion operacion, DateTime dateTime);
        Task EditarSumatorioOperacionesDelUsuarioOnEditarSolicitud(Operacion operacionOld, Operacion operacionNew);
        Task<int> ContarOperacionesPendientesByCorreoUsuario(string CorreoUsuario);
        Task<IEnumerable<Operacion>> GetOperacionesHistoricasByCorreoUsuarioPaginacion(string CorreoUsuario, PaginacionViewModel paginacionViewModel);
        Task<int> ContarOperacionesHistoricasByCorreoUsuario(string correoUsuario);
        Task<IEnumerable<Operacion>> ListarOperacionesPendientesPaginacion(PaginacionViewModel paginacionViewModel);
        Task<int> ContarOperacionesPendientes();
        Task<IEnumerable<Operacion>> ListarOperacionesHistoricasPaginacion(PaginacionViewModel paginacionViewModel);
        Task<int> ContarOperacionesHistoricas();
        Task<int> ContarSumatorioOperacionesPendites();
        Task<int> ContarSumatorioOperacionesProcesadas();
        Task EditarSumatorioOperacionesDelUsuarioOnAprobarOperacionReactivada(Operacion operacion);
        Task EditarSumatorioOperacionesDelUsuarioOnRechazarOperacionReactivada(Operacion operacion);
    }

    public class RepositorioOperaciones : IRepositorioOperaciones
    {
        private readonly String connectionString;

        public RepositorioOperaciones(IConfiguration configuration )
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Operacion>> ListarOperaciones()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Operacion>("ListarOperaciones",commandType:System.Data.CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Operacion>> ListarOperacionesReactivadas()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Operacion>("ListarOperacionesReactivadas", commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Operacion>> ListarOperacionesHistoricas()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Operacion>("ListarOperacionesHistoricas", commandType: System.Data.CommandType.StoredProcedure);
        }


        public async Task<IEnumerable<Operacion>> GetOperacionesByIdUsuario(string Id)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Operacion>(
                "GetOperacionesByIdUsuario",
                new
                {
                    Id
                },
                commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Operacion>> GetOperacionesByCorreoUsuario(string CorreoUsuario)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Operacion>(
                "GetOperacionesByCorreoUsuario",
                new
                {
                    CorreoUsuario           
                },
                commandType: System.Data.CommandType.StoredProcedure);
        }

        

        public async Task<IEnumerable<Operacion>> GetOperacionesPendientesAprobadasOnHoldReactivadasByCorreoUsuario(string CorreoUsuario)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Operacion>(
                "GetOperacionesPendientesAprobadasOnHoldReactivadasByCorreoUsuario",
                new
                {
                    CorreoUsuario
                },
                commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Operacion>> GetOperacionesReactivadasByCorreoUsuario(string CorreoUsuario)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Operacion>(
                "GetOperacionesReactivadasByCorreoUsuario",
                new
                {
                    CorreoUsuario
                },
                commandType: System.Data.CommandType.StoredProcedure);
        }


       

        public async Task<IEnumerable<Operacion>> GetOperacionesHistoricasByIdAppUsuario(string EmailAppUsuario)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Operacion>(
                "GetOperacionesHistoricasByIdAppUsuario",
                 new
                 {
                     EmailAppUsuario
                 },
                commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Operacion>> GetOperacionesHistoricasByCorreoUsuarioPaginacion
            (string CorreoUsuario, PaginacionViewModel paginacionViewModel)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Operacion>(
                "GetOperacionesHistoricasByCorreoUsuarioPaginacion",
                 new
                 {
                     CorreoUsuario,
                     paginacionViewModel.RecordsASaltar,
                     paginacionViewModel.RecordsPorPagina
                 },
                commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<int> Crear(Operacion operacion)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(
                                                     "CrearOperacion",
                                                     new
                                                     {
                                                         operacion.IdProducto,
                                                         operacion.Cantidad,
                                                         operacion.Precio,
                                                         operacion.TotalCosto,
                                                         operacion.Descripcion,
                                                         operacion.CorreoUsuario,
                                                         operacion.UrlDocumento,
                                                         operacion.FechaCompra,
                                                         operacion.FechaCreacion,
                                                         operacion.yearmonth

                                                     },
                                                     commandType: System.Data.CommandType.StoredProcedure);
            operacion.Id = id;
            return id;

        }


        public async Task Editar(Operacion operacion)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(
                "EditarOperacion",
                new
                {
                    operacion.Id,
                    operacion.IdProducto,
                    operacion.Cantidad,
                    operacion.Precio,
                    operacion.TotalCosto,
                    operacion.Descripcion,
                    operacion.UrlDocumento,
                    operacion.FechaCompra

                },
                commandType: System.Data.CommandType.StoredProcedure
                );
        }

        public async Task<Operacion> GetOperacionById(int Id)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Operacion>(

                "GetOperacionById", new
                {
                    Id
                },
                commandType: System.Data.CommandType.StoredProcedure
                );
        }

        public async Task<Operacion> GetLastOperacionByCorreoUsuarioAndIdProducto(string CorreoUsuario, int IdProducto)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Operacion>(

                "GetLastOperacionByCorreoUsuarioAndIdProducto", new
                {
                    CorreoUsuario,
                    IdProducto
                },
                commandType: System.Data.CommandType.StoredProcedure
                );
        }

        public async Task Aprobar(Operacion operacion)
        {

            operacion.FechaProcessed = DateTime.Now;
            //1=aprobar,2=recharzar,3=pendiente
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(
                "AprobarOperacion",
                new
                {
                    operacion.Id, 
                    operacion.CorreoAdministrador,
                    operacion.Comentario,
                    operacion.MontoReembolsado,
                    operacion.FechaProcessed,

                },
                commandType: System.Data.CommandType.StoredProcedure
                );
        }

        public async Task Rechazar(Operacion operacion)
        {

            // estado : 1=aprobar,2=recharzar,3=pendiente, 4=on hold, 5=reactivado, 6=desactivado
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(
                "RechazarOperacion",
                new
                {
                    operacion.Id,
                    operacion.CorreoAdministrador,
                    operacion.Comentario

                },
                commandType: System.Data.CommandType.StoredProcedure
                );
        }

        public async Task RechazarOperacionReactivada(Operacion operacion)
        {

            // estado : 1=aprobar,2=recharzar,3=pendiente, 4=on hold, 5=reactivado, 6=desactivado
            using var connection = new SqlConnection(connectionString);
            operacion.FechaProcessed=DateTime.Now;
            await connection.ExecuteAsync(
                "RechazarOperacionReactivada",
                new
                {
                    operacion.Id,
                    operacion.CorreoAdministrador,
                    operacion.Comentario,
                    operacion.FechaProcessed
                },
                commandType: System.Data.CommandType.StoredProcedure
                );
        }

        public async Task OnHold(OperacionCreacionViewModel operacion)
        {

            //1=aprobar,2=recharzar,3=pendiente
            operacion.FechaProcessed = DateTime.Now;
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(
                "OnHoldOperacion",
                new
                {
                    operacion.Id,
                    operacion.CorreoAdministrador,
                    operacion.Comentario,
                    operacion.FechaProcessed
                },
                commandType: System.Data.CommandType.StoredProcedure
                );
        }

        public async Task CrearOperacionReactivada(OperacionCreacionViewModel operacion)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(
                "CrearOperacionReactivada",
                new
                {
                    operacion.IdOperacion,
                    operacion.IdProducto,
                    operacion.Cantidad,
                    operacion.Precio,
                    operacion.TotalCosto,
                    operacion.Descripcion,
                    operacion.UrlDocumento,
                    operacion.CorreoUsuario,
                    operacion.CorreoAdministrador,
                    operacion.FechaCompra,
                    operacion.FechaCreacion,
                    operacion.yearmonth
                },
                commandType: System.Data.CommandType.StoredProcedure
                );
        }

        public async Task<IEnumerable<Operacion>> GetOperacionesReactivadasByIdOperacion(int IdOperacion)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Operacion>(
                "GetOperacionesReactivadasByIdOperacion",
                new
                {
                    IdOperacion
                },
                commandType: System.Data.CommandType.StoredProcedure);
        }

       public async Task Reactivar(Operacion operacion)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(
                "ReactivarOperacion",
                new
                {
                    operacion.Id
                },
             
                commandType: System.Data.CommandType.StoredProcedure
                );
        }

       public async Task<Operacion> GetOperacionReactivadaByIdOperacion(int Id)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Operacion>(

                "GetOperacionReactivadaByIdOperacion", new
                {
                    Id
                },
                commandType: System.Data.CommandType.StoredProcedure
                );
        }

        public async Task<Operacion> GetOperacionReactivadaById(int Id)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Operacion>(

                "GetOperacionReactivadaById", new
                {
                    Id
                },
                commandType: System.Data.CommandType.StoredProcedure
                );
        }

        public async Task EditarOperacionReactivada(Operacion operacionNew)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(
                "EditarOperacionReactivada",
                new
                {
                    operacionNew.Id,
                    operacionNew.IdProducto,
                    operacionNew.Cantidad,
                    operacionNew.Precio,
                    operacionNew.TotalCosto,
                    operacionNew.Descripcion,
                    operacionNew.UrlDocumento,
                    operacionNew.FechaCompra
                },
                commandType: System.Data.CommandType.StoredProcedure
                );
        }

        public async Task AprobarOperacionReactivada(Operacion operacionToAprobar)
        {
            //1=aprobar,2=recharzar,3=pendiente
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(
                "AprobarOperacionReactivada",
                new
                {
                    operacionToAprobar.Id,
                    operacionToAprobar.CorreoAdministrador,
                    operacionToAprobar.Comentario,
                    operacionToAprobar.MontoReembolsado,
                    operacionToAprobar.FechaProcessed

                },
                commandType: System.Data.CommandType.StoredProcedure
                );
        }

        public async Task Borrar(int Id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(
                "DesactivarSolicitud",
                new
                {
                    Id

                },
                commandType: System.Data.CommandType.StoredProcedure
                );
        }

        public async Task<int> GetCurrentMonthTotalDeOperacionesConsumoOficinaDelColaborador(string CorreoUsuario,int IdProducto, int IdRegla)
            
        {
            using var connection = new SqlConnection(connectionString);
 
            return await connection.QueryFirstOrDefaultAsync<int>(
                "GetCurrentMonthTotalDeOperacionesConsumoOficinaDelColaborador",
                new
                {
                    CorreoUsuario,
                    IdProducto,
                    IdRegla                  
                },
                commandType: System.Data.CommandType.StoredProcedure
                );
        }

        public async Task<IEnumerable<Sumatorio>> GetSumatorioOperacionesPenditesByCorreoUsuario (string CorreoUsuario)

        {
            using var connection = new SqlConnection(connectionString);

            return await connection.QueryAsync<Sumatorio>(
                "GetSumatorioOperacionesPenditesByCorreoUsuario",
                new
                {
                    CorreoUsuario
                },
                commandType: System.Data.CommandType.StoredProcedure
                );
        }

        public async Task<IEnumerable<Sumatorio>> GetSumatorioOperacionesProcesadasByCorreoUsuario(string CorreoUsuario)

        {
            using var connection = new SqlConnection(connectionString);

            return await connection.QueryAsync<Sumatorio>(
                "GetSumatorioOperacionesProcesadasByCorreoUsuario",
                new
                {
                    CorreoUsuario
                },
                commandType: System.Data.CommandType.StoredProcedure
                );
        }

        public async Task EditarSumatorioOperacionesDelUsuarioOnNuevaSolicitud(Operacion operacion)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(
                                                     "EditarSumatorioOperacionesDelUsuarioOnNuevaSolicitud",
                                                     new
                                                     {
                                                         operacion.IdProducto,
                                                         operacion.TotalCosto,
                                                         operacion.yearmonth,
                                                         operacion.CorreoUsuario,
                                                     },
                                                     commandType: System.Data.CommandType.StoredProcedure);

        }

        public async Task EditarSumatorioOperacionesDelUsuarioOnEditarSolicitud(Operacion operacionOld, Operacion operacionNew)
        {
            using var connection = new SqlConnection(connectionString);            
            await connection.ExecuteAsync(
                                                     "EditarSumatorioOperacionesDelUsuarioOnEditarSolicitud",
                                                     new
                                                     {
                                                         operacionOld.IdProducto,
                                                        TotalCostoOld= operacionOld.TotalCosto ,
                                                        TotalCostoNew= operacionNew.TotalCosto,
                                                         operacionOld.yearmonth,
                                                         operacionOld.CorreoUsuario,
                                                     },
                                                     commandType: System.Data.CommandType.StoredProcedure);

        }

        public async Task EditarSumatorioOperacionesDelUsuarioOnBorrarSolicitud(Operacion operacion)
        {
            using var connection = new SqlConnection(connectionString);
            operacion.yearmonth = String.Concat(operacion.FechaCreacion.Year.ToString(), operacion.FechaCreacion.Month.ToString());
            await connection.ExecuteAsync(
                                                     "EditarSumatorioOperacionesDelUsuarioOnBorrarSolicitud",
                                                     new
                                                     {
                                                         operacion.IdProducto,
                                                         operacion.TotalCosto,
                                                         operacion.yearmonth,
                                                         operacion.CorreoUsuario,
                                                         
                                                     },
                                                     commandType: System.Data.CommandType.StoredProcedure);

        }

        public async Task EditarSumatorioOperacionesDelUsuarioOnAprobar (Operacion operacion)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(
                                                     "EditarSumatorioOperacionesDelUsuarioOnAprobar",
                                                     new
                                                     {
                                                         operacion.IdProducto,
                                                         operacion.TotalCosto,
                                                         operacion.yearmonth,
                                                         operacion.CorreoUsuario,
                                                         operacion.MontoReembolsado
                                                         
                                                     },
                                                     commandType: System.Data.CommandType.StoredProcedure);

        }

        public async Task EditarSumatorioOperacionesDelUsuarioOnAprobarOperacionReactivada(Operacion operacion)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(
                                                     "EditarSumatorioOperacionesDelUsuarioOnAprobarOperacionReactivada",
                                                     new
                                                     {
                                                         operacion.IdProducto,
                                                         operacion.TotalCosto,
                                                         operacion.yearmonth,
                                                         operacion.CorreoUsuario,
                                                         operacion.MontoReembolsado

                                                     },
                                                     commandType: System.Data.CommandType.StoredProcedure);

        }
        public async Task EditarSumatorioOperacionesDelUsuarioOnRechazar(Operacion operacion)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(
                                                     "EditarSumatorioOperacionesDelUsuarioOnRechazar",
                                                     new
                                                     {
                                                         operacion.IdProducto,
                                                         operacion.TotalCosto,
                                                         operacion.yearmonth,
                                                         operacion.CorreoUsuario,
                                                     },
                                                     commandType: System.Data.CommandType.StoredProcedure);

        }

        public async Task EditarSumatorioOperacionesDelUsuarioOnRechazarOperacionReactivada (Operacion operacion)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(
                                                     "EditarSumatorioOperacionesDelUsuarioOnRechazarOperacionReactivada",
                                                     new
                                                     {
                                                         operacion.IdProducto,
                                                         operacion.TotalCosto,
                                                         operacion.yearmonth,
                                                         operacion.CorreoUsuario,
                                                     },
                                                     commandType: System.Data.CommandType.StoredProcedure);

        }
        public async Task EditarSumatorioOperacionesDelUsuarioOnHold(Operacion operacion)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(
                                                     "EditarSumatorioOperacionesDelUsuarioOnHold",
                                                     new
                                                     {
                                                         operacion.IdProducto,
                                                         operacion.TotalCosto,
                                                         operacion.yearmonth,
                                                         operacion.CorreoUsuario,
                                                     },
                                                     commandType: System.Data.CommandType.StoredProcedure);

        }

        public async Task<IEnumerable<Sumatorio>> GetSumatorioOperacionesPendites()
        {
            using var connection = new SqlConnection(connectionString);

            return await connection.QueryAsync<Sumatorio>(
                "GetSumatorioOperacionesPendites",
                
                commandType: System.Data.CommandType.StoredProcedure
                );
        }

        public async Task<IEnumerable<Sumatorio>> GetSumatorioOperacionesProcesadas()
        {
            using var connection = new SqlConnection(connectionString);

            return await connection.QueryAsync<Sumatorio>(
                "GetSumatorioOperacionesProcesadas",
               
                commandType: System.Data.CommandType.StoredProcedure
                );
        }

        public async Task EditarSumatorioOperacionesDelUsuarioOnSolicitudreactivada(Operacion operacion)
        {
            using var connection = new SqlConnection(connectionString);
            //operacion.FechaCreacion = DateTime.Now;
            //operacion.yearmonth = String.Concat(operacion.FechaCreacion.Year.ToString(), operacion.FechaCreacion.Month.ToString());
            await connection.ExecuteAsync(
                                                     "EditarSumatorioOperacionesDelUsuarioOnSolicitudreactivada",
                                                     new
                                                     {
                                                         operacion.IdProducto,
                                                         operacion.TotalCosto,
                                                         operacion.yearmonth,
                                                         operacion.CorreoUsuario,
                                                     },
                                                     commandType: System.Data.CommandType.StoredProcedure);

        }

        public async Task<double> GetTotalConsomoOficinaAprobadoByMonthByUsuario(Operacion operacion, DateTime dateTime)
        {
            using var connection = new SqlConnection(connectionString);

            return await connection.QueryFirstOrDefaultAsync<double>(
                "GetTotalConsomoOficinaAprobadoByMonthByUsuario",
                new
                {
                    operacion.CorreoUsuario,
                    dateTime
                },

                commandType: System.Data.CommandType.StoredProcedure
                );
        }

       

        public async Task<int> ContarOperacionesPendientesByCorreoUsuario(string correoUsuario)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.ExecuteScalarAsync<int>(
                "ContarOperacionesPendientesByCorreoUsuario",
                new
                {
                    correoUsuario
                },
                commandType: System.Data.CommandType.StoredProcedure
                );
        }

        public async Task<int> ContarOperacionesHistoricasByCorreoUsuario(string correoUsuario)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.ExecuteScalarAsync<int>(
                "ContarOperacionesHistoricasByCorreoUsuario",
                new
                {
                    correoUsuario
                },
                commandType: System.Data.CommandType.StoredProcedure
                );
        }

        public async Task<IEnumerable<Operacion>> GetOperacionesByCorreoUsuarioPaginacion(string CorreoUsuario, PaginacionViewModel paginacionViewModel)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Operacion>(
                "GetOperacionesByCorreoUsuarioPaginacion",
                new
                {
                    CorreoUsuario,
                    paginacionViewModel.RecordsASaltar,
                    paginacionViewModel.RecordsPorPagina
                },
                commandType: System.Data.CommandType.StoredProcedure);
        }


        public async Task<IEnumerable<Operacion>> ListarOperacionesPendientesPaginacion (PaginacionViewModel paginacionViewModel)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Operacion>(
                "ListarOperacionesPendientesPaginacion",
                new
                {                    
                    paginacionViewModel.RecordsASaltar,
                    paginacionViewModel.RecordsPorPagina
                },
                commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<int> ContarOperacionesPendientes()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.ExecuteScalarAsync<int>(
                "ContarOperacionesPendientes",
                
                commandType: System.Data.CommandType.StoredProcedure
                );
        }

        public async Task<IEnumerable<Operacion>> ListarOperacionesHistoricasPaginacion(PaginacionViewModel paginacionViewModel)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Operacion>(
                "ListarOperacionesHistoricasPaginacion",
                new
                {
                    paginacionViewModel.RecordsASaltar,
                    paginacionViewModel.RecordsPorPagina
                },
                commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<int> ContarOperacionesHistoricas()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.ExecuteScalarAsync<int>(
                "ContarOperacionesHistoricas",

                commandType: System.Data.CommandType.StoredProcedure
                );
        }

        public async Task<int> ContarSumatorioOperacionesPenditesByCorreoUsuario(string CorreoUsuario)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.ExecuteScalarAsync<int>(
                "ContarSumatorioOperacionesPenditesByCorreoUsuario",
                new {
                    CorreoUsuario
                },

                commandType: System.Data.CommandType.StoredProcedure
                );
        }

        public async Task<int> ContarSumatorioOperacionesProcesadasByCorreoUsuario(string CorreoUsuario)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.ExecuteScalarAsync<int>(
                "ContarSumatorioOperacionesPenditesByCorreoUsuario",
                new
                {
                    CorreoUsuario
                },

                commandType: System.Data.CommandType.StoredProcedure
                );
        }

        public async Task<int> ContarSumatorioOperacionesPendites()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.ExecuteScalarAsync<int>(
                "ContarSumatorioOperacionesPendites",
                 commandType: System.Data.CommandType.StoredProcedure
                );
        }

        public async Task<int> ContarSumatorioOperacionesProcesadas()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.ExecuteScalarAsync<int>(
                "ContarSumatorioOperacionesPenditesByCorreoUsuario",
                 commandType: System.Data.CommandType.StoredProcedure
                );
        }
    }
    }

