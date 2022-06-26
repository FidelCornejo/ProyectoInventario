using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Usuarios
    {
        private CD_Usuarios objCapaDato = new CD_Usuarios();
        public List<Usuario> Listar()
        {
            return objCapaDato.Listar();
        }


        public int Registrar(Usuario obj, out string Mensaje)
        {
            Mensaje = string.Empty;


            if (string.IsNullOrEmpty(obj.Nombres) || string.IsNullOrWhiteSpace(obj.Nombres))
            {
                Mensaje = "El nombre del usuario no puede ser vacio";
            }else if (string.IsNullOrEmpty(obj.Apellidos) || string.IsNullOrWhiteSpace(obj.Apellidos))
            {
                Mensaje = "El apellido del usuario no puede ser vacio";
            }else if (string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo))
            {
                Mensaje = "El correo del usuario no puede ser vacio";
            }


            if (string.IsNullOrEmpty(Mensaje))
            { 


                string clave = CN_Recursos.GenerarClave();//genera un password unico
                string asunto = "Creacion de Cuenta";//crea cabecera de correo
                string mensaje_correo = "<h3>Su cuenta fue creada correctamente</h3></br><p>Su contraseña para acceder es: !clave!</p>";//contenido del mensaje
                mensaje_correo = mensaje_correo.Replace("!clave!", clave);//reemplaza !clave! por su valor verdadero

                // invoca a la funcion enviar correo para ver si envio o no en correo
                bool respuesta = CN_Recursos.EnviarCorreo(obj.Correo, asunto, mensaje_correo);

                if(respuesta)//verfica la respuesta recibida
                {
                    obj.Clave = CN_Recursos.ConvertirSha256(clave);//encripta la clave;
                    return objCapaDato.Registrar(obj, out Mensaje);
                }
                else
                {
                    Mensaje = "No se puede enviar el correo";
                    return 0;
                }

                
            }
            else
            {
                return 0;
            }
        }


        public bool Editar(Usuario obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            if (string.IsNullOrEmpty(obj.Nombres) || string.IsNullOrWhiteSpace(obj.Nombres))
            {
                Mensaje = "El nombre del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Apellidos) || string.IsNullOrWhiteSpace(obj.Apellidos))
            {
                Mensaje = "El apellido del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo))
            {
                Mensaje = "El correo del usuario no puede ser vacio";
            }
            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDato.Editar(obj, out Mensaje);
            }
            else
            {
                return false;
            }
        }
        public bool Eliminar(int id, out string Mensaje)
        {
            return objCapaDato.Eliminar(id, out Mensaje);
        }



        public bool CambiarClave(int idusuario, string nuevaclave, out string Mensaje)
        {
            return objCapaDato.CambiarClave(idusuario, nuevaclave, out Mensaje);
        }







        public bool ReestablecerClave(int idusuario, string correo, out string Mensaje)
        {
            Mensaje = string.Empty;
            string nuevaclave = CN_Recursos.GenerarClave();
            bool resultado = objCapaDato.ReestablecerClave(idusuario, CN_Recursos.ConvertirSha256(nuevaclave), out Mensaje);


            if (resultado)
            {
                string asunto = "Contraseña Reestablecida";//crea cabecera de correo
                string mensaje_correo = "<h3>Su cuenta fue reestablecida correctamente</h3></br><p>Su contraseña para acceder es: !clave!</p>";//contenido del mensaje
                mensaje_correo = mensaje_correo.Replace("!clave!", nuevaclave);//reemplaza !clave! por su valor verdadero

                bool respuesta = CN_Recursos.EnviarCorreo(correo, asunto, mensaje_correo);

                if (respuesta)//verfica la respuesta recibida
                {

                    return true;
                }
                else
                {
                    Mensaje = "No se pudo envia el correo";
                    return false;
                }
            }
            else
            {
                Mensaje = "No se pudo reestablacer la clave";

                return false;
            }
        }

    }


}
