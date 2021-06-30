using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASP2.Models;
using Rotativa;

namespace ASP2.Controllers
{
    public class ProductoController : Controller
    {
        // GET: Producto
        [Authorize]
        public ActionResult Index()
        {
            using (var db = new inventarioEntities())
            {
                return View(db.producto.ToList());
            }
        }

        public static string NombreProveedor(int? idProveedor)
        {
            using (var db = new inventarioEntities())
            {
                return db.proveedor.Find(idProveedor).nombre;
            }
        }

        public ActionResult ListarProveedores()
        {
            using (var db = new inventarioEntities())
            {
                return PartialView(db.proveedor.ToList());
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(producto product)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                using (var db = new inventarioEntities())
                {
                    db.producto.Add(product);
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
                var productoEdit = db.producto.Where(a => a.id == id).FirstOrDefault();
                return View(productoEdit);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit(producto productEdit)
        {

            if (!ModelState.IsValid)
                return View();


            try
            {
                using (var db = new inventarioEntities())
                {
                    var oldProduct = db.producto.Find(productEdit.id);
                    oldProduct.nombre = productEdit.nombre;
                    oldProduct.cantidad = productEdit.cantidad;
                    oldProduct.descripcion = productEdit.descripcion;
                    oldProduct.percio_unitario = productEdit.percio_unitario;
                    oldProduct.id_proveedor = productEdit.id_proveedor;
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
                return View(db.producto.Find(id));
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                using (var db = new inventarioEntities())
                {
                    var producto = db.producto.Find(id);
                    db.producto.Remove(producto);
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

        public ActionResult Reporte()
        {
            var db = new inventarioEntities();

            var query = from tabProveedor in db.proveedor
                        join tabProducto in db.producto on tabProveedor.id equals tabProducto.id_proveedor
                        select new Reporte
                        {
                            nombreProveedor = tabProveedor.nombre,
                            telefonoProveedor = tabProveedor.telefono,
                            direccionProveedor = tabProveedor.direccion,
                            nombreProducto = tabProducto.nombre,
                            precioProducto = tabProducto.percio_unitario
                        };
            return View(query);

        }

        public ActionResult ImprimirReporte()
        {
            return new ActionASPdf("Reporte") { FileName = "Reporte.pdf" };
        }
    }
}