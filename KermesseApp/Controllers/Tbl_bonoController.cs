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
    public class Tbl_bonoController : Controller
    {
        // GET: Tbl_bono
        private KERMESSEEntities db = new KERMESSEEntities();
  
        public ActionResult Vw_tbl_bono()
        {
            return View(db.tbl_bonos.Where( model => model.estado != 3));
        }

        public ActionResult Vw_Create_bono()
            // Creación de la Vista para guardar un bono
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create_bono(tbl_bonos model)
            // Método para guarda un nuevo registro de bono
        {
            if (ModelState.IsValid)
            {
                tbl_bonos tbl = new tbl_bonos
                {
                    nombre = model.nombre,
                    valor = model.valor,
                    estado = 1
                };

                db.tbl_bonos.Add(tbl);
                db.SaveChanges();
                ModelState.Clear();
                return RedirectToAction("Vw_tbl_bono");
            }

            var list = db.tbl_bonos.ToList();
            return View("Vw_tbl_bono", list);

        }

        public ActionResult Delete_bono(int id)
        {
            tbl_bonos tbl = new tbl_bonos();
            tbl = db.tbl_bonos.Find(id);
            this.LogicalDelete(tbl);
            var list = db.tbl_bonos.Where(x => x.estado != 3);
            return View("Vw_tbl_bono", list);


        }

        [HttpPost]
        public ActionResult LogicalDelete(tbl_bonos model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.estado = 3;
                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();
                }
                var list = db.tbl_bonos.Where(x => x.estado != 3);
                return View("Vw_tbl_bono", list);
            }
            catch (Exception)
            {
                return View();
                throw;
            }
        }

        public ActionResult Vw_Edit_bono(int id)
        {
            tbl_bonos tbl = db.tbl_bonos.Find(id);

            if (tbl == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(tbl);
            }

        }

        public ActionResult Edit_bono(tbl_bonos model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.estado = 2; //Actualizar a estado modificado

                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Vw_tbl_bono");
            }
            catch (Exception)
            {

                return View();
            }
        }

        //public ActionResult ViewRptBono(String tipo)
        //{
        //    LocalReport lrpt = new LocalReport();
        //    String mt, enc, f;
        //    String[] s;
        //    Warning[] war;

        //    String ruta = Path.Combine(Server.MapPath("~/Reportes"), "RptBono.rdlc");
        //    lrpt.ReportPath = ruta;

        //    var list = db.tbl_bonos.Where(model => model.estado != 3);

        //    ReportDataSource rds = new ReportDataSource("dsRptBono", list);

        //    lrpt.DataSources.Add(rds);

        //    var b = lrpt.Render(tipo, null, out mt, out enc, out f, out s, out war);

        //    return new FileContentResult(b, mt);
        //}

        public ActionResult ViewRptBono(String tipo, String cadena)
        {
            LocalReport rpt = new LocalReport();
            String mt, enc, f;
            String[] s;
            Warning[] w;

            String ruta = Path.Combine(Server.MapPath("~/Reportes"), "RptBono.rdlc");
            rpt.ReportPath = ruta;

            var listafiltrada = db.tbl_bonos.Where(model => model.estado != 3);


            if (String.IsNullOrEmpty(cadena))
            {
                listafiltrada = db.tbl_bonos.Where(x => x.estado != 3);
            }
            else
            {
                listafiltrada = db.tbl_bonos.Where(x => x.nombre.Contains(cadena) && x.estado != 3);

            }

            ReportDataSource rd = new ReportDataSource("dsRptBono", listafiltrada);

            rpt.DataSources.Add(rd);

            var b = rpt.Render(tipo, null, out mt, out enc, out f, out s, out w);

            return new FileContentResult(b, mt);
        }

        public ActionResult BuscarBono(String cadena)
        {
            if (String.IsNullOrEmpty(cadena))
            {
                var list = db.tbl_bonos.Where(model => model.estado != 3);
                return View("Vw_tbl_bono", list);
            }
            else
            {
                var listaFiltrada = db.tbl_bonos.Where(x => x.nombre.Contains(cadena) && x.estado != 3);
                return View("Vw_tbl_bono", listaFiltrada);
            }
        }
    }
}