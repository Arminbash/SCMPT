using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SCMPT.DataAccess;

namespace SCMPT.Controllers
{
    public class ArticulosController : Controller
    {
        private SCMPTEntities3 db = new SCMPTEntities3();

        //
        // GET: /Articulos/
         [HttpGet]
        public ActionResult Crear()
        {
            return View();
        }
         [HttpPost]
        public ActionResult Crear(Noticia objCustomer,HttpPostedFileBase FileUpload1)
        {
            UsuarioDAL usu = new UsuarioDAL();
            objCustomer.idDoctor = usu.obtener_iddoctor(User.Identity.GetUserId());
            using (BinaryReader reader = new BinaryReader(FileUpload1.InputStream))
            {
                byte[] image = reader.ReadBytes(FileUpload1.ContentLength);
                objCustomer.Imagen = image;
            }
            if (ModelState.IsValid)
            {
                db.Noticia.Add(objCustomer);
                db.SaveChanges();
                return RedirectToAction("Doctor", "Usuario");
            }
            ViewBag.error = "Porfavor llene todos los campos";
            return View();
        }
	}
}