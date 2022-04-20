using Fleetmanagement_app_BLL.Repository;
using Fleetmanagement_app_BLL.UnitOfWork;
using Fleetmanagement_app_DAL.Builders;
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
        
        public VoertuigController(IUnitOfWork repository)
        {
            _repo = repository??
                throw new ArgumentNullException(nameof(VoertuigRepository));
            _loggerFactory.CreateLogger("VoertuigController");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Voertuig>>> GetAllActiveVoertuigen()
        {
            var voertuigen = await _repo.Voertuig.GetAllActive();

            return Ok(voertuigen);
        }

        [HttpPost]
        public async Task<IActionResult> CreateVoertuig([FromBody] Voertuigbuilder voertuigBuilder)
        {
            if (ModelState.IsValid)
            {
                if (!voertuigBuilder.IsValid())
                {
                    return BadRequest("Voertuig didn't meet parameters");
                }

                var categorie = await _repo.Categorie.GetById(voertuigBuilder.Categorie.Id);
                voertuigBuilder.Categorie = categorie;

                var brandstof = await _repo.Brandstof.GetById(voertuigBuilder.Brandstof.Id);
                voertuigBuilder.Brandstof = brandstof;

                if(voertuigBuilder.Status != null)
                {
                    var status = await _repo.Status.GetById(voertuigBuilder.Status.Id);
                    voertuigBuilder.Status = status;
                }

                var voertuig = voertuigBuilder.Build();
                

                await _repo.Voertuig.Add(voertuig);
                await _repo.CompleteAsync();

                return CreatedAtAction("GetVoertuig", new {voertuig.Chassisnummer}, voertuig);
            }
            return StatusCode(500);
        }

    }
}
