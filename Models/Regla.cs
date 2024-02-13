using System.ComponentModel.DataAnnotations;

namespace GASTOS_REEMBOLSABLES_VMICA.Models
{
    public class Regla
    {

        public int Id { get; set; }
        public string Nombre { get; set; }

        [Display(Name = "Antigüedad laboral mínima para realizar una solicitud")]
        public int Antiguedad { get; set; }

        public int Veces { get; set; }

        public int Periodo { get; set; }

        [Display(Name = "Porcentaje a reembolsar")]
        public double Porcentaje { get; set; }

        public double Tope { get; set; }

        [Display(Name = "Descripcíon")]
        public string Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaFin { get; set; }

        public int Estado { get; set; }
    }
}
