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

    public partial class tbl_tasacambio_det
    {
        public int id_tasacambio_det { get; set; }

        [Display(Name = "Tasa Cambio: ")]
        [Required(ErrorMessage = "Escriba la tasa de cambio.")]
        public int id_tasacambio { get; set; }

        [Display(Name = "Fecha: ")]
        [Required(ErrorMessage = "Escriba la fecha.")]
        public System.DateTime fecha { get; set; }

        [Display(Name = "Tipo de cambio: ")]
        [Required(ErrorMessage = "Escriba el tipo de cambio.")]
        public decimal tipo_cambio { get; set; }
        public int estado { get; set; }
    
        public virtual tbl_tasacambio tbl_tasacambio { get; set; }
    }
}