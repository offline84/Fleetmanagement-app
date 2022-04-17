using Fleetmanagement_app_BLL.Repository;
using Fleetmanagement_app_BLL.UnitOfWork;
using Fleetmanagement_app_DAL.Database;
using Fleetmanagement_app_DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FleetManagement_app_PL.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VoertuigController : ControllerBase
    {
        private IUnitOfWork _repo;
        private ILoggerFactory _loggerFactory = new LoggerFactory();
        private FleetmanagerContext _context;
        
        public VoertuigController(IUnitOfWork repository)
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
