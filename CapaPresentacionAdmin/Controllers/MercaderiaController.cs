using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CapaEntidad;
using CapaNegocio;
using Newtonsoft.Json;

namespace CapaPresentacionAdmin.Controllers
{
    [Authorize]
    public class MercaderiaController : Controller
    {
        // GET: Mercaderia
        public ActionResult RegistrarEntrada()
        {
            return View();
        }
        public ActionResult ListadoEntrada()
        {
            return View();
        }
        public ActionResult RegistrarSalida()
        {
            return View();
        }
        public ActionResult ListadoSalidas()
        {
            return View();
        }
         
        public ActionResult Categoria()
        {
            return View();
        }
         public ActionResult Marca()
        {
            return View();
        }
        public ActionResult Producto()
        {
            return View();
        }

        // =============CATEGORIA==============
        #region CATEGORIA
        public JsonResult ListarCategoria()
        {

            List<Categoria> oLista = new List<Categoria>();

            oLista = new CN_Categoria().Listar();

            //return Json(oLista, JsonRequestBehavior.AllowGet);
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);//cambiamos la vista para dataTables
        }


        [HttpPost]
        public JsonResult GuardarCategoria(Categoria objeto)
        {
            object resultado;
            string mensaje = string.Empty;
            if (objeto.IdCategoria == 0)//verifica si es un usuario nuevo pasado por objeto
            {
                resultado = new CN_Categoria().Registrar(objeto, out mensaje);//devuelve el Id del usuario
            }
            else
            {
                resultado = new CN_Categoria().Editar(objeto, out mensaje);// edita al usurio 
            }
            //devuelve un json con el resultado de los anteriores if llamado igual resultado
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);//cambiamos la vista para dataTables

        }



        [HttpPost]
        public JsonResult EliminarCategoria(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Categoria().Eliminar(id, out mensaje);
            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        //===============MARCA===============

        #region MARCA
        public JsonResult ListarMarca()
        {

            List<Marca> oLista = new List<Marca>();

            oLista = new CN_Marca().Listar();

            //return Json(oLista, JsonRequestBehavior.AllowGet);
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);//cambiamos la vista para dataTables
        }


        [HttpPost]
        public JsonResult GuardarMarca(Marca objeto)
        {
            object resultado;
            string mensaje = string.Empty;
            if (objeto.IdMarca == 0)//verifica si es un usuario nuevo pasado por objeto
            {
                resultado = new CN_Marca().Registrar(objeto, out mensaje);//devuelve el Id del usuario
            }
            else
            {
                resultado = new CN_Marca().Editar(objeto, out mensaje);// edita al usurio 
            }
            //devuelve un json con el resultado de los anteriores if llamado igual resultado
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);//cambiamos la vista para dataTables

        }



        [HttpPost]
        public JsonResult EliminarMarca(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Marca().Eliminar(id, out mensaje);
            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        //===============PRODUCTO===============
        #region PRODUCTO
        public JsonResult ListarProducto()
        {

            List<Producto> oLista = new List<Producto>();

            oLista = new CN_Producto().Listar();

            //return Json(oLista, JsonRequestBehavior.AllowGet);
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);//cambiamos la vista para dataTables
        }



        [HttpPost]
        public JsonResult GuardarProducto(string objeto, HttpPostedFileBase archivoImagen)
        {
            //object resultado;
            string mensaje = string.Empty;
            bool operacion_exitosa = true;
            bool guardar_imagen_exito = true; 

            Producto oProducto = new Producto();
            oProducto = JsonConvert.DeserializeObject<Producto>(objeto);
            decimal precio;
            //estrae el precio de la BD y lo Transforma al formato de la region 
            if (decimal.TryParse(oProducto.PrecioTexto, NumberStyles.AllowDecimalPoint,new CultureInfo("es-PE"),out precio))
            {
                oProducto.Precio = precio;
            }
            else
            {
                   //en caso de fallar la transformacion devuelve un mensaje
                return Json(new { operacionExitosa = false, mensaje = "El formato del precio debe ser ##.##" }, JsonRequestBehavior.AllowGet);
            }


            if (oProducto.IdProducto == 0)//verifica si es un producto es distinto de vacio
            {
                int idProductoGenerado = new CN_Producto().Registrar(oProducto, out mensaje);//devuelve el Id del producto
                if (idProductoGenerado!=0) //captura el id de producto
                {
                    oProducto.IdProducto = idProductoGenerado;
                }
                else
                {
                    operacion_exitosa = false;
                }
            }
            else
            {
                operacion_exitosa = new CN_Producto().Editar(oProducto, out mensaje);// edita al producto 
            }



            if (operacion_exitosa)
            {
                if (archivoImagen != null)
                {
                    string ruta_guardar = ConfigurationManager.AppSettings["ServidorFotos"];//extrae de Configuracion el campo para guardar las imagnes en app dentro de web.config
                    string extension = Path.GetExtension(archivoImagen.FileName);//para obtener las extenciones
                    string nombre_imagen = string.Concat(oProducto.IdProducto.ToString(), extension);//para extraer el nombre de la imagen
                    try
                    {
                        archivoImagen.SaveAs(Path.Combine(ruta_guardar, nombre_imagen));//almacena la imagen en la ruta
                    }catch(Exception ex)
                    {
                        string msg = ex.Message;
                        guardar_imagen_exito = false;
                    }

                    if (guardar_imagen_exito)//si se guarda la imagen almacena los datos en la BD
                    {
                        oProducto.RutaImagen = ruta_guardar;
                        oProducto.NombreImagen = nombre_imagen;
                        bool rspta = new CN_Producto().GuardarDatosImagen(oProducto, out mensaje);
                    }
                    else
                    {
                        mensaje = "Se guardo el producto pero hubo problemas con la imagen";
                    }
                }
            }


            //devuelve un json con el resultado de los anteriores if llamado igual resultado
            return Json(new { operacionExitosa = operacion_exitosa, idGenerado=oProducto.IdProducto, mensaje = mensaje}, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ImagenProducto(int id)
        {
            bool conversion;
            Producto oproducto = new CN_Producto().Listar().Where(p => p.IdProducto == id).FirstOrDefault();//extrae una consulta
            string textoBase64 = CN_Recursos.ConvertirBase64(Path.Combine(oproducto.RutaImagen, oproducto.NombreImagen),out conversion);


            return Json(new
            {
                conversion = conversion,
                textoBase64=textoBase64,
                extension=Path.GetExtension(oproducto.NombreImagen)
            },
            JsonRequestBehavior.AllowGet
            );
        }


        [HttpPost]
        public JsonResult EliminarProducto(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Producto().Eliminar(id, out mensaje);
            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}