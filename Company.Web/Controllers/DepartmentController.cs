using Company.Data.Contexts;
using Company.Data.Entities;
using Company.Repository.Interfaces;
using Company.Repository.Repositories;
using Company.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Company.Web.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentServices _departmentServices;

        public DepartmentController(IDepartmentServices departmentServices)
        {
            _departmentServices = departmentServices;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var Departments = _departmentServices.GetAll();
            return View(Departments);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Department department)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _departmentServices.Add(department);

                    return RedirectToAction(nameof(Index));

                }
                ModelState.AddModelError("DepartmentError", "ValidationError");

                return View(department);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("DepartmentError", ex.Message);
                return View(department);
            }
        }
        public IActionResult Details(int? id , string ViewName = "Details")
        {
            var department = _departmentServices.GetById(id);

            if (department is null)
                return RedirectToAction("NotFoundPage" , null , "Home");

            return View(ViewName ,department);
        }

        [HttpGet]
        public IActionResult Update(int? id)
        {
            return Details(id , "Update");
        }


        [HttpPost]
        public IActionResult Update(int? id , Department department )
        {
            if (department.Id != id.Value)
                return RedirectToAction("NotFoundPage", null, "Home");

            _departmentServices.Update(department);

            return RedirectToAction(nameof(Index));

        }

        public IActionResult Delete (int id)
        {
            var department = _departmentServices.GetById(id);

            if (department is null)
                return RedirectToAction("NotFoundPage", null, "Home");

            _departmentServices.Delete(department);

            return RedirectToAction(nameof(Index));

        }
    }
}
