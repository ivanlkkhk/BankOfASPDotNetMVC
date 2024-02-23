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
    public class PlatinumStatesController : Controller
    {
        private BankOfBIT_KLContext db = new BankOfBIT_KLContext();

        // GET: PlatinumStates
        public ActionResult Index()
        {
            return View(PlatinumState.GetInstance());
        }

        // GET: PlatinumStates/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlatinumState platinumState = (PlatinumState)db.AccountStates.Find(id);
            if (platinumState == null)
            {
                return HttpNotFound();
            }
            return View(platinumState);
        }

        // GET: PlatinumStates/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PlatinumStates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AccountStateId,LowerLimit,UpperLimit,Rate")] PlatinumState platinumState)
        {
            if (ModelState.IsValid)
            {
                db.AccountStates.Add(platinumState);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(platinumState);
        }

        // GET: PlatinumStates/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlatinumState platinumState = (PlatinumState)db.AccountStates.Find(id);
            if (platinumState == null)
            {
                return HttpNotFound();
            }
            return View(platinumState);
        }

        // POST: PlatinumStates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AccountStateId,LowerLimit,UpperLimit,Rate")] PlatinumState platinumState)
        {
            if (ModelState.IsValid)
            {
                db.Entry(platinumState).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(platinumState);
        }

        // GET: PlatinumStates/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlatinumState platinumState = (PlatinumState)db.AccountStates.Find(id);
            if (platinumState == null)
            {
                return HttpNotFound();
            }
            return View(platinumState);
        }

        // POST: PlatinumStates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PlatinumState platinumState = (PlatinumState)db.AccountStates.Find(id);
            db.AccountStates.Remove(platinumState);
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
