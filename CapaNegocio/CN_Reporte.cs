using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Reporte
    {

        private CD_Reporte objCapData = new CD_Reporte();

        public List<Reporte> Ventas(string fechaInicio, string fechaFin, string idTransaccion)
        {
            return objCapData.Ventas(fechaInicio, fechaFin, idTransaccion);
        }

        public Dashboard VerDashboard()
        {
            return objCapData.VerDashboard();
        }

    }
}
    