using AutoMapper;
using Fleetmanagement_app_BLL.UnitOfWork;
using Fleetmanagement_app_DAL.Builders;
using Fleetmanagement_app_DAL.Entities;
using FleetManagement_app_PL.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace FleetManagement_app_PL.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TankkaartController : ControllerBase
    {
        private readonly IUnitOfWork _repo;
        private readonly IMapper _mapper;
        private ILoggerFactory _loggerFactory = new LoggerFactory();

        public TankkaartController(IUnitOfWork repository, IMapper mapper)
        {
            _repo = repository ??
                throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

            _loggerFactory.CreateLogger("TankkaartController");
        }

        [HttpGet]
        [Route("{kaartnummer}")]
        public async Task<ActionResult<TankkaartForViewingDto>> GetTankkaartById([FromRoute] string kaartnummer)
        {
            var tankkaart = await _repo.Tankkaart.GetById(kaartnummer);

            return Ok(_mapper.Map<TankkaartForViewingDto>(tankkaart));
        }

    }
}
