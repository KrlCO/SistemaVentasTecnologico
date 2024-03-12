using CapaDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Client
    {
        private CD_Client objCap = new CD_Client();

        public List<Cliente> Listar()
        {
            return objCap.Listar();
        }

        public int Register(Cliente obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if(string.IsNullOrEmpty(obj.Nombres) || string.IsNullOrWhiteSpace(obj.Nombres))
            {
                Mensaje = "Este campo no puede quedar vacio";
            }
            else if(string.IsNullOrEmpty(obj.Apellidos) || string.IsNullOrWhiteSpace(obj.Apellidos))
            {
                Mensaje = "Este campo no puede quedar vacio";
            }
            else if(string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo)){
                Mensaje = "Este campo no puede quedar vacio";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                obj.Clave = CN_Recursos.ConvertSha256(obj.Clave);
                return objCap.Register(obj, out Mensaje);
            }
            else
            {
                return 0;
            }

        }

        public bool ChangePassword(int idClient, string newpass, out string Mensaje)
        {
            return objCap.ChangePassword(idClient, newpass, out Mensaje);
        }

        public bool ResetPassword(int idClient, string mail, out string Mensaje)
        {
            Mensaje = string.Empty;
            string newpass = CN_Recursos.GenPassword();
            bool result = objCap.ChangePassword(idClient, CN_Recursos.ConvertSha256(newpass), out Mensaje);

            if (result)
            {
                string asunto = "Password resstablecida";
                string menssage_mail = "<h3>Tu cuenta fue reestablecida correctamenteM</h3></br><p>Tu nueva password para acceder ahora es:!password!</p>";

                bool rpta = CN_Recursos.SendEmail(mail, asunto, menssage_mail);

                if (rpta)
                {
                    return true;
                }
                else
                {
                    Mensaje = "No se pudo enviar el email";
                    return false;
                }
            }
            else
            {
                Mensaje = "No se pudo reestablecer el password";
                return false;
            }


        }



    }
}
