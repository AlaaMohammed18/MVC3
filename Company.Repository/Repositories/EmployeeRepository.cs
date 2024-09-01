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

        public Employee GetEmployeeByName(string name)
        {
            throw new NotImplementedException();
        }
    }
}
