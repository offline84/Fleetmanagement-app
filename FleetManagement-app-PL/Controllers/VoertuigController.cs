using Fleetmanagement_app_BLL.Repository;
using Fleetmanagement_app_BLL.UnitOfWork;
using Fleetmanagement_app_DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FleetManagement_app_PL
{
    [Route("voertuig")]
    [ApiController]
    class VoertuigController : ControllerBase
    {
        private IUnitOfWork _repo;
        private ILoggerFactory _loggerFactory = new LoggerFactory();
        
        public VoertuigController(UnitOfWork repository)
        {
            _repo = repository??
                throw new ArgumentNullException(nameof(VoertuigRepository));
            _loggerFactory.CreateLogger("VoertuigController");
        }

        [HttpGet]
        public ActionResult<string> GetAllCustomers()
        {
            return Ok("Hello world");
        }

    }
}
