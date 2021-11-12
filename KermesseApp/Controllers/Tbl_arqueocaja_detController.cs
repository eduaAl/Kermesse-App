using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KermesseApp.Models;

namespace KermesseApp.Controllers
{
    public class Tbl_arqueocaja_detController : Controller
    {
        private KERMESSEEntities db = new KERMESSEEntities();

        // GET: Tbl_arqueocaja_det
        public ActionResult ListArqueoCajaDet()
        {
            return View(db.tbl_arqueocaja_det.ToList());
        }

        public ActionResult VistaGuardarArqueoCajaDet()
        {
            ViewBag.id_arqueocaja = new SelectList(db.tbl_arqueocaja, "id_arqueocaja", "fecha_arqueo");
            ViewBag.id_moneda = new SelectList(db.tbl_moneda, "id_moneda", "nombre");
            ViewBag.id_denominacion = new SelectList(db.tbl_denominacion, "id_denominacion", "valor");
            return View();
        }

        [HttpPost]
        public ActionResult GuardarArqueoCajaDet(tbl_arqueocaja_det tacd)
        {
            if (ModelState.IsValid)
            {
                tbl_arqueocaja_det tacdet = new tbl_arqueocaja_det();

                tacdet.id_arqueocaja = tacd.id_arqueocaja;
                tacdet.id_moneda = tacd.id_moneda;
                tacdet.id_denominacion = tacd.id_denominacion;
                tacdet.cantidad = tacd.cantidad;
                tacdet.subtotal = tacd.subtotal;

                db.tbl_arqueocaja_det.Add(tacdet);
                db.SaveChanges();
                ModelState.Clear();

                return RedirectToAction("ListArqueoCajaDet");
            }
            ViewBag.id_arqueocaja = new SelectList(db.tbl_arqueocaja, "id_arqueocaja", "fecha_arqueo");
            ViewBag.id_moneda = new SelectList(db.tbl_moneda, "id_moneda", "nombre");
            ViewBag.id_denominacion = new SelectList(db.tbl_denominacion, "id_denominacion", "valor");
            return View("VistaGuardarArqueoCajaDet");
        }

        public ActionResult EliminarArqueoCajaDet(int id)
        {
            tbl_arqueocaja_det tacdet = new tbl_arqueocaja_det();

            tacdet = db.tbl_arqueocaja_det.Find(id);
            db.tbl_arqueocaja_det.Remove(tacdet);
            db.SaveChanges();

            var list = db.tbl_arqueocaja_det.ToList();
            return View("ListArqueoCajaDet", list);
        }

        public ActionResult VerArqueoCajaDet(int id)
        {
            var tacdet = db.tbl_arqueocaja_det.Where(x => x.id_arqueocaja_det == id).First();
            return View(tacdet);
        }

        public ActionResult EditarArqueoCajaDet(int id)
        {
            tbl_arqueocaja_det tacdet = db.tbl_arqueocaja_det.Find(id);
            if (tacdet == null)
            {
                return HttpNotFound();
            }
            else
            {
                ViewBag.id_arqueocaja = new SelectList(db.tbl_arqueocaja, "id_arqueocaja", "fecha_arqueo");
                ViewBag.id_moneda = new SelectList(db.tbl_moneda, "id_moneda", "nombre");
                ViewBag.id_denominacion = new SelectList(db.tbl_denominacion, "id_denominacion", "valor");
                return View(tacdet);
            }
        }

        [HttpPost]
        public ActionResult ActualizarArqueoCajaDet(tbl_arqueocaja_det tacdet)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(tacdet).State = EntityState.Modified;
                    db.SaveChanges();
                }
                ViewBag.id_arqueocaja = new SelectList(db.tbl_arqueocaja, "id_arqueocaja", "fecha_arqueo");
                ViewBag.id_moneda = new SelectList(db.tbl_moneda, "id_moneda", "nombre");
                ViewBag.id_denominacion = new SelectList(db.tbl_denominacion, "id_denominacion", "valor");
                return RedirectToAction("ListArqueoCajaDet");
            }
            catch
            {
                return View();
                throw;
            }
        }

        [HttpPost]
        public ActionResult FiltrarArqueoCajaDet(String cadena)
        {
            if (String.IsNullOrEmpty(cadena))
            {
                var list = db.tbl_arqueocaja_det.ToList();
                return View("ListArqueoCajaDet", list);
            }
            else
            {
                var listFiltrada = db.tbl_arqueocaja_det.Where(x => x.subtotal.ToString().Contains(cadena) || x.cantidad.ToString().Contains(cadena));
                return View("ListArqueoCajaDet", listFiltrada);
            }
        }
    }
}