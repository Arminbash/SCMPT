using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using SCMPT.Models;

namespace SCMPT.DataAccess
{
    public class UsuarioDAL
    {
        private SCMPTEntities3 db =new SCMPTEntities3();
        #region procesos con usuarios
        public string Nuevo_Usuario(Models.Proc_Usuarioo objcust,int tipo)
        {
            SqlConnection con = null;
            string result = "";
            try
            {
                //Cambie el nombre del connectionString
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
                SqlCommand cmd = new SqlCommand("NuevoUsuario", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", objcust.id);
                cmd.Parameters.AddWithValue("@Primer_Nombre", objcust.Primer_Nombre);
                cmd.Parameters.AddWithValue("@Segundo_Nombre", objcust.Segundo_Nombre);
                cmd.Parameters.AddWithValue("@Primer_Apellido", objcust.Primer_Apellido);
                cmd.Parameters.AddWithValue("@Segundo_Apellido", objcust.Segundo_Apellido);
                cmd.Parameters.AddWithValue("@Fecha_Nacimiento", objcust.Fecha_Nacimiento);
                cmd.Parameters.AddWithValue("@Genero", objcust.Genero);
                cmd.Parameters.AddWithValue("@Cedula", objcust.Cedula);
                cmd.Parameters.AddWithValue("@Telefono", objcust.Telefono);
                cmd.Parameters.AddWithValue("@Email", objcust.Email);
                cmd.Parameters.AddWithValue("@Especialidad", objcust.Especialidad);
                cmd.Parameters.AddWithValue("@Tipo",tipo);
                con.Open();
                result = cmd.ExecuteScalar().ToString();
                return result;
            }
            catch
            {
                return result = "";
            }
            finally
            {
                con.Close();
            }


        }

        public int obtener_id(string id)
        {
            SqlConnection con = null;
            int result ;
            try
            {
                //Cambie el nombre del connectionString
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
                SqlCommand com = new SqlCommand
                ("select t2.idPaciente from Persona t1 inner join Paciente t2 on t1.idPersona=t2.idPaciente where t1.idUsuario='" + id + "'", con);
                con.Open();
                result = Int32.Parse(com.ExecuteScalar().ToString());
                return result;
            }
            catch
            {
                return 0;
            }
            finally
            {
                con.Close();
            }


        }
        public int obtener_iddoctor(string id)
        {
            SqlConnection con = null;
            int result;
            try
            {
                //Cambie el nombre del connectionString
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
                SqlCommand com = new SqlCommand
                ("select t2.idDoctor from Persona t1 inner join Doctor t2 on t1.idPersona=t2.idDoctor where t1.idUsuario='" + id + "'", con);
                con.Open();
                result = Int32.Parse(com.ExecuteScalar().ToString());
                return result;
            }
            catch
            {
                return 0;
            }
            finally
            {
                con.Close();
            }


        }
        public string index(string id)
        {
            SqlConnection con = null;
            string result = "";
            try
            {
                //Cambie el nombre del connectionString
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
                SqlCommand com = new SqlCommand
                   ("select p.Primer_Nombre+' ' +p.Primer_Apellido from Persona as p where idUsuario='" + id+"'", con);
                con.Open();
                result = com.ExecuteScalar().ToString();
                return result;
            }
            catch
            {
                return result = "Default";
            }
            finally
            {
                con.Close();
            }
        }
#endregion
#region doctores
        public string especialidad(string id)
        {
            SqlConnection con = null;
            string result = "";
            try
            {
                //Cambie el nombre del connectionString
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
                SqlCommand com = new SqlCommand
                   ("select 'Dr. '+t2.Especialidad from Persona as t1 inner join Doctor t2 on t1.idPersona= t2.idDoctor where t1.idUsuario='" + id + "'", con);
                con.Open();
                result = com.ExecuteScalar().ToString();
                return result;
            }
            catch
            {
                return result = "Default";
            }
            finally
            {
                con.Close();
            }
        }

        public List<Pacienteo> citas(string id)
        {
            SqlConnection con = null;
            try
            {
                //Cambie el nombre del connectionString
                List<Pacienteo> list = new List<Pacienteo>();
                Pacienteo es = new Pacienteo();
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
                SqlCommand com = new SqlCommand
                   ("select t4.Primer_Nombre +' '+ t4.Segundo_Apellido  as paciente, t3.Fecha , t3.Hora from Persona t1 inner join Doctor t2 on t1.idPersona=t2.idDoctor inner join Citas t3 on t2.idDoctor = t3.idDoctor inner join Persona t4 on t3.idPaciente = t4.idPersona where t1.idUsuario='"+id+"' and  t3.Fecha < GETDATE()", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    es.paciente = Convert.ToString(reader["paciente"]);
                    es.Fecha = Convert.ToDateTime(reader["Fecha"]);
                    es.Hora = Convert.ToDateTime(reader["Hora"]);
                    list.Add(es);
                }
                return list;
            }
            catch
            {
                return null;
            }
            finally
            {
                con.Close();
            }
        }
#endregion
#region usuario
 public int usuariopaciente(string nombre)
        {
            SqlConnection con = null;
            int result =0;
            try
            {
                ////Cambie el nombre del connectionString
                //con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
                //SqlCommand com = new SqlCommand
                //("select t1.UserName from AspNetUsers t1 inner join Persona t2 on t1.Id = t2.idUsuario inner join Paciente t3 on t2.idPersona= t3.idPaciente where t1.UserName='" + nombre + "'", con);
                //con.Open();
                //result = Convert.ToInt32(com.ExecuteScalar());
                var temp = db.AspNetUsers.Where(x => x.UserName == nombre).FirstOrDefault();
                var temp2 = db.Persona.Where(x => x.idUsuario == temp.Id).FirstOrDefault();
                result = temp2.Paciente.idPaciente;
                return result;
            }
            catch
            {
                return 0;
            }
            //finally
            //{
            //    con.Close();
            //}
    }
 
 public int usuariodoctor(string nombre)
 {
     SqlConnection con = null;
     int result=0;
     try
     {
     //    //Cambie el nombre del connectionString
     //    con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
     //    SqlCommand com = new SqlCommand
     //    ("select t1.UserName from AspNetUsers t1 inner join Persona t2 on t1.Id = t2.idUsuario inner join Doctor t3 on t2.idPersona= t3.idDoctor where t1.UserName='" + nombre + "'", con);
     //    con.Open();
     //    result = com.ExecuteNonQuery();
         var temp = db.AspNetUsers.Where(x => x.UserName == nombre).FirstOrDefault();
         var temp2 = db.Persona.Where(x => x.idUsuario == temp.Id).FirstOrDefault();
         result = temp2.Doctor.idDoctor;
         return result;
     }
     catch
     {
         return 0;
     }
     //finally
     //{
     //    con.Close();
     //}
 }
#endregion
   public static void GuardarImagen( string id,byte[] imagen)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                conn.Open();
                string query = "update Persona set Foto=@imagen where idUsuario=@id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                SqlParameter imageParam = cmd.Parameters.Add("@imagen", System.Data.SqlDbType.Image);
                imageParam.Value = imagen;

                cmd.ExecuteNonQuery();

            }
    }

       
    }
}