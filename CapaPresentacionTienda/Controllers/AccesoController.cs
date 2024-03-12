using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Services.Description;

namespace CapaPresentacionTienda.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        public ActionResult ResetPassword()
        {
            return View();
        }

        public ActionResult ChangePassword()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Index(string email, string pass)
        {
            Cliente xClient = null;

            xClient = new CN_Client().Listar().Where(item => item.Correo == email && item.Clave == CN_Recursos.ConvertSha256(pass)).FirstOrDefault();

            if(xClient == null)
            {
                ViewBag.Error = "Email o clave incorrectas";
                return View();
            }
            else
            {
                if (xClient.Reestablecer)
                {
                    TempData["IdCliente"] = xClient.IdCliente;
                    return RedirectToAction("ChangePassword", "Acceso");
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(xClient.Correo, false);
                    Session["Cliente"] = xClient;

                    ViewBag.Error = null;
                    return RedirectToAction("Index", "Tienda");


                }
            }
        }

        [HttpPost]
        public ActionResult Register(Cliente obj)
        {
            int result;
            string mensaje = string.Empty;

            ViewData["Nombres"] = string.IsNullOrEmpty(obj.Nombres) ? "" : obj.Nombres;
            ViewData["Apellidos"] = string.IsNullOrEmpty(obj.Apellidos) ? "" : obj.Apellidos;
            ViewData["Correo"] = string.IsNullOrEmpty(obj.Correo) ? "" : obj.Correo;

            if (obj.Clave != obj.ConfirmarClave)
            {
                ViewBag.Error = "Las claves no coinciden";
                return View();
            }

            result = new CN_Client().Register(obj, out mensaje);

            if(result > 0)
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

        [HttpPost]
        public ActionResult ResetPassword(string email)
        {
            Cliente client = new Cliente();

            client = new CN_Client().Listar().Where(item => item.Correo == email).FirstOrDefault();

            if(client == null)
            {
                ViewBag.Error = "No existe un Cliente relacionado a ese email";
                return View();
            }

            string messag = string.Empty;
            bool rpta = new CN_Client().ResetPassword(client.IdCliente, email, out messag);

            if (rpta)
            {
                ViewBag.Error = null;
                return RedirectToAction("Index", "Acceso");
            }
            else
            {
                ViewBag.Error = messag;
                return View();
            }
        }

        [HttpPost]
        public ActionResult ChangePassword(string idCliente, string currentKey, string newpass, string confirmpass)
        {
            Cliente xClient = new Cliente();

            xClient = new CN_Client().Listar().Where(u => u.IdCliente == int.Parse(idCliente)).FirstOrDefault();

            if(xClient.Clave != CN_Recursos.ConvertSha256(currentKey))
            {
                TempData["IdCliente"] = idCliente;
                ViewData["vclave"] = "";
                ViewBag.Error = "La clave actual no es correcta";
                return View();
            }
            else if(newpass != confirmpass)
            {
                TempData["IdClien"] = idCliente;
                ViewData["vclave"] = currentKey;
                ViewBag.Error = "Las claves no coinciden";
                return View();
            }
            ViewData["vclave"] = "";

            newpass = CN_Recursos.ConvertSha256(newpass);

            string mensaje = string.Empty;

            bool rpta = new CN_Client().ChangePassword(int.Parse(idCliente), newpass, out mensaje);

            if (rpta)
            {
                return RedirectToAction("Index");
            }
            else
            {
                TempData["IdCliente"] = idCliente;

                ViewBag.Error = mensaje;
                return View();

            }
        }

        public ActionResult CerrarSeion()
        {
            Session["Cliente"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Acceso");
        }


    }
}