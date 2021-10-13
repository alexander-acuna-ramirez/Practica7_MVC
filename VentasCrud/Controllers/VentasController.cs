using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VentasCrud;

namespace VentasCrud.Controllers
{
    public class VentasController : Controller
    {
        private SysVentasEntities db = new SysVentasEntities();

        /*Se le retorna al index el listado de ventas*/
        public ActionResult Index()
        {
            var ventas = db.Ventas.Include(v => v.Cliente).Include(v => v.Producto);
            return View(ventas.ToList());
        }

        /*Se retorna los datos de un registro en especifico a una vista de detalles*/
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Venta venta = db.Ventas.Find(id);
            if (venta == null)
            {
                return HttpNotFound();
            }
            return View(venta);
        }
        /* Se retorna la vista de creacion pero a la vez consultamos los datos de las tablas relacionadas
         * para poder crear el registro */
        public ActionResult Create()
        {
            ViewBag.ID_Cliente = new SelectList(db.Clientes, "ID", "Nombre");
            ViewBag.ID_Producto = new SelectList(db.Productos, "ID", "Nombre");
            return View();
        }

        /* Si un registro es valido se guarda en la BD y se redirige al index */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ID_Producto,ID_Cliente")] Venta venta)
        {
            if (ModelState.IsValid)
            {
                db.Ventas.Add(venta);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ID_Cliente = new SelectList(db.Clientes, "ID", "Nombre", venta.ID_Cliente);
            ViewBag.ID_Producto = new SelectList(db.Productos, "ID", "Nombre", venta.ID_Producto);
            return View(venta);
        }
        /* Se consulta los datos de un registro en especial para luego mandarlo a la vista,
         * Tambien se cargan algunos registros que son de las tablas relacionadas*/
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Venta venta = db.Ventas.Find(id);
            if (venta == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_Cliente = new SelectList(db.Clientes, "ID", "Nombre", venta.ID_Cliente);
            ViewBag.ID_Producto = new SelectList(db.Productos, "ID", "Nombre", venta.ID_Producto);
            return View(venta);
        }
         /* Se guarda un registro ya editado */ 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ID_Producto,ID_Cliente")] Venta venta)
        {
            if (ModelState.IsValid)
            {
                db.Entry(venta).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID_Cliente = new SelectList(db.Clientes, "ID", "Nombre", venta.ID_Cliente);
            ViewBag.ID_Producto = new SelectList(db.Productos, "ID", "Nombre", venta.ID_Producto);
            return View(venta);
        }

        /* Se busca un registro y se le pasa a la vista de eliminar */
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Venta venta = db.Ventas.Find(id);
            if (venta == null)
            {
                return HttpNotFound();
            }
            return View(venta);
        }
        /*Se elimina un registro en especifico*/

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Venta venta = db.Ventas.Find(id);
            db.Ventas.Remove(venta);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
