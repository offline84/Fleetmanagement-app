using AutoMapper;
using Fleetmanagement_app_BLL.Repository;
using Fleetmanagement_app_BLL.UnitOfWork;
using Fleetmanagement_app_DAL.Database;
using FleetManagement_app_PL.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace FleetManagement_app_PL.Controllers
{
    [ApiController]
     [Route("api/"+"[controller]")]
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
        /// Haalt alle bestuurders op.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<BestuurderViewingDto>> GetAllBestuurders()
        {
            try
            {
                var bestuurders = _unitOfWork.Bestuurder.GetAll();
                return _mapper.Map<List<BestuurderViewingDto>>(bestuurders);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
                return new List<BestuurderViewingDto>();
            }
        }

        /// <summary>
        /// Haalt alle actieve bestuurders op.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("ActiveBestuurders")]
        public ActionResult<IEnumerable<BestuurderViewingDto>> GetAllActiveBestuurders()
        {
            try
            {
                var bestuurders = _unitOfWork.Bestuurder.GetAllActive();
                return _mapper.Map<List<BestuurderViewingDto>>(bestuurders);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
                return new List<BestuurderViewingDto>();
            }
        }

        /// <summary>
        /// Haalt alle niet actieve bestuurders op, beter gezegd gearchiveerd bestuurders.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("ArchivedBestuurders")]
        public ActionResult<IEnumerable<BestuurderViewingDto>> GetAllArchivedBestuurders()
        {
            try
            {
                var bestuurders = _unitOfWork.Bestuurder.GetAllArchived();
                return _mapper.Map<List<BestuurderViewingDto>>(bestuurders);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
                return new List<BestuurderViewingDto>();
            }
        }

        /// <summary>
        /// Haal een bestuurder op met behulp van zijn rijksregisternummer.
        /// </summary>
        /// <param name="rijksregisternummer"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{rijksregisternummer}")]
        public ActionResult<BestuurderViewingDto> GetDriverById(string rijksregisternummer)
        {
            try
            {
                var drivers = _unitOfWork.Bestuurder.GetById(rijksregisternummer);
                return _mapper.Map<BestuurderViewingDto>(drivers);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
                return new BestuurderViewingDto();
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

        #region POST/PUT


        #endregion

    }
}
