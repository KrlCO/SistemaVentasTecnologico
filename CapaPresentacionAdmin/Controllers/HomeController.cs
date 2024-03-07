using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CapaEntidad;
using CapaNegocio;
using ClosedXML.Excel;

namespace CapaPresentacionAdmin.Controllers
{
    [Authorize] //Para poder autorizar el acceso solo si se ha utilizado las credenciales adecuadas
    //Dentro del Web.config se termino de configurar la restriccion
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

        [HttpGet]
        public JsonResult ListReport(string fechaInicio, string fechaFin, string idTransaccion)
        {
            List<Reporte> xList = new List<Reporte>();

            xList = new CN_Reporte().Ventas(fechaInicio, fechaFin, idTransaccion);

            return Json(new { data = xList }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult VistaDashboard()
        {
            Dashboard obj = new CN_Reporte().VerDashboard();

            return Json(new { resultado = obj }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public FileResult ExportSale(string fechaIni, string fechaFin, string idTrans)
        {
            List<Reporte> xList = new List<Reporte>();
            xList = new CN_Reporte().Ventas(fechaIni, fechaFin, idTrans);

            DataTable dt = new DataTable();

            dt.Locale = new System.Globalization.CultureInfo("es-Pe");
            dt.Columns.Add("Fecha Venta", typeof(string));
            dt.Columns.Add("Cliente", typeof(string));
            dt.Columns.Add("Producto", typeof(string));
            dt.Columns.Add("Precio", typeof(decimal));
            dt.Columns.Add("Cantidad", typeof(int));
            dt.Columns.Add("Total", typeof(decimal));
            dt.Columns.Add("IdTransaccion", typeof(string));

            foreach(Reporte rp in xList)
            {
                dt.Rows.Add(new object[]
                {
                    rp.FechaVenta,
                    rp.Cliente,
                    rp.Producto,
                    rp.Precio,
                    rp.Cantidad,
                    rp.Total,
                    rp.IdTransaccion
                });
            }

            dt.TableName = "Datos";

            using(XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using(MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheettml.sheet", "ReportSales" + DateTime.Now.ToString() + ".xlsx");
                    
                }
            }
        }

    }
}