using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using KermesseApp.Models;

namespace KermesseApp.Controllers
{
    
    public class tbl_rolUsuarioController : Controller
    {
        private KERMESSEEntities db = new KERMESSEEntities();
        // GET: rolUsuario
        public ActionResult ListRolUsuario()
        {
            return View(db.tbl_rol_usuario);
        }
        public ActionResult VguardarRolUsuario()
        {
            ViewBag.id_rol = new SelectList(db.tbl_rol, "id_rol", "rol");
            ViewBag.id_usuario = new SelectList(db.tbl_usuario, "id_usuario", "usuario");
            return View();
        }
        [HttpPost]
        public ActionResult guardarRolUsuario(tbl_rol_usuario tru)
        {
            ViewBag.id_rol = new SelectList(db.tbl_rol, "id_rol", "rol");
            ViewBag.id_usuario = new SelectList(db.tbl_usuario, "id_usuario", "usuario");
            if (ModelState.IsValid)
            {
                tbl_rol_usuario tcRolUsuario = new tbl_rol_usuario();
                tcRolUsuario.id_rol = tru.id_rol;
                tcRolUsuario.id_usuario = tru.id_usuario;
                db.tbl_rol_usuario.Add(tcRolUsuario);
                db.SaveChanges();
                ModelState.Clear();
                return RedirectToAction("ListRolUsuario");

            }
            return View("VguardarRolUsuario");
        }
        public ActionResult deleteRolUsuario(int id)
        {
            tbl_rol_usuario tcRolUsuario = new tbl_rol_usuario();
            tcRolUsuario = db.tbl_rol_usuario.Find(id);
            db.tbl_rol_usuario.Remove(tcRolUsuario);

            db.SaveChanges();
            var list = db.tbl_rol_usuario.ToList();
            return View("ListRolUsuario", list);
        }

        public ActionResult verRolUsuario(int id)
        {
            var catUsuario = db.tbl_rol_usuario.Where(x => x.id_rol_usuario == id).First();
            return View(catUsuario);

        }
        public ActionResult editRolUsuario(int id)
        {
            tbl_rol_usuario tcu = db.tbl_rol_usuario.Find(id);
            ViewBag.id_rol = new SelectList(db.tbl_rol, "id_rol", "rol");
            ViewBag.id_usuario = new SelectList(db.tbl_usuario, "id_usuario", "usuario");
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
        public ActionResult updateRolUsuario(tbl_rol_usuario tcu)
        {
            
            try
            {
                ViewBag.id_rol = new SelectList(db.tbl_rol, "id_rol", "rol");
                ViewBag.id_usuario = new SelectList(db.tbl_usuario, "id_usuario", "usuario");
                if (ModelState.IsValid)
                {
                    db.Entry(tcu).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("ListRolUsuario");
            }
            catch
            {
                return View();
            }
        }
    }
}