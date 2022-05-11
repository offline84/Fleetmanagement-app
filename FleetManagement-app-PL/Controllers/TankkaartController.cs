using AutoMapper;
using Fleetmanagement_app_BLL.UnitOfWork;
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

        /// <summary>
        ///     Bij aanmaak wordt nagegaan of er al een tankkaart met hetzelfde kaartnummer in de DB zit.
        ///     Vervolgens wordt nagegaan of de verplichte velden kaartnummer en Geldigheidsdatum aanwezig zijn.
        /// </summary>
        /// <param name="tankkaartDto"></param>
        /// <returns>Kaartnummer + TankkaartForViewingDto</returns>
        [HttpPost]
        public async Task<IActionResult> CreateTankkaart([FromBody] TankkaartForViewingDto tankkaartDto)
        {
            if (ModelState.IsValid)
            {
                var tankkakaartinBD = await _repo.Tankkaart.GetById(tankkaartDto.Kaartnummer);
                if (tankkakaartinBD != null)
                {
                    return Conflict("Tankkaart already exists in Database, try updating it");
                }

                if (VerplichteVeldenLeeg(tankkaartDto.Kaartnummer, tankkaartDto.GeldigheidsDatum))
                {
                    return BadRequest("Tankaart kaartnummer en Geldigheidsdatum zijn verplicht");
                }

                try
                {
                    var tankkaart = _mapper.Map<Tankkaart>(tankkaartDto);

                    if (await _repo.Tankkaart.Add(tankkaart))
                    {
                        await _repo.CompleteAsync();
                        return CreatedAtAction(tankkaart.Kaartnummer, tankkaartDto);
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

        [HttpPatch] //[HttpPut] ?
        [Route("update")]
        public async Task<IActionResult> UpdateTankkaart([FromBody] TankkaartForViewingDto tankkaartDto)
        {
            if (ModelState.IsValid)
            {
                var tankkakaartinBD = await _repo.Tankkaart.GetById(tankkaartDto.Kaartnummer);
                if (tankkakaartinBD == null)
                {
                    return Conflict("Tankkaart does not exists in Database, try updating it");
                }

                if (VerplichteVeldenLeeg(tankkaartDto.Kaartnummer, tankkaartDto.GeldigheidsDatum))
                {
                    return BadRequest("Tankaart kaartnummer en Geldigheidsdatum zijn verplicht");
                }

                try
                {
                    var tankkaart = _mapper.Map<Tankkaart>(tankkaartDto);
                    if (await _repo.Tankkaart.Update(tankkaart))
                    {
                        await _repo.CompleteAsync();
                        return CreatedAtAction(tankkaart.Kaartnummer, tankkaartDto);
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

        
        [HttpDelete(Name = "DeleteTankkaart")]
        [Route("delete/{kaartnummer}")]
        public async Task<IActionResult> DeleteTankkaart([FromRoute] string kaartnummer)
        {
            try
            {
                var tankkaartinDB = await _repo.Tankkaart.GetById(kaartnummer);

                if (tankkaartinDB == null)
                {
                    return Conflict("Tankkaart does not exists in Database");
                }

                if (tankkaartinDB.IsGearchiveerd)
                {
                    return Conflict("Tankkaart already archived in Database");
                }

                if (await _repo.Tankkaart.Delete(kaartnummer))
                {
                    await _repo.Koppeling.KoppelLosTankkaart(kaartnummer);
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

                if (tankkaartinDB == null)
                {
                    return Conflict("Tankkaart does not exists in Database");
                }

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

        [HttpPatch(Name = "KoppelAanBestuurder")]
        [Route("koppel/{bestuurderRRN}/{kaartnummer}")]
        public async Task<IActionResult> KoppelAanBestuurder([FromRoute] string bestuurderRRN, string kaartnummer)
        {
            try
            {
                if (_repo.Koppeling.TankkaartAlGekoppeldAanAndereBestuurder(bestuurderRRN, kaartnummer))
                {
                    return Conflict("Tankkaart already linked in Database, decouple it first");
                }

                if (_repo.Koppeling.BestuurderAlGekoppeldAanEenTankkaart(bestuurderRRN))
                {
                    return Conflict("Bestuurder already linked in Database, decouple it first");
                }

                if (await _repo.Koppeling.KoppelBestuurderEnTankkaart(bestuurderRRN, kaartnummer))
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

        private bool VerplichteVeldenLeeg(string kaartnummer, DateTime geldigheidsDatum)
        {
            return (geldigheidsDatum == default | kaartnummer == "");
        }
    }
}
