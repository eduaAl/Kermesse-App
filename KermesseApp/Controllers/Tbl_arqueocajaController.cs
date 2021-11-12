using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KermesseApp.Models;

namespace KermesseApp.Controllers
{
    public class Tbl_arqueocajaController : Controller
    {
        private KERMESSEEntities db = new KERMESSEEntities();

        // GET: Tbl_arqueocaja
        public ActionResult ListArqueos()
        {
            return View(db.tbl_arqueocaja.ToList());
        }

        public ActionResult VistaGuardarArqueo()
        {
            ViewBag.id_kermesse = new SelectList(db.tbl_kermesse, "id_kermesse", "nombre");
            return View();
        }

        [HttpPost]
        public ActionResult GuardarArqueo(tbl_arqueocaja ta)
        {
            if (ModelState.IsValid)
            {
                tbl_arqueocaja tar = new tbl_arqueocaja();

                tar.idkermesse = ta.idkermesse;
                tar.fecha_arqueo = ta.fecha_arqueo;
                tar.gran_total = ta.gran_total;
                tar.usuario_creacion = 1;
                tar.fecha_creacion = DateTime.Now;
                tar.estado = 1;

                db.tbl_arqueocaja.Add(tar);
                db.SaveChanges();
                ModelState.Clear();

                return RedirectToAction("ListArqueos");
            }
            ViewBag.id_kermesse = new SelectList(db.tbl_kermesse, "id_kermesse", "nombre");
            return View("VistaGuardarArqueo");
        }

        public ActionResult EliminarArqueo(int id)
        {
            tbl_arqueocaja tar = new tbl_arqueocaja();

            tar = db.tbl_arqueocaja.Find(id);
            db.tbl_arqueocaja.Remove(tar);
            db.SaveChanges();

            var list = db.tbl_arqueocaja.ToList();
            return View("ListArqueos", list);
        }

        public ActionResult VerArqueo(int id)
        {
            var arqueo = db.tbl_arqueocaja.Where(x => x.id_arqueocaja == id).First();
            return View(arqueo);
        }

        public ActionResult EditarArqueo(int id)
        {
            tbl_arqueocaja ta = db.tbl_arqueocaja.Find(id);
            if (ta == null)
            {
                return HttpNotFound();
            }
            else
            {
                ViewBag.id_kermesse = new SelectList(db.tbl_kermesse, "id_kermesse", "nombre");
                return View(ta);
            }
        }

        [HttpPost]
        public ActionResult ActualizarArqueo(tbl_arqueocaja ta)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ta.estado = 2;
                    db.Entry(ta).State = EntityState.Modified;
                    db.SaveChanges();
                }
                ViewBag.id_kermesse = new SelectList(db.tbl_kermesse, "id_kermesse", "nombre");
                return RedirectToAction("ListArqueos");
            }
            catch
            {
                return View();
                throw;
            }
        }

        [HttpPost]
        public ActionResult FiltrarArqueo(String cadena)
        {
            if (String.IsNullOrEmpty(cadena))
            {
                var list = db.tbl_arqueocaja.ToList();
                return View("ListArqueos", list);
            }
            else
            {
                var listFiltrada = db.tbl_arqueocaja.Where(x => x.fecha_arqueo.ToString().Contains(cadena) || x.gran_total.ToString().Contains(cadena));
                return View("ListArqueos", listFiltrada);
            }
        }
    }
}