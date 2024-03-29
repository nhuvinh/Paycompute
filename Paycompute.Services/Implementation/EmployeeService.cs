﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Paycompute.Entity;
using Paycompute.Persitence;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Paycompute.Services.Implementation
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _context;
        private decimal studentLoanAmount;

        public EmployeeService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(Employee newEmployee)
        {
            await _context.Employees.AddAsync(newEmployee);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int employeeId)
        {
            var employee = GetById(employeeId);
            _context.Remove(employee);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<Employee> GetAll()
        {
            return _context.Employees;
        }

        public IEnumerable<SelectListItem> GetAllEmployeesForPayRoll()
        {
            return GetAll().Select(emp => new SelectListItem()
            {
                Text = emp.FullName,
                Value = emp.Id.ToString()
            });
        }

        public Employee GetById(int employeeId) => _context.Employees.SingleOrDefault(e => e.Id == employeeId);


        public decimal StudentLoanRepaymentAmount(int id, decimal totalAmount)
        {
            var employee = GetById(id);
            if (employee.StudentLoan == StudentLoan.Yes && totalAmount > 1750 && totalAmount < 2000)
            {
                studentLoanAmount = 15m;
            }
            else if (employee.StudentLoan == StudentLoan.Yes && totalAmount >= 2000 && totalAmount < 2250)
            {
                studentLoanAmount = 38m;
            }
            else if (employee.StudentLoan == StudentLoan.Yes && totalAmount >= 2250 && totalAmount < 2500)
            {
                studentLoanAmount = 60m;
            }
            else if (employee.StudentLoan == StudentLoan.Yes && totalAmount >= 2500)
            {
                studentLoanAmount = 83m;
            }
            else
            {
                studentLoanAmount = 0m;
            }
            return studentLoanAmount;
        }

        public decimal UnionFees(int id)
        {
            var employee = GetById(id);
            var unionFee = employee.UnionMember == UnionMember.Yes ? 10m : 0m;
            return unionFee;
        }

        public async Task UpdateAsync(Employee employee)
        {
            _context.Update(employee);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id)
        {
            var employee = GetById(id);
            _context.Update(employee);
            await _context.SaveChangesAsync();
        }
    }
}
