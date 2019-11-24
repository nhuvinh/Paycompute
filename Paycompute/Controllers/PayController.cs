using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Paycompute.Entity;
using Paycompute.Models;
using Paycompute.Services;

namespace Paycompute.Controllers
{
    public class PayController : Controller
    {
								private IPayComputationService _payComputationServie;
								private IEmployeeService _employeeService;
								private ITaxService _taxService;
								private INationalInsuranceContributionService _nationalInsuranceContributionService;
								private decimal _overTimeHours;
								private decimal _overtimeEarnings;
								private decimal _contractualEarnings;
								private decimal _totalEarnings;
								private decimal _tax;
								private decimal _unionFee;
								private decimal _nic;
								private decimal _slc;
								private decimal _totalDeduction;

								public PayController(IPayComputationService payComputationService, 
																												IEmployeeService employeeService, 
																												ITaxService taxService,
																												INationalInsuranceContributionService nationalInsuranceContributionService)
								{
												_payComputationServie = payComputationService;
												_employeeService = employeeService;
												_taxService = taxService;
												_nationalInsuranceContributionService = nationalInsuranceContributionService;
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

								[HttpGet]
								public IActionResult Create()
								{
												ViewBag.employees = _employeeService.GetAllEmployeesForPayRoll();
												ViewBag.taxYears = _payComputationServie.GetAllTaxYear();
												var viewModel = new PaymentRecordCreateViewModel();
												return View(viewModel);

								}

								[HttpPost]
								[ValidateAntiForgeryToken]
								public async Task<IActionResult> Create(PaymentRecordCreateViewModel viewModel)
								{
												if (ModelState.IsValid)
												{
																var payRecord = new PaymentRecord
																{
																				Id = viewModel.Id,
																				EmployeeId = viewModel.EmployeeId,
																				FullName = _employeeService.GetById(viewModel.Id).FullName,
																				NiNo = viewModel.NiNo,
																				PayDate = viewModel.PayDate,
																				PayMonth = viewModel.PayMonth,
																				TaxYearId = viewModel.TaxYearId,
																				TaxCode = viewModel.TaxCode,
																				HourlyRate = viewModel.HourlyRate,
																				HoursWorked = viewModel.HoursWorked,
																				ContractualHours = viewModel.ContractualHours,
																				OvertimeHours = _overTimeHours = _payComputationServie.OvertimeHours(viewModel.HoursWorked, viewModel.ContractualHours),
																				ContractualEarnings = _contractualEarnings = _payComputationServie.ContractualEarning(viewModel.ContractualHours, viewModel.HoursWorked, viewModel.HourlyRate),
																				OvertimeEarnings = _overtimeEarnings = _payComputationServie.OvertimeEarnings(_payComputationServie.OvertimeRate(viewModel.HourlyRate), _overTimeHours),
																				TotalEarnings = _totalEarnings = _payComputationServie.TotalEarnings(_overtimeEarnings, _contractualEarnings),
																				Tax = _tax = _taxService.TaxAmount(_totalEarnings),
																				UnionFee = _unionFee = _employeeService.UnionFees(viewModel.Id),
																				SLC = _slc = _employeeService.StudentLoanRepaymentAmount(viewModel.Id, _totalEarnings),
																				NIC = _nic = _nationalInsuranceContributionService.NIContribution(_totalEarnings),
																				TotalDeduction = _totalDeduction = _payComputationServie.TotalDeduction(_tax, _nic, _slc, _unionFee),
																				NetPayment = _payComputationServie.NetPay(_totalEarnings, _totalDeduction)
																};
																await _payComputationServie.CreateAsync(payRecord);
																return RedirectToAction(nameof(Index));
												}

												ViewBag.employees = _employeeService.GetAllEmployeesForPayRoll();
												ViewBag.taxYears = _payComputationServie.GetAllTaxYear();
												return View();
								}
    }
}