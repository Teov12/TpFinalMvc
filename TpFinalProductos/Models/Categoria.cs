using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace TpFinalProductos.Models
{
    public class Categoria
    {
        public int Id { get; set; }

        [Display(Name = "Categoria")]
        public string descripcion { get; set; }
    }
}
