using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using TpFinalProductos.Models;

namespace TpFinalProductos.ModelsView
{
    public class VehiculosViewModel
    {
        public List<Vehiculo> ListaVehiculos { get; set; }
        public string busquedaNombre { get; set; }
        public string busquedaMarca { get; set; }
        public string busquedaCategoria { get; set; }
        public paginador paginador { get; set; }
    }

    public class paginador
    {
        public int cantReg { get; set; }
        public int regXpag { get; set; }
        public int pagActual { get; set; }
        public int totalPag => (int)Math.Ceiling((decimal)cantReg / regXpag);
        public Dictionary<string, string> ValoresQueryString { get; set; } = new Dictionary<string, string>();
    }
}
