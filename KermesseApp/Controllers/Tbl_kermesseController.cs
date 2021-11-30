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
    public class Tbl_kermesseController : Controller
    {
        private KERMESSEEntities db = new KERMESSEEntities();
        // GET: Tbl_kermesse
        public ActionResult Vw_tbl_kermesse()
        {
            return View(db.tbl_kermesse.Where(x => x.estado != 3));
        }

        public ActionResult Vw_guardarKer ()
        {
            ViewBag.id_parroquia = new SelectList(db.tbl_parroquia, "idparroquia", "nombre");
            return View();
        }

        //Crear una vista
        [HttpPost]
        public ActionResult GuardarKer(tbl_kermesse tcke)
        {
            if (ModelState.IsValid)
            {
                tbl_kermesse tcker = new tbl_kermesse();
                tcker.id_parroquia = tcke.id_parroquia;
                tcker.nombre = tcke.nombre;
                tcker.fecha_inicio = tcke.fecha_inicio;
                tcker.fecha_fin = tcke.fecha_fin;
                tcker.desc_general = tcke.desc_general;
                tcker.usuario_creacion = 1;
                tcker.fecha_creacion = DateTime.Now;
                tcker.estado = 1;
                tcker.usuario_eliminacion = null;
                tcker.usuario_modificacion = null;
                tcker.fecha_modificacion = null;
                tcker.fecha_eliminacion = null;

                db.tbl_kermesse.Add(tcker);
                db.SaveChanges();
                ModelState.Clear();
            }
           
            ViewBag.id_parroquia = new SelectList(db.tbl_parroquia, "idparroquia", "nombre");

            return View("Vw_guardarKer");

        }

        //se le agrega vista
        public ActionResult Vw_detKer(int id)
        {
            var ker = db.tbl_kermesse.Where(x => x.id_kermesse == id).First();
            return View(ker);
        }

        public ActionResult EliminarKer(int id)
        {
            tbl_kermesse tcke = new tbl_kermesse();
            tcke = db.tbl_kermesse.Find(id);
            this.LogicalDelete(tcke);

            var list = db.tbl_kermesse.Where(x => x.estado != 3);
            return View("Vw_tbl_kermesse", list);
        }

        [HttpPost]
        public ActionResult LogicalDelete(tbl_kermesse model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.estado = 3;
                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();
                }
                var list = db.tbl_kermesse.Where(x => x.estado != 3);
                return View("Vw_tbl_kermesse", list);
            }
            catch (Exception)
            {
                return View();
                throw;
            }
        }
        //Se agrega vista
        public ActionResult Vw_EditKer(int id)
        {
            tbl_kermesse tcke = db.tbl_kermesse.Find(id);

            

            if (tcke == null)
            {
                return HttpNotFound();
            }
            else
            {
                ViewBag.id_parroquia = new SelectList(db.tbl_parroquia, "idparroquia", "nombre");
                return View(tcke);
            }

        }

        [HttpPost]
        public ActionResult UpdtKer(tbl_kermesse tcke)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    tcke.estado = 2;
                    tcke.usuario_eliminacion = null;
                    tcke.usuario_modificacion = 1;
                    tcke.fecha_modificacion = DateTime.Now;
                    tcke.fecha_eliminacion = null;

                    db.Entry(tcke).State = EntityState.Modified;
                    db.SaveChanges();
                }

                ViewBag.id_parroquia = new SelectList(db.tbl_parroquia, "idparroquia", "nombre");
                return RedirectToAction("Vw_tbl_kermesse");
            }
            catch (Exception)
            {

                return View();
            }
        }

        //public ActionResult ViewRptKermesse(String tipo)
        //{
        //    LocalReport lrpt = new LocalReport();
        //    String mt, enc, f;
        //    String[] s;
        //    Warning[] war;

        //    String ruta = Path.Combine(Server.MapPath("~/Reportes"), "RptKermesse.rdlc");
        //    lrpt.ReportPath = ruta;

        //    var list = db.Vw_Kermesse.Where(x => x.estado != 3);

        //    ReportDataSource rds = new ReportDataSource("dsRptKermesse", list);

        //    lrpt.DataSources.Add(rds);

        //    var b = lrpt.Render(tipo, null, out mt, out enc, out f, out s, out war);

        //    return new FileContentResult(b, mt);
        //}

        public ActionResult ViewRptKermesse(String tipo, String cadena)
        {
            LocalReport rpt = new LocalReport();
            String mt, enc, f;
            String[] s;
            Warning[] w;

            String ruta = Path.Combine(Server.MapPath("~/Reportes"), "RptKermesse.rdlc");
            rpt.ReportPath = ruta;

            var listafiltrada = db.Vw_Kermesse.Where(x => x.estado != 3);

            if (String.IsNullOrEmpty(cadena))
            {
                listafiltrada = db.Vw_Kermesse.Where(x => x.estado != 3);
            }
            else
            {
                listafiltrada = db.Vw_Kermesse.Where(x => (x.Parroquia.Contains(cadena) || x.Kermesse.Contains(cadena)) && x.estado != 3);

            }

            ReportDataSource rd = new ReportDataSource("dsRptKermesse", listafiltrada);

            rpt.DataSources.Add(rd);

            var b = rpt.Render(tipo, null, out mt, out enc, out f, out s, out w);

            return new FileContentResult(b, mt);
        }

        public ActionResult BuscarKermesse(String cadena)
        {
            if (String.IsNullOrEmpty(cadena))
            {
                var list = db.tbl_kermesse.Where(x => x.estado != 3);
                return View("Vw_tbl_kermesse", list);
            }
            else
            {
                var listaFiltrada = db.tbl_kermesse.Where(x => (x.nombre.Contains(cadena) || x.tbl_parroquia.nombre.Contains(cadena)) && x.estado !=3);
                return View("Vw_tbl_kermesse", listaFiltrada);
            }
        }
    }
}