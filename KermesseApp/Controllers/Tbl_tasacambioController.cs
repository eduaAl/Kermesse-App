using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KermesseApp.Models;

namespace KermesseApp.Controllers
{
    public class Tbl_tasacambioController : Controller
    {
        private KERMESSEEntities db = new KERMESSEEntities();

        // GET: Tbl_tasacambio
        public ActionResult ListTasaCambio()
        {
            return View(db.tbl_tasacambio.ToList());
        }

        public ActionResult VistaGuardarTasaCambio()
        {
            ViewBag.id_monedaO = new SelectList(db.tbl_moneda, "id_moneda", "nombre");
            ViewBag.id_monedaC = new SelectList(db.tbl_moneda, "id_moneda", "nombre");
            return View();
        }

        [HttpPost]
        public ActionResult GuardarTasaCambio(tbl_tasacambio tc)
        {
            if (ModelState.IsValid)
            {
                tbl_tasacambio tcam = new tbl_tasacambio();

                tcam.id_monedaO = tc.id_monedaO;
                tcam.id_monedaC = tc.id_monedaC;
                tcam.mes = tc.mes;
                tcam.anio = tc.anio;
                tcam.estado = 1;

                db.tbl_tasacambio.Add(tcam);
                db.SaveChanges();
                ModelState.Clear();

                return RedirectToAction("ListTasaCambio");
            }
            ViewBag.id_monedaO = new SelectList(db.tbl_moneda, "id_moneda", "nombre");
            ViewBag.id_monedaC = new SelectList(db.tbl_moneda, "id_moneda", "nombre");
            return View("VistaGuardarTasaCambio");
        }

        public ActionResult EliminarTasaCambio(int id)
        {
            tbl_tasacambio tc = new tbl_tasacambio();

            tc = db.tbl_tasacambio.Find(id);
            db.tbl_tasacambio.Remove(tc);
            db.SaveChanges();

            var list = db.tbl_tasacambio.ToList();
            return View("ListTasaCambio", list);
        }

        public ActionResult VerTasaCambio(int id)
        {
            var tasacambio = db.tbl_tasacambio.Where(x => x.id_tasacambio == id).First();
            return View(tasacambio);
        }

        public ActionResult EditarTasaCambio(int id)
        {
            tbl_tasacambio tc = db.tbl_tasacambio.Find(id);
            if (tc == null)
            {
                return HttpNotFound();
            }
            else
            {
                ViewBag.id_monedaO = new SelectList(db.tbl_moneda, "id_moneda", "nombre");
                ViewBag.id_monedaC = new SelectList(db.tbl_moneda, "id_moneda", "nombre");
                return View(tc);
            }
        }

        [HttpPost]
        public ActionResult ActualizarTasaCambio(tbl_tasacambio tc
        {
            try
            {
                if (ModelState.IsValid)
                {
                    tc.estado = 2;
                    db.Entry(tc).State = EntityState.Modified;
                    db.SaveChanges();
                }
                ViewBag.id_monedaO = new SelectList(db.tbl_moneda, "id_moneda", "nombre");
                ViewBag.id_monedaC = new SelectList(db.tbl_moneda, "id_moneda", "nombre");
                return RedirectToAction("ListTasaCambio");
            }
            catch
            {
                return View();
                throw;
            }
        }

        [HttpPost]
        public ActionResult FiltrarTasaCambio(String cadena)
        {
            if (String.IsNullOrEmpty(cadena))
            {
                var list = db.tbl_tasacambio.ToList();
                return View("ListTasaCambio", list);
            }
            else
            {
                var listFiltrada = db.tbl_tasacambio.Where(x => x.mes.ToString().Contains(cadena) || x.anio.ToString().Contains(cadena));
                return View("ListTasaCambio", listFiltrada);
            }
        }
    }
}