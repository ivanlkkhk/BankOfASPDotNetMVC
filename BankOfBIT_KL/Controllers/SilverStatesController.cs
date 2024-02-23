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
    public class SilverStatesController : Controller
    {
        private BankOfBIT_KLContext db = new BankOfBIT_KLContext();

        // GET: SilverStates
        public ActionResult Index()
        {
            return View(SilverState.GetInstance());
        }

        // GET: SilverStates/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SilverState silverState = (SilverState)db.AccountStates.Find(id);
            if (silverState == null)
            {
                return HttpNotFound();
            }
            return View(silverState);
        }

        // GET: SilverStates/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SilverStates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AccountStateId,LowerLimit,UpperLimit,Rate")] SilverState silverState)
        {
            if (ModelState.IsValid)
            {
                db.AccountStates.Add(silverState);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(silverState);
        }

        // GET: SilverStates/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SilverState silverState = (SilverState)db.AccountStates.Find(id);
            if (silverState == null)
            {
                return HttpNotFound();
            }
            return View(silverState);
        }

        // POST: SilverStates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AccountStateId,LowerLimit,UpperLimit,Rate")] SilverState silverState)
        {
            if (ModelState.IsValid)
            {
                db.Entry(silverState).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(silverState);
        }

        // GET: SilverStates/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SilverState silverState = (SilverState)db.AccountStates.Find(id);
            if (silverState == null)
            {
                return HttpNotFound();
            }
            return View(silverState);
        }

        // POST: SilverStates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SilverState silverState = (SilverState)db.AccountStates.Find(id);
            db.AccountStates.Remove(silverState);
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
