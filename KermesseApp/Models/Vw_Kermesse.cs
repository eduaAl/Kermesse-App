//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KermesseApp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Vw_Kermesse
    {
        public int id_kermesse { get; set; }
        public string Parroquia { get; set; }
        public string Kermesse { get; set; }
        public System.DateTime fecha_inicio { get; set; }
        public System.DateTime fecha_fin { get; set; }
        public string desc_general { get; set; }
        public int estado { get; set; }
        public int usuario_creacion { get; set; }
        public Nullable<int> usuario_modificacion { get; set; }
        public Nullable<int> usuario_eliminacion { get; set; }
        public System.DateTime fecha_creacion { get; set; }
        public Nullable<System.DateTime> fecha_modificacion { get; set; }
        public Nullable<System.DateTime> fecha_eliminacion { get; set; }
    }
}
