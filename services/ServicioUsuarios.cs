using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace GASTOS_REEMBOLSABLES_VMICA.services
{
    public interface IServicioUsuarios
    {
        string GetADUserEmail();
        string GetIdUsuario();
    }
    public class ServicioUsuarios : IServicioUsuarios
    {
        private readonly HttpContext httpContext;

        public ServicioUsuarios(IHttpContextAccessor  httpContextAccessor)
        {
            httpContext = httpContextAccessor.HttpContext;
        }

        public IHttpContextAccessor HttpContextAccessor { get; }

        public string GetIdUsuario()
        {
            if (httpContext.User.Identity.IsAuthenticated)
            {
                var idClaim = httpContext.User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault();
                // var id = int.Parse(idClaim.Value);
                var id = idClaim.Value;
                return id;
            }
            else
            {
                throw new ApplicationException("El usuario no esta autenticado");
            }
        }

        public string GetADUserEmail()
        {
            if (httpContext.User.Identity.IsAuthenticated)
            {
                var email = httpContext.User.Identity.Name.ToUpper();
                return email;
            }
            else
            {
                throw new ApplicationException("El usuario no esta autenticado");
            }
        }
    }
}
