using CapaEntidad;
using CapaNegocio;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CapaPresentacionAdmin.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        public ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string correo, string clave)
        {
            Usuario xUser = new Usuario();

            xUser = new CN_Usuarios().Listar().Where(u => u.Correo == correo && u.Clave == CN_Recursos.ConvertSha256(clave)).FirstOrDefault();

            if (xUser == null)
            {
                ViewBag.Error = "Email o Contraseña erroneo";
                return View();
            }
            else
            {
                ViewBag.Error = null;
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

    }
}