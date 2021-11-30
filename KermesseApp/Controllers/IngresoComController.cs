using KermesseApp.Models;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KermesseApp.Controllers
{
    public class IngresoComController : Controller
    {

        private KERMESSEEntities db = new KERMESSEEntities();
        // GET: IngresoComDet

        //Vista para la lista de Ingreso Comunidad
        public ActionResult Vw_tbl_IngresoCom()
        {
            return View(db.tbl_ingreso_com.Where(model => model.estado != 3));
        }


        //Vista para la creación de nuevo registro de Ingreso Comunidad
        public ActionResult Vw_Create_IngresoCom()
        {
            ViewBag.id_kermesse = new SelectList(db.tbl_kermesse.Where(x => x.estado != 3), "id_kermesse", "nombre");
            ViewBag.fecha_inicio = new SelectList(db.tbl_kermesse.Where(x => x.estado != 3), "id_kermesse", "fecha_inicio");
            ViewBag.fecha_fin = new SelectList(db.tbl_kermesse.Where(x => x.estado != 3), "id_kermesse", "fecha_fin");
            ViewBag.id_comunidad = new SelectList(db.tbl_comunidad.Where(x => x.estado != 3), "id_comunidad", "nombre");
            ViewBag.id_producto = new SelectList(db.tbl_productos.Where(x => x.estado != 3), "id_producto", "nombre");
            ViewBag.id_bono = new SelectList(db.tbl_bonos.Where(x => x.estado != 3), "id_bono", "nombre");
            ViewBag.valor = new SelectList(db.tbl_bonos.Where(x => x.estado != 3), "id_bono", "valor");
            return View();
        }

        [HttpPost]
        public ActionResult Create_IngresoCom(tbl_ingreso_com model)
        {
            try
            {

                using (db)
                {
                    tbl_ingreso_com ic = new tbl_ingreso_com();
                    ic.id_kermesse = model.id_kermesse;
                    ic.id_comunidad = model.id_comunidad;
                    ic.id_producto = model.id_producto;
                    ic.cant_producto = model.cant_producto;
                    ic.precio_producto = model.precio_producto; ;
                    ic.total_bonos = model.total_bonos;
                    ic.usuario_creacion = 1;
                    ic.usuario_modificacion = null;
                    ic.usuario_eliminacion = null;
                    ic.fecha_creacion = DateTime.Now;
                    ic.fecha_modificacion = null;
                    ic.fecha_eliminacion = null;
                    ic.estado = 1;

                    db.tbl_ingreso_com.Add(ic);
                    db.SaveChanges();



                    ViewBag.id_kermesse = new SelectList(db.tbl_kermesse.Where(x => x.estado != 3), "id_kermesse", "nombre");
                    ViewBag.id_comunidad = new SelectList(db.tbl_comunidad.Where(x => x.estado != 3), "id_comunidad", "nombre");
                    ViewBag.id_producto = new SelectList(db.tbl_productos.Where(x => x.estado != 3), "id_producto", "nombre");


                    foreach (var m in model.ingresocom_Dets)
                    {
                        tbl_ingresocom_det icd = new tbl_ingresocom_det();
                        icd.id_ingresocom = ic.id_ingresocom;
                        icd.id_bono = m.id_bono;
                        icd.denominacion = m.denominacion;
                        icd.cantidad = m.cantidad;
                        foreach (var b in db.tbl_bonos)
                        {
                            if (b.id_bono == m.id_bono)
                            {
                                icd.subtotal = (decimal)(b.valor * m.cantidad);
                            }
                        }
                        db.tbl_ingresocom_det.Add(icd);

                    }
                    db.SaveChanges();
                    ViewBag.id_bono = new SelectList(db.tbl_bonos.Where(x => x.estado != 3), "id_bono", "nombre");

                }

                var list = db.tbl_ingreso_com.Where(x => x.estado != 3);
                return RedirectToAction("Vw_tbl_IngresoCom",list);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                var list = db.tbl_ingreso_com.Where(x => x.estado != 3);
                return RedirectToAction("Vw_tbl_IngresoCom", list);
                
            }

        }

        public ActionResult Delete_ingresocom(int id)
        {
            tbl_ingreso_com tbl = new tbl_ingreso_com();
            tbl = db.tbl_ingreso_com.Find(id);
            this.LogicalDelete(tbl);
            var list = db.tbl_ingreso_com.Where(x => x.estado != 3);
            return View("Vw_tbl_IngresoCom", list);
        }

        [HttpPost]
        public ActionResult LogicalDelete(tbl_ingreso_com model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.estado = 3;
                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();
                }
                var list = db.tbl_ingreso_com.Where(x => x.estado != 3);
                return View("Vw_tbl_IngresoCom", list);
            }
            catch (Exception)
            {

                return View("Vw_tbl_IngresoCom");
                throw;

            }
        }

        public ActionResult Vw_Details_ingresocom(int id)
        {
            var detalle = db.tbl_ingresocom_det.Where(model => model.id_ingresocom == id);
            return View(detalle);
        }

        public ActionResult Vw_Edit_ingresocom(int id)
        {
            tbl_ingreso_com tbl = db.tbl_ingreso_com.Find(id);
            if (tbl == null)
            {
                return HttpNotFound();
            }
            else
            {
                ViewBag.id_kermesse = new SelectList(db.tbl_kermesse.Where(x => x.estado != 3), "id_kermesse", "nombre");
                ViewBag.id_comunidad = new SelectList(db.tbl_comunidad.Where(x => x.estado != 3), "id_comunidad", "nombre");
                ViewBag.id_producto = new SelectList(db.tbl_productos.Where(x => x.estado != 3), "id_producto", "nombre");

                return View(tbl);
            }
        }

        [HttpPost]
        public ActionResult Edit_ingresocom(tbl_ingreso_com tbl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    tbl.estado = 2;
                    tbl.usuario_eliminacion = null;
                    tbl.usuario_modificacion = 1;
                    tbl.fecha_modificacion = DateTime.Now;
                    tbl.fecha_eliminacion = null;

                    db.Entry(tbl).State = EntityState.Modified;
                    db.SaveChanges();
                }
                ViewBag.id_kermesse = new SelectList(db.tbl_kermesse.Where(x => x.estado != 3), "id_kermesse", "nombre");
                ViewBag.id_comunidad = new SelectList(db.tbl_comunidad.Where(x => x.estado != 3), "id_comunidad", "nombre");
                ViewBag.id_producto = new SelectList(db.tbl_productos.Where(x => x.estado != 3), "id_producto", "nombre");

                return RedirectToAction("Vw_tbl_IngresoCom");
            }
            catch (Exception)
            {
                return View();
                throw;
            }
        }

        [HttpPost]
        public ActionResult Create_IngresoComDet(tbl_ingreso_com model)
        {

            int lista = 0;
            try
            {
                
                    foreach (var m in model.ingresocom_Dets)
                    {
                        tbl_ingresocom_det icd = new tbl_ingresocom_det();
                        icd.id_ingresocom = m.id_ingresocom;
                        lista = m.id_ingresocom;
                        icd.id_bono = m.id_bono;
                        icd.denominacion = m.denominacion;
                        icd.cantidad = m.cantidad;
                        foreach (var b in db.tbl_bonos)
                        {
                            if (b.id_bono == m.id_bono)
                            {
                                icd.subtotal = (decimal)(b.valor * m.cantidad);
                            }
                        }
                        db.tbl_ingresocom_det.Add(icd);

                    }
                    db.SaveChanges();
                    ViewBag.id_bono = new SelectList(db.tbl_bonos.Where(x => x.estado != 3), "id_bono", "nombre");

                return RedirectToAction("Vw_Details_ingresocom", new { id = lista});

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return RedirectToAction("Vw_Details_ingresocom", new { id = lista });

            }

        }

        public ActionResult Vw_Edit_ingresocomdet(int id)
        {
            tbl_ingresocom_det tbl = db.tbl_ingresocom_det.Find(id);
            if (tbl == null)
            {
                return HttpNotFound();
            }
            else
            {
                ViewBag.id_bono = new SelectList(db.tbl_bonos.Where(x => x.estado != 3), "id_bono", "nombre");

                return View(tbl);
            }
        }

        [HttpPost]
        public ActionResult Edit_ingresocomdet(tbl_ingresocom_det tbl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    foreach (var b in db.tbl_bonos)
                    {
                        if (b.id_bono == tbl.id_bono)
                        {
                            tbl.subtotal = (decimal)(b.valor * tbl.cantidad);
                        }
                    }

                    db.Entry(tbl).State = EntityState.Modified;
                    db.SaveChanges();
                }
                ViewBag.id_bono = new SelectList(db.tbl_bonos.Where(x => x.estado != 3), "id_bono", "nombre");

                return RedirectToAction("Vw_Details_ingresocom", new { id = tbl.id_ingresocom});
            }
            catch (Exception)
            {
                return View();
                throw;
            }
        }

        public ActionResult EliminarIngresoComDet(int id, int id2) //Método para eliminar
        {
            tbl_ingresocom_det tcpa = new tbl_ingresocom_det();
            tcpa = db.tbl_ingresocom_det.Find(id);
            db.tbl_ingresocom_det.Remove(tcpa);
            db.SaveChanges();

            return RedirectToAction("Vw_Details_ingresocom", new { id = id2 });
        }

        public ActionResult BuscarIngresoCom(String cadena)
        {
            if (String.IsNullOrEmpty(cadena))
            {
                var list = db.tbl_ingreso_com.Where(x => x.estado != 3);
                return View("Vw_tbl_IngresoCom", list);
            }
            else
            {
                var listaFiltrada = db.tbl_ingreso_com.Where(x => (x.tbl_kermesse.nombre.Contains(cadena) || x.tbl_comunidad.nombre.Contains(cadena) || x.tbl_productos.nombre.Contains(cadena)) && x.estado != 3);
                return View("Vw_tbl_IngresoCom", listaFiltrada);
            }
        }

        public ActionResult Vw_Create_IngresoComDet()
        {
            ViewBag.id_bono = new SelectList(db.tbl_bonos.Where(x => x.estado != 3), "id_bono", "nombre");
            ViewBag.valor = new SelectList(db.tbl_bonos.Where(x => x.estado != 3), "id_bono", "valor");
            return View();
        }

        public ActionResult BuscarIngresoComDet(String cadena, int id)
        {
            if (String.IsNullOrEmpty(cadena))
            {
                var listafiltrada = db.tbl_ingresocom_det.Where(model => model.id_ingresocom == id);
                return View("Vw_Details_ingresocom", listafiltrada);
            }
            else
            {
                var listfiltrada = db.tbl_ingresocom_det.Where(model => model.tbl_bonos.nombre.Contains(cadena) && model.id_ingresocom == id);
                return View("Vw_Details_ingresocom", listfiltrada);
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

            String ruta = Path.Combine(Server.MapPath("~/Reportes"), "RptIngresoDet.rdlc");
            rpt.ReportPath = ruta;

            if (String.IsNullOrEmpty(cadena))
            {
                var listafiltrada = db.Vw_IngresoDet.Where(x => x.id_ingresocom == ID );
                rd = new ReportDataSource("dsRptIngresoDet", listafiltrada);
            }
            else
            {
                var listafiltrada = db.Vw_IngresoDet.Where(x => x.bono.Contains(cadena) && x.denominacion.Contains(cadena) && x.id_ingresocom == ID);
                rd = new ReportDataSource("dsRptIngresoDet", listafiltrada);
            }

            rpt.DataSources.Add(rd);

            var b = rpt.Render(tipo, null, out mt, out enc, out f, out s, out w);

            return new FileContentResult(b, mt);
        }
    }
}