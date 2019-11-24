using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Paycompute.Models;
using Paycompute.Services;

namespace Paycompute.Controllers
{
    public class PayController : Controller
    {
								private IPayComputationService _payComputationServie;

								public PayController(IPayComputationService payComputationService)
								{
												_payComputationServie = payComputationService;
								}

        public IActionResult Index()
        {
												var payRecords = _payComputationServie.GetAll().Select(pay => new PaymentRecordIndexViewModel
												{
																Id = pay.Id,
																EmployeeId = pay.EmployeeId,
																FullName = pay.FullName,
																PayDate = pay.PayDate,
																PayMonth = pay.PayMonth,
																TaxYearId = pay.TaxYearId,
																Year = _payComputationServie.GetTaxYearById(pay.TaxYearId).YearOfTax,
																TotalEarnings = pay.TotalEarnings,
																TotalDeduction = pay.TotalDeduction,
																Employee = pay.Employee
												});
            return View(payRecords);
        }
    }
}