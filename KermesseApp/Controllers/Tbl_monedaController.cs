using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KermesseApp.Models;

namespace KermesseApp.Controllers
{
    public class Tbl_monedaController : Controller
    {
        private KERMESSEEntities db = new KERMESSEEntities();

        // GET: Tbl_moneda
        public ActionResult ListMonedas()
        {
            return View(db.tbl_moneda.ToList());
        }

        public ActionResult VwGuardarMoneda()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GuardarMoneda(tbl_moneda tm)
        {
            if (ModelState.IsValid)
            {
                tbl_moneda tMod = new tbl_moneda();

                tMod.nombre = tm.nombre;
                tMod.signo = tm.signo;
                tMod.estado = 1;

                db.tbl_moneda.Add(tMod);
                db.SaveChanges();
                ModelState.Clear();

                return RedirectToAction("ListMonedas");
            }
            return View("VwGuardarMoneda");
        }

        public ActionResult EliminarMoneda(int id)
        {
            tbl_moneda tMod = new tbl_moneda();

            tMod = db.tbl_moneda.Find(id);
            db.tbl_moneda.Remove(tMod);
            db.SaveChanges();

            var list = db.tbl_moneda.ToList();
            return View("ListMonedas", list);
        }

        public ActionResult VerMoneda(int id)
        {
            var moneda = db.tbl_moneda.Where(x => x.id_moneda == id).First();
            return View(moneda);
        }

        public ActionResult EditarMoneda(int id)
        {
            tbl_moneda tMod = db.tbl_moneda.Find(id);
            if(tMod == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(tMod);
            }
        }

        [HttpPost]
        public ActionResult ActualizarMoneda(tbl_moneda tm)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    tm.estado = 2;
                    db.Entry(tm).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("ListMonedas");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult FiltrarMoneda(String cadena)
        {
            if (String.IsNullOrEmpty(cadena))
            {
                var list = db.tbl_moneda.ToList();
                return View("ListMonedas", list);
            }
            else
            {
                var listFiltrada = db.tbl_moneda.Where(x => x.nombre.Contains(cadena) || x.signo.Contains(cadena));
                return View("ListMonedas", listFiltrada);
            }
        }
    }
}