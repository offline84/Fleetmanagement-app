using AutoMapper;
using Fleetmanagement_app_BLL.Repository;
using Fleetmanagement_app_BLL.UnitOfWork;
using Fleetmanagement_app_DAL.Entities;
using FleetManagement_app_PL.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FleetManagement_app_PL.Controllers
{
    [ApiController]
    [Route("api/" + "[controller]")]
    public class BestuurderController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILoggerFactory _loggerFactory = new LoggerFactory();

        public BestuurderController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ??
                throw new ArgumentNullException(nameof(BestuurderRepository));
            _mapper = mapper ??
              throw new ArgumentNullException(nameof(mapper));
            _loggerFactory.CreateLogger("BestuurderController");
        }

        #region Getters

        /// <summary>
        /// Haalt alle bestuurders op vooralle bestuurders.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BestuurderViewingDto>>> GetAllBestuurders()
        {
            try
            {
                var bestuurders = await _unitOfWork.Bestuurder.GetAll();
                var listResult = _mapper.Map<List<BestuurderViewingDto>>(bestuurders);
                return Ok(listResult);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
                return StatusCode(500, ex);
            }
        }

        /// <summary>
        /// Haalt alle actieve bestuurders op vooralle bestuurders.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Active")]
        public async Task<ActionResult<IEnumerable<BestuurderViewingDto>>> GetAllActiveBestuurders()
        {
            try
            {
                var bestuurders = await _unitOfWork.Bestuurder.GetAllActive();
                var listResult = _mapper.Map<List<BestuurderViewingDto>>(bestuurders);
                return Ok(listResult);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
                return StatusCode(500, ex);
            }
        }

        /// <summary>
        /// Haalt alle niet actieve bestuurders op, beter gezegd gearchiveerd bestuurders.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("ArchivedBestuurders")]
        public async Task<ActionResult<IEnumerable<BestuurderViewingDto>>> GetAllArchivedBestuurders()
        {
            try
            {
                var bestuurders = await _unitOfWork.Bestuurder.GetAllArchived();
                var listResult = _mapper.Map<List<BestuurderViewingDto>>(bestuurders);
                return Ok(listResult);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
                return StatusCode(500, ex);
            }
        }

        [HttpGet]
        [Route("Rijbewijzen")]
        public async Task<ActionResult<IEnumerable<RijbewijsViewingDto>>> GetRijbewijzen()
        {
            var seeds = await _unitOfWork.Rijbewijs.GetAll();
            var view = _mapper.Map<IEnumerable<RijbewijsViewingDto>>(seeds);

            return Ok(view);
        }

        /// <summary>
        /// Haal een bestuurder op met behulp van zijn rijksregisternummer.
        /// </summary>
        /// <param name="rijksregisternummer"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{rijksregisternummer}")]
        public async Task<ActionResult<BestuurderViewingDto>> GetBestuurderById([FromRoute] string rijksregisternummer)
        {
            try
            {
                var bestuurder = await _unitOfWork.Bestuurder.GetById(rijksregisternummer);
                var result = _mapper.Map<BestuurderViewingDto>(bestuurder);
                return Ok(result);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
                return StatusCode(500, ex);
            }
        }

        [HttpGet]
        [Route("voertuig")]
        public async Task<ActionResult<IEnumerable<BestuurderViewingDto>>> GetKoppelbareBestuurdersAanVoertuig([FromQuery] string chassisnummer, [FromQuery] string typeBrandstof)
        {
            try
            {
                var bestuurders = await _unitOfWork.Bestuurder.GetAllActive();
                var tankkaarten = await _unitOfWork.Tankkaart.GetAllActive();

                var filteredListOfBestuurders = bestuurders.Where(b => ((b.Koppeling.Chassisnummer == null || b.Koppeling.Chassisnummer == chassisnummer) && b.Koppeling.Kaartnummer == null)).ToList();
                var listToFilter = bestuurders.Where(b => ((b.Koppeling.Chassisnummer == null || b.Koppeling.Chassisnummer == chassisnummer) && b.Koppeling.Kaartnummer != null)).ToList();
                
                if (listToFilter != null)
                {
                    foreach (var bestuurder in listToFilter)
                    {
                        var tankkaart = tankkaarten.Where(t => t.Kaartnummer == bestuurder.Koppeling.Kaartnummer).Single();
                        if (tankkaart.MogelijkeBrandstoffen.Any(t => t.Brandstof.TypeBrandstof == typeBrandstof))
                        {
                            filteredListOfBestuurders.Add(bestuurder);
                        }
                    }
                }

                var result = _mapper.Map<IEnumerable<BestuurderViewingDto>>(filteredListOfBestuurders);
                return Ok(result);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
                return StatusCode(500, ex);
            }
        }

        [HttpGet]
        [Route("tankkaart")]
         public async Task<ActionResult<IEnumerable<BestuurderViewingDto>>> GetKoppelbareBestuurdersAanTankkaart([FromQuery] string kaartnummer)
        {
           try
            {
                var bestuurders = await _unitOfWork.Bestuurder.GetAllActive();
                var voertuigen = await _unitOfWork.Voertuig.GetAllActive();
                var tankkaart = await _unitOfWork.Tankkaart.GetById(kaartnummer);

                var filteredListOfBestuurders = bestuurders.Where(b => ((b.Koppeling.Kaartnummer == null || b.Koppeling.Kaartnummer == kaartnummer) && b.Koppeling.Chassisnummer == null)).ToList();
                var listToFilter = bestuurders.Where(b => ((b.Koppeling.Kaartnummer == null || b.Koppeling.Kaartnummer == kaartnummer) && b.Koppeling.Chassisnummer != null)).ToList();
                
                if (listToFilter != null)
                {
                    foreach (var bestuurder in listToFilter)
                    {
                        var voertuig = voertuigen.Where(t => t.Chassisnummer == bestuurder.Koppeling.Chassisnummer).Single();
                        if(tankkaart.MogelijkeBrandstoffen.Any(t => t.Brandstof.TypeBrandstof == voertuig.Brandstof.TypeBrandstof))
                        {
                            filteredListOfBestuurders.Add(bestuurder);
                        }
                        
                    }
                }

                var result = _mapper.Map<IEnumerable<BestuurderViewingDto>>(filteredListOfBestuurders);
                return Ok(result);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
                return StatusCode(500, ex);
            }
        }


        #endregion

        #region POST/PUT/PATCH

        /// <summary>
        ///   Bij creatie wordt er gechekt of er een item wordt gevonden met behulp van zijn rijksregisternummer, indien hij niet gevonden wordt kan de flow van creatie verder.
        ///   Anders wordt er een foutmelding gegooid.
        /// </summary>
        /// <param name="bestuurderDto"></param>
        /// <returns>BestuurderViewingDto bestuurderDto + link naar bestuurder</returns>
        [HttpPost]
        public async Task<IActionResult> CreateBestuurder([FromBody] BestuurderViewingDto bestuurderDto)
        {
            if (ModelState.IsValid)
            {
                var bestuurderData = await _unitOfWork.Bestuurder.GetById(bestuurderDto.Rijksregisternummer);
                if (bestuurderData != null)
                {
                    return Conflict("Bestuurder already exists in Database, try updating it");
                }

                if (!IsRijksregisternummer(bestuurderDto.Rijksregisternummer))
                {
                    return Conflict("Rijksregisternummer is not valid!");
                }

                if (!IsGeboortedatumGelijk(bestuurderDto.GeboorteDatum, bestuurderDto.Rijksregisternummer))
                {
                    return Conflict("Geboortedatum does not correspond to Rijksregisternummer!");
                }

                try
                {
                    var bestuurder = _mapper.Map<Bestuurder>(bestuurderDto);
                    if (await _unitOfWork.Bestuurder.Add(bestuurder))
                    {
                        await _unitOfWork.CompleteAsync();

                        await _unitOfWork.Koppeling.CreateKoppeling(bestuurder.Rijksregisternummer);

                        await _unitOfWork.CompleteAsync();

                        return CreatedAtAction(bestuurder.Rijksregisternummer, bestuurderDto);
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
        ///   Bij update wordt er gechekt of er een item wordt gevonden met behulp van zijn rijksregisternummer, indien hij gevonden wordt kan de flow van update verder.
        ///   Anders wordt er een foutmelding gegooid.
        /// </summary>
        /// <remarks>
        ///     Het is echter niet mogelijk via deze weg het veld isGearchiveerd aan te passen. zie hiervoor de AdminController.
        /// </remarks>
        /// <param name="bestuurderDto"></param>
        /// <returns>BestuurderViewingDto bestuurderDto + link naar bestuurder</returns>
        [HttpPatch]
        [Route("update")]
        public async Task<IActionResult> UpdateBestuurder([FromBody] BestuurderViewingDto bestuurderDto)
        {
            if (ModelState.IsValid)
            {
                var bestuurderData = await _unitOfWork.Bestuurder.GetByIdNoTracking(bestuurderDto.Rijksregisternummer);
                if (bestuurderData == null)
                {
                    return Conflict("Bestuurder not found in Database, try creating one");
                }

                if (!IsRijksregisternummer(bestuurderDto.Rijksregisternummer))
                {
                    return Conflict("Rijksregisternummer is not valid!");
                }

                if (!IsGeboortedatumGelijk(bestuurderDto.GeboorteDatum, bestuurderDto.Rijksregisternummer))
                {
                    return Conflict("Geboortedatum does not correspond to Rijksregisternummer!");
                }
               
                try
                {
                    var bestuurder = _mapper.Map<Bestuurder>(bestuurderDto);
                    bestuurder.Koppeling = bestuurderData.Koppeling;
                    if (await _unitOfWork.Bestuurder.Update(bestuurder))
                    {
                        await _unitOfWork.CompleteAsync();
                        bestuurder = await _unitOfWork.Bestuurder.GetById(bestuurderDto.Rijksregisternummer);
                        bestuurderDto = _mapper.Map<BestuurderViewingDto>(bestuurder);
                        return CreatedAtAction(bestuurder.Rijksregisternummer, bestuurderDto);
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
        ///   Bij delete wordt er gechekt of er een item wordt gevonden met behulp van zijn rijksregisternummer, indien hij gevonden wordt kan de flow van delete verder.
        ///   Anders wordt er een foutmelding gegooid.
        /// </summary>
        /// <param name="rijksRegisternummer"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{rijksRegisternummer}")]
        public async Task<IActionResult> ArchiveBestuurder([FromRoute] string rijksRegisternummer)
        {
            var bestuurderData = await _unitOfWork.Bestuurder.GetById(rijksRegisternummer);
            if (bestuurderData == null)
            {
                return Conflict("Bestuurder not found in Database, try creating one");
            }

            if (bestuurderData.IsGearchiveerd)
            {
                return Conflict("Bestuurder already archived in Database");
            }

            try
            {
                if (await _unitOfWork.Bestuurder.Delete(rijksRegisternummer))
                {
                    await _unitOfWork.Koppeling.KoppelLosBestuurder(rijksRegisternummer);
                    await _unitOfWork.CompleteAsync();
                    return NoContent();
                }
                else
                {
                    _unitOfWork.Dispose();
                    return BadRequest("Unable to Write to Database");
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        /// <summary>
        /// Koppel de bestuurder los van het Voertuig en de tankkaart. Geen effect als de bestuurder niet gekoppeld is
        /// </summary>
        /// <param name="rijksRegisternummer"></param>
        /// <returns></returns>
        [HttpPatch(Name = "KoppelLosBestuurder")]
        [Route("koppelLos/{rijksRegisternummer}")]
        public async Task<IActionResult> KoppelLosBestuurder([FromRoute] string rijksRegisternummer)
        {
            try
            {
                if (await _unitOfWork.Koppeling.KoppelLosBestuurder(rijksRegisternummer))
                {
                    await _unitOfWork.CompleteAsync();
                    return NoContent();
                }
                else
                {
                    _unitOfWork.Dispose();
                    return BadRequest("Unable to Write to Database");
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        #endregion

        #region private

        /// <summary>
        /// Return true indien het over een geldige rijksregisternummer gaat
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal bool IsRijksregisternummer(string data)
        {
            try
            {
                if (data == null) return false;
                if (data.Length != 11) return false;
                string leftPartS = data.Substring(0, 9);
                string rightPartS = data.Substring(9);

                int leftPartI = int.Parse(leftPartS);
                int rightPartI = int.Parse(rightPartS);

                // Special check for children of 21st century
                int year = DateTime.Today.Year - 2000;
                if (leftPartI < year * 10000000)
                {
                    leftPartI += 2000000000;
                }

                int mod = leftPartI % 97;
                int compareTo = 97 - mod;
                return compareTo == rightPartI;
            }
            catch (ApplicationException)
            {
                return false;
            }
        }

        /// <summary>
        /// Return true indien het geboortedatum overeenkomt met rijksregisternummer
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal bool IsGeboortedatumGelijk(DateTime geboortedatum, string rijksregisternr)
        {
            try
            {
                if (rijksregisternr.Length == 11)
                {
                    var datum = rijksregisternr.Substring(0, 6);
                    var jaar2 = datum.Substring(0, 2);
                    var maand = datum.Substring(2, 2);
                    var dag = datum.Substring(4, 2);
                    var jaar1 = int.Parse(jaar2) - 100 > 0 ? "20":"19";
                    datum = $"{dag}-{maand}-{jaar1}{jaar2}";
                    if(datum.ToString() != geboortedatum.ToString("dd/MM/yyyy"))
                    {
                        return false;
                    }
                    return true;
                }
                return false;

            }
            catch (ApplicationException)
            {
                return false;
            }
        }
        #endregion


    }
}
