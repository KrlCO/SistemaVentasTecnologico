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

        public int Registrar(Usuario usr, out string Mensaje)
        {
            Mensaje = string.Empty;
            
            if(string.IsNullOrEmpty(usr.Nombres) || string.IsNullOrWhiteSpace(usr.Nombres))
            {
                Mensaje = "Por favor, ingresa un nombre de Nombres";
            }else if(string.IsNullOrEmpty(usr.Apellidos) || string.IsNullOrWhiteSpace(usr.Apellidos))
            {
                Mensaje = "Por favor, ingresas Apellidos";
            }else if(string.IsNullOrEmpty(usr.Correo) || string.IsNullOrWhiteSpace(usr.Correo))
            {
                Mensaje = "Por favor ingrea un Correo valido";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                string pass = "passwordStrong123";
                usr.Clave = CN_Recursos.ConvertSha256(pass);

                return objCapaDato.Registrar(usr, out Mensaje);

            }
            else
            {
                return 0;
            }


        }



        public bool Editar(Usuario usr, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(usr.Nombres) || string.IsNullOrWhiteSpace(usr.Nombres))
            {
                Mensaje = "El nombre del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(usr.Apellidos) || string.IsNullOrWhiteSpace(usr.Apellidos))
            {
                Mensaje = "El apellido del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(usr.Correo) || string.IsNullOrWhiteSpace(usr.Correo))
            {
                Mensaje = "El correo del usuario no puede ser vacio";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {

                return objCapaDato.Editar(usr, out Mensaje);
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


    }
}
