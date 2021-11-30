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
    public class Tbl_parroquiaController : Controller
    {
        private KERMESSEEntities db = new KERMESSEEntities();
        // GET: Tbl_parroquia
        public ActionResult Vw_tbl_parroquia()
        {
            return View(db.tbl_parroquia.ToList());
        }

        //Se crea la vista para guardar una parroquia
        public ActionResult Vw_guardarParroquia()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GuardarParroquia(tbl_parroquia tcpa) //Método para guardar una parroquia
        {
            if (ModelState.IsValid)
            {
                tbl_parroquia tcpar = new tbl_parroquia();
                tcpar.nombre = tcpa.nombre;
                tcpar.direccion = tcpa.direccion;
                tcpar.telefono = tcpa.telefono;
                tcpar.parroco = tcpa.parroco;
                tcpar.url_logo = tcpa.url_logo;
                tcpar.sitio_web = tcpa.sitio_web;

                db.tbl_parroquia.Add(tcpar);
                db.SaveChanges();
            }
            ModelState.Clear();

            return View("Vw_guardarParroquia");

        }

        //Vista para visualizar los detalles de una parroquia
        public ActionResult Vw_detParroquia(int id) 
        {
            var par = db.tbl_parroquia.Where(x => x.idparroquia == id).First();
            return View(par);
        }

        public ActionResult EliminarParroquia(int id) //Método para eliminar una parroquia
        {
            tbl_parroquia tcpa = new tbl_parroquia();
            tcpa = db.tbl_parroquia.Find(id);
            db.tbl_parroquia.Remove(tcpa);
            db.SaveChanges();

            var list = db.tbl_parroquia.ToList();
            return View("Vw_tbl_parroquia", list);
        }

        //Vista para editar una parroquia
        public ActionResult Vw_EditParroquia(int id)
        {
            tbl_parroquia tcpa = db.tbl_parroquia.Find(id);

            if (tcpa == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(tcpa);
            }

        }

        [HttpPost]
        public ActionResult UpdtParroquia(tbl_parroquia tcpa) //Método para editar una parroquia
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(tcpa).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Vw_tbl_parroquia");
            }
            catch (Exception)
            {

                return View();
            }
        }

        public ActionResult ViewRptParroquia(String tipo, String cadena)
        {
            LocalReport rpt = new LocalReport();
            String mt, enc, f;
            String[] s;
            Warning[] w;

            String ruta = Path.Combine(Server.MapPath("~/Reportes"), "RptParroquia.rdlc");
            rpt.ReportPath = ruta;
            ReportDataSource rd = null;

            if (String.IsNullOrEmpty(cadena))
            {
                var listafiltrada = db.tbl_parroquia.ToList();
                rd = new ReportDataSource("dsRptParroquia", listafiltrada);
            }
            else
            {
                var listafiltrada = db.tbl_parroquia.Where(x => x.nombre.Contains(cadena) || x.parroco.Contains(cadena) || x.telefono.Contains(cadena));
                rd = new ReportDataSource("dsRptParroquia", listafiltrada);

            } 

            rpt.DataSources.Add(rd);

            var b = rpt.Render(tipo, null, out mt, out enc, out f, out s, out w);

            return new FileContentResult(b, mt);
        }

        public ActionResult BuscarParroquia(String cadena)
        {
            if (String.IsNullOrEmpty(cadena))
            {
                var list = db.tbl_parroquia.ToList();
                return View("Vw_tbl_parroquia", list);
            }
            else
            {
                var listaFiltrada = db.tbl_parroquia.Where(x => x.nombre.Contains(cadena) || x.parroco.Contains(cadena) || x.telefono.Contains(cadena));
                return View("Vw_tbl_parroquia", listaFiltrada);
            }
        }
    }
}