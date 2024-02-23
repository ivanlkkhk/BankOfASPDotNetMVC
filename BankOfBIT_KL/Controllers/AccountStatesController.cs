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
    public class AccountStatesController : Controller
    {
        private BankOfBIT_KLContext db = new BankOfBIT_KLContext();

        // GET: AccountStates
        public ActionResult Index()
        {
            return View(db.AccountStates.ToList());
        }

        // GET: AccountStates/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccountState accountState = db.AccountStates.Find(id);
            if (accountState == null)
            {
                return HttpNotFound();
            }
            return View(accountState);
        }

        // GET: AccountStates/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AccountStates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AccountStateId,LowerLimit,UpperLimit,Rate")] AccountState accountState)
        {
            if (ModelState.IsValid)
            {
                db.AccountStates.Add(accountState);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(accountState);
        }

        // GET: AccountStates/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccountState accountState = db.AccountStates.Find(id);
            if (accountState == null)
            {
                return HttpNotFound();
            }
            return View(accountState);
        }

        // POST: AccountStates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AccountStateId,LowerLimit,UpperLimit,Rate")] AccountState accountState)
        {
            if (ModelState.IsValid)
            {
                db.Entry(accountState).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(accountState);
        }

        // GET: AccountStates/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccountState accountState = db.AccountStates.Find(id);
            if (accountState == null)
            {
                return HttpNotFound();
            }
            return View(accountState);
        }

        // POST: AccountStates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AccountState accountState = db.AccountStates.Find(id);
            db.AccountStates.Remove(accountState);
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
