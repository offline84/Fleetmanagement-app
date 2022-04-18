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
    [Route("[controller]")]
    public class BestuurderController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        private ILoggerFactory _loggerFactory = new LoggerFactory();
        private FleetmanagerContext _context;
        private readonly IMapper _mapper;

        public BestuurderController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ??
                throw new ArgumentNullException(nameof(BestuurderRepository));
            _loggerFactory.CreateLogger("BestuurderController");
            _mapper = mapper;
        }

        #region Getters

        [HttpGet]
        public ActionResult<IEnumerable<BestuurderViewModel>> GetAllDrivers()
        {
            try
            {
                var bestuurders = _unitOfWork.Bestuurder.GetAll();
                return _mapper.Map<List<BestuurderViewModel>>(bestuurders);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
                return new List<BestuurderViewModel>();
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<BestuurderViewModel>> GetAllActiveDrivers()
        {
            try
            {
                var bestuurders = _unitOfWork.Bestuurder.GetAllActive();
                return _mapper.Map<List<BestuurderViewModel>>(bestuurders);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
                return new List<BestuurderViewModel>();
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<BestuurderViewModel>> GetAllArchivedDrivers()
        {
            try
            {
                var bestuurders = _unitOfWork.Bestuurder.GetAllArchived();
                return _mapper.Map<List<BestuurderViewModel>>(bestuurders);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
                return new List<BestuurderViewModel>();
            }
        }

        [HttpGet]
        public ActionResult<BestuurderViewModel> GetDriverById(string rijksregisternr)
        {
            try
            {
                var drivers = _unitOfWork.Bestuurder.GetById(rijksregisternr);
                return _mapper.Map<BestuurderViewModel>(drivers);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
                return new BestuurderViewModel();
            }
        }

        [HttpGet]
        public ActionResult<List<RijbewijsViewModel>> GetDriverLicensesForDriver(string rijksregisternr)
        {
            try
            {
                var driverLicenses = _unitOfWork.Bestuurder.GetDriverLicensesForDriver(rijksregisternr);
                return _mapper.Map<List<RijbewijsViewModel>>(driverLicenses);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
                return new List<RijbewijsViewModel>();
            }
        }

        #endregion

        #region POST/PUT


        #endregion

    }
}
