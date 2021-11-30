using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using KermesseApp.Models;

namespace KermesseApp.Controllers
{
    public class tbl_rolController : Controller
    {
        private KERMESSEEntities db = new KERMESSEEntities();
        // GET: tbl_rol
        public ActionResult ListRol()
        {
            return View(db.tbl_rol.ToList());
        }
        public ActionResult VguardarRol()
        {
            return View();
        }
        [HttpPost]
        public ActionResult guardarRol(tbl_rol tcr)
        {
            if (ModelState.IsValid)
            {
                tbl_rol tcRol = new tbl_rol();
                tcRol.rol = tcr.rol;
                tcRol.estado = 1;
                db.tbl_rol.Add(tcRol);
                db.SaveChanges();
                return RedirectToAction("ListRol");
            }
            ModelState.Clear();
            return View("VguardarRol");
        }
        public ActionResult deleteRol(int id)
        {
            tbl_rol tcRol = new tbl_rol();
            tcRol = db.tbl_rol.Find(id);
            db.tbl_rol.Remove(tcRol);

            db.SaveChanges();
            var list = db.tbl_rol.ToList();
            return View("ListRol", list);
        }
        public ActionResult verRol(int id)
        {
            var catRol = db.tbl_rol.Where(x => x.id_rol == id).First();
            return View(catRol);

        }
        public ActionResult editRol(int id)
        {
            tbl_rol tco = db.tbl_rol.Find(id);
            if (tco == null)
            {
                return HttpNotFound();

            }
            else
            {
                return View(tco);
            }
        }
        [HttpPost]
        public ActionResult updateRol(tbl_rol tco)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    tco.estado = 2;
                    db.Entry(tco).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("ListRol");
            }
            catch
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult filtrarRol(String cadena)
        {
            if (String.IsNullOrEmpty(cadena))
            {
                var list = db.tbl_rol.ToList();
                return View("ListRol", list);
            }
            else
            {
                var listFiltrada = db.tbl_rol.Where(x => x.rol.Contains(cadena));
                return View("ListRol", listFiltrada);
            }
        }
    }
}