using Company.Data.Contexts;
using Company.Data.Entities;
using Company.Repository.Interfaces;
using Company.Repository.Repositories;
using Company.Services.Interfaces;
using Company.Services.Dto;
using Company.Services.Services;
using Company.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Company.Web.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeServices _employeeServices;
        private readonly IDepartmentServices _departmentServices;

        public EmployeeController(IEmployeeServices employeeServices , IDepartmentServices departmentServices)
        {
            _employeeServices = employeeServices;
            _departmentServices = departmentServices;
        }

        public IActionResult Index(string SearchInp)
        {
            ////ViewBag , ViewData , TempData
            //ViewBag.Message = "Hello From Employee Index(ViewBag)";

            //ViewData["TextMessage"] = "Hello From Employee Index(ViewData)";

            //TempData["TextTempMessage"] = "Hello From Employee Index(TempData)";

            IEnumerable<EmployeeDto> Employees = new List<EmployeeDto>();

            if (string.IsNullOrEmpty(SearchInp))
            {
                Employees = _employeeServices.GetAll();
                return View(Employees);
            }
            else
            {
                Employees = _employeeServices.GetEmployeeByName(SearchInp);
                return View(Employees);
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Departments = _departmentServices.GetAll();
            return View();
        }
        
        [HttpPost]
        public IActionResult Create(EmployeeDto employee)
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
        public IActionResult Update(int? id, EmployeeDto employee)
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
