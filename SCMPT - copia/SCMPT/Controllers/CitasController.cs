using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SCMPT.DataAccess;
using SCMPT.Models;

namespace SCMPT.Controllers
{
	public class CitasController : Controller
	{
        SCMPTEntities3 db = new SCMPTEntities3();
		//
		// GET: /Citas/
		[HttpGet]
		public ActionResult ProgramarCita()
		{
            ViewBag.idDoctor = new SelectList(db.Doctor, "idDoctor", "Especialidad");
            return View();
		}
		[HttpPost]
        public ActionResult ProgramarCita([Bind(Include = "idCita,idDoctor,Fecha,Hora")] Citas citas)
		{
			UsuarioDAL us =new UsuarioDAL();
			
			citas.idPaciente = us.obtener_id(User.Identity.GetUserId());
		    citas.Active = true;
			citas.Asistida = true;
		    if (ModelState.IsValid) //checking model is valid or not
		    {
		        //CitasDAL objDB = new CitasDAL();
		        //string result = objDB.ProgramarCita(cit);
		        //ViewData["result"] = result;
                db.Citas.Add(citas);
                db.SaveChanges();
                return RedirectToAction("Index","Usuario");
		    }
		    ViewBag.idDoctor = new SelectList(db.Doctor, "idDoctor", "Especialidad", citas.idDoctor);
            return View(citas);
		}
	}
}