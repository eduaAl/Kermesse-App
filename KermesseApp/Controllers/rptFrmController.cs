using KermesseApp.Models;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KermesseApp.Controllers
{
    public class rptFrmController : Controller
    {
        private KERMESSEEntities db = new KERMESSEEntities();
        // GET: rptFrm
        public ActionResult Vw_RptFrm()
        {
            ViewBag.id_kermesse = new SelectList(db.tbl_kermesse.Where(x => x.estado != 3), "id_kermesse", "nombre");
            ViewBag.id_comunidad = new SelectList(db.tbl_comunidad.Where(x => x.estado != 3), "id_comunidad", "nombre");
            ViewBag.id_listaprecio = new SelectList(db.tbl_listaprecio.Where(x => x.estado != 3), "id_listaprecio", "nombre");
            ViewBag.id_producto = new SelectList(db.tbl_productos.Where(x => x.estado != 3), "id_producto", "nombre");
            ViewBag.reportes = reportes();

            return View();
        }
   
        public List<SelectListItem> reportes()
        {
            return new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text = "Reporte de ingresos",
                    Value = "1"
                },
                new SelectListItem()
                {
                    Text = "Reporte de egresos",
                    Value = "2"
                },
                new SelectListItem()
                {
                    Text = "Reporte de listas de precio",
                    Value = "3"
                },
                new SelectListItem()
                {
                    Text = "Reporte de arqueo",
                    Value = "4"
                },
                new SelectListItem()
                {
                    Text = "Reporte de resumen de ingresos y egresos",
                    Value = "5"
                }
            };
        }

        [HttpPost]
        public ActionResult ViewRpt(String Tipos, String Reportes, RecuperarIDs id = null)
        {
            LocalReport rpt = new LocalReport();
            String mt, enc, f;
            String[] s;
            Warning[] w;
            String ruta = "";
            ReportDataSource rd = null;

            switch (Reportes)
            {
                case "1": //Ingreso comunidad
                    {
                        ruta = Path.Combine(Server.MapPath("~/Reportes"), "RptIngresoFiltrados.rdlc");
                        rpt.ReportPath = ruta;

                        String nombre_ker = "";
                        String nombre_com = "";

                        foreach (var x in db.tbl_kermesse)
                        {
                            if (x.id_kermesse == id.id_kermesse)
                            {
                                nombre_ker = x.nombre;
                            }
                        }
                        foreach (var a in db.tbl_comunidad)
                        {
                            if (a.id_comunidad == id.id_comunidad)
                            {
                                nombre_com = a.nombre;
                            }
                        }

                        var lista = db.Vw_IngresoFiltrada.Where(x => x.estado != 3 && x.kermesse == nombre_ker && x.comunidad == nombre_com);
                        rd = new ReportDataSource("dsRptIngreso", lista);
                        break;
                    }
                case "2": //Gastos
                    {
                        ruta = Path.Combine(Server.MapPath("~/Reportes"), "RptGastosFiltrados.rdlc");
                        rpt.ReportPath = ruta;
                        String nombre = "";
                        foreach (var x in db.tbl_kermesse)
                        {
                            if (x.id_kermesse == id.id_kermesse)
                            {
                                nombre = x.nombre;
                            }
                        }
                        var lista = db.Vw_Gastos.Where(x => x.estado != 3 && x.kermesse == nombre);
                        rd = new ReportDataSource("dsRptGastos", lista);
                        break;
                    }
                case "3": //Lista precio
                    {
                        ruta = Path.Combine(Server.MapPath("~/Reportes"), "RptGastos.rdlc"); //FALTA
                        rpt.ReportPath = ruta;
                        var lista = db.tbl_listaprecio.Where(x => x.estado != 3); //FALTA
                        rd = new ReportDataSource("dsRptGastos", lista);
                        break;
                    }
                case "4": //Arqueo caja
                    {
                        ruta = Path.Combine(Server.MapPath("~/Reportes"), "RptGastos.rdlc");
                        rpt.ReportPath = ruta;
                        String nombre = "";
                        foreach (var x in db.tbl_kermesse)
                        {
                            if (x.id_kermesse == id.id_kermesse)
                            {
                                nombre = x.nombre;
                            }
                        }
                        var lista = db.Vw_Gastos.Where(x => x.estado != 3 && x.kermesse == nombre);
                        rd = new ReportDataSource("dsRptGastos", lista);
                        break;
                    }
                case "5": //Resumen de ingresos y egresos
                    {
                        ruta = Path.Combine(Server.MapPath("~/Reportes"), "RptGastos.rdlc");
                        rpt.ReportPath = ruta;
                        var lista = db.tbl_gastos.Where(x => x.estado != 3 && x.id_kermesse == id.id_kermesse);
                        rd = new ReportDataSource("dsRptGastos", lista);
                        Console.WriteLine("SABRA DIOS COMO ES");
                        break;
                    }
                default:
                    Console.WriteLine("NO EXISTE ESA OPCIÓN");

                    break;
            }
            rpt.DataSources.Add(rd);
            var b = rpt.Render(Tipos, null, out mt, out enc, out f, out s, out w);

            return new FileContentResult(b, mt);
        }

    }
}