using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Categoria
    {
        private CD_Categoria objCapData = new CD_Categoria();

        public List<Categoria> Listar()
        {
            return objCapData.Listar();
        }

        public int Registrar(Categoria obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if(string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "Por favor, ingresa una categoria!";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapData.Registrar(obj, out Mensaje);
            }

            else
            {
                return 0;
            }
        }

        public bool Editar(Categoria obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if(string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrEmpty(obj.Descripcion))
            {
                Mensaje = "Por favor, ingresa una descripcion para la categoria";
            }
            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapData.Editar(obj, out Mensaje);
            }
            else
            {
                return false;
            }
        }

        public bool Eliminar(int id, out string Mensaje)
        {
            return objCapData.Eliminar(id, out Mensaje);
        }


    }
}
