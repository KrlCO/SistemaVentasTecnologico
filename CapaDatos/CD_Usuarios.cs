using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;


namespace CapaDatos
{
    public class CD_Usuarios
    {
        public List<Usuario> Listar()
        {

            List<Usuario> lista = new List<Usuario>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {

                    string query = "usp_UsuarioListar";

                    using (SqlCommand cmd = new SqlCommand(query, oconexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        oconexion.Open();

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                lista.Add(
                                    new Usuario()
                                    {
                                        IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                                        Nombres = dr["Nombres"].ToString(),
                                        Apellidos = dr["Apellidos"].ToString(),
                                        Correo = dr["Correo"].ToString(),
                                        Clave = dr["Clave"].ToString(),
                                        Reestablecer = Convert.ToBoolean(dr["Reestablecer"]),
                                        Activo = Convert.ToBoolean(dr["Activo"])
                                    }
                                    );
                            }
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Usuario>();
            }
            return lista;
        }

        public int Registrar(Usuario usr, out string Mensaje)
        {
            int idautogenerate = 0;

            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconec = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarUsuario", oconec);
                    cmd.Parameters.AddWithValue("Nombres", usr.Nombres);
                    cmd.Parameters.AddWithValue("Apellidos", usr.Apellidos);
                    cmd.Parameters.AddWithValue("Correo", usr.Correo);
                    cmd.Parameters.AddWithValue("Clave", usr.Clave);
                    cmd.Parameters.AddWithValue("Activo", usr.Activo);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconec.Open();

                    cmd.ExecuteNonQuery();

                    idautogenerate = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                } 
            }
            catch(Exception ex)
            {
                idautogenerate = 0;
                Mensaje = ex.Message;
            }
            return idautogenerate;
        }

        public bool Editar(Usuario usr, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconec = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditarUsuario", oconec);
                    cmd.Parameters.AddWithValue("IdUsuario", usr.IdUsuario);
                    cmd.Parameters.AddWithValue("Nombres", usr.Nombres);
                    cmd.Parameters.AddWithValue("Apellidos", usr.Apellidos);
                    cmd.Parameters.AddWithValue("Correo", usr.Correo);
                    cmd.Parameters.AddWithValue("Activo", usr.Activo);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconec.Open();

                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }

            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;
        }


        public bool Eliminar(int id, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconec = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EliminarUsuario", oconec);
                    cmd.Parameters.AddWithValue("@IdUsuario", id);
                    cmd.CommandType = CommandType.StoredProcedure;
                    oconec.Open();
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;
        }




    }
}
