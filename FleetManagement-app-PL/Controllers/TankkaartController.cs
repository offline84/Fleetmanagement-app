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

        /// <summary>
        /// Geeft de gevraagde tankkaart terug
        /// </summary>
        /// <param name="kaartnummer">Kaartnummer van de tankkaart</param>
        /// <returns>Tankkaart</returns>
        [HttpGet(Name = "GetTankkaartById")]
        [Route("{kaartnummer}")]
        public async Task<ActionResult<TankkaartForViewingDto>> GetTankkaartById([FromRoute] string kaartnummer)
        {
            var tankkaart = await _repo.Tankkaart.GetById(kaartnummer);

            return Ok(_mapper.Map<TankkaartForViewingDto>(tankkaart));
        }

        /// <summary>
        /// Geeft alle tankkaarten terug
        /// </summary>
        /// <returns>Tankkaarten</returns>
        [HttpGet(Name = "GetAllTankkaarten")]
        [Route("All")]
        public async Task<ActionResult<IEnumerable<TankkaartForViewingDto>>> GetAllTankkaarten([FromRoute] string kaartnummer)
        {
            var tankkaarten = await _repo.Tankkaart.GetAll();
            var tankkaartenForView = _mapper.Map<IEnumerable<TankkaartForViewingDto>>(tankkaarten);
            return Ok(tankkaartenForView);
        }

        /// <summary>
        /// Geeft alle active tankkaarten terug
        /// </summary>
        /// <returns>Tankkaarten</returns>
        [HttpGet(Name = "GetAllActiveTankkaarten")]
        [Route("Active")]
        public async Task<ActionResult<IEnumerable<TankkaartForViewingDto>>> GetAllActiveTankkaarten()
        {
            var tankkaarten = await _repo.Tankkaart.GetAllActive();
            var tankkaartenForView = _mapper.Map<IEnumerable<TankkaartForViewingDto>>(tankkaarten);
            return Ok(tankkaartenForView);
        }

        /// <summary>
        /// Geeft alle gearchiveerde tankkaarten terug
        /// </summary>
        /// <returns>Tankkaarten</returns>
        [HttpGet(Name = "GetAllArchivedTankkaarten")]
        [Route("Archived")]
        public async Task<ActionResult<IEnumerable<TankkaartForViewingDto>>> GetAllArchivedTankkaarten()
        {
            var tankkaarten = await _repo.Tankkaart.GetAllArchived();
            var tankkaartenForView = _mapper.Map<IEnumerable<TankkaartForViewingDto>>(tankkaarten);
            return Ok(tankkaartenForView);
        }

        /// <summary>
        /// Geeft alle brandstoffen terug
        /// </summary>
        /// <returns>Tankkaarten</returns>
        [HttpGet(Name = "GetFuels")]
        [Route("brandstoffen")]
        public async Task<ActionResult<IEnumerable<Status>>> GetFuels()
        {
            var brandstoffen = await _repo.Brandstof.GetAll();
            return Ok(brandstoffen);
        }
        /*
        /// <summary>
        ///     Bij creatie wordt nagegaan of er reeds een voertuig met zelfde chassisnummer in de database zit.
        ///     Zo ja wordt de opdracht afgebroken.
        ///     Daarna wordt gekeken of het voertuig correct opgebouwd is d.m.v. de builder en worden de geneste klassen ingeladen.
        ///     Belangrijk hierbij is dat de correcte Id's van deze entiteiten gebruikt worden, anders zal men een foutmelding krijgen.
        ///     Dan wordt het voertuig gebouwd en en wordt er gecontroleerd of de nummerplaat niet aanwezig is in de database in een andere schrijfwijze.
        ///     Dan pas wordt het voertuig toegevoegd aan de database.
        /// </summary>
        /// <param name="voertuigBuilder"></param>
        /// <returns>VoertuigForViewingDto voertuig + link naar voertuig</returns>
        [HttpPost] //[HttpPut] ?
        public async Task<IActionResult> CreateTankkaart([FromBody] Tankkaart tankkaart)
        {
            if (ModelState.IsValid)
            {
                //Al in repo. Hier ook?
                if (tankkaart.GeldigheidsDatum == null | tankkaart.Kaartnummer == "")
                {
                    return BadRequest("Tankaart kaartnummer en Geldigheidsdatum zijn verplicht");
                }

                var tankkakaartinBD = await _repo.Tankkaart.GetById(tankkaart.Kaartnummer);
                if (tankkakaartinBD != null)
                {
                    return Conflict("Tankkaart already exists in Database, try updating it");
                }

                try
                {
                    //DateTime geldigheidsDatum = DateTime.Parse(geldigheidsDatumString);

                    if (await _repo.Tankkaart.Add(tankkaart))
                    {
                        await _repo.CompleteAsync();
                        return CreatedAtAction(tankkaart.Kaartnummer, _mapper.Map<TankkaartForViewingDto>(tankkaart));
                    }
                    else
                    {
                        return BadRequest("Unable to Write to Database");
                    }
                }
                catch (Exception e)
                {
                    return StatusCode(500, e);
                }
            }
            return StatusCode(500);
        }

        [HttpPost] //[HttpPut] ?
        public async Task<IActionResult> UpdateTankkaart([FromBody] Voertuigbuilder voertuigBuilder)
        {
            throw new NotImplementedException();
        }
        */
        [HttpDelete(Name = "DeleteTankkaart")]
        [Route("{kaartnummer}")]
        public async Task<IActionResult> DeleteTankkaart([FromRoute] string kaartnummer)
        {
            try
            {
                var tankkaartinDB = await _repo.Tankkaart.GetById(kaartnummer);

                if (tankkaartinDB.IsGearchiveerd)
                {
                    return Conflict("Tankkaart already archived in Database");
                }

                if (await _repo.Tankkaart.Delete(kaartnummer))
                {
                    await _repo.Koppeling.KoppelLosVanTankkaart(kaartnummer);
                    await _repo.CompleteAsync();
                    return NoContent();
                }
                else
                {
                    _repo.Dispose();
                    return BadRequest("Unable to Write to Database");
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        [HttpPatch(Name = "BlokkeerTankkaart")]
        [Route("blokeer/{kaartnummer}")]
        public async Task<IActionResult> BlokkeerTankkaart([FromRoute] string kaartnummer)
        {
            try
            {
                var tankkaartinDB = await _repo.Tankkaart.GetById(kaartnummer);

                if (tankkaartinDB.IsGeblokkeerd)
                {
                    return Conflict("Tankkaart already blocked in Database");
                }

                if (await _repo.Tankkaart.Blokkeren(kaartnummer))
                {
                    await _repo.CompleteAsync();
                    return NoContent();
                }
                else
                {
                    _repo.Dispose();
                    return BadRequest("Unable to Write to Database");
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        //Hier of in eigen Controller?
        [HttpPatch(Name = "KoppelAanBestuurder")]
        [Route("koppel/{bestuurderRRN}/{kaartnummer}")]
        public async Task<IActionResult> KoppelAanBestuurder([FromRoute] string bestuurderRRN, string kaartnummer)
        {
            try
            {
                var koppelingInDB = await _repo.Koppeling.GetByTankkaart(kaartnummer);

                if (koppelingInDB != null)
                {
                    return Conflict("Tankkaart already linked in Database");
                }

                //TODO: check if bestuurder excists? Already done in Repo

                if (await _repo.Koppeling.KoppelAanTankkaart(bestuurderRRN, kaartnummer))
                {
                    await _repo.CompleteAsync();
                    return NoContent();
                }
                else
                {
                    _repo.Dispose();
                    return BadRequest("Unable to Write to Database");
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }
    }
}
