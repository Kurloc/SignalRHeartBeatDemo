using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SignalRSharedModels;

namespace DashboardWebserver.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HeartBeatDataController : ControllerBase
    {

        private readonly ILogger<HeartBeatDataController> _logger;
        private readonly IHeartBeatDbModelService _heartBeatDbModelService;

        public HeartBeatDataController(ILogger<HeartBeatDataController> logger, IHeartBeatDbModelService heartBeatDbModelService)
        {
            _logger = logger;
            _heartBeatDbModelService = heartBeatDbModelService;
        }

        [HttpGet]
        public List<HeartBeatDbModel> GetLast100()
        {
            return _heartBeatDbModelService.GetLast10HeartBeats();
        }
    }
}