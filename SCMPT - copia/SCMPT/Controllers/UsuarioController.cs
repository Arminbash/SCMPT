using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Protocols;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using SCMPT.DataAccess;
using SCMPT.Models;

namespace SCMPT.Controllers
{
    public class UsuarioController : Controller
    {
        public UsuarioController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
        }

        private SCMPTEntities3 ss = new SCMPTEntities3();

        public UsuarioController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }


        public ActionResult us()
        {
            return View();
        }

        // GET: /Usuario/

        #region Nuevo_usuario

        [HttpGet]
        public ActionResult Nuevo_usuario()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Nuevo_usuario(Proc_Usuarioo objCustomer)
        {
            objCustomer.Fecha_Nacimiento = Convert.ToDateTime(objCustomer.Fecha_Nacimiento);
            string id = User.Identity.GetUserId();
            objCustomer.id = id;
            objCustomer.Especialidad = "";
            if (ModelState.IsValid) //checking model is valid or not
            {
                UsuarioDAL objDB = new UsuarioDAL();
                string result = objDB.Nuevo_Usuario(objCustomer, 1);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Error in saving data");
                return View();
            }
        }

        #endregion

        #region Index

        [HttpGet]
        public ActionResult Index()
        {
            UsuarioDAL ojBD = new UsuarioDAL();
            string id = User.Identity.GetUserId();
            var result = ojBD.index(id);
            ViewBag.nombre = result;
            List<Noticia> noticias = ss.Noticia.ToList();
            return View(noticias);
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase FileUpload1)
        {
            //if (FileUpload1 != null && FileUpload1.ContentLength > 0)
            using (BinaryReader reader = new BinaryReader(FileUpload1.InputStream))
            {
                byte[] image = reader.ReadBytes(FileUpload1.ContentLength);

                UsuarioDAL.GuardarImagen(User.Identity.GetUserId(), image);

            }
            return View();
        }

        [HttpGet]
        public ActionResult Examenes()
        {
            UsuarioDAL ojBD = new UsuarioDAL();
            int numid = ojBD.obtener_id(User.Identity.GetUserId());
            List<Citas> temp = ss.Citas.Where(x => x.idPaciente == numid && x.Fecha.Month == DateTime.Now.Month).ToList();
            return View(temp);
        }
        [HttpGet]
        public ActionResult Vercita(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Detalles_Cita det = ss.Detalles_Cita.Where(x => x.idCita == id).FirstOrDefault();
            if (det == null)
            {
                return View();
            }
            List<Resultado_examen> result = ss.Resultado_examen.Where(x => x.idDetalle == det.idDetalle).ToList();
            detaexamen temp = new detaexamen();
            temp.DetallesCita = det;
            temp.ResultadoExamens = result;
            return View(temp);
        }
    #endregion
#region Doctor
        [HttpGet]
        public ActionResult Doctor()
        {
            UsuarioDAL ojBD = new UsuarioDAL();
            string id = User.Identity.GetUserId();
            var result = ojBD.index(id);
            ViewBag.nombre = result;
            result = ojBD.especialidad(id);
            ViewBag.especialidad = result;
            //UsuarioDAL obtener = new UsuarioDAL();
            //List<Pacienteo> pacientes = obtener.citas(User.Identity.GetUserId()).ToList();
            //return View(pacientes);
            int a= ojBD.obtener_iddoctor(User.Identity.GetUserId());
            List<Citas> Citas = ss.Citas.Where(x => x.idDoctor == a && x.Fecha==DateTime.Today).ToList();
            if(Citas.Count >0)
            return View(Citas);
           else
            return View();
        }
        [HttpPost]
        public ActionResult Doctor(HttpPostedFileBase FileUpload1)
        {
            //if (FileUpload1 != null && FileUpload1.ContentLength > 0)
            using (BinaryReader reader = new BinaryReader(FileUpload1.InputStream))
            {
                byte[] image = reader.ReadBytes(FileUpload1.ContentLength);

                UsuarioDAL.GuardarImagen(User.Identity.GetUserId(), image);

            }
            return View();  
        }
#endregion
        public string nombrepersona(int id)
        {
            SCMPTEntities3 ss = new SCMPTEntities3();
            string temp;
            var s = ss.Persona.Where(x => x.idPersona == id).FirstOrDefault();
            temp = s.Primer_Nombre + ' ' + s.Primer_Apellido;
            return temp;
        }

        public ActionResult imagennoticia(int id)
        {
            var s = ss.Noticia.Where(x => x.idNoticia == id).FirstOrDefault();
                return File(s.Imagen, "image/jpeg");
        }
        public ActionResult convertirimagen(string id)
        {
            //SqlConnection con = null;
            //    con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            //    SqlCommand com = new SqlCommand
            //    ("select t2.imagen from AspNetUsers t1 inner join Imagenes t2 on t1.Id = t2.id where t1.Id='" + id + "'", con);
            //    con.Open();
            //   Imagenes es =new Imagenes();
            //    SqlDataReader reader = com.ExecuteReader();
            //    while (reader.Read())
            //    {
            //        byte[] bytBLOBData = new byte[reader.GetBytes(1, 0, null, 0, int.MaxValue)];
            //        reader.GetBytes(1, 0, bytBLOBData, 0, bytBLOBData.Length);
            //        es.imagen = bytBLOBData;
            //    }
            
            var s = ss.Persona.Where(x => x.idUsuario == id).FirstOrDefault();
            if (s.Foto != null)
                return File(s.Foto, "image/jpeg");
            else
                return File("~/images/key2.png","image/jpeg");
        }
        [HttpGet]
        public ActionResult Cita(int? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //Detalles_Cita cit = ss.Detalles_Cita.Find(id);
            //if (cit == null)
            //{
            //    return HttpNotFound();
            //}
            //ViewBag.id = cit.idCita;
            ViewBag.id = id;
            var cit = ss.Citas.Where(x => x.idCita == id).FirstOrDefault();
            ViewBag.nombre = nombrepersona(cit.idPaciente);
            Session["id"] = id;
            return View();
        }

        [HttpPost]
        public ActionResult Cita(SCMPT.Detalles_Cita objCita)
        {
            objCita.idCita = Int32.Parse(Session["id"].ToString());
            objCita.Active = true;
            if (ModelState.IsValid)
            {
                ss.Detalles_Cita.Add(objCita);
                ss.SaveChanges();
                return RedirectToAction("Doctor");
            }

            return View();
        }
      #region Admin
        [HttpGet]
        public ActionResult Admin()
        {
            UsuarioDAL ojBD = new UsuarioDAL();
            string id = User.Identity.GetUserId();
            var result = ojBD.index(id);
            ViewBag.nombre = result;
            return View();
        }
        [HttpPost]
        public ActionResult Admin(int id)
        {
            return View();
        }

#region doctores
        [HttpGet]
        public ActionResult Nuevodoctor()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Nuevodoctor(Usuario_Doctoro objCustomer)
        {
            Proc_Usuarioo us =new Proc_Usuarioo();
            RegisterViewModel re =new RegisterViewModel();
            re.UserName = objCustomer.UserName;
            re.Password = objCustomer.Password;
            re.ConfirmPassword = objCustomer.ConfirmPassword;
            us.Primer_Nombre = objCustomer.Primer_Nombre;
            us.Segundo_Nombre = objCustomer.Segundo_Nombre;
            us.Primer_Apellido = objCustomer.Primer_Apellido;
            us.Segundo_Apellido = objCustomer.Segundo_Apellido;
            us.Genero = objCustomer.Cedula;
            us.Cedula = objCustomer.Cedula;
            us.Email = objCustomer.Email;
            us.Telefono = objCustomer.Telefono;
            us.Especialidad = objCustomer.Especialidad;
            us.Fecha_Nacimiento = Convert.ToDateTime(objCustomer.Fecha_Nacimiento);
            if (ModelState.IsValid) //checking model is valid or not
            {
                var user = new ApplicationUser() { UserName = re.UserName };
                var results = await UserManager.CreateAsync(user, re.Password);
                us.id = user.Id;
                UsuarioDAL objDB = new UsuarioDAL();
                string result = objDB.Nuevo_Usuario(us, 2);
                return RedirectToAction("Admin");
            }
           
                ModelState.AddModelError("", "Error in saving data");
                return View();
        }

        [HttpGet]
        public ActionResult Lista_doctores()
        {
            List<Persona> Doctores =new List<Persona>();
            foreach (var s in ss.Persona)
            {
                if (s.Doctor != null)
                {
                    Doctores.Add(s);
                }
            }
            return View(Doctores.ToList());
        }
#endregion
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Persona persona = ss.Persona.Find(id);
            if (persona == null)
            {
                return HttpNotFound();
            }
            return View(persona);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idPersona,idUsuario,Primer_Nombre,Segundo_Nombre,Primer_Apellido,Segundo_Apellido,Fecha_Nacimiento,Genero,Cedula,Telefono,Email,Foto,Active")] Persona Doc, HttpPostedFileBase FileUpload1)
        {
           
            if (FileUpload1 != null && FileUpload1.ContentLength > 0)
            {
                byte[] image = null;
                using (BinaryReader reader = new BinaryReader(FileUpload1.InputStream))
                {
                    image = reader.ReadBytes(FileUpload1.ContentLength);
                }
                Doc.Foto = image;
            }
            if (ModelState.IsValid)
            {
                ss.Entry(Doc).State = EntityState.Modified;
                ss.SaveChanges();
                return RedirectToAction("Admin");
            }
            return View();
        }

#region paciente
        [HttpGet]
        public ActionResult Nuevopaciente()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Nuevopaciente(Usuario_pacienteo  objCustomer)
        {
            Proc_Usuarioo us = new Proc_Usuarioo();
            RegisterViewModel re = new RegisterViewModel();
            re.UserName = objCustomer.UserName;
            re.Password = objCustomer.Password;
            re.ConfirmPassword = objCustomer.ConfirmPassword;
            us.Primer_Nombre = objCustomer.Primer_Nombre;
            us.Segundo_Nombre = objCustomer.Segundo_Nombre;
            us.Primer_Apellido = objCustomer.Primer_Apellido;
            us.Segundo_Apellido = objCustomer.Segundo_Apellido;
            us.Genero = objCustomer.Cedula;
            us.Cedula = objCustomer.Cedula;
            us.Email = objCustomer.Email;
            us.Telefono = objCustomer.Telefono;
            us.Fecha_Nacimiento = Convert.ToDateTime(objCustomer.Fecha_Nacimiento);
            if (ModelState.IsValid) //checking model is valid or not
            {
                var user = new ApplicationUser() { UserName = re.UserName };
                var results = await UserManager.CreateAsync(user, re.Password);
                us.id = user.Id;
                UsuarioDAL objDB = new UsuarioDAL();
                string result = objDB.Nuevo_Usuario(us, 1);
                return RedirectToAction("Admin");
            }
                ModelState.AddModelError("", "Error in saving data");
                return View();
        }

        [HttpGet]
        public ActionResult Lista_paciente()
        {
            List<Persona> Pacientes = new List<Persona>();
            foreach (var s in ss.Persona)
            {
                if (s.Paciente != null)
                {
                    Pacientes.Add(s);
                }
            }
            return View(Pacientes.ToList());
        }
#endregion
        [HttpGet]
        public ActionResult ListaCitas()
        {
            List<todacita> citas = new List<todacita>();
            todacita cit = new todacita();
            foreach (var s in ss.Citas)
            {
                if (s.Detalles_Cita != null)
                {
                    var tempo = ss.Detalles_Cita.Where(x => x.idCita == s.idCita).FirstOrDefault();
                    cit.paciente = nombrepersona(s.idPaciente);
                    cit.idCita = s.idCita;
                    cit.doctor = nombrepersona(s.idDoctor);
                    cit.Fecha = s.Fecha;
                    if (tempo != null) cit.idDetalle = tempo.idDetalle;
                    citas.Add(cit);
                }
            }
            return View(citas.ToList());
        }

        [HttpGet]
        public ActionResult Resultados(int? id)
        {
            List<Resultado_examen> result = ss.Resultado_examen.Where(x => x.idDetalle==id).ToList();
            Session["iddet"] = id;
            if (result.Count > 0)
                return View(result);
            else
                return View();
        }

        [HttpPost]
        public ActionResult Resultados(HttpPostedFileBase FileUpload1, string tipo)
        {
            Resultado_examen result = new Resultado_examen();
            if (FileUpload1 != null && FileUpload1.ContentLength > 0)
            {
                using (BinaryReader reader = new BinaryReader(FileUpload1.InputStream))
                {
                   byte[] image = reader.ReadBytes(FileUpload1.ContentLength);
                    result.Resultado = image;
                }
            result.Fecha=DateTime.Now;
            result.idDetalle = Int32.Parse(Session["iddet"].ToString());
                result.tipo = tipo;
                result.Active = true;
            if (ModelState.IsValid)
            {
                ss.Resultado_examen.Add(result);
                ss.SaveChanges();
            }
             }
            return RedirectToAction("Resultados",new {id =result.idDetalle});
        }
        public ActionResult exmanenimagen(int id)
        {

            var s = ss.Resultado_examen.Where(x => x.idResultado == id).FirstOrDefault();
                return File(s.Resultado, "image/jpeg");
        #endregion
    }
}
}