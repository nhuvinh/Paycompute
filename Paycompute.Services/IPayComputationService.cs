using Microsoft.AspNetCore.Mvc.Rendering;
using Paycompute.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Paycompute.Services
{
				public interface IPayComputationService
				{
								Task CreateAsync(PaymentRecord paymentRecord);
								TaxYear GetTaxYearById(int id);
								IEnumerable<PaymentRecord> GetAll();
								IEnumerable<SelectListItem> GetAllTaxYear();
								decimal OvertimeHours(decimal hoursWorked, decimal contractualHours);
								decimal ContractualEarning(decimal contractualHours, decimal hoursWorked, decimal hourlyRate);
								decimal OvertimeRate(decimal hourlyRate);
								decimal OvertimeEarnings(decimal overtimeRate, decimal overtimeHours);
								decimal TotalEarnings(decimal overtimeEarnings, decimal contractualEarnings);
								decimal TotalDeduction(decimal tax, decimal nic, decimal studentLoanRepayment, decimal unionFee);
								decimal NetPay(decimal totalEarnings, decimal totalDeduction);
								PaymentRecord GetById(int id);
				}
}
