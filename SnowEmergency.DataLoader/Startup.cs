using SnowEmergency.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnowEmergency.DataLoader
{
    public class StartUp
    {
        private readonly IWorkerService _workerService;

        public StartUp(IWorkerService workerService)
        {
            _workerService = workerService;
        }

        public void Run(string[] args)
        {
            Task task = _workerService.ProcessSnowEmergency();
            task.Wait();
        }

    }
}
