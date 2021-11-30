using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KermesseApp.Models;
using System.Data.Entity;

namespace KermesseApp.Controllers
{
    public class tbl_tasacambioController : Controller
    {
        // GET: tbl_tasacambio
        KERMESSEEntities db = new KERMESSEEntities();
        public ActionResult LisTasaCambio()
        {
            return View(db.tbl_tasacambio.Where(model => model.estado != 3));
        }
        public ActionResult LisTasaCambioDet(int id)
        {
            var cambio = db.tbl_tasacambio_det.Where(model => model.id_tasacambio == id);
            return View(cambio);
        }
        public ActionResult ViewInsertTasaCambio()
        {
            ViewBag.id_monedaC = new SelectList(db.tbl_moneda.Where(model => model.estado != 3), "id_moneda", "nombre");
            ViewBag.id_monedaO = new SelectList(db.tbl_moneda.Where(model => model.estado != 3), "id_moneda", "nombre");
            return View();
        }
        public ActionResult EditTasaCambio(int id)
        {
            tbl_tasacambio tsc = db.tbl_tasacambio.Find(id);
            if (tsc == null)
            {
                return HttpNotFound();
            }
            else
            {
                ViewBag.id_monedaC = new SelectList(db.tbl_moneda.Where(model => model.estado != 3), "id_moneda", "nombre");
                ViewBag.id_monedaO = new SelectList(db.tbl_moneda.Where(model => model.estado != 3), "id_moneda", "nombre");
                return View(tsc);
            }
        }
        public ActionResult EditTasaCambioDet(int id)
        {
            tbl_tasacambio_det tlpd = db.tbl_tasacambio_det.Find(id);
            if (tlpd == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(tlpd);
            }
        }

        [HttpPost]
        public ActionResult UpdateTasaCambio(tbl_tasacambio tsc)
        {
            try
            {
                    tsc.estado = 2;
                    db.Entry(tsc).State = EntityState.Modified;
                    db.SaveChanges();
                
                ViewBag.id_monedaC = new SelectList(db.tbl_moneda.Where(model => model.estado != 3), "id_moneda", "nombre");
                ViewBag.id_monedaO = new SelectList(db.tbl_moneda.Where(model => model.estado != 3), "id_moneda", "nombre");
                return RedirectToAction("LisTasaCambio");
            }
            catch (Exception)
            {
                return View();
                throw;
            }
        }
        [HttpPost]
        public ActionResult UpdateTasaCambioDet(tbl_tasacambio_det tlpd)
        {
            int idlista = tlpd.id_tasacambio;
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(tlpd).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("LisTasaCambioDet", "tbl_tasacambio", new { id = idlista });
            }
            catch (Exception)
            {
                return RedirectToAction("LisTasaCambioDet", "tbl_tasacambio", new { id = idlista });
                throw;
            }
        }
        public ActionResult DeleteTasaCambio(int id)
        {
            tbl_tasacambio tsc = new tbl_tasacambio();
            tsc = db.tbl_tasacambio.Find(id);
            this.LogicalDeleteTasaCambio(tsc);
            return RedirectToAction("LisTasaCambio");
        }

        [HttpPost]
        public ActionResult InsertarTasaCambio(tbl_tasacambio tasacambio)
        {
            try
            {
                    Console.WriteLine("This is C#");
                    tbl_tasacambio lp = new tbl_tasacambio();
                    lp.id_monedaO = tasacambio.id_monedaO;
                    lp.id_monedaC = tasacambio.id_monedaC;
                    lp.mes = tasacambio.mes;
                    lp.anio = tasacambio.anio;
                    lp.estado = 1;
                    db.tbl_tasacambio.Add(lp);
                    db.SaveChanges();
                    foreach (var dlp in tasacambio.tbl_tasacambio_det)
                    {
                        tbl_tasacambio_det lpdet = new tbl_tasacambio_det();
                        lpdet.id_tasacambio = lp.id_tasacambio;
                        lpdet.fecha = dlp.fecha;
                        lpdet.tipo_cambio = dlp.tipo_cambio;
                        lpdet.estado = 1;
                        db.tbl_tasacambio_det.Add(lpdet);
                    }
                    db.SaveChanges();
                
                ModelState.Clear();
                return RedirectToAction("LisTasaCambio");
            }
            catch (Exception e)
            {
                return View("ViewInsertTasaCambio");
            }
        }
        [HttpPost]
        public ActionResult ImpLP(tbl_tasacambio tasacambio)
        {
            int idlista = tasacambio.id_tasacambio;
            try
            {
                tbl_tasacambio lp = new tbl_tasacambio();
                lp.id_tasacambio = tasacambio.id_tasacambio;
                foreach (var dlp in tasacambio.tbl_tasacambio_det)
                {
                    tbl_tasacambio_det lpdet = new tbl_tasacambio_det();
                    lpdet.id_tasacambio = lp.id_tasacambio;
                    lpdet.fecha = dlp.fecha;
                    lpdet.tipo_cambio = dlp.tipo_cambio;
                    lpdet.estado = 1;
                    db.tbl_tasacambio_det.Add(lpdet);
                }
                db.SaveChanges();

                ModelState.Clear();
                return RedirectToAction("LisTasaCambioDet", "tbl_tasacambio", new { id = idlista });
            }
            catch (Exception e)
            {
                return RedirectToAction("LisTasaCambioDet", "tbl_tasacambio", new { id = idlista });
                throw;
            }
        }
        [HttpPost]
        public ActionResult LogicalDeleteTasaCambio(tbl_tasacambio tsc)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    tsc.estado = 3;
                    db.Entry(tsc).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("LisTasaCambio");
            }
            catch (Exception)
            {

                return View();
                throw;
            }
        }
        [HttpPost]
        public ActionResult FilterTasaCambio(String Cadena)
        {
            if (String.IsNullOrEmpty(Cadena))
            {
                var list = db.tbl_tasacambio.Where(x => x.estado != 3);
                return View("LisTasaCambio", list);
            }
            else
            {
                var listFiltrada = db.tbl_tasacambio.Where(x => x.tbl_moneda.nombre.Contains(Cadena)
                || x.mes.ToString().Contains(Cadena) || x.anio.ToString().Contains(Cadena));
                var newList = listFiltrada.Where(x => x.estado != 3);
                return View("LisTasaCambio", newList);
            }

        }
        //public ActionResult FilterTasaCambioDet(String cadena)
        //{
        //    if (String.IsNullOrEmpty(cadena))
        //    {
        //        var list = db.tbl_tasacambio_det.ToList();
        //        var newList = list.Where(x => x.estado != 3);
        //        return View("LisTasaCambioDet", newList);
        //    }
        //    else
        //    {
        //        var listFiltrada = db.tbl_tasacambio_det.Where(x => x.fecha.ToString().Contains(cadena)
        //        || x.tipo_cambio.ToString().Contains(cadena));
        //        var newList = listFiltrada.Where(x => x.estado != 3);
        //        return View("LisTasaCambioDet", newList);
        //    }

        //}
        public ActionResult FilterTasaCambioDet(String cadena, int id)
        {
            if (String.IsNullOrEmpty(cadena))
            {
                var listafiltrada = db.tbl_tasacambio_det.Where(model => model.id_tasacambio == id);
                var newList = listafiltrada.Where(x => x.estado != 3);
                return View("LisTasaCambioDet", newList);
            }
            else
            {
                var listfiltrada = db.tbl_tasacambio_det.Where(model => (model.fecha.ToString().Contains(cadena) || model.tipo_cambio.ToString().Contains(cadena))
                && model.id_tasacambio == id);
                var newList = listfiltrada.Where(x => x.estado != 3);
                return View("LisTasaCambioDet", newList);
            }

        }
        public ActionResult Imp(int id)
        {
            tbl_tasacambio lp = db.tbl_tasacambio.Find(id);
            if (lp == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(lp);
            }
        }
        public ActionResult DeleteTasaCambioDet(int id)
        {
            tbl_tasacambio_det tlpd = new tbl_tasacambio_det();
            tlpd = db.tbl_tasacambio_det.Find(id);
            int idtasacambio = tlpd.id_tasacambio;
            db.tbl_tasacambio_det.Remove(tlpd);
            db.SaveChanges();
            var list = db.tbl_tasacambio_det.Where(model => model.id_tasacambio == idtasacambio);
            return View("LisTasaCambioDet", list);

        }
    }
}