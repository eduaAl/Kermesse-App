using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using KermesseApp.Models;

namespace KermesseApp.Controllers
{
    public class Tbl_opcionesController : Controller
    {

        private KERMESSEEntities db = new KERMESSEEntities();
        // GET: Tbl_opciones
        public ActionResult ListOpciones()
        {
            return View(db.tbl_opciones.ToList());
        }
        public ActionResult VguardarOpcion()
        {
            return View();
        }
        [HttpPost]
        public ActionResult guardarOpcion(tbl_opciones tcp)
        {
            if (ModelState.IsValid)
            {
                tbl_opciones tcOpcion = new tbl_opciones();
                tcOpcion.opcion = tcp.opcion;
                tcOpcion.estado = 1;
                db.tbl_opciones.Add(tcOpcion);
                db.SaveChanges();
                return RedirectToAction("ListOpciones");
            }
            ModelState.Clear();
            return View("VguardarOpcion");
        }
        public ActionResult deleteOpcion(int id)
        {
            tbl_opciones tcOpcion = new tbl_opciones();
            tcOpcion = db.tbl_opciones.Find(id);
            db.tbl_opciones.Remove(tcOpcion);

            db.SaveChanges();
            var list = db.tbl_opciones.ToList();
            return View("ListOpciones", list);
        }

        public ActionResult verOpcion(int id)
        {
            var catOpcion = db.tbl_opciones.Where(x => x.id_opciones == id).First();
            return View(catOpcion);

        }
        public ActionResult editOpcion(int id)
        {
            tbl_opciones tco = db.tbl_opciones.Find(id);
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
        public ActionResult updateOpcion(tbl_opciones tco)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    tco.estado = 2;
                    db.Entry(tco).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("ListOpciones");
            }
            catch
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult filtrarOpcion(String cadena)
        {
            if (String.IsNullOrEmpty(cadena))
            {
                var list = db.tbl_opciones.ToList();
                return View("ListOpciones", list);
            }
            else
            {
                var listFiltrada = db.tbl_opciones.Where(x => x.opcion.Contains(cadena));
                return View("ListOpciones", listFiltrada);
            }
        }
    }
}