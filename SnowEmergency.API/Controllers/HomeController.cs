using Microsoft.AspNetCore.Mvc;
using SnowEmergency.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SnowEmergency.API.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWorkerService _workerService;

        public HomeController(IWorkerService workerService)
        {
            _workerService = workerService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _workerService.IsThereASnowEmergency();
            return View(model);
        }
    }
}
