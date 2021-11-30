using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KermesseApp.Models;
using System.Data.Entity;
using Microsoft.Reporting.WebForms;
using System.IO;

namespace KermesseApp.Controllers
{
    public class Tbl_gastosController : Controller
    {
        private KERMESSEEntities db = new KERMESSEEntities();
        // GET: Tbl_gastos
        public ActionResult Vw_tbl_gastos()
        {
            return View(db.tbl_gastos.Where(x => x.estado != 3));
        }

        public ActionResult Vw_guardarGast()
        { 
            ViewBag.id_kermesse = new SelectList(db.tbl_kermesse.Where(x => x.estado != 3), "id_kermesse", "nombre");
            ViewBag.id_cat_gasto = new SelectList(db.tbl_cat_gastos.Where(x => x.estado != 3), "id_cat_gasto", "nombre_cat");

            return View();
        }

        [HttpPost]
        public ActionResult GuardarGast(tbl_gastos tcg)
        {
            if (ModelState.IsValid)
            {
                tbl_gastos tcgs = new tbl_gastos();
                tcgs.id_kermesse = tcg.id_kermesse;
                tcgs.id_cat_gasto = tcg.id_cat_gasto;
                tcgs.fecha_gasto = tcg.fecha_gasto;
                tcgs.monto = tcg.monto;
                tcgs.concepto = tcg.concepto;
                tcgs.estado = 1;
                tcgs.usuario_creacion = 1;
                tcgs.usuario_eliminacion = null;
                tcgs.usuario_modificacion = null;              
                tcgs.fecha_creacion = DateTime.Now;
                tcgs.fecha_modificacion = null;
                tcgs.fecha_eliminacion = null;

                db.tbl_gastos.Add(tcgs);
                db.SaveChanges();
                ModelState.Clear();
            }

            ViewBag.id_kermesse = new SelectList(db.tbl_kermesse.Where(x => x.estado != 3), "id_kermesse", "nombre");
            ViewBag.id_cat_gasto = new SelectList(db.tbl_cat_gastos.Where(x => x.estado != 3), "id_cat_gasto", "nombre_cat");

            return View("Vw_guardarGast");
        }

        public ActionResult Vw_detGast(int id)
        {
            var gas = db.tbl_gastos.Where(x => x.id_gasto == id).First();
            return View(gas);
        }

        public ActionResult EliminarGast(int id)
        {
            tbl_gastos tcgs = new tbl_gastos();
            tcgs = db.tbl_gastos.Find(id);
            this.LogicalDelete(tcgs);

            var list = db.tbl_gastos.Where(x => x.estado != 3);
            return View("Vw_tbl_gastos", list);
        }

        [HttpPost]
        public ActionResult LogicalDelete(tbl_gastos model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.estado = 3;
                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();
                }
                var list = db.tbl_gastos.Where(x => x.estado != 3);
                return View("Vw_tbl_gastos", list);
            }
            catch (Exception)
            {
                return View();
                throw;
            }
        }

        public ActionResult Vw_EditGast(int id)
        {
            tbl_gastos tcg = db.tbl_gastos.Find(id);

            if (tcg == null)
            {
                return HttpNotFound();
            }
            else
            {
                ViewBag.id_kermesse = new SelectList(db.tbl_kermesse.Where(x => x.estado != 3), "id_kermesse", "nombre");
                ViewBag.id_cat_gasto = new SelectList(db.tbl_cat_gastos.Where(x => x.estado != 3), "id_cat_gasto", "nombre_cat");
                return View(tcg);
               
            }

        }

        public ActionResult UpdtGast(tbl_gastos tcg)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    tcg.estado = 2;
                    tcg.usuario_eliminacion = null;
                    tcg.usuario_modificacion = 1;
                    tcg.fecha_modificacion = DateTime.Now;
                    tcg.fecha_eliminacion = null;

                    db.Entry(tcg).State = EntityState.Modified;
                    db.SaveChanges();
                }
                ViewBag.id_kermesse = new SelectList(db.tbl_kermesse.Where(x => x.estado != 3), "id_kermesse", "nombre");
                ViewBag.id_cat_gasto = new SelectList(db.tbl_cat_gastos.Where(x => x.estado != 3), "id_cat_gasto", "nombre_cat");

                return RedirectToAction("Vw_tbl_gastos");
            }
            catch (Exception)
            {

                return View();
            }
        }

        public ActionResult ViewRptGastos(String tipo, String cadena)
        {
            LocalReport rpt = new LocalReport();
            String mt, enc, f;
            String[] s;
            Warning[] w;

            String ruta = Path.Combine(Server.MapPath("~/Reportes"), "RptGastos.rdlc");
            rpt.ReportPath = ruta;

            var listafiltrada = db.Vw_Gastos.Where(x => x.estado != 3);

            if (String.IsNullOrEmpty(cadena))
            {
                listafiltrada = db.Vw_Gastos.Where(x => x.estado != 3);
            }
            else
            {
                listafiltrada = db.Vw_Gastos.Where(x => (x.kermesse.Contains(cadena) || x.categoria.Contains(cadena)) && x.estado != 3);

            }

            ReportDataSource rd = new ReportDataSource("dsRptGastos", listafiltrada);

            rpt.DataSources.Add(rd);

            var b = rpt.Render(tipo, null, out mt, out enc, out f, out s, out w);

            return new FileContentResult(b, mt);
        }

        public ActionResult BuscarGastos(String cadena)
        {
            if (String.IsNullOrEmpty(cadena))
            {
                var list = db.tbl_gastos.Where(x => x.estado != 3);
                return View("Vw_tbl_gastos", list);
            }
            else
            {
                var listaFiltrada = db.tbl_gastos.Where(x => (x.tbl_kermesse.nombre.Contains(cadena) || x.tbl_cat_gastos.nombre_cat.Contains(cadena)) && x.estado != 3);
                return View("Vw_tbl_gastos", listaFiltrada);
            }
        }
    }
}