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
    public class Tbl_cat_gastosController : Controller
    {

        private KERMESSEEntities db = new KERMESSEEntities();
        // GET: Tbl_cat_gastos
        public ActionResult Vw_tbl_cat_gastos()
        {
            return View(db.tbl_cat_gastos.Where(model => model.estado != 3)); 
        }

        public ActionResult Vw_guardarCat_gastos()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GuardarCat_gastos(tbl_cat_gastos tcg)
        {
            if (ModelState.IsValid) 
            { 
                tbl_cat_gastos tcGas = new tbl_cat_gastos();

                tcGas.nombre_cat = tcg.nombre_cat;
                tcGas.desc_cat = tcg.desc_cat;
                tcGas.estado = 1;

                db.tbl_cat_gastos.Add(tcGas);
                db.SaveChanges();
            }
            ModelState.Clear();

            return View("Vw_guardarCat_gastos");

        }

        public ActionResult Vw_detCat_gastos(int id)
        {
            var catGasto = db.tbl_cat_gastos.Where(x => x.id_cat_gasto == id).First();
            return View(catGasto);
        }

        public ActionResult EliminarCat_gasto(int id)
        {
            tbl_cat_gastos tcgto = new tbl_cat_gastos();
            tcgto = db.tbl_cat_gastos.Find(id);
            this.LogicalDelete(tcgto);

            var list = db.tbl_cat_gastos.Where(model => model.estado != 3);
            return View("Vw_tbl_cat_gastos", list);
        }

        [HttpPost]
        public ActionResult LogicalDelete(tbl_cat_gastos model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.estado = 3;
                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();
                }
                var list = db.tbl_cat_gastos.Where(x => x.estado != 3);
                return View("Vw_tbl_cat_gastos", list);
            }
            catch (Exception)
            {
                return View();
                throw;
            }
        }

        public ActionResult Vw_EditCatGastos(int id)
        {
            tbl_cat_gastos tcg = db.tbl_cat_gastos.Find(id);

            if(tcg == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(tcg);
            }
                
        }

        [HttpPost]
        public ActionResult UpdtCat_gasto(tbl_cat_gastos tcg)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    tcg.estado = 2;
                    db.Entry(tcg).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Vw_tbl_cat_gastos");
            }
            catch (Exception)
            {

                return View();
            }
        }

        public ActionResult BuscarCatGastos(String cadena)
        {
            if (String.IsNullOrEmpty(cadena))
            {
                var list = db.tbl_cat_gastos.Where(model => model.estado != 3);
                return View("Vw_tbl_cat_gastos", list);
            }
            else
            {
                var listaFiltrada = db.tbl_cat_gastos.Where(x => x.nombre_cat.Contains(cadena) && x.estado != 3);
                return View("Vw_tbl_cat_gastos", listaFiltrada);
            }
        }

        //public ActionResult ViewRptCatGasto(String tipo)
        //{
        //    LocalReport lrpt = new LocalReport();
        //    String mt, enc, f;
        //    String[] s;
        //    Warning[] w;

        //    String ruta = Path.Combine(Server.MapPath("~/Reportes"), "RptCatGastos.rdlc");
        //    lrpt.ReportPath = ruta;

        //    var list = db.tbl_cat_gastos.Where(model => model.estado != 3 );

        //    ReportDataSource rds = new ReportDataSource("dssRptCatGastos", list);

        //    lrpt.DataSources.Add(rds);

        //    var b = lrpt.Render(tipo, null, out mt, out enc, out f, out s, out w);

        //    return new FileContentResult(b, mt);
        //}

        public ActionResult ViewRptCatGasto(String tipo, String cadena)
        {
            LocalReport rpt = new LocalReport();
            String mt, enc, f;
            String[] s;
            Warning[] w;

            String ruta = Path.Combine(Server.MapPath("~/Reportes"), "RptCatGastos.rdlc");
            rpt.ReportPath = ruta;

            var listafiltrada = db.tbl_cat_gastos.Where(model => model.estado != 3);

            if (String.IsNullOrEmpty(cadena))
            {
                listafiltrada = db.tbl_cat_gastos.Where(model => model.estado != 3);
            }
            else
            {
                listafiltrada = db.tbl_cat_gastos.Where(model => (model.nombre_cat.Contains(cadena) || model.desc_cat.Contains(cadena)) && model.estado != 3);

            }

            ReportDataSource rd = new ReportDataSource("dssRptCatGastos", listafiltrada);

            rpt.DataSources.Add(rd);

            var b = rpt.Render(tipo, null, out mt, out enc, out f, out s, out w);

            return new FileContentResult(b, mt);
        }

     
    }
}