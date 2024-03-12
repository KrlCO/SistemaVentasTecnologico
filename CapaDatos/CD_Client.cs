using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Client
    {

        public List<Cliente> Listar()
        {
            List<Cliente> list = new List<Cliente>();

            try
            {
                using (SqlConnection xCon = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_ListUsers", xCon);
                    cmd.CommandType = CommandType.StoredProcedure;

                    xCon.Open();

                    using(SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            list.Add(new Cliente()
                            {
                                IdCliente = Convert.ToInt32(dr["IdCliente"]),
                                Nombres = dr["Nombres"].ToString(),
                                Apellidos = dr["Apellidos"].ToString(),
                                Correo = dr["Correo"].ToString(),
                                Clave = dr["Clave"].ToString(),
                                Reestablecer = Convert.ToBoolean(dr["Reestablecer"])
                            });
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                list = new List<Cliente>();
            }
            return list;
        }

        public int Register(Cliente obj, out string Mensaje)
        {
            int idAutoGen = 0;

            Mensaje = string.Empty;
            try
            {
                using(SqlConnection xCon = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegisterClient", xCon);
                    cmd.Parameters.AddWithValue("Nombres", obj.Nombres);
                    cmd.Parameters.AddWithValue("Apellidos", obj.Apellidos);
                    cmd.Parameters.AddWithValue("Correo", obj.Correo);
                    cmd.Parameters.AddWithValue("Clave", obj.Clave);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    xCon.Open();

                    cmd.ExecuteNonQuery();

                    idAutoGen = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch(Exception ex)
            {
                idAutoGen = 0;
                Mensaje = ex.Message;
            }
            return idAutoGen;

        }

        public bool ChangePassword(int idcliente, string newpass, out string Mensaje)
        {

            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using(SqlConnection xCon = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_UpdatePassClient", xCon);
                    cmd.Parameters.AddWithValue("@Id", idcliente);
                    cmd.Parameters.AddWithValue("@Newpass", newpass);
                    cmd.CommandType = CommandType.StoredProcedure;
                    xCon.Open();
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
                    

                }
            }
            catch(Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;

            }
            return resultado;


        }

    }
}
