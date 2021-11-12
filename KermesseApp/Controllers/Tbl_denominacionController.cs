using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KermesseApp.Models;

namespace KermesseApp.Controllers
{
    public class Tbl_denominacionController : Controller
    {
        private KERMESSEEntities db = new KERMESSEEntities();

        // GET: Tbl_denominacion
        public ActionResult ListDenominaciones()
        {
            return View(db.tbl_denominacion.ToList());
        }

        public ActionResult VistaGuardarDenominacion()
        {
            ViewBag.id_moneda = new SelectList(db.tbl_moneda, "id_moneda", "nombre");
            return View();
        }

        [HttpPost]
        public ActionResult GuardarDenominacion(tbl_denominacion td)
        {
            if (ModelState.IsValid)
            {
                tbl_denominacion tDen = new tbl_denominacion();

                tDen.id_moneda = td.id_moneda;
                tDen.valor = td.valor;
                tDen.valor_letras = td.valor_letras;
                tDen.estado = 1;

                db.tbl_denominacion.Add(tDen);
                db.SaveChanges();
                ModelState.Clear();

                return RedirectToAction("ListDenominaciones");
            }
            ViewBag.id_moneda = new SelectList(db.tbl_moneda, "id_moneda", "nombre");
            return View("VistaGuardarDenominacion");
        }

        public ActionResult EliminarDenominacion(int id)
        {
            tbl_denominacion tDen = new tbl_denominacion();

            tDen = db.tbl_denominacion.Find(id);
            db.tbl_denominacion.Remove(tDen);
            db.SaveChanges();

            var list = db.tbl_denominacion.ToList();
            return View("ListDenominaciones", list);
        }

        public ActionResult VerDenominacion(int id)
        {
            var denominacion = db.tbl_denominacion.Where(x => x.id_denominacion == id).First();
            return View(denominacion);
        }

        public ActionResult EditarDenominacion(int id)
        {
            tbl_denominacion tDen = db.tbl_denominacion.Find(id);
            if (tDen == null)
            {
                return HttpNotFound();
            }
            else
            {
                ViewBag.id_moneda = new SelectList(db.tbl_moneda, "id_moneda", "nombre");
                return View(tDen);
            }
        }

        [HttpPost]
        public ActionResult ActualizarDenominacion(tbl_denominacion td)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    td.estado = 2;
                    db.Entry(td).State = EntityState.Modified;
                    db.SaveChanges();
                }
                ViewBag.id_moneda = new SelectList(db.tbl_moneda, "id_moneda", "nombre");
                return RedirectToAction("ListDenominaciones");
            }
            catch
            {
                return View();
                throw;
            }
        }

        [HttpPost]
        public ActionResult FiltrarDenominacion(String cadena)
        {
            if (String.IsNullOrEmpty(cadena))
            {
                var list = db.tbl_denominacion.ToList();
                return View("ListDenominaciones", list);
            }
            else
            {
                var listFiltrada = db.tbl_denominacion.Where(x => x.valor.ToString().Contains(cadena) || x.valor_letras.Contains(cadena));
                return View("ListDenominaciones", listFiltrada);
            }
        }
    }
}