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
    public class Tbl_productoController : Controller
    {
        // GET: Tbl_producto
        KERMESSEEntities db = new KERMESSEEntities();
        public ActionResult ListProductos()
        {
            return View(db.tbl_productos.Where(model => model.estado != 3));
        }

        public ActionResult ViewInsertProducto()
        {
            ViewBag.id_comunidad = new SelectList(db.tbl_comunidad.Where(model => model.estado != 3), "id_comunidad", "nombre");
            ViewBag.id_cat_producto = new SelectList(db.tbl_cat_producto.Where(model => model.estado != 3), "id_cat_producto", "nombre");
            return View();
        }

        [HttpPost]
        public ActionResult InsertProducto(tbl_productos tp)
        {
            if (ModelState.IsValid)
            {
                tbl_productos tProd = new tbl_productos();
                tProd.id_comunidad = tp.id_comunidad;
                tProd.id_cat_producto = tp.id_cat_producto;
                tProd.nombre = tp.nombre;
                tProd.desc_presentacion = tp.desc_presentacion;
                tProd.cantidad = tp.cantidad;
                tProd.precio_venta = tp.precio_venta;
                tProd.estado = 1;
               
                db.tbl_productos.Add(tProd);
                db.SaveChanges();
                ModelState.Clear();
                return RedirectToAction("ListProductos");

            }

            ViewBag.id_comunidad = new SelectList(db.tbl_comunidad.Where(model => model.estado != 3), "id_comunidad", "nombre");
            ViewBag.id_cat_producto = new SelectList(db.tbl_cat_producto.Where(model => model.estado != 3), "id_cat_producto", "nombre");
            return View("ViewInsertProducto");

        }

        public ActionResult DeleteProducto(int id)
        {
            tbl_productos tp = new tbl_productos();
            tp = db.tbl_productos.Find(id);
            this.LogicalDeleteProducto(tp);

            return RedirectToAction("ListProductos");
        }

        public ActionResult ReadProducto(int id)
        {
            var Producto = db.tbl_productos.Where(x => x.id_producto == id).First();
            return View(Producto);
        }

        public ActionResult EditProducto(int id)
        {
            tbl_productos tp = db.tbl_productos.Find(id);
            if (tp == null)
            {
                return HttpNotFound();
            }
            else
            {
                ViewBag.id_comunidad = new SelectList(db.tbl_comunidad, "id_comunidad", "nombre");
                ViewBag.id_cat_producto = new SelectList(db.tbl_cat_producto, "id_cat_producto", "nombre");
                return View(tp);
            }
        }


        [HttpPost]
        public ActionResult LogicalDeleteProducto(tbl_productos tp)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    tp.estado = 3;
                    db.Entry(tp).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("ListProductos");
            }
            catch (Exception)
            {
                return View();
                throw;
            }
        }

        [HttpPost]
        public ActionResult UpdateProducto(tbl_productos tp)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    tp.estado = 2;//Actualizando el estado de los datos
                    db.Entry(tp).State = EntityState.Modified;
                    db.SaveChanges();
                }
                ViewBag.id_comunidad = new SelectList(db.tbl_comunidad, "id_comunidad", "nombre");
                ViewBag.id_cat_producto = new SelectList(db.tbl_cat_producto, "id_cat_producto", "nombre");
                return RedirectToAction("ListProductos");
            }
            catch (Exception)
            {
                return View();
                throw;
            }

        }

        [HttpPost]
        public ActionResult FilterProducto(String cadena)
        {
            if (String.IsNullOrEmpty(cadena))
            {
                var listfiltrada = db.tbl_productos.Where(model => model.estado != 3);
                return View("ListProductos", listfiltrada);
            }
            else
            {
                var listfiltrada = db.tbl_productos.Where(x => x.nombre.Contains(cadena) || x.desc_presentacion.Contains(cadena) && x.estado != 3);
                return View("ListProductos", listfiltrada);
            }

        }


        public ActionResult VerRptProductos(String tipo, string cadena)
        {
            LocalReport lrpt = new LocalReport();
            string mt, enc, f;
            String[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "rptProducto.rdlc");
            lrpt.ReportPath = ruta;

            ReportDataSource rd = null;

            if (String.IsNullOrEmpty(cadena))
            {
                var listFiltrada = db.view_productos.Where(x => x.estado != 3);
                rd = new ReportDataSource("dsRptProducto", listFiltrada);
            }
            else
            {
                var listFiltrada = db.view_productos.Where(x => x.estado != 3 && (x.producto.Contains(cadena) || x.desc_presentacion.Contains(cadena))); ;
                rd = new ReportDataSource("dsRptProducto", listFiltrada);
            }
            lrpt.DataSources.Add(rd);
            var b = lrpt.Render(tipo, null, out mt, out enc, out f, out s, out w);
            return new FileContentResult(b, mt);

        }
    }
}