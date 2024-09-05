using Company.Data.Entities;
using Company.Repository.Interfaces;
using Company.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Services.Services
{
    public class DepartmentServices : IDepartmentServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentServices( IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }
        public void Add(Department department)
        {
            var MappedDepartment = new Department
            {
                Code = department.Code,
                Name = department.Name,
                CreateAt = DateTime.Now
            };

            _unitOfWork.DepartmentRepository.Add(MappedDepartment);
            _unitOfWork.Complete();
        }

        public void Delete(Department department)
        {
            _unitOfWork.DepartmentRepository.Delete(department);
            _unitOfWork.Complete();
        }

        public IEnumerable<Department> GetAll()
        {
            var departments = _unitOfWork.DepartmentRepository.GetAll();

            return departments;
        }

        public Department GetById(int? id)
        {
            if (id is null) 
                return null;

            var department = _unitOfWork.DepartmentRepository.GetById(id.Value);

            if (department is null)
                return null;

            return department;
        }

        public void Update(Department department)
        {
            _unitOfWork.DepartmentRepository.Update(department);
            _unitOfWork.Complete();

        }
    }
}
