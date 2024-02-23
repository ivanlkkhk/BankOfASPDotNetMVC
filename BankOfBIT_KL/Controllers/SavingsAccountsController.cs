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
    public class SavingsAccountsController : Controller
    {
        private BankOfBIT_KLContext db = new BankOfBIT_KLContext();

        // GET: SavingsAccounts
        public ActionResult Index()
        {
            var bankAccounts = db.SavingsAccounts.Include(s => s.AccountState).Include(s => s.Client);
            return View(bankAccounts.ToList());
        }

        // GET: SavingsAccounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SavingsAccount savingsAccount = db.SavingsAccounts.Find(id);
            if (savingsAccount == null)
            {
                return HttpNotFound();
            }
            return View(savingsAccount);
        }

        // GET: SavingsAccounts/Create
        public ActionResult Create()
        {
            ViewBag.AccountStateId = new SelectList(db.AccountStates, "AccountStateId", "Description");
            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "FullName");
            return View();
        }

        // POST: SavingsAccounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BankAccountId,ClientId,AccountStateId,AccountNumber,Balance,DateCreated,Notes,SavingsServiceCharges")] SavingsAccount savingsAccount)
        {
            savingsAccount.SetNextAccountNumber();
            if (ModelState.IsValid)
            {
                savingsAccount.ChangeState();
                db.BankAccounts.Add(savingsAccount);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AccountStateId = new SelectList(db.AccountStates, "AccountStateId", "Description", savingsAccount.AccountStateId);
            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "FullName", savingsAccount.ClientId);
            return View(savingsAccount);
        }

        // GET: SavingsAccounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SavingsAccount savingsAccount = db.SavingsAccounts.Find(id);
            if (savingsAccount == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccountStateId = new SelectList(db.AccountStates, "AccountStateId", "Description", savingsAccount.AccountStateId);
            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "FullName", savingsAccount.ClientId);
            return View(savingsAccount);
        }

        // POST: SavingsAccounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BankAccountId,ClientId,AccountStateId,AccountNumber,Balance,DateCreated,Notes,SavingsServiceCharges")] SavingsAccount savingsAccount)
        {
            if (ModelState.IsValid)
            {
                savingsAccount.ChangeState();
                db.Entry(savingsAccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AccountStateId = new SelectList(db.AccountStates, "AccountStateId", "Description", savingsAccount.AccountStateId);
            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "FullName", savingsAccount.ClientId);
            return View(savingsAccount);
        }

        // GET: SavingsAccounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SavingsAccount savingsAccount = db.SavingsAccounts.Find(id);
            if (savingsAccount == null)
            {
                return HttpNotFound();
            }
            return View(savingsAccount);
        }

        // POST: SavingsAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SavingsAccount savingsAccount = db.SavingsAccounts.Find(id);
            db.BankAccounts.Remove(savingsAccount);
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
