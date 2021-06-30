using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASP2.Models;
using Rotativa;


namespace ASP2.Controllers
{
    public class CompraController : Controller
    {
        // GET: Compra
        [Authorize]
        public ActionResult Index()
        {
            using (var db = new inventarioEntities())
            {
                return View(db.compra.ToList());
            }
        }

        public static string NombreUsuario(int? idUsuario)
        {
            using (var db = new inventarioEntities())
            {
                return db.usuario.Find(idUsuario).nombre;
            }
        }

        public ActionResult ListarUsuarios()
        {
            using (var db = new inventarioEntities())
            {
                return PartialView(db.usuario.ToList());
            }
        }

        public static string NombreCliente(int? idCliente)
        {
            using (var db = new inventarioEntities())
            {
                return db.cliente.Find(idCliente).nombre;
            }
        }

        public ActionResult ListarClientes()
        {
            using (var db = new inventarioEntities())
            {
                return PartialView(db.cliente.ToList());
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(compra purchase)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                using (var db = new inventarioEntities())
                {
                    db.compra.Add(purchase);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception Ex)
            {
                ModelState.AddModelError("", $"error {Ex}");
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            using (var db = new inventarioEntities())
            {
                var productoEdit = db.compra.Where(a => a.id == id).FirstOrDefault();
                return View(productoEdit);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit(compra purchaseEdit)
        {

            if (!ModelState.IsValid)
                return View();

            try
            {
                using (var db = new inventarioEntities())
                {
                    var oldPurchase = db.compra.Find(purchaseEdit.id);
                    oldPurchase.fecha = purchaseEdit.fecha;
                    oldPurchase.total = purchaseEdit.total;
                    oldPurchase.id_cliente = purchaseEdit.id_cliente;
                    oldPurchase.id_usuario = purchaseEdit.id_usuario;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception Ex)
            {
                ModelState.AddModelError("", $"error{Ex}");
                return View();
            }
        }

        public ActionResult Details(int id)
        {
            using (var db = new inventarioEntities())
            {
                return View(db.compra.Find(id));
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                using (var db = new inventarioEntities())
                {
                    var compra = db.compra.Find(id);
                    db.compra.Remove(compra);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception Ex)
            {
                ModelState.AddModelError("", $"error {Ex}");
                return View();
            }
        }

        public ActionResult Revision()
        {
            var db = new inventarioEntities();
            var query = from tabCliente in db.cliente
                        join tabCompra in db.compra on tabCliente.id equals tabCompra.id_cliente
                        select new Revision
                        {
                            nombreCliente = tabCliente.nombre,
                            documentoCliente = tabCliente.documento,
                            emailCliente = tabCliente.email,
                            fechaCompra = tabCompra.fecha,
                            totalCompra = tabCompra.total
                        };
            return View(query);
        }

        public ActionResult ImprimirRevision()
        {
            return new ActionAsPdf("Revision") { FileName = "Revision.pdf" };
        }
    }
}