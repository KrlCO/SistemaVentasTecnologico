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
                if (xUser.Reestablecer)
                {
                    TempData["IdUsuario"] = xUser.IdUsuario;
                    return RedirectToAction("ChangePassword");
                }

                FormsAuthentication.SetAuthCookie(xUser.Correo, false);
                ViewBag.Error = null;
                return RedirectToAction("Index", "Home");
            }

        }

        [HttpPost]
        public ActionResult ChangePassword(string idusuario, string currentkey, string newpass, string confirmpass)
        {
            Usuario xUser = new Usuario();

            int userId;
            if (!int.TryParse(idusuario, out userId))
            {
                // La cadena idusuario no representa un número entero válido.
                // Debes manejar este caso adecuadamente, por ejemplo, lanzando una excepción o mostrando un mensaje de error al usuario.
                TempData["IdUsuario"] = idusuario;
                ViewData["vclave"] = "";
                ViewBag.Error = "El ID de usuario no es válido";
                return View();
            }

            xUser = new CN_Usuarios().Listar().Where(u => u.IdUsuario == userId).FirstOrDefault();

            if (xUser.Clave != CN_Recursos.ConvertSha256(currentkey))
            {
                TempData["IdUsuario"] = idusuario;
                ViewData["vclave"] = "";
                ViewBag.Error = "La clave no es correcta";
                return View();
            }
            else if (newpass != confirmpass)
            {
                TempData["IdUsuario"] = idusuario;
                ViewData["vclave"] = currentkey;
                ViewBag.Error = "Las claves no coinciden";
                return View();
            }
            ViewData["vclave"] = "";

            newpass = CN_Recursos.ConvertSha256(newpass);

            string mensaje = string.Empty;

            bool rpta = new CN_Usuarios().ChangePass(userId, newpass, out mensaje);

            if (rpta)
            {
                return RedirectToAction("Index");
            }
            else
            {
                TempData["IdUsuario"] = idusuario;

                ViewBag.Error = mensaje;
                return View();
            }
        }

        [HttpPost]
        public ActionResult ResetPassword(string correo)
        {
            Usuario xUser = new Usuario();

            xUser = new CN_Usuarios().Listar().Where(item => item.Correo == correo).FirstOrDefault();

            if(xUser == null)
            {
                ViewBag.Error = "No se encontro un usuario asociado a este email";
                return View();
            }

            string mensaje = string.Empty;
            bool rpta = new CN_Usuarios().ResetPassword(xUser.IdUsuario, correo, out mensaje);

            if (rpta)
            {
                ViewBag.Error = null;
                return RedirectToAction("Index", "Acceso");
            }
            else
            {
                ViewBag.Error = mensaje;
                return View();
            }
        }

        public ActionResult CerrarSesion()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Acceso");
        }

    }
}