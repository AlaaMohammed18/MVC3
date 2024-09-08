using AutoMapper;
using Company.Data.Entities;
using Company.Repository.Interfaces;
using Company.Services.Dto;
using Company.Services.Helper;
using Company.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Services.Services
{
    public class EmployeeServices : IEmployeeServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeServices(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public void Add(EmployeeDto employeeDto)
        {
            //Manual Mapping
            //Employee employee = new Employee
            //{
            //    Name = employeeDto.Name,
            //    Age = employeeDto.Age,
            //    Address = employeeDto.Address, 
            //    Salary = employeeDto.Salary,
            //    Email = employeeDto.Email,
            //    PhoneNumber = employeeDto.PhoneNumber,
            //    HiringDate = employeeDto.HiringDate,
            //    DepartmentID = employeeDto.DepartmentID,
            //    ImageURL = employeeDto.ImageURL,
            //    CreateAt = DateTime.Now

            //};

            employeeDto.ImageURL = DocumentSetting.UploadFile(employeeDto.Image, "Images");

            Employee employee = _mapper.Map<Employee>(employeeDto);

            _unitOfWork.EmployeeRepository.Add(employee);

            _unitOfWork.Complete();
        }

        public void Delete(EmployeeDto employeeDto)
        {
            //Employee employee = new Employee
            //{
            //    Name = employeeDto.Name,
            //    Age = employeeDto.Age,
            //    Address = employeeDto.Address,
            //    Salary = employeeDto.Salary,
            //    Email = employeeDto.Email,
            //    PhoneNumber = employeeDto.PhoneNumber,
            //    HiringDate = employeeDto.HiringDate,
            //    DepartmentID = employeeDto.DepartmentID,
            //    ImageURL = employeeDto.ImageURL

            //};
            Employee employee = _mapper.Map<Employee>(employeeDto);

            _unitOfWork.EmployeeRepository.Delete(employee);
            _unitOfWork.Complete();
        }

        public IEnumerable<EmployeeDto> GetAll()
        {
            var Employees = _unitOfWork.EmployeeRepository.GetAll();

            //var MappedEmployees = Employees.Select(x => new EmployeeDto
            //{ 
            //    Id = x.Id,
            //    Name = x.Name,
            //    Age = x.Age,
            //    Address = x.Address,
            //    Salary = x.Salary,
            //    Email = x.Email,
            //    PhoneNumber = x.PhoneNumber,
            //    HiringDate = x.HiringDate,
            //    DepartmentID = x.DepartmentID,
            //    ImageURL = x.ImageURL,
            //    CreateAt = x.CreateAt

            //});
            IEnumerable<EmployeeDto> MappedEmployees =_mapper.Map<IEnumerable<EmployeeDto>>(Employees);


            return MappedEmployees;
        }

        public EmployeeDto GetById(int? id)
        {
            if (id is null)
                return null;

            var employee = _unitOfWork.EmployeeRepository.GetById(id.Value);

            if (employee is null)
                return null;

            //EmployeeDto employeeDto = new EmployeeDto
            //{
            //    Name = employee.Name,
            //    Age = employee.Age,
            //    Address = employee.Address,
            //    Salary = employee.Salary,
            //    Email = employee.Email,
            //    PhoneNumber = employee.PhoneNumber,
            //    HiringDate = employee.HiringDate,
            //    DepartmentID = employee.DepartmentID,
            //    ImageURL = employee.ImageURL,
            //    CreateAt = employee.CreateAt,
            //    Id = employee.Id

            //};

            EmployeeDto employeeDto = _mapper.Map<EmployeeDto>(employee);


            return employeeDto;
        }

        public IEnumerable<EmployeeDto> GetEmployeeByName(string name)
        {
            var Employees = _unitOfWork.EmployeeRepository.GetEmployeeByName(name);

            //var MappedEmployees = Employees.Select(x => new EmployeeDto
            //{
            //    Id = x.Id,
            //    Name = x.Name,
            //    Age = x.Age,
            //    Address = x.Address,
            //    Salary = x.Salary,
            //    Email = x.Email,
            //    PhoneNumber = x.PhoneNumber,
            //    HiringDate = x.HiringDate,
            //    DepartmentID = x.DepartmentID,
            //    ImageURL = x.ImageURL,
            //    CreateAt = x.CreateAt

            //});
            IEnumerable<EmployeeDto> MappedEmployees = _mapper.Map<IEnumerable<EmployeeDto>>(Employees);


            return MappedEmployees;

        }

        public void Update(EmployeeDto employee)
        {
            //_unitOfWork.EmployeeRepository.Update(employee);
            _unitOfWork.Complete();
        }
    }
}
