using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
            return View();
        }
    }
}