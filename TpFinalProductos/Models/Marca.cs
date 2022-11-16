using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace TpFinalProductos.Models
{
    public class Marca
    {
        public int Id { get; set; }

        [Display(Name = "Marca")]
        public string Nombre { get; set; }

        [Display(Name = "Fecha de fundacion")]
        public DateTime FechaDeFundacion { get; set; }
    }
}
