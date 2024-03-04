using CapaEntidad;
using CapaNegocio;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacionAdmin.Controllers
{
    public class MantenedorController : Controller
    {
        // GET: Mantenedor
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

        //=========================Categoria====================================\\
        #region CATEGORY
        [HttpGet]
        public JsonResult ListCategories()
        {
            List<Categoria> xlist = new List<Categoria>();
            xlist = new CN_Categoria().Listar();
            return Json(new { data = xlist }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveCategory(Categoria obj)
        {
            object resultado;
            string mensaje = string.Empty;

            if(obj.IdCategoria == 0)
            {
                resultado = new CN_Categoria().Registrar(obj, out mensaje);
            }
            else
            {
                resultado = new CN_Categoria().Editar(obj, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteCategory(int id)
        {
            bool rpta = false;
            string mensaje = string.Empty;

            rpta = new CN_Categoria().Eliminar(id, out mensaje);

            return Json(new { resultado = rpta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }
        #endregion

        //=========================Marca====================================\\
        #region MARK
        [HttpGet]
        public JsonResult ListMark()
        {
            List<Marca> xList = new List<Marca>();
            xList = new CN_Marca().Listar();
            return Json(new { data = xList }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveMark(Marca obj)
        {
            object resultado;
            string mensaje = string.Empty;

            if(obj.IdMarca == 0)
            {
                resultado = new CN_Marca().Registrar(obj, out mensaje);
            }
            else
            {
                resultado = new CN_Marca().Editar(obj, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteMark(int id)
        {
            bool rpta = false;
            string mensaje = string.Empty;

            rpta = new CN_Marca().Eliminar(id, out mensaje);

            return Json(new { resultado = rpta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        //=========================Product=======================================\\
        #region PRODUCT

        [HttpGet]
        public JsonResult ListProduct()
        {
            List<Producto> xList = new List<Producto>();
            xList = new CN_Producto().Listar();
            return Json(new { data = xList }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveProduct(string obj, HttpPostedFileBase archivoImagen)
        {
            string mensaje = string.Empty;
            bool operation_success = true;
            bool save_img_success = true;

            Producto oProd = new Producto();
            oProd = JsonConvert.DeserializeObject<Producto>(obj);

            decimal precio;

            if (decimal.TryParse(oProd.PrecioTexto, NumberStyles.AllowDecimalPoint, new CultureInfo("es-PE"), out precio))
            {
                oProd.Precio = precio;
            }
            else
            {
                return Json(new { operationSuccess = false, mensaje = "El formato del precio debe ser ##.##" }, JsonRequestBehavior.AllowGet);
            }


            if(oProd.IdProducto == 0)
            {
                int idProductGenerate = new CN_Producto().Registrar(oProd, out mensaje);

                if(idProductGenerate != 0)
                {
                    oProd.IdProducto = idProductGenerate;
                }
                else
                {
                    operation_success = false;
                }
            }
            else
            {
                operation_success = new CN_Producto().Editar(oProd, out mensaje);
            }

            if (operation_success)
            {
                if (archivoImagen != null)
                {
                    string rute_save = ConfigurationManager.AppSettings["ServerImages"];
                    string extension = Path.GetExtension(archivoImagen.FileName);
                    string name_img = string.Concat(oProd.IdProducto.ToString(), extension);

                    try
                    {
                        archivoImagen.SaveAs(Path.Combine(rute_save, name_img));
                    }
                    catch(Exception ex)
                    {
                        string msg = ex.Message;
                        save_img_success = false;
                    }

                    if (save_img_success)
                    {
                        oProd.RutaImagen = rute_save;
                        oProd.NombreImagen = name_img;
                        bool rpta = new CN_Producto().GuardarDatosImagen(oProd, out mensaje);
                    }
                    else
                    {
                        mensaje = "El producto fue guardado, pero hubo problemas al guardar la imagen";
                    }

                }
            }

            return Json(new { operation_success = operation_success, idGenerado = oProd.IdProducto, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public JsonResult ImgProduct(int id)
        {
            bool convertion;
            Producto xProd = new CN_Producto().Listar().Where(p => p.IdProducto == id).FirstOrDefault();

            string textBase64 = CN_Recursos.ConvertBase64(Path.Combine(xProd.RutaImagen, xProd.NombreImagen), out convertion);

            return Json(new { convertion = convertion, textBase64 = textBase64, extension = Path.GetExtension(xProd.NombreImagen) }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult DeleteProduct(int id)
        {
            bool rpta = false;
            string mensaje = string.Empty;

            rpta = new CN_Producto().Eliminar(id, out mensaje);

            return Json(new { resultado = rpta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}