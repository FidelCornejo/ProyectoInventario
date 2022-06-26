using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
//    create table salida(
//IdCarr int primary key identity,
//IdCliente int references Cliente(IdCliente),
//IdProducto int references producto(IdProducto),
//Cantidad int)
    public class Salida
    {
        public int IdCarr { get; set; }
        public Cliente oCliente { get; set; }
        public Producto oProducto { get; set; }
    }
}
