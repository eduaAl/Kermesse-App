using KermesseApp.Models;
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
    public class Tbl_listaprecioController : Controller
    {
        // GET: Tbl_listaprecio
        KERMESSEEntities db = new KERMESSEEntities();
        //Métodos de listamiento
        public ActionResult ListListaPrecio()
        {
            return View(db.tbl_listaprecio.Where(model => model.estado!=3));
        }

        public ActionResult LisListaPrecioDet(int id)
        {
            var detalle = db.tbl_listaprecio_det.Where(model => model.id_listaprecio == id);
            return View(detalle);
        }

        //Metodos de acceso a las paginas

        public ActionResult ViewInsertListaPrecio()
        {
            ViewBag.id_kermesse = new SelectList(db.tbl_kermesse.Where(model => model.estado != 3), "id_kermesse", "nombre");
            ViewBag.id_producto = new SelectList(db.tbl_productos.Where(model => model.estado != 3), "id_producto", "nombre");
            return View();
        }

        public ActionResult EditListaPrecio(int id)
        {
            tbl_listaprecio tlp = db.tbl_listaprecio.Find(id);
            if(tlp == null)
            {
                return HttpNotFound();
            }
            else
            {
                ViewBag.id_kermesse = new SelectList(db.tbl_kermesse.Where(model => model.estado != 3), "id_kermesse", "nombre");
                return View(tlp);
            }
        }

        public ActionResult EditListaPrecioDet(int id)
        {
            tbl_listaprecio_det tlpd = db.tbl_listaprecio_det.Find(id);
            if(tlpd == null)
            {
                return HttpNotFound();
            }
            else
            {
                ViewBag.id_producto = new SelectList(db.tbl_productos.Where(model => model.estado != 3), "id_producto", "nombre");
                return View(tlpd);
            }
        }


        //Metodos del CRUD
        //INSERTAR
        [HttpPost]
        public ActionResult InsertarListaPrecio(tbl_listaprecio listaprecio)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    tbl_listaprecio lp = new tbl_listaprecio();
                    lp.id_kermesse = listaprecio.id_kermesse;
                    lp.nombre = listaprecio.nombre;
                    lp.descripcion = listaprecio.descripcion;
                    lp.estado = 1;
                    db.tbl_listaprecio.Add(lp);
                    db.SaveChanges();
                    foreach (var dlp in listaprecio.listaprecio_Dets)
                    {
                        tbl_listaprecio_det lpdet = new tbl_listaprecio_det();
                        lpdet.id_producto = dlp.id_producto;
                        lpdet.precio_venta = dlp.precio_venta;
                        lpdet.id_listaprecio = lp.id_listaprecio;
                        db.tbl_listaprecio_det.Add(lpdet);
                    }
                    db.SaveChanges();
                }
                ModelState.Clear();
                return RedirectToAction("ListListaPrecio");
            }
            catch (Exception e)
            {
                return View("ViewInsertListaPrecio");
            }
        }

        [HttpPost]
        public ActionResult InpLP(tbl_listaprecio listaprecio)
        {
            int idlista = listaprecio.id_listaprecio;

            try
            {
                    tbl_listaprecio lp = new tbl_listaprecio();
                    lp.id_listaprecio = listaprecio.id_listaprecio;
                    foreach (var dlp in listaprecio.listaprecio_Dets)
                    {
                        tbl_listaprecio_det lpdet = new tbl_listaprecio_det();
                        lpdet.id_producto = dlp.id_producto;
                        lpdet.precio_venta = dlp.precio_venta;
                        lpdet.id_listaprecio = lp.id_listaprecio;
                        db.tbl_listaprecio_det.Add(lpdet);
                    }
                    db.SaveChanges();

                ModelState.Clear();
                return RedirectToAction("LisListaPrecioDet", "Tbl_listaprecio", new { id = idlista});
            }
            catch (Exception e)
            {
                return RedirectToAction("LisListaPrecioDet", "Tbl_listaprecio", new { id = idlista });
                throw;
            }
        }


        //EDITAR
        [HttpPost]
        public ActionResult UpdateListPrecio(tbl_listaprecio tlp)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    tlp.estado = 2;
                    db.Entry(tlp).State = EntityState.Modified;
                    db.SaveChanges();
                }
                ViewBag.id_kermesse = new SelectList(db.tbl_kermesse.Where(model => model.estado != 3), "id_kermesse", "nombre");
                return RedirectToAction("ListListaPrecio");
            }
            catch (Exception)
            {
                return View();
                throw;
            }
        }

        [HttpPost]
        public ActionResult UpdateListPrecioDet(tbl_listaprecio_det tlpd)
        {
            int idlista = tlpd.id_listaprecio;
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(tlpd).State = EntityState.Modified;
                    db.SaveChanges();
                }
                ViewBag.id_producto = new SelectList(db.tbl_productos.Where(model => model.estado != 3), "id_producto", "nombre");
                return RedirectToAction("LisListaPrecioDet", "Tbl_listaprecio", new { id = idlista });
            }
            catch (Exception)
            {
                return RedirectToAction("LisListaPrecioDet", "Tbl_listaprecio", new { id = idlista });
                throw;
            }
        }

        //ELIMINAR

        public ActionResult DeleteListaPrecio(int id)
        {
            tbl_listaprecio tlp = new tbl_listaprecio();
            tlp = db.tbl_listaprecio.Find(id);
            this.LogicalDeleteListaPrecio(tlp);
            return RedirectToAction("ListListaPrecio");
        }

        [HttpPost]
        public ActionResult LogicalDeleteListaPrecio(tbl_listaprecio tlp)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    tlp.estado = 3;
                    db.Entry(tlp).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("ListListaPrecio");
            }
            catch (Exception)
            {

                return View();
                throw;
            }
        }

        public ActionResult DeleteListaPrecioDet (int id)
        {
            tbl_listaprecio_det tlpd = new tbl_listaprecio_det();
            tlpd = db.tbl_listaprecio_det.Find(id);
            int idlistaprecio = tlpd.id_listaprecio;
            db.tbl_listaprecio_det.Remove(tlpd);
            db.SaveChanges();
            var list = db.tbl_listaprecio_det.Where(model => model.id_listaprecio == idlistaprecio);
            return View("LisListaPrecioDet", list);

        }


        public ActionResult Inp (int id)
        {
            tbl_listaprecio lp = db.tbl_listaprecio.Find(id);
            if (lp == null)
            {
                return HttpNotFound();
            }
            else
            {
                ViewBag.id_producto = new SelectList(db.tbl_productos.Where(model => model.estado != 3), "id_producto", "nombre");
                ViewBag.id_kermesse = new SelectList(db.tbl_kermesse.Where(model => model.estado != 3), "id_kermesse", "nombre");
                return View(lp);
            }
        }

        [HttpPost]
        public ActionResult FilterListaPrecios(String cadena)
        {
            if (String.IsNullOrEmpty(cadena))
            {
                var listfiltrada = db.tbl_listaprecio.Where(model => model.estado != 3);
                return View("ListListaPrecio", listfiltrada);
            }
            else
            {
                var listfiltrada = db.tbl_listaprecio.Where(x => x.nombre.Contains(cadena) || x.descripcion.Contains(cadena) && x.estado != 3);
                return View("ListListaPrecio", listfiltrada);
            }

        }

        public ActionResult FilterListaPreciosDet(String cadena, int id)
        {
            if (String.IsNullOrEmpty(cadena))
            {
                var listafiltrada = db.tbl_listaprecio_det.Where(model => model.id_listaprecio == id);
                return View("LisListaPrecioDet", listafiltrada);
            }
            else
            {
                var listfiltrada = db.tbl_listaprecio_det.Where(model => model.tbl_productos.nombre.Contains(cadena) && model.id_listaprecio == id);
                return View("LisListaPrecioDet", listfiltrada);
            }

        }

        public ActionResult VerRptListaPrecio(String tipo, String cadena)
        {
            LocalReport lrpt = new LocalReport();
            string mt, enc, f;
            String[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "rptListaPrecio.rdlc");
            lrpt.ReportPath = ruta;
            ReportDataSource rd = null;

            if (String.IsNullOrEmpty(cadena))
            {
                var listFiltrada = db.vw_listaprecio.Where(x => x.estado != 3);
                rd = new ReportDataSource("dsListaPrecio", listFiltrada);
            }
            else
            {
                var listFiltrada = db.vw_listaprecio.Where(x => x.estado != 3 && (x.lista_precio.Contains(cadena) || x.descripcion.Contains(cadena))); ;
                rd = new ReportDataSource("dsListaPrecio", listFiltrada);
            }
            lrpt.DataSources.Add(rd);
            var b = lrpt.Render(tipo, null, out mt, out enc, out f, out s, out w);
            return new FileContentResult(b, mt);
        }

        public ActionResult VerRptListaPrecioDet(String tipo, String cadena, String idlistaprecio)
        {
            LocalReport lrpt = new LocalReport();
            string mt, enc, f;
            String[] s;
            Warning[] w;
            int id = Int32.Parse(idlistaprecio);
            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "rptListaPrecioDet.rdlc");
            lrpt.ReportPath = ruta;

            ReportDataSource rd = null;
            if (String.IsNullOrEmpty(cadena))
            {
                var listaFiltrada = db.vw_listaprecio_det.Where(x => x.id_listaprecio == id).ToList(); ;
                rd = new ReportDataSource("dsRptListaPrecioDet", listaFiltrada);
            }
            else
            {
                var listaFiltrada = db.vw_listaprecio_det.Where(x => x.listaprecio.Contains(cadena) || x.producto.Contains(cadena) && x.id_listaprecio == id);
                rd = new ReportDataSource("dsRptListaPrecioDet", listaFiltrada);
            }

            lrpt.DataSources.Add(rd);
            var b = lrpt.Render(tipo, null, out mt, out enc, out f, out s, out w);
            return new FileContentResult(b, mt);

        }

        public ActionResult VistaRptListaPrecio()
        {
            ViewBag.id_listaprecio = new SelectList(db.tbl_listaprecio.Where(x => x.estado !=3), "id_listaprecio", "nombre");
            ViewBag.id_producto = new SelectList(db.tbl_productos.Where(x => x.estado !=3), "id_producto", "nombre");
            return View();
        }

        [HttpPost]
        public ActionResult VerRptListaPrecioFiltrada(vw_listaprecio_det vld)
        {
            LocalReport lrpt = new LocalReport();
            string mt, enc, f, tipo;
            String[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "rptListaPrecioFiltrada.rdlc");
            lrpt.ReportPath = ruta;

            //List<tbl_cat_producto> lista = new List<tbl_cat_producto>();
            var listaFiltrada = db.vw_listaprecio_det.Where(x => x.id_listaprecio == vld.id_listaprecio & x.id_producto == vld.id_producto);

            ReportDataSource rds = new ReportDataSource("dsRptListaPrecioFiltrada", listaFiltrada);
            lrpt.DataSources.Add(rds);
            tipo = "PDF";
            var b = lrpt.Render(tipo, null, out mt, out enc, out f, out s, out w);
            return new FileContentResult(b, mt);
        }
    }

    

}