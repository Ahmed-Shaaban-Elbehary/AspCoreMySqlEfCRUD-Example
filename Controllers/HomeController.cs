using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Test.Models;

namespace Test.Controllers
{
    public class HomeController : Controller
    {
        private readonly DbContainer _container;

        public HomeController(DbContainer dbContainer)
        {
            _container = dbContainer;
        }
        public ActionResult Index()
        {
            IEnumerable<Customer> customers = _container.customers.ToList();
            ViewBag.Success = TempData["Success"];
            ViewBag.Error = TempData["Error"];
            return View(customers);
        }

        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(Customer customer)
        {
            if (customer != null)
            {
                _container.Add(customer);
                _container.SaveChanges();
                TempData["Success"] = "Add New Customer Successed!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error"] = "Form Data Required!";
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            Customer customer = _container.customers.Find(id);
            if (customer != null)
            {
                return View(customer);
            }
            else
            {
                TempData["Error"] = "This Customer Not Found!";
                return RedirectToAction("index");
            }
        }
        [HttpPost]
        public ActionResult Edit(int id, Customer customer)
        {
            if (customer != null)
            {
                Customer customerInDb = _container.customers.Find(id);
                if (customerInDb != null)
                {
                    customerInDb.Name = customer.Name;
                    customerInDb.Address = customer.Address;
                    customerInDb.PhoneNumber = customer.PhoneNumber;
                    _container.Entry(customerInDb).State = EntityState.Modified;
                    _container.SaveChanges();
                    TempData["Success"] = "Modify Customer Successed!";
                    return RedirectToAction("index");
                }
            }
            else
            {
                TempData["Error"] = "This Customer Not Found!";
                return RedirectToAction("index");
            }
            return View();
        }

        public ActionResult Delete(int id)
        {
            Customer customer = _container.customers.Find(id);
            if (customer != null)
            {
                _container.Remove(customer);
                _container.SaveChanges();
                TempData["Success"] = "Delete Customer Successed!";
                return RedirectToAction("index");
            }
            else
            {
                TempData["Error"] = "This Customer Not Found!";
                return RedirectToAction("index");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _container.Dispose();
                GC.Collect();
            }
            base.Dispose(disposing);
        }
    }
}
