using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Reggie.Data;
using Reggie.Models;
using Reggie.ViewModels;


namespace Reggie.Controllers
{
    public class InsuranceController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthorizationService _authorizationService;
        private readonly UserManager<ApplicationUser> _userManager;
        
        public InsuranceController(
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            _authorizationService = authorizationService;
        }

        public async Task<IActionResult> Index(string p, string q)
        {
            //if no state is selected, then return to home
            if (p == null)
            {
                return RedirectToRoute("home-index"); 
            }

            List<SearchResultsViewModel> searchResultsList = new List<SearchResultsViewModel>();
            DateTime currentDateTime = DateTime.Now;
            char[] delimiterChars = { ' ', ',', '+' };
            var isAuthorized = User.IsInRole("Admin") || 
                    User.IsInRole("Admin");

            //Filter data via search params
            var plans = from m in _context.Insurances
                select m;
   
            if (!String.IsNullOrEmpty(q))
            {
                string[] words = q.Split(delimiterChars);
                foreach (string word in words)
                {
                    plans = plans.Where(s => s.Payor.Contains(word) || s.Plan.Contains(word) || s.Keywords.Contains(word) || s.EDI_Number.Contains(word)  );
                }
            }

            //Check State Assignment
            if (p == "Alaska")
            {
                plans = plans.Where(x => x.Location_AK == true);
            }
            else if (p == "California")
            {
                plans = plans.Where(x => x.Location_CA == true);
            }
            else if (p == "Montana")
            {
                plans = plans.Where(x => x.Location_MT == true);
            }
            else if (p == "Oregon")
            {
                plans = plans.Where(x => x.Location_OR == true);
            }
            else if (p == "Washington")
            {
                plans = plans.Where(x => x.Location_WA == true);
            }
            else
            {
                //do nothing
            }

            //If not ADMIN, then do not show NOT PUBLISHED items in search results.
            if (!isAuthorized)
            {
                plans = plans.Where(c => c.ReggiePublishStartDate <= currentDateTime && ( c.ReggiePublishEndDate == null || c.ReggiePublishEndDate > currentDateTime ) );
            }
            
            //Order results by Payor & Plan names
            plans = plans.OrderBy(j => j.Payor).ThenBy(j => j.Plan);

            //Create list for View Model
            var listData = await (from n in plans
                select new
                {
                    n.Id,
                    n.Payor,
                    n.Plan,
                    n.Keywords,
                    n.PlanActiveDate,
                    n.PlanInactiveDate,
                    n.ReggiePublishStartDate,
                    n.ReggiePublishEndDate

                }
                ).ToListAsync();

            //Set IsReggiePublished and IsPlanActive based on dates
            listData.ForEach(x =>
            {
                SearchResultsViewModel Obj = new SearchResultsViewModel();

                if (x.ReggiePublishStartDate == null || x.ReggiePublishStartDate > currentDateTime)
                {
                    Obj.IsReggiePublished = false;
                } 
                else if (x.ReggiePublishEndDate <= currentDateTime)
                {
                    Obj.IsReggiePublished = false;
                }
                else // ReggiePublishStartDate is today or BEFORE && ReggiePublishEndDate is null or in future
                {
                    Obj.IsReggiePublished = true;
                }

                if (x.PlanActiveDate == null || x.PlanActiveDate > currentDateTime)
                {
                    Obj.IsPlanActive = false;
                } 
                else if (x.PlanInactiveDate <= currentDateTime)
                {
                    Obj.IsPlanActive = false;
                }
                else // PlanActiveDate is today or BEFORE && PlanInactiveDate is null or in future
                {
                    Obj.IsPlanActive = true;
                }

                Obj.Id = x.Id;
                Obj.Payor = x.Payor;
                Obj.Plan = x.Plan;
                Obj.Keywords = x.Keywords;
                searchResultsList.Add(Obj);
            });

            return View(searchResultsList);
        }

        [HttpPost]
        
        public string Index(string searchString, bool notUsed)
        {
            return "From [HttpPost]Index: filter on " + searchString;
        }

        public IActionResult Details(int id)
        {

            //details page for view model
            DateTime currentDateTime = DateTime.Now;
            var isAuthorized = User.IsInRole("Admin") || 
                    User.IsInRole("Admin");
            var plan = _context.Insurances.FirstOrDefault(e => e.Id == id);
            DetailsPageViewModel model = new DetailsPageViewModel();
            
            //If user is NOT ADMIN
            if (!isAuthorized)
            {
                //Check Reggie Publish status. If record status is NOT PUBLISHED, reroute NOT ADMIN to HOME
                if (plan.ReggiePublishStartDate == null || plan.ReggiePublishStartDate > currentDateTime || plan.ReggiePublishEndDate <= currentDateTime)
                {
                    return RedirectToRoute("home-index"); 
                }

            }

            //Set IsReggiePublished  based on dates
            if (plan.ReggiePublishStartDate == null || plan.ReggiePublishStartDate > currentDateTime)
            {
                model.IsReggiePublished = false;
            } 
            else if (plan.ReggiePublishEndDate <= currentDateTime)
            {
                model.IsReggiePublished = false;
            }
            else // ReggiePublishStartDate is today or BEFORE && ReggiePublishEndDate is null or in future
            {
                model.IsReggiePublished = true;
            }

            //Set IsPlanActive  based on dates
            if (plan.PlanActiveDate == null || plan.PlanActiveDate > currentDateTime)
            {
                model.IsPlanActive = false;
            } 
            else if (plan.PlanInactiveDate <= currentDateTime)
            {
                model.IsPlanActive = false;
            }
            else // PlanActiveDate is today or BEFORE && PlanInactiveDate is null or in future
            {
                model.IsPlanActive = true;
            }
            
            model.Id = plan.Id;
            model.Payor = plan.Payor;
            model.PayorWebsite = plan.PayorWebsite;
            model.OtherContactInfo = plan.OtherContactInfo;
            model.Plan = plan.Plan;
            model.Keywords = plan.Keywords;
            model.Type_Commercial = plan.Type_Commercial;
            model.Type_CommercialMang = plan.Type_CommercialMang;
            model.Type_Medicare = plan.Type_Medicare;
            model.Type_MedicareMang = plan.Type_MedicareMang;
            model.Type_MedicareSup = plan.Type_MedicareSup;
            model.Type_Medicaid = plan.Type_Medicaid;
            model.Type_MedicaidMang = plan.Type_MedicaidMang;
            model.Type_Military = plan.Type_Military;
            model.Product_EPO = plan.Product_EPO;
            model.Product_HMO = plan.Product_HMO;
            model.Product_Ind = plan.Product_Ind;
            model.Product_PEBB = plan.Product_PEBB;
            model.Product_PPO = plan.Product_PPO;
            model.Location_AK = plan.Location_AK;
            model.Location_CA = plan.Location_CA;
            model.Location_MT = plan.Location_MT;
            model.Location_OR = plan.Location_OR;
            model.Location_WA = plan.Location_WA;
            model.Address1 = plan.Address1;
            model.Address2 = plan.Address2;
            model.City = plan.City;
            model.State = plan.State;
            model.Zip = plan.Zip;
            model.ID_Description = plan.ID_Description;
            model.ReadingHint1 = plan.ReadingHint1;
            model.ReadingHint2 = plan.ReadingHint2;
            model.ReadingHint3 = plan.ReadingHint3;
            model.ReadingHint4 = plan.ReadingHint4;
            model.ReadingHint5 = plan.ReadingHint5;
            model.RegistrationHint1 = plan.RegistrationHint1;
            model.RegistrationHint2 = plan.RegistrationHint2;
            model.RegistrationHint3 = plan.RegistrationHint3;
            model.RegistrationHint4 = plan.RegistrationHint4;
            model.RegistrationHint5 = plan.RegistrationHint5;
            model.RTE_Info1 = plan.RTE_Info1;
            model.RTE_Info2 = plan.RTE_Info2;
            model.RTE_Info3 = plan.RTE_Info3;
            model.RTE_Info4 = plan.RTE_Info4;
            model.RTE_Info5 = plan.RTE_Info5;
            model.Assoc_Workflow1 = plan.Assoc_Workflow1;
            model.Assoc_Workflow1_Title = plan.Assoc_Workflow1_Title;
            model.Assoc_Workflow2 = plan.Assoc_Workflow2;
            model.Assoc_Workflow2_Title = plan.Assoc_Workflow2_Title;
            model.Assoc_Workflow3 = plan.Assoc_Workflow3;
            model.Assoc_Workflow3_Title = plan.Assoc_Workflow3_Title;
            model.Assoc_Workflow4 = plan.Assoc_Workflow4;
            model.Assoc_Workflow4_Title = plan.Assoc_Workflow4_Title;
            model.Assoc_Workflow5 = plan.Assoc_Workflow5;
            model.Assoc_Workflow5_Title = plan.Assoc_Workflow5_Title;
            model.EDI_Number = plan.EDI_Number;
            model.HB_OfficeInfo1 = plan.HB_OfficeInfo1;
            model.HB_OfficeInfo2 = plan.HB_OfficeInfo2;
            model.HB_OfficeInfo3 = plan.HB_OfficeInfo3;
            model.HB_OfficeInfo4 = plan.HB_OfficeInfo4;
            model.HB_OfficeInfo5 = plan.HB_OfficeInfo5;
            model.PB_OfficeInfo1 = plan.PB_OfficeInfo1;
            model.PB_OfficeInfo2 = plan.PB_OfficeInfo2;
            model.PB_OfficeInfo3 = plan.PB_OfficeInfo3;
            model.PB_OfficeInfo4 = plan.PB_OfficeInfo4;
            model.PB_OfficeInfo5 = plan.PB_OfficeInfo5;

            return View(model);
        }

        // GET: Insurance/Create
        [Authorize(Policy = "CanDoAnything")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Insurance/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "CanDoAnything")]
        public async Task<IActionResult> Create(Insurance insurance)
        {
            if (ModelState.IsValid)
            {
                _context.Add(insurance);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new {id = insurance.Id }); 
            }
            return View(insurance);
        }

        // GET: Insurance/Edit/5
        
        [Authorize(Policy = "CanDoAnything")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insurance = await _context.Insurances.SingleOrDefaultAsync(m => m.Id == id);
            if (insurance == null)
            {
                return NotFound();
            }
            return View(insurance);
        }

        // POST: Insurance/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "CanDoAnything")]
        public async Task<IActionResult> Edit(int id, Insurance insurance)
        {
            if (id != insurance.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(insurance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IdExists(insurance.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                
                return RedirectToAction("Details", new {id = insurance.Id }); 
            }

            return View(insurance);
        }

                
        // GET: Insurance/Delete/5
        [Authorize(Policy = "CanDoAnything")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insurance = await _context.Insurances.SingleOrDefaultAsync(m => m.Id == id);
            if (insurance == null)
            {
                return NotFound();
            }
            return View(insurance);
        }

        // POST: Insurance/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "CanDoAnything")]
        public async Task<IActionResult> Delete(int id, bool notUsed)
        {
            var insurance = await _context.Insurances.SingleOrDefaultAsync(m => m.Id == id);
            _context.Insurances.Remove(insurance);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public IActionResult Error()
        {
            return View();
        }
        
        [AcceptVerbs("Get", "Post")]
        public IActionResult ValidateUniqueInsurance(string Payor, string InitialPayor, string Plan, string InitialPlan, bool Location_AK, string InitialAK, bool Location_CA,string InitialCA, bool Location_MT, string InitialMT, bool Location_OR, string InitialOR, bool Location_WA, string InitialWA)
        {
            if (Payor == InitialPayor && Plan == InitialPlan && Location_AK == Convert.ToBoolean(InitialAK) && Location_CA == Convert.ToBoolean(InitialCA) && Location_MT == Convert.ToBoolean(InitialMT) && Location_OR == Convert.ToBoolean(InitialOR) && Location_WA == Convert.ToBoolean(InitialWA))
            {
                return Json(data: true);
            }
            else
            {
                if (UniqueInsuranceExists(Payor, Plan, Location_AK, Location_CA, Location_MT, Location_OR, Location_WA))
                {
                    return Json(data: $"This combination of Payor, Plan, & Location is already in use.");
                }
                else
                {
                    return Json(data: true);
                }
                
            }

        }

        private bool UniqueInsuranceExists(string payorName, string planName, bool AK, bool CA, bool MT, bool OR, bool WA)
        {
            return _context.Insurances.Any(e => e.Payor == payorName && e.Plan == planName && e.Location_AK == AK && e.Location_CA == CA && e.Location_MT == MT && e.Location_OR == OR && e.Location_WA == WA);
        }

        private bool IdExists(int id)
        {
            return _context.Insurances.Any(e => e.Id == id);
        }



    }
}