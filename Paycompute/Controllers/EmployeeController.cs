using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Paycompute.Services;
using Paycompute.Models;
using Paycompute.Entity;
using System.IO;
using Microsoft.AspNetCore.Hosting.Internal;

namespace Paycompute.Controllers
{
    public class EmployeeController : Controller
    {
								private readonly IEmployeeService _employeeService;
								private readonly HostingEnvironment _hostingEnvironment;

								public EmployeeController(IEmployeeService employeeService, HostingEnvironment hostingEnvironment)
								{
												_employeeService = employeeService;
												_hostingEnvironment = hostingEnvironment;
								}

        public IActionResult Index()
        {
												var employees = _employeeService.GetAll().Select(employee => new EmployeeIndexViewModel
												{
																Id = employee.Id,
																EmployeeNo = employee.EmployeeNo,
																ImageUrl = employee.ImageUrl,
																FullName = employee.FullName,
																Gender = employee.Gender,
																Designation = employee.Designation,
																City = employee.City,
																DateJoined = employee.DateJoined
												}).ToList();
            return View(employees);
        }

								[HttpGet]
								public IActionResult Create()
								{
												var viewModel = new EmployeeCreateViewModel();
												return View(viewModel);
								}

								[HttpPost]
								[ValidateAntiForgeryToken] // Prevents cross-site Request Forgery attack
								public async Task<IActionResult> Create(EmployeeCreateViewModel model)
								{
												if (ModelState.IsValid)
												{
																var employee = new Employee
																{
																				Id = model.Id,
																				EmployeeNo = model.EmployeeNo,
																				FirstName = model.FirstName,
																				MiddleName = model.MiddleName,
																				LastName = model.LastName,
																				FullName = model.FullName,
																				Gender = model.Gender,
																				Email = model.Email,
																				DOB = model.DOB,
																				DateJoined = model.DateJoined,
																				Designation = model.Designation,
																				NationalInsuranceNo = model.NationalInsuranceNo,
																				PaymentMethod = model.PaymentMethod,
																				StudentLoan = model.StudentLoan,
																				UnionMember = model.UnionMember,
																				Address = model.Address,
																				City = model.City,
																				PhoneNumber = model.PhoneNumber,
																				PosteCode = model.PosteCode
																};
																if(model.ImageUrl != null && model.ImageUrl.Length > 0)
																{
																				var uploadDir = @"images/employee";
																				var fileName = Path.GetFileNameWithoutExtension(model.ImageUrl.FileName);
																				var extension = Path.GetExtension(model.ImageUrl.FileName);
																				var webRootPath = _hostingEnvironment.WebRootPath;

																				fileName = DateTime.UtcNow.ToString("yymmssfff") + fileName + extension;
																				var path = Path.Combine(webRootPath, uploadDir, fileName);

																				await model.ImageUrl.CopyToAsync(new FileStream(path, FileMode.Create));

																				employee.ImageUrl = "/" + uploadDir + "/" + fileName;
																}
															 await _employeeService.CreateAsync(employee);
																return RedirectToAction(nameof(Index));
												}
												return View();
								}
    }
}