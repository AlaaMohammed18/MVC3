using Company.Data.Contexts;
using Company.Data.Entities;
using Company.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Repository.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee> , IEmployeeRepository
    {
        private readonly CompanyDbContext _context;

        public EmployeeRepository(CompanyDbContext context) : base(context) 
        {
            _context = context;
        }

        public IEnumerable<Employee> GetAllEmployeesByAddress(string Address)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Employee> GetEmployeeByName(string name)
            => _context.Employees.Where(x =>
            x.Name.Trim().ToLower().Contains(name.Trim().ToLower()) 
            ).ToList();
    }
}
