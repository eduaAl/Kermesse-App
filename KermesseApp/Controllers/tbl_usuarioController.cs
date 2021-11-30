using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using KermesseApp.Models;

namespace KermesseApp.Controllers
{
    public class tbl_usuarioController : Controller
    {
        private KERMESSEEntities db = new KERMESSEEntities();
        // GET: tbl_usuario
        public ActionResult ListUsuario()
        {
            return View(db.tbl_usuario.ToList());
        }
        public ActionResult VguardarUsuario()
        {
            return View();
        }
        public ActionResult ViewLogin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password)
        {
            if (String.IsNullOrEmpty(username) && String.IsNullOrEmpty(password))
            {
                return View("ViewLogin");
            }
            else
            {
                var objeto = db.tbl_usuario.Where(x => x.usuario.Equals(username) && x.pwd.Equals(password)).FirstOrDefault();
                if (objeto != null)
                {
                    Session["usuario"] = objeto;
                    return Redirect("~/Home/Index");
                    //return RedirectToRoute("Default", new { controller = "Home", action = "Index" });
                    //return this.RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.error = "¡Los datos de accesos son incorrectos, por favor intente nuevamente!";
                    return View("ViewLogin");
                }
            }
        }
        [HttpGet]
        public ActionResult Logout()
        {
            Session.Remove("usuario");
            return Redirect("~/tbl_usuario/ViewLogin");
            //return View("ViewLogin");
        }
        [HttpPost]
        public ActionResult guardarUsuario(tbl_usuario tcu)
        {
            if (ModelState.IsValid)
            {
                tbl_usuario tcUsuario = new tbl_usuario();
                tcUsuario.usuario = tcu.usuario;
                tcUsuario.pwd = tcu.pwd;
                tcUsuario.nombres = tcu.nombres;
                tcUsuario.apellidos = tcu.apellidos;
                tcUsuario.email = tcu.email;
                tcUsuario.estado = 1;
                db.tbl_usuario.Add(tcUsuario);
                db.SaveChanges();
                return RedirectToAction("ListUsuario");
            }
            ModelState.Clear();
            return View("VguardarUsuario");
        }
        public ActionResult deleteUsuario(int id)
        {
            tbl_usuario tcUsuario = new tbl_usuario();
            tcUsuario = db.tbl_usuario.Find(id);
            db.tbl_usuario.Remove(tcUsuario);

            db.SaveChanges();
            var list = db.tbl_usuario.ToList();
            return View("ListUsuario", list);
        }
        public ActionResult verUsuario(int id)
        {
            var catUsuario = db.tbl_usuario.Where(x => x.id_usuario == id).First();
            return View(catUsuario);

        }
        public ActionResult editUsuario(int id)
        {
            tbl_usuario tco = db.tbl_usuario.Find(id);
            if (tco == null)
            {
                return HttpNotFound();

            }
            else
            {
                return View(tco);
            }
        }
        [HttpPost]
        public ActionResult updateUsuario(tbl_usuario tco)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    tco.estado = 2;
                    db.Entry(tco).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("ListUsuario");
            }
            catch
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult filtrarUsuario(String cadena)
        {
            if (String.IsNullOrEmpty(cadena))
            {
                var list = db.tbl_usuario.ToList();
                return View("ListUsuario", list);
            }
            else
            {
                var listFiltrada = db.tbl_usuario.Where(x => x.usuario.Contains(cadena) || x.nombres.Contains(cadena) || x.apellidos.Contains(cadena));
                return View("ListUsuario", listFiltrada);
            }
        }

    }
}