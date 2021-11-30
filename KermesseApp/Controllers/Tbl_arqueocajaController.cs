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
    public class Tbl_arqueocajaController : Controller
    {
        private KERMESSEEntities db = new KERMESSEEntities();

        // GET: VISTAS
        public ActionResult ListArqueos()
        {
            return View(db.tbl_arqueocaja.Where(model => model.estado != 3));
        }
        public ActionResult ListArqueoCajaDet(int id)
        {
            var detalle = db.tbl_arqueocaja_det.Where(model => model.id_arqueocaja == id);
            return View(detalle);
        }

        public ActionResult VistaGuardarArqueo()
        {
            ViewBag.id_kermesse = new SelectList(db.tbl_kermesse.Where(model => model.estado != 3), "id_kermesse", "nombre");
            ViewBag.id_moneda = new SelectList(db.tbl_moneda.Where(model => model.estado != 3), "id_moneda", "nombre");
            ViewBag.id_denominacion = new SelectList(db.tbl_denominacion.Where(model => model.estado != 3), "id_denominacion", "valor");
            return View();
        }

        public ActionResult VistaGuardarArqueoCajaDet(int id)
        {
            tbl_arqueocaja ta = db.tbl_arqueocaja.Find(id);
            if (ta == null)
            {
                return HttpNotFound();
            }
            else
            {
                ViewBag.id_moneda = new SelectList(db.tbl_moneda.Where(model => model.estado != 3), "id_moneda", "nombre");
                ViewBag.id_denominacion = new SelectList(db.tbl_denominacion.Where(model => model.estado != 3), "id_denominacion", "valor");
                return View(ta);
            }
        }

        public ActionResult EditarArqueo(int id)
        {
            tbl_arqueocaja ta = db.tbl_arqueocaja.Find(id);
            if (ta == null)
            {
                return HttpNotFound();
            }
            else
            {
                ViewBag.id_kermesse = new SelectList(db.tbl_kermesse.Where(model => model.estado != 3), "id_kermesse", "nombre");
                return View(ta);
            }
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
                ViewBag.id_moneda = new SelectList(db.tbl_moneda.Where(model => model.estado != 3), "id_moneda", "nombre");
                ViewBag.id_denominacion = new SelectList(db.tbl_denominacion.Where(model => model.estado != 3), "id_denominacion", "valor");
                return View(tacdet);
            }
        }

        // Guardar

        [HttpPost]
        public ActionResult GuardarArqueo(tbl_arqueocaja ta)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    tbl_arqueocaja tar = new tbl_arqueocaja();

                    tar.idkermesse = ta.idkermesse;
                    tar.fecha_arqueo = ta.fecha_arqueo;
                    tar.gran_total = ta.gran_total;
                    tar.usuario_creacion = 1;
                    tar.fecha_creacion = DateTime.Now;
                    tar.estado = 1;

                    db.tbl_arqueocaja.Add(tar);
                    db.SaveChanges();

                    foreach (var dar in ta.tbl_arqueocaja_dets)
                    {
                        tbl_arqueocaja_det tacdet = new tbl_arqueocaja_det();

                        tacdet.id_arqueocaja = tar.id_arqueocaja;
                        tacdet.id_moneda = dar.id_moneda;
                        tacdet.id_denominacion = dar.id_denominacion;
                        tacdet.cantidad = dar.cantidad;
                        tacdet.subtotal = dar.subtotal;
                        db.tbl_arqueocaja_det.Add(tacdet);
                    }
                    db.SaveChanges();
                }
                ModelState.Clear();
                return RedirectToAction("ListArqueos");
            }
            catch (Exception e)
            {
                return View("VistaGuardarArqueo");
            }
        }

        [HttpPost]
        public ActionResult GuardarArqueoCajaDet(tbl_arqueocaja ta)
        {
            int idArqueo = ta.id_arqueocaja;

            try
            {
                tbl_arqueocaja tac = new tbl_arqueocaja();

                tac.id_arqueocaja = ta.id_arqueocaja;
                foreach (var tar in ta.tbl_arqueocaja_dets)
                {
                    tbl_arqueocaja_det tacd = new tbl_arqueocaja_det();
                    tacd.id_arqueocaja = tac.id_arqueocaja;
                    tacd.id_moneda = tar.id_moneda;
                    tacd.id_denominacion = tar.id_denominacion;
                    tacd.cantidad = tar.cantidad;
                    tacd.subtotal = tar.subtotal;

                    db.tbl_arqueocaja_det.Add(tacd);
                }
                db.SaveChanges();
                ModelState.Clear();
                return RedirectToAction("ListArqueoCajaDet", "Tbl_arqueocaja", new { id = idArqueo });
            }
            catch (Exception e)
            {
                return RedirectToAction("ListArqueoCajaDet", "Tbl_arqueocaja", new { id = idArqueo });
                throw;
            }
        }


        // Eliminar
        public ActionResult EliminarArqueo(int id)
        {
            tbl_arqueocaja tar = new tbl_arqueocaja();

            tar = db.tbl_arqueocaja.Find(id);
            this.LogicalDeleteArqueo(tar);
            return RedirectToAction("ListArqueos");
        }

        public ActionResult EliminarArqueoCajaDet(int id) 
        {
            tbl_arqueocaja_det tacdet = new tbl_arqueocaja_det();

            tacdet = db.tbl_arqueocaja_det.Find(id);
            int idarqueo = tacdet.id_arqueocaja;
            db.tbl_arqueocaja_det.Remove(tacdet);
            db.SaveChanges();

            var list = db.tbl_arqueocaja_det.Where(model => model.id_arqueocaja == idarqueo);
            return View("ListArqueoCajaDet", list);
        }

        [HttpPost]
        public ActionResult LogicalDeleteArqueo(tbl_arqueocaja tar)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    tar.estado = 3;
                    db.Entry(tar).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("ListArqueos");
            }
            catch (Exception)
            {
                return View();
                throw;
            }
        }


        // Editar
        [HttpPost]
        public ActionResult ActualizarArqueo(tbl_arqueocaja ta)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ta.estado = 2;
                    ta.usuario_eliminacion = null;
                    ta.fecha_eliminacion = null;
                    ta.usuario_modificacion = 1;
                    ta.fecha_modificacion = DateTime.Now;
                    db.Entry(ta).State = EntityState.Modified;
                    db.SaveChanges();
                }
                ViewBag.id_kermesse = new SelectList(db.tbl_kermesse.Where(model => model.estado != 3), "id_kermesse", "nombre");
                return RedirectToAction("ListArqueos");
            }
            catch (Exception)
            {
                return View();
                throw;
            }
        }

        [HttpPost]
        public ActionResult ActualizarArqueoCajaDet(tbl_arqueocaja_det tacdet)
        {
            int idArqueo = tacdet.id_arqueocaja;
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(tacdet).State = EntityState.Modified;
                    db.SaveChanges();
                }
                ViewBag.id_moneda = new SelectList(db.tbl_moneda.Where(model => model.estado != 3), "id_moneda", "nombre");
                ViewBag.id_denominacion = new SelectList(db.tbl_denominacion.Where(model => model.estado != 3), "id_denominacion", "valor");
                return RedirectToAction("ListArqueoCajaDet", "Tbl_arqueocaja", new { id = idArqueo });
            }
            catch (Exception)
            {
                return RedirectToAction("ListArqueoCajaDet", "Tbl_arqueocaja", new { id = idArqueo });
                throw;
            }
        }

        // Filtros

        [HttpPost]
        public ActionResult FiltrarArqueo(String cadena)
        {
            if (String.IsNullOrEmpty(cadena))
            {
                var list = db.tbl_arqueocaja.Where(x => x.estado != 3);
                return View("ListArqueos", list);
            }
            else
            {
                var listFiltrada = db.tbl_arqueocaja.Where(x => x.fecha_arqueo.ToString().Contains(cadena) 
                || x.gran_total.ToString().Contains(cadena) || x.tbl_kermesse.nombre.Contains(cadena));
                var newList = listFiltrada.Where(x => x.estado != 3);
                return View("ListArqueos", newList);
            }
        }

        [HttpPost]
        public ActionResult FiltrarArqueoCajaDet(String cadena, int id)
        {
            if (String.IsNullOrEmpty(cadena))
            {
                var list = db.tbl_arqueocaja_det.Where(model => model.id_arqueocaja == id);
                return View("ListArqueoCajaDet", list);
            }
            else
            {
                var listFiltrada = db.tbl_arqueocaja_det.Where(x => x.id_arqueocaja == id &&
                (x.subtotal.ToString().Contains(cadena) || x.tbl_denominacion.valor.ToString().Contains(cadena) || 
                 x.tbl_moneda.nombre.Contains(cadena) || x.cantidad.ToString().Contains(cadena)));
                return View("ListArqueoCajaDet", listFiltrada);
            }
        }

        public ActionResult ViewRptDetalle(String tipo, String cadena, String id)
        {
            LocalReport rpt = new LocalReport();
            String mt, enc, f;
            String[] s;
            Warning[] w;
            ReportDataSource rd = null;
            int ID = 0;
            ID = Int32.Parse(id);

            String ruta = Path.Combine(Server.MapPath("~/Reportes"), "rptArqueoCajaDet.rdlc");
            rpt.ReportPath = ruta;

            if (String.IsNullOrEmpty(cadena))
            {
                var listafiltrada = db.Vw_arqueoCajaDet.Where(x => x.id_arqueocaja == ID);
                rd = new ReportDataSource("dsRptArqueoDetalle", listafiltrada);
            }
            else
            {
                var listafiltrada = db.Vw_arqueoCajaDet.Where(x => (x.nombre.Contains(cadena) || x.valor.ToString().Contains(cadena) || x.subtotal.ToString().Contains(cadena) || x.cantidad.ToString().Contains(cadena)) && x.id_arqueocaja == ID);
                rd = new ReportDataSource("dsRptArqueoDetalle", listafiltrada);
            }

            rpt.DataSources.Add(rd);
            var b = rpt.Render(tipo, null, out mt, out enc, out f, out s, out w);

            return new FileContentResult(b, mt);
        }
    }
}