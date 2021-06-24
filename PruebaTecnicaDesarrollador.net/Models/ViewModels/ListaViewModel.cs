using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PruebaTecnicaDesarrollador.net.Models.ViewModels
{
    public class ListaViewModel
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string identificacion { get; set; }
        public string celular { get; set; }
        public string direccion { get; set; }
        public string ciudad { get; set; }
        public string correo { get; set; }
    }
}