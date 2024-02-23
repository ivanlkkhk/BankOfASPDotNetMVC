using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BankOfBIT_KL.Data;
using BankOfBIT_KL.Models;

namespace BankOfBIT_KL.Controllers
{
    public class NextClientsController : Controller
    {
        private BankOfBIT_KLContext db = new BankOfBIT_KLContext();

        // GET: NextClients
        public ActionResult Index()
        {
            return View(NextClient.GetInstance());
        }

        // GET: NextClients/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NextClient nextClient = (NextClient)db.NextUniqueNumbers.Find(id);
            if (nextClient == null)
            {
                return HttpNotFound();
            }
            return View(nextClient);
        }

        // GET: NextClients/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NextClients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NextUniqueNumberId,NextAvailableNumber")] NextClient nextClient)
        {
            if (ModelState.IsValid)
            {
                db.NextClients.Add(nextClient);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nextClient);
        }

        // GET: NextClients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NextClient nextClient = (NextClient)db.NextUniqueNumbers.Find(id);
            if (nextClient == null)
            {
                return HttpNotFound();
            }
            return View(nextClient);
        }

        // POST: NextClients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NextUniqueNumberId,NextAvailableNumber")] NextClient nextClient)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nextClient).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nextClient);
        }

        // GET: NextClients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NextClient nextClient = (NextClient)db.NextUniqueNumbers.Find(id);
            if (nextClient == null)
            {
                return HttpNotFound();
            }
            return View(nextClient);
        }

        // POST: NextClients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NextClient nextClient = (NextClient)db.NextUniqueNumbers.Find(id);
            db.NextClients.Remove(nextClient);
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
