using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace TpFinalProductos.Models
{
    public class Vehiculo
    {
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        public string nombre { get; set; }
        [Display(Name = "Precio")]
        public int precio { get; set; }

        [Display(Name = "Resumen")]
        public string resumen { get; set; }

        public int tipoDeVehiculoId { get; set; }

        public TipoDeVehiculo? TipoVehiculo {get; set;}
        
        public int marcaId { get; set; }
        
        public Marca? Marca { get; set; }
        public int categoriaId { get; set; }

        public Categoria? Categoria { get; set; }
        public string foto { get; set; }


    }
}
