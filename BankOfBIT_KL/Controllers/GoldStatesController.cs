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
    public class GoldStatesController : Controller
    {
        private BankOfBIT_KLContext db = new BankOfBIT_KLContext();

        // GET: GoldStates
        public ActionResult Index()
        {
            return View(GoldState.GetInstance());
        }

        // GET: GoldStates/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GoldState goldState = (GoldState)db.AccountStates.Find(id);
            if (goldState == null)
            {
                return HttpNotFound();
            }
            return View(goldState);
        }

        // GET: GoldStates/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GoldStates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AccountStateId,LowerLimit,UpperLimit,Rate")] GoldState goldState)
        {
            if (ModelState.IsValid)
            {
                db.AccountStates.Add(goldState);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(goldState);
        }

        // GET: GoldStates/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GoldState goldState = (GoldState)db.AccountStates.Find(id);
            if (goldState == null)
            {
                return HttpNotFound();
            }
            return View(goldState);
        }

        // POST: GoldStates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AccountStateId,LowerLimit,UpperLimit,Rate")] GoldState goldState)
        {
            if (ModelState.IsValid)
            {
                db.Entry(goldState).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(goldState);
        }

        // GET: GoldStates/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GoldState goldState = (GoldState)db.AccountStates.Find(id);
            if (goldState == null)
            {
                return HttpNotFound();
            }
            return View(goldState);
        }

        // POST: GoldStates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GoldState goldState = (GoldState)db.AccountStates.Find(id);
            db.AccountStates.Remove(goldState);
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
