﻿namespace GASTOS_REEMBOLSABLES_VMICA.Models
{
    public class PaginacionRespuesta
    {
        public int Pagina { get; set; } = 1;
        public int RecordsPorPagina { get; set; } = 5;
        public int CantidadTotalRecords { get; set; }
        public int CantidadTotalDePaginas =>(int)Math.Ceiling((double)CantidadTotalRecords/RecordsPorPagina);

        public string BaseURL { get; set; }

        
    }

    public class PaginacionRespuesta<T>: PaginacionRespuesta
    {
        public IEnumerable<T> Elementos { get; set; }
        public AppUsuario appUsuario { get; set; }
    }
}
