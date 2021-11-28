using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KermesseApp.Models;
using Microsoft.Reporting.WebForms;
using System.IO;

namespace KermesseApp.Controllers
{
    public class Tbl_denominacionController : Controller
    {
        private KERMESSEEntities db = new KERMESSEEntities();

        // GET: Tbl_denominacion
        public ActionResult ListDenominaciones()
        {
            return View(db.tbl_denominacion.Where(model => model.estado != 3));
        }

        public ActionResult VistaGuardarDenominacion()
        {
            ViewBag.id_moneda = new SelectList(db.tbl_moneda.Where(model => model.estado != 3), "id_moneda", "nombre");
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
            ViewBag.id_moneda = new SelectList(db.tbl_moneda.Where(model => model.estado != 3), "id_moneda", "nombre");
            return View("VistaGuardarDenominacion");
        }

        public ActionResult EliminarDenominacion(int id)
        {
            tbl_denominacion tDen = new tbl_denominacion();

            tDen = db.tbl_denominacion.Find(id);
            this.LogicalDelete(tDen);
            return RedirectToAction("ListDenominaciones");
        }

        [HttpPost]
        public ActionResult LogicalDelete(tbl_denominacion td)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    td.estado = 3;
                    db.Entry(td).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("ListDenominaciones");
            }
            catch (Exception)
            {
                return View();
                throw;
            }
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
                ViewBag.id_moneda = new SelectList(db.tbl_moneda.Where(model => model.estado != 3), "id_moneda", "nombre");
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
                ViewBag.id_moneda = new SelectList(db.tbl_moneda.Where(model => model.estado != 3), "id_moneda", "nombre");
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
                var listFiltrada = db.tbl_denominacion.Where(x => x.valor.ToString().Contains(cadena) || x.valor_letras.Contains(cadena) ||
                 x.tbl_moneda.nombre.Contains(cadena));
                return View("ListDenominaciones", listFiltrada);
            }
        }

        public ActionResult VerRptDenominacion(String tipo, String cadena)
        {
            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "rptDenominacion.rdlc");
            rpt.ReportPath = ruta;

            ReportDataSource rd = null;
            if (String.IsNullOrEmpty(cadena))
            {
                var listFiltrada = db.Vw_denominacion.Where(x => x.estado != 3);
                rd = new ReportDataSource("dsRptDenominacion", listFiltrada);
            }
            else
            {
                var listFiltrada = db.Vw_denominacion.Where(x => x.estado !=3 && (x.nombre.Contains(cadena) || x.valor.ToString().Contains(cadena) ||
                x.valor_letras.Contains(cadena))); ;
                rd = new ReportDataSource("dsRptDenominacion", listFiltrada);
            }

            rpt.DataSources.Add(rd);
            //tipo = "PDF";
            var b = rpt.Render(tipo, null, out mt, out enc, out f, out s, out w);
            return new FileContentResult(b, mt);
        }
    }
}