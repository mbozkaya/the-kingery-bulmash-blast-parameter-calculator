using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Web.DBModel;
using Web.Entity;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ECDBContext _eCDBContext;
        private double _weight;
        private double _range;
        private ExplosiveType _explosive;

        public HomeController(ILogger<HomeController> logger, ECDBContext eCDBContext)
        {
            _logger = logger;
            _eCDBContext = eCDBContext;
        }

        public IActionResult Index()
        {
            List<DBModel.ExplosiveType> model = _eCDBContext.ExplosiveTypes.ToList();
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public JsonResult Calculate(int explosiveType, double weight, double range)
        {
            _weight = weight;
            _range = range;
            Exception _ex = null;

            try
            {
                DoCalculate(explosiveType);
            }
            catch (Exception ex)
            {
                _ex = ex;
            }

            return new JsonResult(new { error = _ex != null, errorMessage = _ex?.Message ?? string.Empty });
        }

        [HttpPost]
        public JsonResult GetData(DataTableParameters dataTableParameters)
        {

            int page = dataTableParameters.Start == 0 ? 0 : (dataTableParameters.Start / dataTableParameters.Length) - 1;
            DataTableReturnModel returnModel = new DataTableReturnModel
            {
                Data = _eCDBContext.CalculationResults.Select(s => new DataTableDataModel
                {
                    Id = s.Id,
                    CreatedDate = s.CreatedDate,
                    ExplosiveType = s.CalculationEntry.ExplosiveType.Name,
                    IncidentImpulse = s.IncidentImpulse,
                    IncidentPressure = s.IncidentPressure,
                    PositivePhaseDuration = s.PositivePhaseDuration,
                    Range = s.CalculationEntry.Range,
                    ReflectedImpulse = s.ReflectedImpulse,
                    ReflectedPressure = s.ReflectedPressure,
                    ShockFrontVelocity = s.ShockFrontVelocity,
                    TimeOfArrival = s.TimeOfArrival,
                    TNTWeightForImpulse = s.TNTWeightForImpulse,
                    TNTWeightForPressure = s.TNTWeightForPressure,
                    Weight = s.CalculationEntry.Weight,
                })
                .Skip(page * dataTableParameters.Length)
                .Take(dataTableParameters.Length)
                .OrderByDescending(o => o.Id)
            .ToList(),
                Draw = dataTableParameters.Draw,
                RecordsFiltered = _eCDBContext.CalculationResults.Count(),
                RecordsTotal = _eCDBContext.CalculationResults.Count(),
            };


            return new JsonResult(returnModel);
        }

        private CalculationResult DoCalculate(int explosionType)
        {
            var newEntry = new CalculationEntry
            {
                ExplosiveTypeId = explosionType,
                Range = _range,
                Weight = _weight
            };
            _eCDBContext.CalculationEntries.Add(newEntry);
            _eCDBContext.SaveChanges();

            ExplosiveType explosive = _eCDBContext.ExplosiveTypes.Where(w => w.Id == explosionType).FirstOrDefault();
            _explosive = explosive;

            var t = getScaledDistance();

            var newResult = new CalculationResult
            {
                TNTWeightForPressure = explosive.PeakPressureTNTEquiv * newEntry.Weight,
                TNTWeightForImpulse = explosive.ImpulseTNTEquiv * newEntry.Weight,
                IncidentPressure = calculateIncidentPressure(t),
                IncidentImpulse = calculateIncidentImpulse(t),
                ReflectedPressure = calculateReflectedPressure(t),
                ReflectedImpulse = calculateReflectedImpulse(t),
                TimeOfArrival = calculateTimeOfArrival(t),
                PositivePhaseDuration = calculatePositivePhaseDuration(t),
                ShockFrontVelocity = calculateShockFrontVelocity(t),
                CalculationEntryId = newEntry.Id,
                CreatedDate = DateTime.Now,
            };

            _eCDBContext.CalculationResults.Add(newResult);

            _eCDBContext.SaveChanges();

            return newResult;
        }

        private double getChargeWeight(string equivType = "")
        {
            if (equivType == "impulse")
            {
                return _weight * (double)_explosive.ImpulseTNTEquiv;
            }
            return _weight * (double)_explosive.PeakPressureTNTEquiv;
        }

        private double getScaledDistance()
        {
            double constantVal = 0.3333333;
            return (double)_range / Math.Pow((double)getChargeWeight(), constantVal);
        }

        private double calculateIncidentPressure(double t)
        { //NATO AASTP version
            var U = -0.214362789151 + 1.35034249993 * t;
            var ip = 2.78076916577 - 1.6958988741 * U -
                0.154159376846 * Math.Pow(U, 2) +
                0.514060730593 * Math.Pow(U, 3) +
                0.0988534365274 * Math.Pow(U, 4) -
                0.293912623038 * Math.Pow(U, 5) -
                0.0268112345019 * Math.Pow(U, 6) +
                0.109097496421 * Math.Pow(U, 7) +
                0.00162846756311 * Math.Pow(U, 8) -
                0.0214631030242 * Math.Pow(U, 9) +
                0.0001456723382 * Math.Pow(U, 10) +
                0.00167847752266 * Math.Pow(U, 11);
            ip = Math.Pow(10, ip);
            return ip;
        }

        private double calculateIncidentImpulse(double t)
        {
            var scaledDistance = getScaledDistance();
            var cubeRootOfChargeWeight = Math.Pow(getChargeWeight("impulse"), 0.3333333);
            double ii = 0;
            if (scaledDistance > 0.0674 && scaledDistance <= 0.955)
            { //NATO version
                var U = 2.06761908721 + 3.0760329666 * t;
                ii = 2.52455620925 - 0.502992763686 * U +
                    0.171335645235 * Math.Pow(U, 2) +
                    0.0450176963051 * Math.Pow(U, 3) -
                    0.0118964626402 * Math.Pow(U, 4);

            }
            else if (scaledDistance > 0.955 && scaledDistance < 40)
            { //version from ???
                var U = -1.94708846747 + 2.40697745406 * t;
                ii = 1.67281645863 - 0.384519026965 * U -
                    0.0260816706301 * Math.Pow(U, 2) +
                    0.00595798753822 * Math.Pow(U, 3) +
                    0.014544526107 * Math.Pow(U, 4) -
                    0.00663289334734 * Math.Pow(U, 5) -
                    0.00284189327204 * Math.Pow(U, 6) +
                    0.0013644816227 * Math.Pow(U, 7);
            }
            else
            {
                return 0;
            }
            ii = Math.Pow(10, ii);
            ii = ii * cubeRootOfChargeWeight;
            return ii;
        }

        private double calculateReflectedPressure(double t)
        {
            var U = -0.240657322658 + 1.36637719229 * t;
            var rp = 3.40283217581 - 2.21030870597 * U -
                0.218536586295 * Math.Pow(U, 2) +
                0.895319589372 * Math.Pow(U, 3) +
                0.24989009775 * Math.Pow(U, 4) -
                0.569249436807 * Math.Pow(U, 5) -
                0.11791682383 * Math.Pow(U, 6) +
                0.224131161411 * Math.Pow(U, 7) +
                0.0245620259375 * Math.Pow(U, 8) -
                0.0455116002694 * Math.Pow(U, 9) -
                0.00190930738887 * Math.Pow(U, 10) +
                0.00361471193389 * Math.Pow(U, 11);
            rp = Math.Pow(10, rp);
            return rp;
        }

        private double calculateReflectedImpulse(double t)
        {
            var cubeRootOfChargeWeight = Math.Pow(getChargeWeight("impulse"), 0.3333333);
            var U = -0.246208804814 + 1.33422049854 * t;
            var ir = 2.70588058103 - 0.949516092853 * U +
                0.112136118689 * Math.Pow(U, 2) -
                0.0250659183287 * Math.Pow(U, 3);
            ir = Math.Pow(10, ir);
            ir = ir * cubeRootOfChargeWeight;
            return ir;
        }

        private double calculateTimeOfArrival(double t)
        {
            var cubeRootOfChargeWeight = Math.Pow(getChargeWeight("impulse"), 0.3333333);
            var U = -0.202425716178 + 1.37784223635 * t;
            var toa = -0.0591634288046 + 1.35706496258 * U +
                0.052492798645 * Math.Pow(U, 2) -
                0.196563954086 * Math.Pow(U, 3) -
                0.0601770052288 * Math.Pow(U, 4) +
                0.0696360270891 * Math.Pow(U, 5) +
                0.0215297490092 * Math.Pow(U, 6) -
                0.0161658930785 * Math.Pow(U, 7) -
                0.00232531970294 * Math.Pow(U, 8) +
                0.00147752067524 * Math.Pow(U, 9);
            toa = Math.Pow(10, toa);
            toa = toa * cubeRootOfChargeWeight;
            return toa;
        }

        private double calculatePositivePhaseDuration(double t)
        {
            var scaledDistance = getScaledDistance();
            var cubeRootOfChargeWeight = Math.Pow(getChargeWeight("impulse"), 0.3333333);
            double ppd = 0;
            if (scaledDistance > 0.178 && scaledDistance <= 1.01)
            {
                var U = 1.92946154068 + 5.25099193925 * t;
                ppd = -0.614227603559 + 0.130143717675 * U +
                    0.134872511954 * Math.Pow(U, 2) +
                    0.0391574276906 * Math.Pow(U, 3) -
                    0.00475933664702 * Math.Pow(U, 4) -
                    0.00428144598008 * Math.Pow(U, 5);

            }
            else if (scaledDistance > 1.01 && scaledDistance < 2.78)
            {
                var U = 2.12492525216 + 9.2996288611 * t;
                ppd = 0.315409245784 - 0.0297944268976 * U +
                    0.030632954288 * Math.Pow(U, 2) +
                    0.0183405574086 * Math.Pow(U, 3) -
                    0.0173964666211 * Math.Pow(U, 4) -
                    0.00106321963633 * Math.Pow(U, 5) +
                    0.00562060030977 * Math.Pow(U, 6) +
                    0.0001618217499 * Math.Pow(U, 7) -
                    0.0006860188944 * Math.Pow(U, 8);

            }
            else if (scaledDistance > 2.78 && scaledDistance < 40.0)
            {
                var U = -3.53626218091 + 3.46349745571 * t;
                ppd = 0.686906642409 + 0.0933035304009 * U -
                    0.0005849420883 * Math.Pow(U, 2) -
                    0.00226884995013 * Math.Pow(U, 3) -
                    0.00295908591505 * Math.Pow(U, 4) +
                    0.00148029868929 * Math.Pow(U, 5);

            }
            else
            {
                ppd = 0;
            }
            ppd = Math.Pow(10, ppd);
            ppd = ppd * cubeRootOfChargeWeight;
            return ppd;
        }

        private double calculateShockFrontVelocity(double t)
        {
            var U = -0.202425716178 + 1.37784223635 * t;
            var sv = -0.06621072854 - 0.698029762594 * U +
                0.158916781906 * Math.Pow(U, 2) +
                0.443812098136 * Math.Pow(U, 3) -
                0.113402023921 * Math.Pow(U, 4) -
                0.369887075049 * Math.Pow(U, 5) +
                0.129230567449 * Math.Pow(U, 6) +
                0.19857981197 * Math.Pow(U, 7) -
                0.0867636217397 * Math.Pow(U, 8) -
                0.0620391900135 * Math.Pow(U, 9) +
                0.0307482926566 * Math.Pow(U, 10) +
                0.0102657234407 * Math.Pow(U, 11) -
                0.00546533250772 * Math.Pow(U, 12) -
                0.000693180974 * Math.Pow(U, 13) +
                0.0003847494916 * Math.Pow(U, 14);
            sv = Math.Pow(10, sv) * 1000;
            return sv;
        }
    }
}
