using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASP2.Models;
using System.IO;


namespace ASP2.Controllers
{
    public class ClienteController : Controller
    {
        // GET: Cliente
        [Authorize]
        public ActionResult Index()
        {
            using (var db = new inventarioEntities())
            {
                return View(db.cliente.ToList());
            }

        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create(cliente cliente)
        {
            try
            {
                using (var db = new inventarioEntities())
                {
                    db.cliente.Add(cliente);

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"error {ex}");
                return View();
            }
        }

        public ActionResult Details(int id)
        {
            using (var db = new inventarioEntities())
            {
                var findCustomer = db.cliente.Find(id);
                return View(findCustomer);
            }
        }

        public ActionResult Edit(int id)
        {

            if (!ModelState.IsValid)
                return View();

            try
            {
                using (var db = new inventarioEntities())
                {
                    var findCustomer = db.cliente.Where(a => a.id == id).FirstOrDefault();
                    return View(findCustomer);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"error {ex}");
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit(cliente editCustomer)
        {
            try
            {
                using (var db = new inventarioEntities())
                {
                    var customer = db.cliente.Find(editCustomer.id);

                    customer.nombre = editCustomer.nombre;
                    customer.documento = editCustomer.documento;
                    customer.email = editCustomer.email;


                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"error {ex}");
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                using (var db = new inventarioEntities())
                {
                    var findCustomer = db.cliente.Find(id);
                    db.cliente.Remove(findCustomer);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"error {ex}");
                return View();
            }
        }

        public ActionResult CargarCSV()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CargarCSV(HttpPostedFileBase fileForms)
        {

            string filePath = string.Empty;

            if (fileForms != null)
            {

                string path = Server.MapPath("~/Uploads/");


                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                filePath = path + Path.GetFileName(fileForms.FileName);

                string extension = Path.GetExtension(fileForms.FileName);

                fileForms.SaveAs(filePath);

                string csvData = System.IO.File.ReadAllText(filePath);
                foreach (string row in csvData.Split('\n'))
                {
                    if (!string.IsNullOrEmpty(row))
                    {
                        var newCliente = new cliente
                        {
                            nombre = row.Split(';')[0],
                            documento = row.Split(';')[1],
                            email = row.Split(';')[2]
                        };

                        using (var db = new inventarioEntities())
                        {
                            db.cliente.Add(newCliente);
                            db.SaveChanges();
                        }
                    }
                }

            }
            return RedirectToAction("Index");
        }
    }
}