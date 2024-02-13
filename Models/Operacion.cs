using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel;
using System.Web;


namespace GASTOS_REEMBOLSABLES_VMICA.Models
{
    public class Operacion
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [ValidateNever]
        public int IdOperacion { get; set; }

        [Display(Name = "Producto")]
        public int IdProducto { get; set; }

        public int Cantidad { get; set; }


        public double Precio { get; set; }

        [Display(Name = "Total")]
        public double TotalCosto => Cantidad * Precio;

        [Required(ErrorMessage = "Campo mandatorio")]
        [Display(Name = "Reembolso")]
        public double MontoReembolsado { get; set; }

    public double Tope { get; set; }

        [Display(Name = "Tope")]
        public double TotalTope => Cantidad * Tope;

        [ValidateNever]
        [Display(Name = "Descripción")]
        public String Descripcion { get; set; }

        [Display(Name = "Usuario")]
        [ValidateNever]
        public string CorreoUsuario { get; set; }

        [Display(Name = "Colaborador")]
        [ValidateNever]
        public int IdColaborador { get; set; }


        [Display(Name = "IdAprobador")]
        public int IdAdministrator { get; set; }

        [Display(Name = "Comprobante")]
        [ValidateNever]
        public string UrlDocumento { get; set; }

        [Display(Name = "Fecha Compra")]
        public DateTime FechaCompra { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaProcessed { get; set; }

        public int Estado { get; set; }

        [Display(Name = "Nombre Producto")]
        [ValidateNever]
        public string NombreProducto { get; set; }


        [Display(Name = "Colaborador")]
        [ValidateNever]
        public string ApellidoNombre { get; set; }

        [Display(Name = "Aprobador")]
        [ValidateNever]
        public string CorreoAdministrador { get; set; }


        [ValidateNever]
        public Operacion UltimoOperaciondelProductoDelUsuario { get; set; }


        [ValidateNever]
        public string Comentario { get; set; }


        [ValidateNever]

        //is=1 no=0
        public int IfIsReactivada { get; set; }

        [ValidateNever]
        public IEnumerable<Operacion> operacionesReactivadas { get; set; }

        public int IdRegla { get; set; }
        [ValidateNever]
        public string yearmonth { get; set; }


    }
}
