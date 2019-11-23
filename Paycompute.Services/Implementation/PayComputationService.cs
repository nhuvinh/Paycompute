using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Paycompute.Entity;
using Paycompute.Persitence;
using System.Linq;

namespace Paycompute.Services.Implementation
{
				public class PayComputationService : IPayComputationService
				{
								private readonly ApplicationDbContext _context;
								private decimal contractualEarnings;
								private decimal overtimeHours;

								public PayComputationService(ApplicationDbContext context)
								{
												_context = context;
								}

								public decimal ContractualEarning(decimal contractualHours, decimal hoursWorked, decimal hourlyRate)
								{
												if(hoursWorked < contractualHours)
												{
																contractualEarnings = hoursWorked * hourlyRate;
												}
												else
												{
																contractualEarnings = contractualHours * hourlyRate;
												}

												return contractualEarnings;
								}

								public async Task CreateAsync(PaymentRecord paymentRecord)
								{
												await _context.PaymentRecords.AddAsync(paymentRecord);
												await _context.SaveChangesAsync();
								}

								public IEnumerable<PaymentRecord> GetAll()
								{
												return _context.PaymentRecords.OrderBy(p => p.EmployeeId);
								}

								public IEnumerable<SelectListItem> GetAllTaxYear()
								{
												var allTaxYear = _context.TaxYears.Select(taxYears => new SelectListItem
												{
																Text = taxYears.YearOfTax,
																Value = taxYears.Id.ToString(),
												}
												);

												return allTaxYear;
								}

								public decimal NetPay(decimal totalEarnings, decimal totalDeduction) => totalEarnings - totalDeduction;


								public decimal OvertimeEarnings(decimal overtimeRate, decimal overtimeHours)
								{
												return overtimeHours * overtimeRate;
								}

								public decimal OvertimeHours(decimal hoursWorked, decimal contractualHours)
								{
												if (hoursWorked <= contractualHours)
												{
																overtimeHours = 0.00m;
												}
												else if (hoursWorked > contractualHours)
												{
																overtimeHours = hoursWorked - contractualHours;
												}

												return overtimeHours;
								}

								public decimal OvertimeRate(decimal hourlyRate)
								{
												return hourlyRate * 1.5m;
								}

								public decimal TotalDeduction(decimal tax, decimal nic, decimal studentLoanRepayment, decimal unionFee)
								{
												return tax + nic + studentLoanRepayment + unionFee;
								}

								public decimal TotalEarnings(decimal overtimeEarnings, decimal contractualEarnings)
								{
												return overtimeEarnings + contractualEarnings;
								}

								public PaymentRecord GetById(int id)
								{
												return _context.PaymentRecords.Where(p => p.Id == id).FirstOrDefault();
								}
				}
}
