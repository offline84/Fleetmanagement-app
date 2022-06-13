using AutoMapper;
using Fleetmanagement_app_BLL.Repository;
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
                var bestuurderData = await _unitOfWork.Bestuurder.GetById(bestuurderDto.Rijksregisternummer);
                if (bestuurderData == null)
                {
                    return Conflict("Bestuurder not found in Database, try creating one");
                }

                if (!IsRijksregisternummer(bestuurderDto.Rijksregisternummer))
                {
                    return Conflict("Rijksregisternummer is not valid!");
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
        public static bool IsRijksregisternummer(string data)
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

        #endregion


    }
}
