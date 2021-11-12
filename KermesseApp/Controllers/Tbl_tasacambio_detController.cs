using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KermesseApp.Models;

namespace KermesseApp.Controllers
{
    public class Tbl_tasacambio_detController : Controller
    {
        private KERMESSEEntities db = new KERMESSEEntities();

        // GET: Tbl_tasacambio_det
        public ActionResult ListTasaCambioDet()
        {
            return View(db.tbl_tasacambio_det.ToList());
        }

        public ActionResult VistaGuardarTasaCambioDet()
        {
            ViewBag.id_tasacambio = new SelectList(db.tbl_tasacambio, "id_tasacambio", "id_monedaO");
            return View();
        }

        [HttpPost]
        public ActionResult GuardarTasaCambioDet(tbl_tasacambio_det tacadet)
        {
            if (ModelState.IsValid)
            {
                tbl_tasacambio_det tcd = new tbl_tasacambio_det();

                tcd.id_tasacambio = tacadet.id_tasacambio;
                tcd.fecha = tacadet.fecha;
                tcd.tipo_cambio = tacadet.tipo_cambio;
                tcd.estado = 1;

                db.tbl_tasacambio_det.Add(tcd);
                db.SaveChanges();
                ModelState.Clear();

                return RedirectToAction("ListTasaCambioDet");
            }
            ViewBag.id_tasacambio = new SelectList(db.tbl_tasacambio, "id_tasacambio", "id_monedaO");
            return View("VistaGuardarTasaCambioDet");
        }

        public ActionResult EliminarTasaCambioDet(int id)
        {
            tbl_tasacambio_det tcd = new tbl_tasacambio_det();

            tcd = db.tbl_tasacambio_det.Find(id);
            db.tbl_tasacambio_det.Remove(tcd);
            db.SaveChanges();

            var list = db.tbl_tasacambio_det.ToList();
            return View("ListTasaCambioDet", list);
        }

        public ActionResult VerTasaCambioDet(int id)
        {
            var tacadet = db.tbl_tasacambio_det.Where(x => x.id_tasacambio_det == id).First();
            return View(tacadet);
        }

        public ActionResult EditarTasaCambioDet(int id)
        {
            tbl_tasacambio_det tcd = db.tbl_tasacambio_det.Find(id);
            if (tcd == null)
            {
                return HttpNotFound();
            }
            else
            {
                ViewBag.id_tasacambio = new SelectList(db.tbl_tasacambio, "id_tasacambio", "id_monedaO");
                return View(tcd);
            }
        }

        [HttpPost]
        public ActionResult ActualizarTasaCambioDet(tbl_tasacambio_det td)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    td.estado = 2;
                    db.Entry(td).State = EntityState.Modified;
                    db.SaveChanges();
                }
                ViewBag.id_tasacambio = new SelectList(db.tbl_tasacambio, "id_tasacambio", "id_monedaO");
                return RedirectToAction("ListTasaCambioDet");
            }
            catch
            {
                return View();
                throw;
            }
        }

        [HttpPost]
        public ActionResult FiltrarTasaCambioDet(String cadena)
        {
            if (String.IsNullOrEmpty(cadena))
            {
                var list = db.tbl_tasacambio_det.ToList();
                return View("ListTasaCambioDet", list);
            }
            else
            {
                var listFiltrada = db.tbl_tasacambio_det.Where(x => x.fecha.ToString().Contains(cadena) || x.tipo_cambio.ToString().Contains(cadena));
                return View("ListTasaCambioDet", listFiltrada);
            }
        }
    }
}