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
    public class NextInvestmentAccountsController : Controller
    {
        private BankOfBIT_KLContext db = new BankOfBIT_KLContext();

        // GET: NextInvestmentAccounts
        public ActionResult Index()
        {
            return View(NextInvestmentAccount.GetInstance());
        }

        // GET: NextInvestmentAccounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NextInvestmentAccount nextInvestmentAccount = (NextInvestmentAccount)db.NextUniqueNumbers.Find(id);
            if (nextInvestmentAccount == null)
            {
                return HttpNotFound();
            }
            return View(nextInvestmentAccount);
        }

        // GET: NextInvestmentAccounts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NextInvestmentAccounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NextUniqueNumberId,NextAvailableNumber")] NextInvestmentAccount nextInvestmentAccount)
        {
            if (ModelState.IsValid)
            {
                db.NextInvestmentAccounts.Add(nextInvestmentAccount);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nextInvestmentAccount);
        }

        // GET: NextInvestmentAccounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NextInvestmentAccount nextInvestmentAccount = (NextInvestmentAccount)db.NextUniqueNumbers.Find(id);
            if (nextInvestmentAccount == null)
            {
                return HttpNotFound();
            }
            return View(nextInvestmentAccount);
        }

        // POST: NextInvestmentAccounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NextUniqueNumberId,NextAvailableNumber")] NextInvestmentAccount nextInvestmentAccount)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nextInvestmentAccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nextInvestmentAccount);
        }

        // GET: NextInvestmentAccounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NextInvestmentAccount nextInvestmentAccount = (NextInvestmentAccount)db.NextUniqueNumbers.Find(id);
            if (nextInvestmentAccount == null)
            {
                return HttpNotFound();
            }
            return View(nextInvestmentAccount);
        }

        // POST: NextInvestmentAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NextInvestmentAccount nextInvestmentAccount = (NextInvestmentAccount)db.NextUniqueNumbers.Find(id);
            db.NextInvestmentAccounts.Remove(nextInvestmentAccount);
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
