using AutoMapper;
using Company.Data.Entities;
using Company.Repository.Interfaces;
using Company.Services.Dto;
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
        private readonly IMapper _mapper;

        public DepartmentServices( IUnitOfWork unitOfWork , IMapper mapper) 
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public void Add(DepartmentDto departmentDto)
        {
            //var MappedDepartment = new DepartmentDto
            //{
            //    Code = department.Code,
            //    Name = department.Name,
            //    CreateAt = DateTime.Now
            //};

            Department department = _mapper.Map<Department>(departmentDto);

            _unitOfWork.DepartmentRepository.Add(department);
            _unitOfWork.Complete();
        }

        public void Delete(DepartmentDto departmentDto)
        {
            Department department = _mapper.Map<Department>(departmentDto);

            _unitOfWork.DepartmentRepository.Delete(department);
            _unitOfWork.Complete();
        }

        public IEnumerable<DepartmentDto> GetAll()
        {
            var departments = _unitOfWork.DepartmentRepository.GetAll();

            IEnumerable<DepartmentDto> MappedDepartments = _mapper.Map<IEnumerable<DepartmentDto>>(departments);

            return MappedDepartments;
        }

        public DepartmentDto GetById(int? id)
        {
            if (id is null) 
                return null;

            var department = _unitOfWork.DepartmentRepository.GetById(id.Value);

            if (department is null)
                return null;

            DepartmentDto departmentDto = _mapper.Map<DepartmentDto>(department);


            return departmentDto;
        }

        public void Update(DepartmentDto department)
        {
            //_unitOfWork.DepartmentRepository.Update(department);
            _unitOfWork.Complete();

        }
    }
}
