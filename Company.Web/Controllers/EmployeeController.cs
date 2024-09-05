using Company.Data.Contexts;
using Company.Data.Entities;
using Company.Repository.Interfaces;
using Company.Repository.Repositories;
using Company.Services.Interfaces;
using Company.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace Company.Web.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeServices _employeeServices;

        public EmployeeController(IEmployeeServices employeeServices)
        {
            _employeeServices = employeeServices;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var Employees = _employeeServices.GetAll();
            return View(Employees);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            try
            {
                _employeeServices.Add(employee);

                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
                return View(employee);
            }
        }
        public IActionResult Details(int? id, string ViewName = "Details")
        {
            var employee = _employeeServices.GetById(id);

            if (employee is null)
                return RedirectToAction("NotFoundPage", null, "Home");

            return View(ViewName, employee);
        }

        [HttpGet]
        public IActionResult Update(int? id)
        {
            return Details(id, "Update");
        }

        [HttpPost]
        public IActionResult Update(int? id, Employee employee)
        {
            if (employee.Id != id.Value)
                return RedirectToAction("NotFoundPage", null, "Home");

            _employeeServices.Update(employee);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var employee = _employeeServices.GetById(id);

            if (employee is null)
                return RedirectToAction("NotFoundPage", null, "Home");

            _employeeServices.Delete(employee);

            return RedirectToAction(nameof(Index));

        }
    }
}
