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
    public class Tbl_comunidadController : Controller
    {
        // GET: Tbl_comunidad
        private KERMESSEEntities db = new KERMESSEEntities();

        public ActionResult Vw_tbl_comunidad()
        {
            return View(db.tbl_comunidad.Where(model => model.estado != 3));
        }

        public ActionResult Vw_Create_comunidad()
        // Creación de la Vista para guardar una comunidad
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create_comunidad(tbl_comunidad model)
        // Método para guarda un nuevo registro de comunidad
        {
            if (ModelState.IsValid)
            {
                tbl_comunidad tbl = new tbl_comunidad
                {
                    nombre = model.nombre,
                    responsable = model.responsable,
                    desc_contribucion = model.desc_contribucion,
                    estado = 1
                };

                db.tbl_comunidad.Add(tbl);
                db.SaveChanges();
            }
            ModelState.Clear();

            var list = db.tbl_comunidad.Where(x => x.estado != 3);
            return View("Vw_tbl_comunidad", list);

        }

        public ActionResult Vw_Details_comunidad(int id)
        {
            var com = db.tbl_comunidad.Where(x => x.id_comunidad == id).First();
            return View(com);
        }

        public ActionResult Delete_comunidad(int id)
        {
            tbl_comunidad tbl = new tbl_comunidad();
            tbl = db.tbl_comunidad.Find(id);
            this.LogicalDelete(tbl);

            var list = db.tbl_comunidad.Where(model => model.estado != 3);
            return View("Vw_tbl_comunidad", list);
        }

        [HttpPost]
        public ActionResult LogicalDelete(tbl_comunidad model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.estado = 3;
                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();
                }
                var list = db.tbl_comunidad.Where(x => x.estado != 3);
                return View("Vw_tbl_comunidad", list);
            }
            catch (Exception)
            {
                return View();
                throw;
            }
        }

        public ActionResult Vw_Edit_comunidad(int id)
        {
            tbl_comunidad tbl = db.tbl_comunidad.Find(id);

            if (tbl == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(tbl);
            }

        }

        public ActionResult Edit_comunidad(tbl_comunidad model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.estado = 2;

                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Vw_tbl_comunidad");
            }
            catch (Exception)
            {

                return View();
            }
        }

        //public ActionResult ViewRptComunidad(String tipo)
        //{
        //    LocalReport lrpt = new LocalReport();
        //    String mt, enc, f;
        //    String[] s;
        //    Warning[] war;

        //    String ruta = Path.Combine(Server.MapPath("~/Reportes"), "RptComunidad.rdlc");
        //    lrpt.ReportPath = ruta;

        //    var list = db.tbl_comunidad.Where(model => model.estado != 3);

        //    ReportDataSource rds = new ReportDataSource("dsRptComunidad", list);

        //    lrpt.DataSources.Add(rds);

        //    var b = lrpt.Render(tipo, null, out mt, out enc, out f, out s, out war);

        //    return new FileContentResult(b, mt);
        //}

        public ActionResult ViewRptComunidad(String tipo, String cadena)
        {
            LocalReport rpt = new LocalReport();
            String mt, enc, f;
            String[] s;
            Warning[] w;

            String ruta = Path.Combine(Server.MapPath("~/Reportes"), "RptComunidad.rdlc");
            rpt.ReportPath = ruta;

            var listafiltrada = db.tbl_comunidad.Where(x => x.estado != 3);

            if (String.IsNullOrEmpty(cadena))
            {
                listafiltrada = db.tbl_comunidad.Where(x => x.estado != 3);
            }
            else
            {
                listafiltrada = db.tbl_comunidad.Where(x => (x.nombre.Contains(cadena) || x.responsable.Contains(cadena)) && x.estado != 3);

            }

            ReportDataSource rd = new ReportDataSource("dsRptComunidad", listafiltrada);

            rpt.DataSources.Add(rd);

            var b = rpt.Render(tipo, null, out mt, out enc, out f, out s, out w);

            return new FileContentResult(b, mt);
        }

        public ActionResult BuscarComunidad(String cadena)
        {
            if (String.IsNullOrEmpty(cadena))
            {
                var list = db.tbl_comunidad.Where(model => model.estado != 3);
                return View("Vw_tbl_comunidad", list);
            }
            else
            {
                var listaFiltrada = db.tbl_comunidad.Where(x => (x.nombre.Contains(cadena) || x.responsable.Contains(cadena)) && x.estado != 3);
                return View("Vw_tbl_comunidad", listaFiltrada);
            }
        }
    }
}