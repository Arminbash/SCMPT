using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Microsoft.SqlServer.Server;
using SCMPT.Models;

namespace SCMPT.DataAccess
{
    public class CitasDAL
    {
        public string ProgramarCita(Models.Citaso objcust)
        {

            SqlConnection con = null;
            string result = "";
            try
            {
                //Cambie el string de connection a Default..etc
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
                SqlCommand cmd = new SqlCommand("ProgramarCita", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Idcita", 0);
                cmd.Parameters.AddWithValue("@Idpaciente", objcust.idPaciente);
                cmd.Parameters.AddWithValue("@Iddoctor", objcust.idDoctor);
                cmd.Parameters.AddWithValue("@Fecha", objcust.Fecha);
                cmd.Parameters.AddWithValue("@Hora", objcust.Hora);
                cmd.Parameters.AddWithValue("@Asistida", objcust.Asistida);
                cmd.Parameters.AddWithValue("@Query", 1);
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

        public List<Doctor> Especialidades()
        {
            try
            {
                List<Doctor> list = new List<Doctor>();
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                 {
                     SqlCommand com = new SqlCommand
                   ("select Especialidad from Doctor group by Especialidad", con);
                     con.Open();
                     return list;
                 }
 
            }
            catch
            {
                return null;
            }
        }

        public List<Doctor> listaespecialidades()
        {
            SCMPTEntities3 ss = new SCMPTEntities3();
            List<Doctor> lista = new List<Doctor>();
            lista = ss.Doctor.ToList();
            return lista;
        }
        public List<Doctoro> Doctores(string especialidad)
        {
            try
            {
                List<Doctoro> list = new List<Doctoro>();
                Doctoro es = new Doctoro();
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    SqlCommand com = new SqlCommand
                  ("select t2.idUsuario, t2.Primer_Nombre+' '+t2.Primer_Apellido from Doctor as t1 inner join Persona as t2 on t1.idDoctor = t2.idPersona where t1.Especialidad ='" + especialidad + "'", con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        es.idDoctor = reader[0].ToString();
                        es.Nombre = reader[1].ToString();
                        list.Add(es);
                    }

                    return list;
                }

            }
            catch
            {
                return null;
            }
        }
    }
}