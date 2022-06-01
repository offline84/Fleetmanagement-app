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
    [Route("api/"+"[controller]")]
    public class VoertuigController : ControllerBase
    {
        private readonly IUnitOfWork _repo;
        private readonly IMapper _mapper;
        private ILoggerFactory _loggerFactory = new LoggerFactory();

        public VoertuigController(IUnitOfWork repository, IMapper mapper)
        {
            _repo = repository ??
                throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

            _loggerFactory.CreateLogger("VoertuigController");
        }

        [HttpGet]
        [Route("{chassisnummer}")]
        public async Task<ActionResult<VoertuigForViewingDto>> GetVoertuigById([FromRoute] string chassisnummer)
        {
            var voertuig = await _repo.Voertuig.GetById(chassisnummer);

            return Ok(_mapper.Map<VoertuigForViewingDto>(voertuig));
        }

        [HttpGet]
        [Route("Active")]
        public async Task<ActionResult<IEnumerable<VoertuigForViewingDto>>> GetAllActiveVoertuigen()
        {
            var voertuigen = await _repo.Voertuig.GetAllActive();

            var voertuigenForView = _mapper.Map<IEnumerable<VoertuigForViewingDto>>(voertuigen);

            return Ok(voertuigenForView);
        }

        [HttpGet]
        [Route("Archived")]
        public async Task<ActionResult<IEnumerable<VoertuigForViewingDto>>> GetAllArchivedVoertuigen()
        {
            var voertuigen = await _repo.Voertuig.GetAllArchived();

            var voertuigenForView = _mapper.Map<IEnumerable<VoertuigForViewingDto>>(voertuigen);

            return Ok(voertuigenForView);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VoertuigForViewingDto>>> GetAllVoertuigen()
        {
            var voertuigen = await _repo.Voertuig.GetAll();

            var voertuigenForView = _mapper.Map<IEnumerable<VoertuigForViewingDto>>(voertuigen);

            return Ok(voertuigenForView);
        }

        [HttpGet]
        [Route("Statusses")]
        public async Task<ActionResult<IEnumerable<StatusForViewingDto>>> GetStatusses()
        {
            var seeds = await _repo.Status.GetAll();

            var view = _mapper.Map<IEnumerable<StatusForViewingDto>>(seeds);

            return Ok(view);
        }

        [HttpGet]
        [Route("Categories")]
        public async Task<ActionResult<IEnumerable<CategorieForViewingDto>>> GetCategories()
        {
            var seeds = await _repo.Categorie.GetAll();
            var view = _mapper.Map<IEnumerable<CategorieForViewingDto>>(seeds);

            return Ok(view);
        }

        [HttpGet]
        [Route("brandstoffen")]
        public async Task<ActionResult<IEnumerable<BrandstofForViewingDto>>> GetFuels()
        {
            var seeds = await _repo.Brandstof.GetAll();
             var view = _mapper.Map<IEnumerable<BrandstofForViewingDto>>(seeds);

            return Ok(view);
        }

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
        [HttpPost]
        public async Task<IActionResult> CreateVoertuig([FromBody] Voertuigbuilder voertuigBuilder)
        {
            if (ModelState.IsValid)
            {
                var voertuigInDb = await _repo.Voertuig.GetById(voertuigBuilder.Chassisnummer);
                if (voertuigInDb != null)
                    return Conflict("Voertuig already exists in Database, try updating it");

                if (!voertuigBuilder.IsValid())
                {
                    return BadRequest("Voertuig didn't meet parameters");
                }

                try
                {
                    var categorie = await _repo.Categorie.GetById(voertuigBuilder.Categorie.Id);
                    voertuigBuilder.Categorie = categorie;

                    var brandstof = await _repo.Brandstof.GetById(voertuigBuilder.Brandstof.Id);
                    voertuigBuilder.Brandstof = brandstof;

                    if (voertuigBuilder.Status != null)
                    {
                        var status = await _repo.Status.GetById(voertuigBuilder.Status.Id);
                        voertuigBuilder.Status = status;
                    }

                    var voertuig = voertuigBuilder.Build();

                    if (!voertuig.Nummerplaat.Contains("-") && voertuig.Nummerplaat.Trim() != "")
                    {
                        if (voertuig.Nummerplaat.Length <= 7 && !voertuig.Nummerplaat.StartsWith("1") && !voertuig.Nummerplaat.StartsWith("2"))
                        {
                            voertuig.Nummerplaat = voertuig.Nummerplaat.Insert(3, "-");
                        }
                        else
                        {
                            voertuig.Nummerplaat = voertuig.Nummerplaat.Insert(1, "-");
                            voertuig.Nummerplaat = voertuig.Nummerplaat.Insert(5, "-");
                        }
                    }
                    if (voertuig.Nummerplaat.Contains(" "))
                    {
                        voertuig.Nummerplaat = voertuig.Nummerplaat.Replace(" ", "-");
                    }

                    var nummerplaatcheck = _repo.Voertuig.Find(v => v.Nummerplaat == voertuig.Nummerplaat).Result.GetEnumerator();

                    if (nummerplaatcheck.MoveNext() && voertuig.Nummerplaat != "")
                    {
                        return BadRequest("Nummerplaat already exists in database.");
                    }

                    if (await _repo.Voertuig.Add(voertuig))
                    {
                        await _repo.CompleteAsync();
                        return CreatedAtAction(voertuig.Chassisnummer, _mapper.Map<VoertuigForViewingDto>(voertuig));
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

        /// <summary>
        ///     Bij het updaten wordt nagegaan of er reeds een voertuig met zelfde chassisnummer in de database zit.
        ///     Zo neen wordt de opdracht afgebroken.
        ///     Daarna wordt gekeken of het voertuig correct opgebouwd is d.m.v. de builder en worden de geneste klassen ingeladen.
        ///     Belangrijk hierbij is dat de correcte Id's van deze entiteiten gebruikt worden, anders zal men een foutmelding krijgen.
        ///     Dan pas wordt het voertuig gebouwd en toegevoegd aan de database.
        /// </summary>
        /// <remarks>
        ///     Het is echter niet mogelijk via deze weg het veld isGearchiveerd aan te passen. zie hiervoor de AdminController.
        /// </remarks>
        /// <param name="voertuigBuilder"></param>
        /// <returns>VoertuigForViewingDto voertuig + link naar voertuig</returns>
        [HttpPatch]
        [Route("update")]
        public async Task<IActionResult> UpdateVoertuig([FromBody] Voertuigbuilder voertuigBuilder)
        {
            if (ModelState.IsValid)
            {
                var voertuigInDb = await _repo.Voertuig.GetById(voertuigBuilder.Chassisnummer);
                if (voertuigInDb == null)
                    return Conflict("No match with chassisnummers in Database");

                if (!voertuigBuilder.IsValid())
                {
                    return BadRequest("Voertuig didn't meet parameters");
                }

                try
                {
                    var categorie = await _repo.Categorie.GetById(voertuigBuilder.Categorie.Id);
                    voertuigBuilder.Categorie = categorie;

                    var brandstof = await _repo.Brandstof.GetById(voertuigBuilder.Brandstof.Id);
                    voertuigBuilder.Brandstof = brandstof;

                    if (voertuigBuilder.Status != null)
                    {
                        var status = await _repo.Status.GetById(voertuigBuilder.Status.Id);
                        voertuigBuilder.Status = status;
                    }

                    var voertuig = voertuigBuilder.Build();

                    if (!voertuig.Nummerplaat.Contains("-") && voertuig.Nummerplaat != "")
                    {
                        if (voertuig.Nummerplaat.Length <= 7 && !voertuig.Nummerplaat.StartsWith("1") && !voertuig.Nummerplaat.StartsWith("2"))
                        {
                            voertuig.Nummerplaat = voertuig.Nummerplaat.Insert(3, "-");
                        }
                        else
                        {
                            voertuig.Nummerplaat = voertuig.Nummerplaat.Insert(1, "-");
                            voertuig.Nummerplaat = voertuig.Nummerplaat.Insert(5, "-");
                        }
                    }
                    if (voertuig.Nummerplaat.Contains(" "))
                    {
                        voertuig.Nummerplaat = voertuig.Nummerplaat.Replace(" ", "-");
                    }

                    var nummerplaatcheck = _repo.Voertuig.Find(v => v.Nummerplaat == voertuig.Nummerplaat).Result.GetEnumerator();

                    while (nummerplaatcheck.MoveNext())
                    {
                        if (nummerplaatcheck.Current.Chassisnummer != voertuig.Chassisnummer && voertuig.Nummerplaat != "")
                        {
                            return BadRequest("Nummerplaat already exists in database.");
                        }
                    }

                    if (await _repo.Voertuig.Update(voertuig))
                    {
                        await _repo.CompleteAsync();
                        return CreatedAtAction(voertuig.Chassisnummer, _mapper.Map<VoertuigForViewingDto>(voertuig));
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
            return StatusCode(500);
        }

        /// <summary>
        ///     De HttpDelete Archiveert het voertuig, past de laatst geupdated tijd aan en koppelt het voertuig los van de bestuurder.
        /// </summary>
        /// <param name="chassisnummer"></param>
        /// <returns>HttpStatus</returns>
        [HttpDelete]
        [Route("{chassisnummer}")]
        public async Task<IActionResult> ArchiveVoertuig([FromRoute] string chassisnummer)
        {
            try
            {
                var voertuigInDb = await _repo.Voertuig.GetById(chassisnummer);

                if (voertuigInDb.IsGearchiveerd)
                    return Conflict("Voertuig already archived in Database");

                if (await _repo.Voertuig.Delete(chassisnummer))
                {
                    await _repo.CompleteAsync();

                    var k = await _repo.Koppeling.GetByvoertuig(chassisnummer);
                    if(await _repo.Koppeling.GetByvoertuig(chassisnummer) != null)
                    {
                        await _repo.Koppeling.KoppelLosVoertuig(chassisnummer);
                        await _repo.CompleteAsync();
                    }
                    
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

        /// <summary>
        /// Koppel de Bestuurder met het Voertuig
        /// </summary>
        /// <param name="bestuurderRRN"></param>
        /// <param name="chassisnummer"></param>
        /// <returns></returns>
        [Route("koppel/{bestuurderRRN}/{chassisnummer}")]
        [HttpPatch(Name = "KoppelAanVoertuig")]
        public async Task<IActionResult> KoppelAanVoertuig([FromRoute] string bestuurderRRN, string chassisnummer)
        {
            try
            {
                if (_repo.Koppeling.VoertuigAlGekoppeldAanAndereBestuurder(bestuurderRRN, chassisnummer))
                {
                    return Conflict("Tankkaart already linked in Database, decouple it first");
                }

                if (_repo.Koppeling.BestuurderAlGekoppeldAanEenVoertuig(bestuurderRRN))
                {
                    return Conflict("Bestuurder already linked in Database, decouple it first");
                }

                if (await _repo.Koppeling.KoppelBestuurderEnVoertuig(bestuurderRRN, chassisnummer))
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

        /// <summary>
        /// Koppel het Voertuig los van de bestuurder. Geen effect als het Voertuig niet gekoppeld is
        /// </summary>
        /// <param name="chassisnummer"></param>
        /// <returns></returns>
        [Route("koppellos/{chassisnummer}")]
        [HttpPatch(Name = "KoppelVoertuigLosVanBestuurder")]
        public async Task<IActionResult> KoppelVoertuigLosVanBestuurder([FromRoute] string chassisnummer)
        {
            try
            {
                if (_repo.Koppeling.KoppelingMetVoertuigBestaatNiet(chassisnummer))
                {
                    return Conflict("Voertuig is niet gelinkt met een bestuurder");
                }
                if (await _repo.Koppeling.KoppelLosVoertuig(chassisnummer))
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