using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Paycompute.Entity;
using Paycompute.Models;
using Paycompute.Services;
using RotativaCore;

namespace Paycompute.Controllers
{
    [Authorize(Roles = "Admin, Manager")]
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

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.employees = _employeeService.GetAllEmployeesForPayRoll();
            ViewBag.taxYears = _payComputationServie.GetAllTaxYear();
            var viewModel = new PaymentRecordCreateViewModel();
            return View(viewModel);

        }

        [Authorize(Roles = "Admin")]
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

        [HttpGet]
        public IActionResult Detail(int id)
        {
            var paymentRecord = _payComputationServie.GetById(id);
            if (paymentRecord == null) return NotFound();

            var viewModel = new PaymentRecordDetailViewModel()
            {
                Id = paymentRecord.Id,
                EmployeeId = paymentRecord.EmployeeId,
                FullName = paymentRecord.FullName,
                NiNo = paymentRecord.NiNo,
                PayDate = paymentRecord.PayDate,
                PayMonth = paymentRecord.PayMonth,
                TaxYearId = paymentRecord.TaxYearId,
                Year = _payComputationServie.GetTaxYearById(id).YearOfTax,
                TaxCode = paymentRecord.TaxCode,
                HourlyRate = paymentRecord.HourlyRate,
                HoursWorked = paymentRecord.HoursWorked,
                ContractualHours = paymentRecord.ContractualHours,
                OvertimeHours = paymentRecord.OvertimeHours,
                OvertimeEarnings = paymentRecord.OvertimeEarnings,
                OvertimeRate = _payComputationServie.OvertimeRate(paymentRecord.HourlyRate),
                ContractualEarnings = paymentRecord.ContractualEarnings,
                Tax = paymentRecord.Tax,
                NIC = paymentRecord.NIC,
                UnionFee = paymentRecord.UnionFee,
                SLC = paymentRecord.SLC,
                TotalEarnings = paymentRecord.TotalEarnings,
                TotalDeduction = paymentRecord.TotalDeduction,
                Employee = paymentRecord.Employee,
                TaxYear = paymentRecord.TaxYear,
                NetPayment = paymentRecord.NetPayment
            };
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Payslip(int id)
        {
            var paymentRecord = _payComputationServie.GetById(id);
            if (paymentRecord == null) return NotFound();

            var viewModel = new PaymentRecordDetailViewModel()
            {
                Id = paymentRecord.Id,
                EmployeeId = paymentRecord.EmployeeId,
                FullName = paymentRecord.FullName,
                NiNo = paymentRecord.NiNo,
                PayDate = paymentRecord.PayDate,
                PayMonth = paymentRecord.PayMonth,
                TaxYearId = paymentRecord.TaxYearId,
                Year = _payComputationServie.GetTaxYearById(id).YearOfTax,
                TaxCode = paymentRecord.TaxCode,
                HourlyRate = paymentRecord.HourlyRate,
                HoursWorked = paymentRecord.HoursWorked,
                ContractualHours = paymentRecord.ContractualHours,
                OvertimeHours = paymentRecord.OvertimeHours,
                OvertimeEarnings = paymentRecord.OvertimeEarnings,
                OvertimeRate = _payComputationServie.OvertimeRate(paymentRecord.HourlyRate),
                ContractualEarnings = paymentRecord.ContractualEarnings,
                Tax = paymentRecord.Tax,
                NIC = paymentRecord.NIC,
                UnionFee = paymentRecord.UnionFee,
                SLC = paymentRecord.SLC,
                TotalEarnings = paymentRecord.TotalEarnings,
                TotalDeduction = paymentRecord.TotalDeduction,
                Employee = paymentRecord.Employee,
                TaxYear = paymentRecord.TaxYear,
                NetPayment = paymentRecord.NetPayment
            };
            return View(viewModel);
        }

        public IActionResult GeneratePayslipPDF(int id)
        {
            var paySlip = new ActionAsPdf("Payslip", new { id = id })
            {
                FileName = "payslip.pdf"
            };
            return paySlip;
        }
    }
}