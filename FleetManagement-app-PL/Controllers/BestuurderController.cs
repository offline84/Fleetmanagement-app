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
    [Route("[controller]")]
    public class BestuurderController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private ILoggerFactory _loggerFactory = new LoggerFactory();

        public BestuurderController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ??
                throw new ArgumentNullException(nameof(BestuurderRepository));
            _loggerFactory.CreateLogger("BestuurderController");
            _mapper = mapper;
        }

        #region Getters

        /// <summary>
        /// Haalt alle bestuurders en rijbewijzen op vooralle bestuurders.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BestuurderViewingDto>>> GetAllBestuurders()
        {
            try
            {
                var bestuurders = await _unitOfWork.Bestuurder.GetAll();
                var rijbewijzen = await _unitOfWork.Rijbewijs.GetAll();

                foreach (var bestuurder in bestuurders)
                {
                    bestuurder.Rijbewijzen = (ICollection<Rijbewijs>)rijbewijzen;
                }

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
        /// Haalt alle actieve bestuurders en rijbewijzen op vooralle bestuurders.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("ActiveBestuurders")]
        public async Task<ActionResult<IEnumerable<BestuurderViewingDto>>> GetAllActiveBestuurders()
        {
            try
            {
                var bestuurders = await _unitOfWork.Bestuurder.GetAllActive();
                var rijbewijzen = await _unitOfWork.Rijbewijs.GetAll();

                foreach (var bestuurder in bestuurders)
                {
                    bestuurder.Rijbewijzen = (ICollection<Rijbewijs>)rijbewijzen;
                }

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
                var rijbewijzen = await _unitOfWork.Rijbewijs.GetAll();

                foreach (var bestuurder in bestuurders)
                {
                    bestuurder.Rijbewijzen = (ICollection<Rijbewijs>)rijbewijzen;
                }

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
        /// Haal een bestuurder op met behulp van zijn rijksregisternummer.
        /// </summary>
        /// <param name="rijksregisternummer"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{rijksregisternummer}")]
        public async Task<ActionResult<BestuurderViewingDto>> GetDriverById([FromRoute] string rijksregisternummer)
        {
            try
            {
                var driver = await _unitOfWork.Bestuurder.GetById(rijksregisternummer);
                var result = _mapper.Map<BestuurderViewingDto>(driver);
                return Ok(result);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
                return StatusCode(500, ex);
            }
        }

        //[HttpGet]
        //[Route("{rijksregisternummer}")]
        //public ActionResult<List<RijbewijsViewingDto>> GetDriverLicensesForDriver(string rijksregisternummer)
        //{
        //    try
        //    {
        //        var driverLicenses = _unitOfWork.Bestuurder.GetDriverLicensesForDriver(rijksregisternummer);
        //        return _mapper.Map<List<RijbewijsViewingDto>>(driverLicenses);
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError("Error", ex.Message);
        //        return new List<RijbewijsViewingDto>();
        //    }
        //}

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

                if (IsRijksregisternummer(bestuurderDto.Rijksregisternummer))
                {
                    return Conflict("Rijksregisternummer is not valid!");
                }

                try
                {
                    var bestuurder = _mapper.Map<Bestuurder>(bestuurderDto);
                    await _unitOfWork.Bestuurder.Add(bestuurder);

                    if (await _unitOfWork.Bestuurder.Add(bestuurder))
                    {
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
        public async Task<IActionResult> UpdateVoertuig([FromBody] BestuurderViewingDto bestuurderDto)
        {
            if (ModelState.IsValid)
            {
                var bestuurderData = await _unitOfWork.Bestuurder.GetById(bestuurderDto.Rijksregisternummer);
                if (bestuurderData != null)
                {
                    return Conflict("Bestuurder not found in Database, try creating one");
                }

                if (IsRijksregisternummer(bestuurderDto.Rijksregisternummer))
                {
                    return Conflict("Rijksregisternummer is not valid!");
                }

                try
                {
                    var bestuurder = _mapper.Map<Bestuurder>(bestuurderDto);
                    await _unitOfWork.Bestuurder.Add(bestuurder);

                    if (await _unitOfWork.Bestuurder.Add(bestuurder))
                    {
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
