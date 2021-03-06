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
    using System.ComponentModel.DataAnnotations;

    public partial class tbl_arqueocaja
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_arqueocaja()
        {
            this.tbl_arqueocaja_det = new HashSet<tbl_arqueocaja_det>();
        }

        public int id_arqueocaja { get; set; }

        [Display(Name = "Kermesse: ")]
        public int idkermesse { get; set; }

        [Display(Name = "Fecha del arqueo: ")]
        [Required(ErrorMessage = "Escriba la fecha del arqueo.")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public System.DateTime fecha_arqueo { get; set; }

        [Display(Name = "Gran Total: ")]
        [Required(ErrorMessage = "Escriba el gran total del arqueo.")]
        public decimal gran_total { get; set; }
        public int usuario_creacion { get; set; }
        public System.DateTime fecha_creacion { get; set; }
        public Nullable<int> usuario_modificacion { get; set; }
        public Nullable<System.DateTime> fecha_modificacion { get; set; }
        public Nullable<int> usuario_eliminacion { get; set; }
        public Nullable<System.DateTime> fecha_eliminacion { get; set; }
        public int estado { get; set; }
        public tbl_arqueocaja_det detalle { get; set; }
        public List<tbl_arqueocaja_det> tbl_arqueocaja_dets { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_arqueocaja_det> tbl_arqueocaja_det { get; set; }
        public virtual tbl_kermesse tbl_kermesse { get; set; }
        public virtual tbl_usuario tbl_usuario { get; set; }
        public virtual tbl_usuario tbl_usuario1 { get; set; }
        public virtual tbl_usuario tbl_usuario2 { get; set; }
    }
}