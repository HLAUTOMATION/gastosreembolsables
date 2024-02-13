using GASTOS_REEMBOLSABLES_VMICA.Models;
using Microsoft.IdentityModel.Tokens;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace GASTOS_REEMBOLSABLES_VMICA.services
{

    public interface IServicioEmailSendGrid
    {
        Task Actualizar(Operacion operacion);
        Task NotificarSolicitudAprobada(Operacion operacion);
		Task NotificarSolicitudOnHold(OperacionCreacionViewModel operacion);
		Task NotificarSolicitudRechazada(OperacionCreacionViewModel operacion);
        Task Solicitar(OperacionCreacionViewModel operacionCreacionViewModel);
    }
    public class ServicioEmailSendGrid : IServicioEmailSendGrid
    {
        private readonly IConfiguration configuration;
        private readonly IServicioUsuarios servicioUsuarios;
        private readonly IRepositorioAppUsuarios repositorioAppUsuarios;
        private readonly IRepositorioProductos repositorioProductos;

        public ServicioEmailSendGrid(
            IConfiguration configuration,
            IServicioUsuarios servicioUsuarios,
            IRepositorioAppUsuarios repositorioAppUsuarios,
            IRepositorioProductos repositorioProductos
            )
        {
            this.configuration = configuration;
            this.servicioUsuarios = servicioUsuarios;
            this.repositorioAppUsuarios = repositorioAppUsuarios;
            this.repositorioProductos = repositorioProductos;
        }

        public async Task Solicitar (OperacionCreacionViewModel operacion)
        {

            var Producto = await repositorioProductos.GetProductoById(operacion.IdProducto);
            var NombreProducto = Producto.Nombre;

            var apiKey = configuration.GetValue<string>("SENDGRID_API_KEY"); 
            var email=configuration.GetValue<string>("SENDGRID_FROM");
            var nombre = configuration.GetValue<string>("SENDGRID_NOMBRE");

            var cliente = new SendGridClient(apiKey);
            var from = new EmailAddress(email,nombre);
            var subject = $"El colaborador " + GetAppUsuarioByEmail().Result.Email+
                $" solicita un reembolso para la compra del producto " +
                NombreProducto;
            var to = new EmailAddress(email,nombre);
            var text = "https://localhost:7214/Operaciones/Procesar/"+operacion.Id;
            var contentHtml = @$"De : {GetAppUsuarioByEmail().Result.Email.Substring(0, operacion.CorreoUsuario.IndexOf("@"))}
                                    - Mensaje:<a href= 'https://localhost:7214/Operaciones/Procesar/{operacion.Id}'> Click </a>";
            var singleEmail = MailHelper.CreateSingleEmail(
                from, to, subject, text, contentHtml);
            var respuesta = await cliente.SendEmailAsync(singleEmail);

        }

        public async Task Actualizar(Operacion operacion)
        {

            var Producto = await repositorioProductos.GetProductoById(operacion.IdProducto);
            var NombreProducto = Producto.Nombre;

            var apiKey = configuration.GetValue<string>("SENDGRID_API_KEY");
            var email = configuration.GetValue<string>("SENDGRID_FROM");
            var nombre = configuration.GetValue<string>("SENDGRID_NOMBRE");

            var cliente = new SendGridClient(apiKey);
            var from = new EmailAddress(email, nombre);
            
            var subject = $"El colaborador " + GetAppUsuarioByEmail().Result.Email +
                $" ha actualizado la solicitud para la compra del producto " +
                NombreProducto;
            var to = new EmailAddress(email, nombre);
            var text = operacion.Descripcion;
            var contentHtml = @$"De : {GetAppUsuarioByEmail().Result.Email}
                                    - Mensaje: {operacion.Descripcion}";
            var singleEmail = MailHelper.CreateSingleEmail(
                from, to, subject, text, contentHtml);
            var respuesta = await cliente.SendEmailAsync(singleEmail);

        }

        public async Task<AppUsuario> GetAppUsuarioByEmail()
        {

            var emailNormalizado = servicioUsuarios.GetADUserEmail().ToUpper();
            var appUsuario = repositorioAppUsuarios.GetAppUsuarioByEmail(emailNormalizado);


            return await appUsuario;

        }

        public async Task NotificarSolicitudAprobada(Operacion operacion)
        {

            //TBD, should be the consultor who recieve the notification
            var apiKey = configuration.GetValue<string>("SENDGRID_API_KEY");
            var email = configuration.GetValue<string>("SENDGRID_FROM");
            var nombre = configuration.GetValue<string>("SENDGRID_NOMBRE");

            var cliente = new SendGridClient(apiKey);
            var from = new EmailAddress(email, nombre);
            var subject = $"Su solicitud con ID :" + $"{operacion.Id} " + " para el producto" +
                $"{operacion.NombreProducto} "+" esta aprobada" ;
            var to = new EmailAddress(operacion.CorreoUsuario, operacion.ApellidoNombre);
            var text = operacion.Descripcion;
            var contentHtml = @$"De : {operacion.CorreoAdministrador}
                                    - Mensaje: {operacion.Descripcion}";
            var singleEmail = MailHelper.CreateSingleEmail(
                from, to, subject, text, contentHtml);
            var respuesta = await cliente.SendEmailAsync(singleEmail);

        }

        public async Task NotificarSolicitudRechazada(OperacionCreacionViewModel operacion)
        {

            //TBD, should be the consultor who recieve the notification
            var apiKey = configuration.GetValue<string>("SENDGRID_API_KEY");
            var email = configuration.GetValue<string>("SENDGRID_FROM");
            var nombre = configuration.GetValue<string>("SENDGRID_NOMBRE");

            var cliente = new SendGridClient(apiKey);
            var from = new EmailAddress(email, nombre);
            var subject = $"Su solicitud con ID : "+ $"{operacion.Id}" + "para el producto " +
                $"{operacion.NombreProducto}" + " esta rechazada";
            var to = new EmailAddress(operacion.CorreoUsuario, operacion.ApellidoNombre);
            var text = operacion.Descripcion;
            var contentHtml = @$"De : {operacion.ApellidoNombre}
                                    - Mensaje: {operacion.Descripcion}";
            var singleEmail = MailHelper.CreateSingleEmail(
                from, to, subject, text, contentHtml);
            var respuesta = await cliente.SendEmailAsync(singleEmail);

        }

        public async Task NotificarSolicitudOnHold(OperacionCreacionViewModel operacion)
        {
            //TBD, should be the consultor who recieve the notification
            var apiKey = configuration.GetValue<string>("SENDGRID_API_KEY");
            var email = configuration.GetValue<string>("SENDGRID_FROM");
            var nombre = configuration.GetValue<string>("SENDGRID_NOMBRE");

            var cliente = new SendGridClient(apiKey);
            var from = new EmailAddress(email, nombre);
            var subject = $"Su solicitud con ID : " + $"{operacion.Id}" + " esta on hold por el motivo " + $"{operacion.Comentario}";
            var to = new EmailAddress(operacion.CorreoUsuario, operacion.ApellidoNombre);
            var text = operacion.Descripcion;
            var contentHtml = @$"De : {operacion.ApellidoNombre}
                                    - Mensaje:<a href= 'https://localhost:7214/Operaciones/Reactivar/{operacion.Id}'> Click </a>";
            var singleEmail = MailHelper.CreateSingleEmail(
                from, to, subject, text, contentHtml);
            var respuesta = await cliente.SendEmailAsync(singleEmail);
        }
    }
}
