using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CapaEntidad;
using CapaNegocio;

namespace CapaPresentacionAdmin.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Usuarios()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListarUsuarios()
        {
            List<Usuario> oLista = new List<Usuario>();

            oLista = new CN_Usuarios().Listar();

            return Json(new {data = oLista }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult RegistrarUsuario(Usuario usr)
        {
            object resultado;
            string mensaje = string.Empty;

            if(usr.IdUsuario == 0)
            {
                resultado = new CN_Usuarios().Registrar(usr, out mensaje);
            }
            else
            {
                resultado = new CN_Usuarios().Editar(usr, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult EliminarUsuario(int id)
        {
            bool rpta = false;
            string mensaje = string.Empty;

            rpta = new CN_Usuarios().Eliminar(id, out mensaje);
            return Json(new { resultado = rpta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

    }
}