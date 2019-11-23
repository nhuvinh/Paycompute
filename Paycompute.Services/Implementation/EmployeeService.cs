using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Paycompute.Entity;
using Paycompute.Persitence;
using System.Linq;

namespace Paycompute.Services.Implementation
{
				public class EmployeeService : IEmployeeService
				{
								private readonly ApplicationDbContext _context;

								public EmployeeService(ApplicationDbContext context)
								{
												_context = context;
								}
								public async Task CreateAsync(Employee newEmployee)
								{
												await _context.Employees.AddAsync(newEmployee);
												await  _context.SaveChangesAsync();
								}

								public async Task Delete(int employeeId)
								{
												var employee = GetById(employeeId);
												_context .Remove(employee);
												await _context.SaveChangesAsync();
								}

								public IEnumerable<Employee> GetAll()
								{
												return _context.Employees;
								}

								public Employee GetById(int employeeId) => _context.Employees.SingleOrDefault(e => e.Id == employeeId);
								

								public decimal StudentLoanRepaymentAmount(int id, decimal totalAmount)
								{
												throw new NotImplementedException();
								}

								public decimal UnionFees(int id)
								{
												throw new NotImplementedException();
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
