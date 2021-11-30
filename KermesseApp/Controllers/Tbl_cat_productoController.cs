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
    public class Tbl_cat_productoController : Controller
    {
        // GET: Tbl_cat_producto
        KERMESSEEntities db = new KERMESSEEntities();
        public ActionResult ListCatProductos()
        {
            return View(db.tbl_cat_producto.Where(model => model.estado != 3));
        }

        public ActionResult InsertarCatProd()
        {
            return View();
        }

        [HttpPost]
        public ActionResult InsertarCatProd(tbl_cat_producto tcp)
        {
            if (ModelState.IsValid)
            {
                tbl_cat_producto tcProd = new tbl_cat_producto();
                tcProd.nombre = tcp.nombre;
                tcProd.descripcion = tcp.descripcion;
                tcProd.estado = 1;

                db.tbl_cat_producto.Add(tcProd);
                db.SaveChanges();

            }
            ModelState.Clear();
            var list = db.tbl_cat_producto.ToList();
            return View("ListCatProductos", list);
        }

        public ActionResult DeleteCatProd(int id)
        {
            tbl_cat_producto tcp = new tbl_cat_producto();
            tcp = db.tbl_cat_producto.Find(id);
            this.LogicalDeleteCatProd(tcp);

            return RedirectToAction("ListCatProductos");
        }

        public ActionResult ReadCatProd(int id)
        {
            var catProd = db.tbl_cat_producto.Where(x => x.id_cat_producto == id).First();
            return View(catProd);
        }

        public ActionResult EditCatProd(int id)
        {
            tbl_cat_producto tcp = db.tbl_cat_producto.Find(id);
            if(tcp == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(tcp);
            }
        }

        [HttpPost]
        public ActionResult LogicalDeleteCatProd(tbl_cat_producto tcp)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    tcp.estado = 3;//Actulizando el estado del datos
                    db.Entry(tcp).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("ListCatProductos");
            }
            catch (Exception)
            {
                return View();
                throw;
            }

        }


        [HttpPost]
        public ActionResult UpdateCatProd(tbl_cat_producto tcp)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    tcp.estado = 2;//Actulizando el estado del datos
                    db.Entry(tcp).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("ListCatProductos");
            }
            catch (Exception)
            {
                return View();
                throw;
            }
            
        }

        [HttpPost]
        public ActionResult FilterCatProd(String cadena)
        {
            if (String.IsNullOrEmpty(cadena))
            {
                var listfiltrada = db.tbl_cat_producto.Where(model => model.estado != 3);
                return View("ListCatProductos", listfiltrada);
            }
            else
            {
                var listfiltrada = db.tbl_cat_producto.Where(x => x.nombre.Contains(cadena) || x.descripcion.Contains(cadena) && x.estado !=3);
                return View("ListCatProductos", listfiltrada);
            }
            
        }

        public ActionResult VerRptCatProductos(String tipo, String cadena)
        {
            LocalReport lrpt = new LocalReport();
            string mt, enc, f;
            String[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "rptCatProducto.rdlc");
            lrpt.ReportPath = ruta;
            ReportDataSource rd = null;
            if (String.IsNullOrEmpty(cadena))
            {
                var listaFiltrada = db.tbl_cat_producto.Where(x => x.estado != 3).ToList(); ;
                rd = new ReportDataSource("dsRptCatProducto", listaFiltrada);
            }
            else
            {
                var listaFiltrada = db.tbl_cat_producto.Where(x => x.nombre.Contains(cadena) || x.descripcion.Contains(cadena) && x.estado != 3);
                rd = new ReportDataSource("dsRptCatProducto", listaFiltrada);
            }
            lrpt.DataSources.Add(rd);
            var b = lrpt.Render(tipo, null, out mt, out enc, out f, out s, out w);
            return new FileContentResult(b, mt);

        }

    }
}