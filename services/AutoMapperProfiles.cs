using AutoMapper;
using GASTOS_REEMBOLSABLES_VMICA.Models;

namespace GASTOS_REEMBOLSABLES_VMICA.services
{
    public class AutoMapperProfiles :Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Producto, ProductoCreacionViewModel>();
            CreateMap<AppUsuario, AppUsuarioCreacionViewModel>();
            CreateMap<Operacion, OperacionCreacionViewModel>();
            CreateMap<Cliente, ClienteCreacionViewModel>();
            CreateMap<Colaborador, ColaboradorCreacionViewModel>();
            CreateMap<Proyecto, ProyectoCreacionViewModel>();
            //CreateMap<Usuario, UsuarioCreacionViewModel>();
            CreateMap<Asignacion, AsignacionCreacionViewModel>();
        }
    }
}
