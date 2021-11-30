using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using KermesseApp.Models;

namespace KermesseApp.Controllers
{
    public class tbl_rolOpcionController : Controller
    {
        private KERMESSEEntities db = new KERMESSEEntities();
        // GET: tbl_rolOpcion
        public ActionResult ListRolOpcion()
        {
            return View(db.tbl_rol_opcion);
        }
        public ActionResult VguardarRolOpcion()
        {
            ViewBag.id_rol = new SelectList(db.tbl_rol, "id_rol", "rol");
            ViewBag.id_opcion = new SelectList(db.tbl_opciones, "id_opciones", "opcion");
            return View();
        }
        [HttpPost]
        public ActionResult guardarRolOpcion(tbl_rol_opcion tro)
        {
            ViewBag.id_rol = new SelectList(db.tbl_rol, "id_rol", "rol");
            ViewBag.id_opcion = new SelectList(db.tbl_opciones, "id_opciones", "opcion");
            if (ModelState.IsValid)
            {
                tbl_rol_opcion tcRolOpcion = new tbl_rol_opcion();
                tcRolOpcion.id_rol = tro.id_rol;
                tcRolOpcion.id_opcion = tro.id_opcion;
                db.tbl_rol_opcion.Add(tcRolOpcion);
                db.SaveChanges();
                ModelState.Clear();
                return RedirectToAction("ListRolOpcion");

            }
            return View("VguardarRolOpcion");
        }
        public ActionResult deleteRolOpcion(int id)
        {
            tbl_rol_opcion tcRolOpcion = new tbl_rol_opcion();
            tcRolOpcion = db.tbl_rol_opcion.Find(id);
            db.tbl_rol_opcion.Remove(tcRolOpcion);

            db.SaveChanges();
            var list = db.tbl_rol_opcion.ToList();
            return View("ListRolOpcion", list);
        }
        public ActionResult verRolOpcion(int id)
        {
            var catUsuario = db.tbl_rol_opcion.Where(x => x.id_rol_opcion == id).First();
            return View(catUsuario);

        }
        public ActionResult editRolOpcion(int id)
        {
            tbl_rol_opcion tcu = db.tbl_rol_opcion.Find(id);
            ViewBag.id_rol = new SelectList(db.tbl_rol, "id_rol", "rol");
            ViewBag.id_opcion = new SelectList(db.tbl_opciones, "id_opciones", "opcion");
            if (tcu == null)
            {
                return HttpNotFound();

            }
            else
            {
                return View(tcu);
            }
        }
        [HttpPost]
        public ActionResult updateRolOpcion(tbl_rol_opcion tcu)
        {

            try
            {
                ViewBag.id_rol = new SelectList(db.tbl_rol, "id_rol", "rol");
                ViewBag.id_opcion = new SelectList(db.tbl_opciones, "id_opciones", "opcion");
                if (ModelState.IsValid)
                {
                    db.Entry(tcu).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("ListRolOpcion");
            }
            catch
            {
                return View();
            }
        }
    }
}