using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace TpFinalProductos.Models
{
    public class TipoDeVehiculo
    {
        public int Id { get; set; }

        [Display(Name = "Vehiculo")]
        public string vehiculo { get; set; }
    }
}
